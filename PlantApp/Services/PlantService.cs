using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PlantApp.Data;
using PlantApp.Helpers;

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
}