using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Data
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string ProfileDescription { get; set; }
        public int Age { get; set; }
        public string City { get; set; }

        public byte[]? UserProfileImage { get; set; } //тут пользователь должен загружать своё фото в профиль (далее оно преобразуется в двоичный код)

    }
}
