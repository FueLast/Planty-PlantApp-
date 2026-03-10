using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class FavoritePlant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PlantId { get; set; }
        public Plant Plant { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}