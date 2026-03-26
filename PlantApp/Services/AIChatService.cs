using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services
{
    public class AIChatService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public AIChatService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<Chat> GetOrCreateChatAsync(int userId)
        {
            using var db = _factory.CreateDbContext();

            var chat = await db.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (chat == null)
            {
                chat = new Chat { UserId = userId };
                db.Chats.Add(chat);
                await db.SaveChangesAsync();
            }

            return chat;
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            using var db = _factory.CreateDbContext();

            db.ChatMessages.Add(message);
            await db.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesAsync(int chatId)
        {
            using var db = _factory.CreateDbContext();

            return await db.ChatMessages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}