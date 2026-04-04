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

    public ObservableCollection<UserPlant> UserPlants { get; } = new();
    public ObservableCollection<User> Friends { get; } = new();

    public ObservableCollection<UserPlant> VisiblePlants { get; } = new();
    public ObservableCollection<User> VisibleFriends { get; } = new();

    [ObservableProperty]
    private UserProfile profile;

    [ObservableProperty]
    private int plantsCount;

    [ObservableProperty]
    private int friendsCount;

    private bool _isPlantsExpanded;
    public bool IsPlantsExpanded
    {
        get => _isPlantsExpanded;
        set
        {
            if (SetProperty(ref _isPlantsExpanded, value))
                UpdatePlantsView();
        }
    }

    private bool _isFriendsExpanded;
    public bool IsFriendsExpanded
    {
        get => _isFriendsExpanded;
        set
        {
            if (SetProperty(ref _isFriendsExpanded, value))
                UpdateFriendsView();
        }
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

    // ===================== прогрузка =====================
    public async Task LoadProfile()
    {
        var userId = _authService.GetUserId();

        var userTask = LoadUserProfile(userId);
        var plantsTask = LoadPlants(userId);
        var friendsTask = LoadFriends(userId);

        await _authService.SetOnline(userId);
        await Task.WhenAll(userTask, plantsTask, friendsTask);
    }

    private async Task LoadUserProfile(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        Profile = await db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    private async Task LoadPlants(int userId)
    {
        var plants = await _plantService.GetUserPlants(userId);

        UserPlants.Clear();
        foreach (var p in plants)
            UserPlants.Add(p);

        PlantsCount = UserPlants.Count;

        UpdatePlantsView();
    }

    private async Task LoadFriends(int userId)
    {
        var list = await _friendService.GetFriendsAsync(userId);

        Friends.Clear();
        foreach (var f in list)
            Friends.Add(f);

        FriendsCount = Friends.Count;

        UpdateFriendsView();
    }

    // ===================== view логика =====================
    private void UpdatePlantsView()
    {
        VisiblePlants.Clear();

        var count = IsPlantsExpanded ? 20 : 5;

        foreach (var p in UserPlants.Take(count))
            VisiblePlants.Add(p);
    }

    private void UpdateFriendsView()
    {
        VisibleFriends.Clear();

        var count = IsFriendsExpanded ? 20 : 5;

        foreach (var f in Friends.Take(count))
            VisibleFriends.Add(f);
    }

    // ===================== комманды =====================
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

    [RelayCommand]
    private async Task OpenAddFriend()
    {
        var page = _serviceProvider.GetRequiredService<AddFriendPage>();

        if (page.BindingContext is AddFriendViewModel vm)
            vm.Init(_authService.GetUserId());

        await Application.Current.MainPage.Navigation.PushAsync(page);

        await LoadFriends(_authService.GetUserId());
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
            await LoadPlants(_authService.GetUserId());
    }

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