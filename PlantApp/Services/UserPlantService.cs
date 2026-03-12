using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

public class UserPlantService
{
    private readonly AppDbContext _context;

    public UserPlantService(AppDbContext context)
    {
        _context = context;
    } 
    public async Task<List<UserPlant>> GetUserPlants(int userId)
    { 

        return await _context.UserPlants
            .Where(p => p.UserId == userId)
            .Include(p => p.Plant)
            .ToListAsync();
    }

    public async Task AddPlant(UserPlant plant)
    { 

        _context.UserPlants.Add(plant);

        await _context.SaveChangesAsync();
    }

     
}