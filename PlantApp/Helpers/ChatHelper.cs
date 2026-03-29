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
            var ids = new[] { user1, user2 };
            Array.Sort(ids);

            return $"{ids[0]}_{ids[1]}";
        }
    }
}
