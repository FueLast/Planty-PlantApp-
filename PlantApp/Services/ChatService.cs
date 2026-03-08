using CommunityToolkit.Mvvm.ComponentModel;
using PlantApp.Data;
using SQLite;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading.Tasks;


namespace PlantApp.Services
{
    public class ChatService
    {
        private readonly SQLiteAsyncConnection _db;

        public ChatService(string chatdbPath)
        {
            _db = new SQLiteAsyncConnection(chatdbPath);
        }



        //бд растений
        public async Task<List<ChatMessage>> GetUsersAsync()
        {
            await InitAsync();                                // 1. Гарантия схемы
            var count = await _db.Table<ChatMessage>().CountAsync();// 2. Проверка состояния БД
            if (count == 0)                                     // 3. Сценарий первого запуска
            {
                var messages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        TextMessage = "Привет! Я тест сообщение 🌱",
                        Date = DateTime.Now,
                        UserProfileId = 1
                    },
                    new ChatMessage
                    {
                        TextMessage = "Чем могу помочь?",
                        Date = DateTime.Now.AddMinutes(1),
                        UserProfileId = 1
                    }
                };


                await _db.InsertAllAsync(messages);
            }
            return await _db.Table<ChatMessage>().ToListAsync();   // 5. Гарантированный результат
        }


        public async Task InitAsync()
        {
            await _db.CreateTableAsync<ChatMessage>();
        }

    }
}
