using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinemate.Models;

namespace Cinemate.Data
{
    public class CinemateDbContext : IdentityDbContext<AppUser>
    {
        public CinemateDbContext(DbContextOptions<CinemateDbContext> options)
            : base(options)
        {
        }

        public DbSet<Favorite> Favorites { get; set; }
        // Eğer Movie tablon varsa buraya ekle: public DbSet<Movie> Movies { get; set; }
        
        // --- BU KISIM EKSİK OLDUĞU İÇİN HATA ALIYORSUN ---
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // <--- İŞTE BU SATIR IDENTITY TABLOLARINI OLUŞTURUR

            // Buraya ekstra ayarlarını ekleyebilirsin ama şu anlık gerek yok.
        }
    }
}