using System.ComponentModel.DataAnnotations;

namespace Cinemate.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
