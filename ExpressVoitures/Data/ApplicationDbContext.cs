using ExpressVoitures.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Voiture> Voitures { get; set; }

        public DbSet<VoitureImage> VoitureImages  { get; set; }

        public DbSet<Annonce> Annonces { get; set; }

        public DbSet<Reparation> Reparations { get; set; }

        public DbSet<Marge> Marges { get; set; }
    }
}
