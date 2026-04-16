using Microsoft.EntityFrameworkCore;

namespace PlantApp.Data
{
    public class FavoriteService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public FavoriteService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task AddToFavorites(int plantId, int userId)
        {
            using var db = await _factory.CreateDbContextAsync();

            var exists = await db.FavoritePlants
                .FirstOrDefaultAsync(f => f.PlantId == plantId && f.UserId == userId);

            if (exists != null)
                return;

            var favorite = new FavoritePlant
            {
                PlantId = plantId,
                UserId = userId
            };

            db.FavoritePlants.Add(favorite);
            await db.SaveChangesAsync();
        }

        public async Task RemoveFromFavorites(int plantId, int userId)
        {
            using var db = await _factory.CreateDbContextAsync();

            var favorite = await db.FavoritePlants
                .FirstOrDefaultAsync(f => f.PlantId == plantId && f.UserId == userId);

            if (favorite != null)
            {
                db.FavoritePlants.Remove(favorite);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Plant>> GetFavorites(int userId)
        {
            using var db = await _factory.CreateDbContextAsync();

            return await db.FavoritePlants
                .Where(f => f.UserId == userId)
                .Include(f => f.Plant)
                .Select(f => f.Plant)
                .ToListAsync();
        }
    }
}