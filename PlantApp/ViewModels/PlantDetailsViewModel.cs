// Базовые пространства имён
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

// MAUI / UI
using Microsoft.Maui.Controls;

// проект
using PlantApp.Data;
using PlantApp.Models;

namespace PlantApp.ViewModels
{
    public partial class PlantDetailsViewModel : ObservableObject
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public PlantDetailsViewModel(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [RelayCommand]
        public async Task ToggleFavorite(Plant plant)
        {
            using var db = _contextFactory.CreateDbContext();

            var fav = db.FavoritePlants
                .FirstOrDefault(f => f.PlantId == plant.Id);

            if (fav == null)
            {
                db.FavoritePlants.Add(new FavoritePlant
                {
                    PlantId = plant.Id
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