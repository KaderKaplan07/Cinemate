using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemate.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FilmId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        // Navigation
        public Film Film { get; set; } = null!;
    }
}
