using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.ViewModels
{
    public partial class PlantDetailsViewModel : ObservableObject
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        // Удаляем ручное объявление ToggleFavoriteCommand из конструктора и свойств

        public PlantDetailsViewModel(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // Атрибут сам создаст ToggleFavoriteCommand (тип IAsyncRelayCommand<Plant>)
        [RelayCommand]
        public async Task ToggleFavorite(Plant plant)
        {
            if (plant == null) return;

            using var db = _contextFactory.CreateDbContext();

            // Важно: нужно учитывать UserId, иначе вы не поймете, чье это избранное
            // Для примера используем заглушку или ваш сервис авторизации
            int currentUserId = 1;

            var fav = await db.FavoritePlants
                .FirstOrDefaultAsync(f => f.PlantId == plant.Id && f.UserId == currentUserId);

            if (fav == null)
            {
                db.FavoritePlants.Add(new FavoritePlant
                {
                    PlantId = plant.Id,
                    UserId = currentUserId // Не забудьте привязать к пользователю
                });
            }
            else
            {
                db.FavoritePlants.Remove(fav);
            }

            await db.SaveChangesAsync();
        }
    }
}
