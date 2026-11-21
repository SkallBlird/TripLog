using Microsoft.EntityFrameworkCore;
using TripLog.Models;

namespace TripLog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().OwnsOne(t => t.Accommodation);
            modelBuilder.Entity<Trip>().OwnsOne(t => t.ThingsToDo);
        }
    }
}