using System;
using Microsoft.EntityFrameworkCore;
using PlantApp.Models;

namespace PlantApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { 
            // Создаем новую БД со всеми таблицами из DbSet
            Database.EnsureCreated();
        }

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
                .HasForeignKey<UserProfile>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoritePlant>()
                .HasKey(fp => new { fp.UserId, fp.PlantId }); // Рекомендую добавить составной ключ

            modelBuilder.Entity<FavoritePlant>()
                .HasOne(fp => fp.User)
                .WithMany()
                .HasForeignKey(fp => fp.UserId);

            modelBuilder.Entity<FavoritePlant>()
                .HasOne(fp => fp.Plant)
                .WithMany()
                .HasForeignKey(fp => fp.PlantId);
        }
    }
}
