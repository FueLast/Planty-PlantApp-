using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using PlantApp.Data;
using PlantApp.Services;

public partial class EncyclopediaViewModel : ObservableObject
{
    private readonly PlantService _plantService;

    [ObservableProperty]
    private ObservableCollection<Plant> plants = new();

    public EncyclopediaViewModel(PlantService plantService)
    {
        _plantService = plantService;
    }

    public async Task LoadPlantsAsync()
    {
        var items = await _plantService.GetPlantsAsync();
        Plants = new ObservableCollection<Plant>(items);
    }
}
