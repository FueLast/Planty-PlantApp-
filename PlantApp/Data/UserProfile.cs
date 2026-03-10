using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class UserProfile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int? Age { get; set; }

        // Внешний ключ для связи с User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

