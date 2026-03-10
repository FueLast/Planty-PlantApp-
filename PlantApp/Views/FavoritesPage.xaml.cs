using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesPageViewModel _viewModel;

    public FavoritesPage(FavoritesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadFavorites();
    }
}