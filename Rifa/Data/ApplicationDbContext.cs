using Microsoft.EntityFrameworkCore;
using Rifa.Models;

namespace Rifa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TmstPlanning> TmstPlannings { get; set; }
        public DbSet<TplanningUser> TplanningUsers { get; set; }
        public DbSet<TplanningRecommendation> TplanningRecommendations { get; set; }
    }
}