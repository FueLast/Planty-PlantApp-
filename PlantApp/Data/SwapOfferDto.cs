using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class SwapOfferDto
    {
        public int id { get; set; }
        public string owner_id { get; set; } = null!; //uuid
        public int user_plant_id { get; set; }

        public string? desired_plant_description { get; set; }
        public DateTime created_at { get; set; }

        public string? owner_name { get; set; }
    }
}
