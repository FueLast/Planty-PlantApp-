using CommunityToolkit.Mvvm.ComponentModel;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views.AdditionalViews;
using System.Collections.ObjectModel;

public partial class EncyclopediaViewModel : ObservableObject
{
    private readonly PlantService _plantService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<Plant> plants = new();

    public EncyclopediaViewModel(
        PlantService plantService,
        INavigationService navigationService)
    {
        _plantService = plantService;
        _navigationService = navigationService;
    }

    public async Task LoadPlantsAsync()
    {
        var items = await _plantService.GetPlantsAsync();
        Plants = new ObservableCollection<Plant>(items);
    }

    public async Task GoBack()
    {
        await _navigationService.NavigateToAsync<PlantDetailsPage>();
    }

}
