using Microsoft.EntityFrameworkCore;
using CryptoMonitor.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CryptoMonitor.Data
{
    public class CryptoMonitorDbContext : DbContext
    {
        public CryptoMonitorDbContext(DbContextOptions<CryptoMonitorDbContext> options)
            : base(options) { }

        public DbSet<CryptoAsset> CryptoAssets { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<ApiSource> ApiSources { get; set; }
        public DbSet<DataUpdateLog> DataUpdateLogs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<ReportLog> ReportLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Watchlist>()
                .HasKey(w => new { w.UserId, w.AssetId });

            modelBuilder.Entity<CryptoAsset>().HasKey(c => c.AssetId);
            modelBuilder.Entity<PriceHistory>().HasKey(p => p.HistoryId);
            modelBuilder.Entity<ApiSource>().HasKey(a => a.SourceId);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<ReportLog>().HasKey(rp => rp.ReportId);
            modelBuilder.Entity<DataUpdateLog>().HasKey(d => d.LogId);
        }
    }
}
