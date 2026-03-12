using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.ViewModels;

namespace PlantApp.Views.AdditionalViews;

public partial class PlantDetailsPage : ContentPage
{
    public PlantDetailsPage(
        Plant plant,
        IDbContextFactory<AppDbContext> factory,
        AuthService authService)
    {
        InitializeComponent();

        BindingContext = new PlantDetailsViewModel(factory, plant, authService);
    }
}