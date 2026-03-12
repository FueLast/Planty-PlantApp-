using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services
{
    public class PlantService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public PlantService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Plant>> GetPlantsAsync()
        {
            using var db = _factory.CreateDbContext();
            return await db.Plants.ToListAsync();
        }
    }
}