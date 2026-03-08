using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class User
    {
        public int Id { get; set; } 
        public UserProfile? Profile { get; set; }
        [Unique]
        public string Login { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;


    }
}
