using Microsoft.AspNetCore.Identity;

namespace Cinemate.Models
{
    // IdentityUser sayesinde Id, UserName, Email vb. otomatik olarak gelir
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? City { get; set; }  // Şehir
        public string? Bio { get; set; }   // Biyografi (Hakkımda)
    }
}