using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel; 

namespace PlantApp.ViewModels;

public partial class AddPlantPopupViewModel : ObservableObject
{
    private readonly UserPlantService _plantService;
    private readonly AuthService _authService;

    // список растений для Picker
    public ObservableCollection<Plant> Plants { get; set; } = new();

    [ObservableProperty]
    private Plant selectedPlant;

    [ObservableProperty]
    private string customName;

    [ObservableProperty]
    private string ageDays;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private string imagePath;

    public AddPlantPopupViewModel(
        UserPlantService plantService,
        AuthService authService)
    {
        _plantService = plantService;
        _authService = authService;

        LoadPlants();
    }

    async Task LoadPlants()
    {
        var list = await _plantService.GetPlants();
        Plants.Clear();
        foreach (var pl in list)
            Plants.Add(pl);
    }

    // команда выбора фото (MediaPicker)
    [RelayCommand]
    async Task PickImage()
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result == null) return;

        var path = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
        using var stream = await result.OpenReadAsync();
        using var fileStream = File.OpenWrite(path);
        await stream.CopyToAsync(fileStream);

        ImagePath = path; // сохраняем путь к фото
    }

    // метод сохранения UserPlant
    public async Task SavePlant()
    {
        if (SelectedPlant == null)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Выберите растение", "OK");
            return;
        }

        var userId = _authService.GetUserId();

        var plant = new UserPlant
        {
            UserId = userId,
            PlantId = SelectedPlant.Id,
            CustomName = CustomName,
            AgeDays = AgeDays,
            Description = Description,
            ImagePath = ImagePath
        };

        await _plantService.AddPlant(plant);
    }
}
