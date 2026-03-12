using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using PlantApp.Views.Popups;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels;

public partial class ProfilePageViewModel : ObservableObject
{
    private readonly UserPlantService _plantService;
    private readonly INavigationService _navigationService;

    // список растений пользователя
    public ObservableCollection<UserPlant> UserPlants { get; set; } = new();

    // путь к фото растения
    private string _imagePath;

    public string ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }

    public ProfilePageViewModel(
        UserPlantService plantService,
        INavigationService navigationService)
    {
        _plantService = plantService; // сохраняем сервис растений
        _navigationService = navigationService;
    }

    // вызывается при открытии страницы
    public async Task LoadProfile()
    {
        await LoadPlants();
    }

    // загрузка растений пользователя
    private async Task LoadPlants()
    {
        var userId = Preferences.Get("UserId", 0);

        var plants = await _plantService.GetUserPlants(userId);

        UserPlants.Clear();

        foreach (var plant in plants)
            UserPlants.Add(plant);
    }

    [RelayCommand]
    async Task AddPlant()
    {
        // пока просто сообщение
        await Application.Current.MainPage.ShowPopupAsync(new AddPlantPopup());
    }

    [RelayCommand]
    async Task PickImage()
    {
        var result = await MediaPicker.PickPhotoAsync();

        if (result == null)
            return;

        var path = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

        using var stream = await result.OpenReadAsync();
        using var fileStream = File.OpenWrite(path);

        await stream.CopyToAsync(fileStream);

        ImagePath = path;
    }

    [RelayCommand]
    async Task Logout()
    {
        Preferences.Remove("UserId");

        await _navigationService.NavigateToAsync<LoginPage>();
    }
}