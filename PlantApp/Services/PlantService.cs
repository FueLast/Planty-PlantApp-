using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Helpers;

namespace PlantApp.Services;

public class PlantService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public PlantService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    // получить ВСЕ растения (каталог)
    public async Task<List<Plant>> GetPlants()
    {
        using var db = await _factory.CreateDbContextAsync();

        return await db.Plants
            .OrderBy(x => x.NamePlant)
            .ToListAsync();
    }

    //получить растения пользователя
    public async Task<List<UserPlant>> GetUserPlants(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        return await db.UserPlants
            .Include(x => x.Plant)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    // добавить растение пользователю
    public async Task AddPlant(UserPlant plant)
    {
        using var db = await _factory.CreateDbContextAsync();

        db.UserPlants.Add(plant);
        await db.SaveChangesAsync();
    }

    public async Task<List<Plant>> GetPlantsAsync()
    {
        using var db = _factory.CreateDbContext();
        return await db.Plants.ToListAsync();
    }

    public async Task<List<Plant>> SearchPlants(string query)
    {
        using var db = _factory.CreateDbContext();

        var plants = await db.Plants.ToListAsync();

        var result = plants
            .Select(p => new
            {
                Plant = p,
                Score = SearchHelper.Compare(p.SearchNames, query)
            })
            .Where(x => x.Score > 0.2) // порог похожести
            .OrderByDescending(x => x.Score)
            .Select(x => x.Plant)
            .ToList();

        return result;
    }
        
}