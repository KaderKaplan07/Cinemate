using Cinemate.Data;
using Cinemate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cinemate.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly CinemateDbContext _db;

        public FavoritesController(CinemateDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await _db.Favorites
                .Include(f => f.Film) // ⭐ ÇOK ÖNEMLİ
                .Where(f => f.UserId == userId)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!_db.Favorites.Any(f => f.UserId == userId && f.FilmId == filmId))
            {
                _db.Favorites.Add(new Favorite
                {
                    UserId = userId!,
                    FilmId = filmId
                });
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Detail", "Home", new { id = filmId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var fav = await _db.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FilmId == filmId);

            if (fav != null)
            {
                _db.Favorites.Remove(fav);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
