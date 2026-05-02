using System.ComponentModel.DataAnnotations;

namespace Cinemate.Models
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        public string? City { get; set; }
        public string? Bio { get; set; }
        
        // Şifre değiştirme de ekleyelim (Opsiyonel)
        public string? NewPassword { get; set; }
    }
}