using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel; 

namespace PlantApp.ViewModels;

public partial class AddPlantPopupViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly SupabaseStorageService _storageService;

    private readonly PlantService _plantService;

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
        AuthService authService,
        SupabaseStorageService storageService,
        PlantService plantService)
    {
        _authService = authService;
        _storageService = storageService;
        _plantService = plantService;

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
        try
        {
            if (SelectedPlant == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка", 
                    "Выберите растение", 
                    "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AgeDays))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка",
                    "Введите возраст растения",
                    "OK");
                return;
            }

            var userId = _authService.GetUserId();

            string imageUrl = null;

            if (!string.IsNullOrEmpty(ImagePath))
            {
                using var stream = File.OpenRead(ImagePath);

                imageUrl = await _storageService.UploadPlantImage(stream, userId);
            }

            var plant = new UserPlant
            {
                UserId = userId,
                PlantId = SelectedPlant.Id,
                CustomName = CustomName,
                Description = Description,
                AgeDays = AgeDays,
                ImagePath = imageUrl
            };

            await _plantService.AddPlant(plant);
        }
        catch (Exception ex)
        {
            var message =
                $"MESSAGE: {ex.Message}\n" +
                $"INNER: {ex.InnerException?.Message}\n" +
                $"STACK: {ex.StackTrace}";

            System.Diagnostics.Debug.WriteLine(message);

            await Application.Current.MainPage.DisplayAlert(
                "ERROR",
                message,
                "OK");
        }
    }
}
