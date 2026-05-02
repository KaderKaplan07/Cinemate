using Cinemate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Cinemate.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cinemate.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CinemateDbContext _db;
        private const string API_KEY = "976c70440c81657e65c80dd8e8bed1e1";

        public HomeController(IHttpClientFactory factory, CinemateDbContext db)
        {
            _httpClient = factory.CreateClient();
            _db = db;
        }

        // 🎬 POPÜLER FİLMLER
        public async Task<IActionResult> Index()
        {
            var films = new List<Film>();

            try
            {
                var url = $"https://api.themoviedb.org/3/movie/popular?api_key={API_KEY}&language=tr-TR";
                var response = await _httpClient.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);
                var results = doc.RootElement.GetProperty("results");

                foreach (var movie in results.EnumerateArray())
                {
                    films.Add(new Film
                    {
                        Id = movie.GetProperty("id").GetInt32(),
                        Title = movie.GetProperty("title").GetString(),
                        PosterPath = movie.GetProperty("poster_path").GetString(),
                        Rating = movie.GetProperty("vote_average").GetDouble()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TMDB fetch failed: " + ex.Message);
            }

            return View(films);
        }

        // 🎬 FİLM DETAY
        public async Task<IActionResult> Detail(int id)
        {
            var url = $"https://api.themoviedb.org/3/movie/{id}?api_key={API_KEY}&language=tr-TR";
            var response = await _httpClient.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            var m = doc.RootElement;

            var film = new Film
            {
                Id = m.GetProperty("id").GetInt32(),
                Title = m.GetProperty("title").GetString(),
                PosterPath = m.GetProperty("poster_path").GetString(),
                Overview = m.GetProperty("overview").GetString(),
                Rating = m.GetProperty("vote_average").GetDouble()
            };

            // ⭐ FAVORİ KONTROLÜ
            ViewBag.IsFavorite = false;
            ViewBag.FavoriteId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(userId))
                {
                    var fav = await _db.Favorites
                        .FirstOrDefaultAsync(f => f.UserId == userId && f.FilmId == id);

                    ViewBag.IsFavorite = fav != null;
                    ViewBag.FavoriteId = fav?.Id;
                }
            }

            return View(film);
        }
    }
}
