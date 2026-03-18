using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class PlantReminder
    {
        public int Id { get; set; }

        public string Title { get; set; } // название (полив, удобрение)

        public DateTime Date { get; set; }

        public int PlantId { get; set; }
    }
}
