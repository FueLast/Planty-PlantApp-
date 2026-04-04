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

    public ObservableCollection<UserPlant> PreviewPlants { get; } = new();
    public ObservableCollection<UserPlant> ExpandedPlants { get; } = new();

    public ObservableCollection<User> PreviewFriends { get; } = new();
    public ObservableCollection<User> ExpandedFriends { get; } = new();

    public ObservableCollection<UserPlant> UserPlants { get; } = new();
    public ObservableCollection<User> Friends { get; } = new();

    [ObservableProperty]
    private UserProfile profile;

    [ObservableProperty]
    private int plantsCount;

    [ObservableProperty]
    private int friendsCount;

    //шторки друзей и растений
    private bool _isPlantsExpanded;
    public bool IsPlantsExpanded
    {
        get => _isPlantsExpanded;
        set => SetProperty(ref _isPlantsExpanded, value);
    }

    private bool _isFriendsExpanded;
    public bool IsFriendsExpanded
    {
        get => _isFriendsExpanded;
        set => SetProperty(ref _isFriendsExpanded, value);
    }

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
         
        EditProfilePopupViewModel.ProfileUpdated += OnProfileUpdated;
    }

    // ===================== ГЛАВНАЯ ЗАГРУЗКА =====================
    public async Task LoadProfile()
    {
        var userId = _authService.GetUserId();

        var userTask = LoadUserProfile(userId);
        var plantsTask = LoadPlants(userId);
        var friendsTask = LoadFriends(userId);

        await _authService.SetOnline(userId);
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
        foreach (var p in plants)
            UserPlants.Add(p);

        PreviewPlants.Clear();
        ExpandedPlants.Clear();

        foreach (var p in UserPlants.Take(5))
            PreviewPlants.Add(p);

        foreach (var p in UserPlants.Take(20))
            ExpandedPlants.Add(p);

        PlantsCount = UserPlants.Count;
    }

    // ===================== ДРУЗЬЯ =====================
    private async Task LoadFriends(int userId)
    {
        var list = await _friendService.GetFriendsAsync(userId);

        Friends.Clear();
        foreach (var f in list)
            Friends.Add(f);

        PreviewFriends.Clear();
        ExpandedFriends.Clear();

        foreach (var f in Friends.Take(5))
            PreviewFriends.Add(f);

        foreach (var f in Friends.Take(20))
            ExpandedFriends.Add(f);

        FriendsCount = Friends.Count;
    }

    // ===================== ШТОРКИ ДРУЗЕЙ И РАСТЕНИЙ =====================
    [RelayCommand]
    private void TogglePlants()
    {
        IsPlantsExpanded = !IsPlantsExpanded;
    }

    [RelayCommand]
    private void ToggleFriends()
    {
        IsFriendsExpanded = !IsFriendsExpanded;
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

    private void OnProfileUpdated(UserProfile updated)
    {
        Profile.UserName = updated.UserName;
        Profile.AvatarId = updated.AvatarId;
        Profile.AvatarUrl = updated.AvatarUrl;
        Profile.Bio = updated.Bio;

        OnPropertyChanged(nameof(Profile));
    }

}