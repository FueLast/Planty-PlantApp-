using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services;

public class ProfileService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ProfileService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    // получаем профиль пользователя
    public async Task<UserProfile?> GetProfile(int userId)
    {
        using var db = _factory.CreateDbContext();

        return await db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    // обновляем профиль пользователя
    public async Task UpdateProfile(UserProfile profile)
    {
        using var db = _factory.CreateDbContext();

        db.UserProfiles.Update(profile);

        await db.SaveChangesAsync();
    }
}