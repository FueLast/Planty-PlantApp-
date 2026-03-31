using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Helpers
{
    public static class ChatHelper
    {
        public static string GetChatId(int user1, int user2)
        {
            var min = Math.Min(user1, user2);
            var max = Math.Max(user1, user2);

            return $"{min}_{max}";
        }
    }
}
