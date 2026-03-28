using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services;

public class UserService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public UserService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<User>> SearchUsersAsync(string query)
    {
        using var db = await _factory.CreateDbContextAsync();

        if (string.IsNullOrWhiteSpace(query))
            return new List<User>();

        return await db.Users
            .Include(u => u.Profile) 
            .Where(x => x.Login.ToLower().Contains(query.ToLower()))
            .Take(20)
            .ToListAsync();
    }
}