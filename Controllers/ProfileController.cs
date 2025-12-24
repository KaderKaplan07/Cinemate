using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Models;

namespace Cinemate.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- R: READ (Profil Sayfasını Getir) ---
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Veritabanındaki verileri modele aktar
            var model = new UserEditViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Bio = user.Bio
            };

            return View(model);
        }

        // --- U: UPDATE (Profili Güncelle) ---
        [HttpPost]
        public async Task<IActionResult> Index(UserEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Verileri güncelle
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.City = model.City;
            user.Bio = model.Bio;

            // Şifre değiştirilmek istenmişse
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Kullanıcı adı değiştiyse oturumu tazelemek gerekir
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.Status = "success";
                ViewBag.Message = "Profiliniz başarıyla güncellendi.";
            }
            else
            {
                ViewBag.Status = "error";
                ViewBag.Message = "Güncelleme sırasında bir hata oluştu.";
            }

            return View(model);
        }

        // --- D: DELETE (Hesabı Sil) ---
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // Kullanıcıyı veritabanından sil
                await _userManager.DeleteAsync(user);
                
                // Oturumu kapat
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}