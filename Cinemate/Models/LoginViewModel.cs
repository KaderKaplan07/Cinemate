using System.ComponentModel.DataAnnotations;

namespace Cinemate.Models
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public bool? RememberMe { get; set; }
    }
} 