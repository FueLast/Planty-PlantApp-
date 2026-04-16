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
        public string desired_plant_description { get; set; }
        public DateTime created_at { get; set; }

        public string owner_name { get; set; }

        public string custom_name { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }

        public string name_plant { get; set; }
    }
}
