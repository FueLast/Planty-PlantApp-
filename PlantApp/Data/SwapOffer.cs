using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class SwapOffer
    {
        public int Id { get; set; }

        public string OwnerId { get; set; } = null!; //uuid

        public int UserPlantId { get; set; }

        public string ImageUrl { get; set; }
        public string PlantName { get; set; }
        public string Description { get; set; }

        public string? DesiredPlantDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? OwnerName { get; set; }
        public string? OwnerCity { get; set; }
        public UserProfile? OwnerProfile { get; set; }

        // навигация
        public User? Owner { get; set; }
        public UserPlant? Plant { get; set; } 
    }
}
