using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlantApp.Models;

namespace PlantApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<ChatMessage>().HasData(
        //        new ChatMessage
        //        {
        //            Id = 2,
        //            UserProfileId = 1,
        //            Date = DateTime.Today,
        //            TextMessage = "  привит ",
        //            Profile_Image = null
        //        }
        //        );

        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<UserProfile>().HasData(
        //        new UserProfile
        //        {
        //            Id = 2,
        //            UserName = "Test",
        //            ProfileDescription = "Test description",
        //            Age = 20,
        //            City = "Kazan",
        //            UserProfileImage = null
        //        }
        //        );
        //}
    }
}
