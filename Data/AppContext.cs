using Microsoft.EntityFrameworkCore;
using CornHoleRevamp.Models;

namespace CornHoleRevamp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Stats> Stats { get; set; }
        public DbSet<GameMatch> GameMatches { get; set; }
        public DbSet<RoundData> RoundData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Stats relationship
            modelBuilder.Entity<Stats>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // GameMatch relationships
            modelBuilder.Entity<GameMatch>()
                .HasOne(g => g.Player1)
                .WithMany()
                .HasForeignKey(g => g.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameMatch>()
                .HasOne(g => g.Player2)
                .WithMany()
                .HasForeignKey(g => g.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // RoundData relationship - FIXED
            modelBuilder.Entity<RoundData>()
                .HasOne(r => r.GameMatch)
                .WithMany(g => g.Rounds) 
                .HasForeignKey(r => r.GameMatchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}