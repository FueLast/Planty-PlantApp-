using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using PlantApp.Views.AdditionalViews;
using PlantApp.Views.Popups;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels;

public partial class ProfilePageViewModel : ObservableObject
{
    private readonly UserPlantService _plantService;
    private readonly INavigationService _navigationService;
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly AuthService _authService;
    private readonly FriendService _friendService;
    private readonly IServiceProvider _serviceProvider;

    public ObservableCollection<UserPlant> UserPlants { get; set; } = new();
    public ObservableCollection<User> Friends { get; set; } = new();

    [ObservableProperty]
    private UserProfile profile;

    public ProfilePageViewModel(
        UserPlantService plantService,
        INavigationService navigationService,
        IDbContextFactory<AppDbContext> factory,
        AuthService authService,
        FriendService friendService,
        IServiceProvider serviceProvider)
    {
        _plantService = plantService;
        _navigationService = navigationService;
        _factory = factory;
        _authService = authService;
        _friendService = friendService;
        _serviceProvider = serviceProvider;
    }

    // ===================== ГЛАВНАЯ ЗАГРУЗКА =====================
    public async Task LoadProfile()
    {
        var userId = _authService.GetUserId();

        var userTask = LoadUserProfile(userId);
        var plantsTask = LoadPlants(userId);
        var friendsTask = LoadFriends(userId);

        await Task.WhenAll(userTask, plantsTask, friendsTask);
    }

    // ===================== ПРОФИЛЬ =====================
    private async Task LoadUserProfile(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        Profile = await db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    // ===================== РАСТЕНИЯ =====================
    private async Task LoadPlants(int userId)
    {
        var plants = await _plantService.GetUserPlants(userId);

        UserPlants.Clear();

        foreach (var plant in plants)
            UserPlants.Add(plant);
    }

    // ===================== ДРУЗЬЯ =====================
    private async Task LoadFriends(int userId)
    {
        var list = await _friendService.GetFriendsAsync(userId);

        Friends.Clear();

        foreach (var f in list)
            Friends.Add(f);
    }

    // Открыть прфоиль друга
    [RelayCommand]
    private async Task OpenProfile(User user)
    {
        if (user == null)
            return;

        var page = _serviceProvider.GetRequiredService<FriendProfilePage>();

        if (page.BindingContext is FriendProfileViewModel vm)
            await vm.Load(user.Id);

        await Application.Current.MainPage.Navigation.PushAsync(page);
    }

    // ===================== ОТКРЫТЬ ДОБАВЛЕНИЕ =====================
    [RelayCommand]
    private async Task OpenAddFriend()
    {
        var page = _serviceProvider.GetRequiredService<AddFriendPage>();

        if (page.BindingContext is AddFriendViewModel vm)
            vm.Init(_authService.GetUserId());

        await Application.Current.MainPage.Navigation.PushAsync(page);

        //ВАЖНО после возврата обновляем список
        await LoadFriends(_authService.GetUserId());
    }

    // ===================== ДОБАВИТЬ РАСТЕНИЕ =====================
    [RelayCommand]
    async Task AddPlant()
    {
        var services = Application.Current.Handler.MauiContext.Services;

        var vm = services.GetRequiredService<AddPlantPopupViewModel>();
        var popup = new AddPlantPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(popup);

        var added = await popup.Result;

        if (added)
            await LoadPlants(_authService.GetUserId());
    }

    // ===================== НАСТРОЙКИ ПРОФИЛЯ =====================
    [RelayCommand]
    private async Task OpenEditProfile()
    {
        var page = _serviceProvider.GetRequiredService<EditProfilePopup>();

        if (page.BindingContext is EditProfilePopupViewModel vm)
            vm.Init(Profile);

        await Application.Current.MainPage.ShowPopupAsync(page);
    }

    private void OnProfileUpdated(UserProfile updatedProfile)
    {
        Profile.UserName = updatedProfile.UserName;
        Profile.AvatarId = updatedProfile.AvatarId;

        OnPropertyChanged(nameof(Profile));
    }

}