using System.Collections.ObjectModel;
using PlantApp.Data;
using PlantApp.Services;

namespace PlantApp.ViewModels
{
    public class FavoritesPageViewModel
    {
        private readonly FavoriteService _favoriteService;
        private readonly AuthService _authService;

        public ObservableCollection<Plant> FavoritePlants { get; } = new();

        public FavoritesPageViewModel(
            FavoriteService favoriteService,
            AuthService authService)
        {
            _favoriteService = favoriteService;
            _authService = authService;
        }

        public async Task LoadFavorites()
        {
            FavoritePlants.Clear();

            int userId = _authService.GetUserId();

            var plants = await _favoriteService.GetFavorites(userId);

            foreach (var plant in plants)
                FavoritePlants.Add(plant);
        }
    }
}