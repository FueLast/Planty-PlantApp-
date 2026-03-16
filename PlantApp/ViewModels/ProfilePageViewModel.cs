using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public ObservableCollection<UserPlant> UserPlants { get; set; } = new();

    //подгружаем для отображения ника, города, возраста и тд
    [ObservableProperty]
    private UserProfile profile;

    private string _imagePath;

    public string ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }

    public ProfilePageViewModel(
        UserPlantService plantService,
        INavigationService navigationService,
        AppDbContext context,
        AuthService authService)
    {
        _plantService = plantService;
        _navigationService = navigationService;
        _context = context;
        _authService = authService;
    }

    public async Task LoadProfile()
    {
        // запускаем обе задачи одновременно
        Task userTask = LoadUserProfile();
        Task plantsTask = LoadPlants();

        // ждем когда обе завершатся
        // это ускорит загрузку экрана в 2 раза
        await Task.WhenAll(userTask, plantsTask);
    }

    private async Task LoadUserProfile()
    {
        var userId = _authService.GetUserId();

        Profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    private async Task LoadPlants()
    {
        var userId = _authService.GetUserId();

        var plants = await _plantService.GetUserPlants(userId);

        UserPlants.Clear();

        foreach (var plant in plants)
            UserPlants.Add(plant);
    }

    [RelayCommand]
    async Task AddPlant()
    {
        var services = Application.Current.Handler.MauiContext.Services;

        var vm = services.GetRequiredService<AddPlantPopupViewModel>();
        var popup = new AddPlantPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(popup);

        var added = await popup.Result;

        if (added)
            await LoadPlants();
    }

    [RelayCommand]
    async Task Logout()
    {
        _authService.Logout();

        await _navigationService.NavigateToAsync<LoginPage>();
    }
}