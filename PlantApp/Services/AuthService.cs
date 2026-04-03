using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services
{
    public class AuthService
    {
        public User? CurrentUser { get; private set; }

        private readonly IDbContextFactory<AppDbContext> _factory;

        public AuthService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public void SetUser(User user)
        {
            CurrentUser = user;
        }

        public int GetUserId()
        {
            if (CurrentUser == null)
                throw new Exception("Пользователь не авторизован");

            return CurrentUser.Id;
        }

        public async Task SetOnline(int userId)
        {
            using var db = await _factory.CreateDbContextAsync();

            var profile = await db.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null) return;

            profile.LastSeen = DateTime.UtcNow;
            profile.IsOnline = true;

            await db.SaveChangesAsync();
        }

        public async Task SetOffline(int userId)
        {
            using var db = await _factory.CreateDbContextAsync();

            var profile = await db.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null) return;

            profile.IsOnline = false;
            profile.LastSeen = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        public bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        public async Task Logout()
        {
            var userId = GetUserId();

            await SetOffline(userId);

            Preferences.Remove("user_id");
        }
    }
}