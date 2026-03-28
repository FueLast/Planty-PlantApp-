using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class User
    { 
        public int Id { get; set; }
        public string Login { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        // Навигационное свойство: ссылка на профиль
        public UserProfile Profile { get; set; } = null!;

        [NotMapped]
        public bool IsRequestSent { get; set; }
    }
}

