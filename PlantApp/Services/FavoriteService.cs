using Microsoft.EntityFrameworkCore;

namespace PlantApp.Data
{
    public class FavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToFavorites(int plantId, int userId)
        {
            var exists = await _context.FavoritePlants
                .FirstOrDefaultAsync(f => f.PlantId == plantId && f.UserId == userId);

            if (exists != null)
                return;

            var favorite = new FavoritePlant
            {
                PlantId = plantId,
                UserId = userId
            };

            _context.FavoritePlants.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromFavorites(int plantId, int userId)
        {
            var favorite = await _context.FavoritePlants
                .FirstOrDefaultAsync(f => f.PlantId == plantId && f.UserId == userId);

            if (favorite != null)
            {
                _context.FavoritePlants.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Plant>> GetFavorites(int userId)
        {
            return await _context.FavoritePlants
                .Where(f => f.UserId == userId)
                .Include(f => f.Plant)
                .Select(f => f.Plant)
                .ToListAsync();
        }
    }
}