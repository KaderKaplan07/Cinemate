using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Models; // Senin AppUser sınıfının olduğu yer

namespace Cinemate.Controllers
{
    public class AccountController : Controller
    {
        // Identity servislerini tanımlıyoruz
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        // Constructor (Yapıcı Metot) - Servisleri içeri alıyoruz
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- REGISTER (ÜYE OLMA SAYFASINI AÇ) ---
[HttpGet]
public IActionResult Register()
{
    return View();
}

// --- REGISTER (ÜYE OLMA İŞLEMİNİ YAP) ---
[HttpPost]
public async Task<IActionResult> Register(string username, string email, string password)
{
    // 1. Veriler boş mu kontrol et
    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        ViewBag.Error = "Lütfen tüm alanları doldurunuz.";
        return View();
    }

    // 2. Yeni kullanıcıyı hazırla
    var user = new AppUser 
    { 
        UserName = username, 
        Email = email,
        // İstersen FullName de ekleyebilirsin: FullName = username 
    };

    // 3. Kullanıcıyı oluştur (Identity şifreyi otomatik hash'ler)
    var result = await _userManager.CreateAsync(user, password);

    if (result.Succeeded)
    {
        // 4. Başarılıysa hemen giriş yaptır
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", "Home");
    }

    // 5. Hata varsa (Örn: Şifre çok basit, bu isimde kullanıcı zaten var vb.)
    foreach (var error in result.Errors)
    {
        // Hataları ekrana yazdırmak için ModelState'e ekle
        ModelState.AddModelError("", error.Description);
    }

    return View();
}

        // --- LOGIN (GİRİŞ YAP) KISMI ---

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Login(string email, string password)
{
    // 1. E-posta veya şifre boş mu kontrol et
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        ViewBag.Error = "Lütfen e-posta ve şifrenizi giriniz.";
        return View();
    }

    // 2. E-posta adresine sahip kullanıcıyı bul
    var user = await _userManager.FindByEmailAsync(email);

    if (user != null)
    {
        // 3. Kullanıcı bulunduysa şifresini kontrol et ve giriş yap
        // ÖNEMLİ: PasswordSignInAsync metoduna 'user' nesnesini veriyoruz.
        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
    }

    // 4. Hatalıysa mesaj göster
    ViewBag.Error = "Hatalı e-posta veya şifre!";
    return View();
}

        // --- LOGOUT (ÇIKIŞ YAP) KISMI ---
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}