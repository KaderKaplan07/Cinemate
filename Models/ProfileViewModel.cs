using System.Collections.Generic;

namespace Cinemate.Models
{
    public class ProfileViewModel
    {
        public User? User { get; set; }
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}