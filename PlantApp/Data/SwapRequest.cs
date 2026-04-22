using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class SwapRequest
    {
        public int Id { get; set; }

        public int SwapOfferId { get; set; }
        public int FromUserId { get; set; }

        public int OfferedPlantId { get; set; }

        public SwapStatus Status { get; set; } = SwapStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // навигация
        public SwapOffer? SwapOffer { get; set; }
        public User? FromUser { get; set; }
        public UserPlant? OfferedPlant { get; set; }
    }
}
