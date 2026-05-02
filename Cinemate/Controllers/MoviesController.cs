using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FilmOneriMVC.Controllers
{
    
    public class MoviesController : Controller
    {
        private readonly string apiKey = "976c70440c81657e65c80dd8e8bed1e1";

        public async Task<IActionResult> Imdb()
        {
            using HttpClient client = new HttpClient();

            // 🎬 EN İYİ FİLMLER
            var movieUrl =
                $"https://api.themoviedb.org/3/discover/movie" +
                $"?sort_by=vote_average.desc" +
                $"&vote_count.gte=1000" +
                $"&api_key={apiKey}" +
                $"&language=tr-TR";

            var movieResponse = await client.GetStringAsync(movieUrl);
            var movieJson = JsonDocument.Parse(movieResponse);
            var bestMovies = movieJson.RootElement.GetProperty("results");

            // 📺 EN İYİ DİZİLER
            var tvUrl =
                $"https://api.themoviedb.org/3/discover/tv" +
                $"?sort_by=vote_average.desc" +
                $"&vote_count.gte=1000" +
                $"&api_key={apiKey}" +
                $"&language=tr-TR";

            var tvResponse = await client.GetStringAsync(tvUrl);
            var tvJson = JsonDocument.Parse(tvResponse);
            var bestSeries = tvJson.RootElement.GetProperty("results");

            ViewBag.BestMovies = bestMovies;
            ViewBag.BestSeries = bestSeries;

            return View();
        }

        public async Task<IActionResult> Details(int id, string type)
        {
            using HttpClient client = new HttpClient();

            var url =
                $"https://api.themoviedb.org/3/{type}/{id}" +
                $"?api_key={apiKey}&language=tr-TR";

            var response = await client.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            return View(json.RootElement);
        }
public async Task<IActionResult> Genre(int id, string name)
{
    using HttpClient client = new HttpClient();

    var url =
        $"https://api.themoviedb.org/3/discover/movie" +
        $"?with_genres={id}" +
        $"&sort_by=popularity.desc" +
        $"&api_key={apiKey}" +
        $"&language=tr-TR";

    var response = await client.GetStringAsync(url);
    var json = JsonDocument.Parse(response);
    var movies = json.RootElement.GetProperty("results");

    ViewBag.GenreName = name;
    return View(movies);
}

// Rastgele Filmler ve Diziler
public async Task<IActionResult> RandomMovies()
{
    using HttpClient client = new HttpClient();
    var random = new Random();
    int randomPage = random.Next(1, 50);

    var url =
        $"https://api.themoviedb.org/3/discover/movie" +
        $"?api_key={apiKey}" +
        $"&language=tr-TR" +
        $"&page={randomPage}";

    var response = await client.GetStringAsync(url);
    var json = JsonDocument.Parse(response);
    var movies = json.RootElement.GetProperty("results");

    ViewBag.Title = "Filmler";
    return View("Random", movies);
}

public async Task<IActionResult> RandomSeries()
{
    using HttpClient client = new HttpClient();
    var random = new Random();
    int randomPage = random.Next(1, 50);

    var url =
        $"https://api.themoviedb.org/3/discover/tv" +
        $"?api_key={apiKey}" +
        $"&language=tr-TR" +
        $"&page={randomPage}";

    var response = await client.GetStringAsync(url);
    var json = JsonDocument.Parse(response);
    var series = json.RootElement.GetProperty("results");

    ViewBag.Title = "Diziler";
    return View("Random", series);
}


    }
}
