using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

public class UserPlantService
{
    private readonly AppDbContext _context;

    public UserPlantService(AppDbContext context)
    {
        _context = context;
    }

    // растения пользователя
    public async Task<List<UserPlant>> GetUserPlants(int userId)
    {
        return await _context.UserPlants
            .Where(p => p.UserId == userId)
            .Include(p => p.Plant)
            .ToListAsync();
    }

    // растения из энциклопедии
    public async Task<List<Plant>> GetPlants()
    {
        return await _context.Plants.ToListAsync();
    }

    // добавление растения
    public async Task AddPlant(UserPlant plant)
    {
        _context.UserPlants.Add(plant);
        await _context.SaveChangesAsync();
    }
}