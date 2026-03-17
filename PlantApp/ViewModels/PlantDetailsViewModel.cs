using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using System.Globalization;

public partial class PlantDetailsViewModel : ObservableObject, IInitialize<Plant>
{
    [ObservableProperty] private Plant plant;
    [ObservableProperty] private bool isFavorite;

    private int _plantId;
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly AuthService _authService;

    public string FavoriteIcon => IsFavorite ? "star_filled.png" : "star_outline.png";
    public PlantDetailsViewModel(IDbContextFactory<AppDbContext> factory, AuthService authService)
    {
        _factory = factory;
        _authService = authService;
    }

    public async void Initialize(Plant plant)
    {
        Plant = plant;
        _plantId = plant.Id;

        using var db = _factory.CreateDbContext();
        int userId = _authService.GetUserId();

        IsFavorite = await db.FavoritePlants
            .AnyAsync(f => f.PlantId == _plantId && f.UserId == userId);
    }

    [RelayCommand]
    public async Task ToggleFavorite()
    {
        using var db = _factory.CreateDbContext();
        int userId = _authService.GetUserId();

        var favorite = await db.FavoritePlants
            .FirstOrDefaultAsync(f => f.PlantId == _plantId && f.UserId == userId);

        if (favorite != null)
        {
            db.FavoritePlants.Remove(favorite);
            IsFavorite = false;
        }
        else
        {
            db.FavoritePlants.Add(new FavoritePlant
            {
                PlantId = _plantId,
                UserId = userId
            });
            IsFavorite = true;
        }

        await db.SaveChangesAsync();
    }

    partial void OnIsFavoriteChanged(bool value)
    {
        OnPropertyChanged(nameof(FavoriteIcon));
    }

    public class BoolToStarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? "⭐" : "☆";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}