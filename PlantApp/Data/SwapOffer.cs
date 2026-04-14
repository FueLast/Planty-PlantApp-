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

        public int OwnerId { get; set; } 

        public int UserPlantId { get; set; }

        public string? DesiredPlantDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? OwnerName { get; set; }

        // навигация
        public User? Owner { get; set; }
        public UserPlant? Plant { get; set; }
    }
}
