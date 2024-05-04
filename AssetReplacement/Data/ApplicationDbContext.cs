using AssetReplacement.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetReplacement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AssetRequest> AssetRequests { get; set; }

        // Tambahkan DbSet untuk model lain yang Anda miliki
        // public DbSet<AssetReplacement> AssetReplacements { get; set; }
        // public DbSet<AssetScrap> AssetScraps { get; set; }
    }
}