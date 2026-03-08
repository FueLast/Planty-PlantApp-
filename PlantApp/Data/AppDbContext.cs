using System;
using Microsoft.EntityFrameworkCore;
using PlantApp.Models;

namespace PlantApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Plant> Plants { get; set; }

        public DbSet<User> Users { get; set; }   

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<FavoritePlant> FavoritePlants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}