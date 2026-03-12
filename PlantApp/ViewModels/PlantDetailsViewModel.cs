using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;

public partial class PlantDetailsViewModel : ObservableObject
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly AuthService _authService;

    public Plant Plant { get; }
    private readonly int _plantId;

    public PlantDetailsViewModel(
        IDbContextFactory<AppDbContext> factory,
        Plant plant,
        AuthService authService)
    {
        _factory = factory;
        _authService = authService;

        Plant = plant;
        _plantId = plant.Id;   // сохраняем ID
    }

    [RelayCommand]
    public async Task ToggleFavorite()
    {
        using var db = _factory.CreateDbContext();

        int currentUserId = _authService.GetUserId();

        var favorite = await db.FavoritePlants
            .FirstOrDefaultAsync(f =>
                f.PlantId == _plantId &&
                f.UserId == currentUserId);

        if (favorite != null)
        {
            db.FavoritePlants.Remove(favorite);
        }
        else
        {
            var newFavorite = new FavoritePlant
            {
                PlantId = _plantId,
                UserId = currentUserId
            };

            db.FavoritePlants.Add(newFavorite);
        }

        await db.SaveChangesAsync();
    }
}