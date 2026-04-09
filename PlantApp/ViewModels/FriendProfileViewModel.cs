using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using System.Collections.ObjectModel;

public partial class FriendProfileViewModel : ObservableObject
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly FriendService _friendService;
    private readonly AuthService _authService;
    private readonly INavigationService _navigation;
    private readonly UserPlantService _plantService;

    private int _currentUserId;
    private int _friendUserId;

    public FriendProfileViewModel(
        IDbContextFactory<AppDbContext> factory,
        FriendService friendService,
        AuthService authService,
        INavigationService navigation,
        UserPlantService plantService)
    {
        _factory = factory;
        _friendService = friendService;
        _authService = authService;
        _navigation = navigation;
        _plantService = plantService;
    }

    // ---------------- профиль ----------------

    [ObservableProperty]
    private UserProfile profile;

    [ObservableProperty]
    private bool isLoading;

    public bool HasBio => !string.IsNullOrWhiteSpace(Profile?.Bio);

    // ---------------- состояние дружбы ----------------

    [ObservableProperty]
    private bool isFriend;

    [ObservableProperty]
    private bool isRequestSent;

    public string FriendButtonText =>
        IsFriend ? "Удалить" :
        IsRequestSent ? "Добавлен" : "Добавить";

    public Color FriendButtonColor =>
        IsFriend ? Colors.White :
        IsRequestSent ? Colors.White :
        Color.FromArgb("#8CD98C");

    public Color FriendButtonTextColor =>
        IsFriend ? Colors.Red :
        Colors.DarkGreen;

    // ---------------- растения ----------------

    public ObservableCollection<UserPlant> AllPlants { get; } = new();

    public ObservableCollection<UserPlant> VisiblePlants { get; } = new();

    private int plantsCount = 5;

    public bool CanShowMorePlants => AllPlants.Count > VisiblePlants.Count;

    // ---------------- друзья ----------------

    public ObservableCollection<User> AllFriends { get; } = new();

    public ObservableCollection<User> VisibleFriends { get; } = new();

    private int friendsCount = 5;

    public bool CanShowMoreFriends => AllFriends.Count > VisibleFriends.Count;

    // ---------------- загрузка ----------------

    public async Task Load(int userId)
    {
        IsLoading = true;

        try
        {
            _currentUserId = _authService.GetUserId();
            _friendUserId = userId;

            using var db = await _factory.CreateDbContextAsync();

            Profile = await db.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            OnPropertyChanged(nameof(HasBio));

            var friendIds = await _friendService.GetFriendIdsAsync(_currentUserId);

            IsFriend = friendIds.Contains(userId);
            IsRequestSent = IsFriend;

            var plants = await _plantService.GetUserPlants(userId);

            AllPlants.Clear();
            foreach (var p in plants)
                AllPlants.Add(p);

            UpdatePlants();

            var friends = await _friendService.GetFriendsAsync(userId);

            AllFriends.Clear();
            foreach (var f in friends)
                AllFriends.Add(f);

            UpdateFriends();
        }
        finally
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsLoading = false;
            });
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdatePlants();
            UpdateFriends();
        });
    }

    // ---------------- логика растений ----------------

    [RelayCommand]
    private void ShowMorePlants()
    {
        plantsCount += 5;
        UpdatePlants();
    }

    private void UpdatePlants()
    {
        VisiblePlants.Clear();

        foreach (var plant in AllPlants.Take(plantsCount))
            VisiblePlants.Add(plant);

        OnPropertyChanged(nameof(CanShowMorePlants));
    }

    // ---------------- логика друзей ----------------

    [RelayCommand]
    private void ShowMoreFriends()
    {
        friendsCount += 5;
        UpdateFriends();
    }

    private void UpdateFriends()
    {
        VisibleFriends.Clear();

        foreach (var friend in AllFriends.Take(friendsCount))
            VisibleFriends.Add(friend);

        OnPropertyChanged(nameof(CanShowMoreFriends));
    }

    // ---------------- действия ----------------

    [RelayCommand]
    private async Task AddFriend()
    {
        if (IsFriend)
            return;

        await _friendService.SendRequestAsync(_currentUserId, _friendUserId);

        IsRequestSent = true;

        OnPropertyChanged(nameof(FriendButtonText));
        OnPropertyChanged(nameof(FriendButtonColor));
    }

    [RelayCommand]
    private async Task OpenChat()
    {
        await _navigation.NavigateToAsync<UserChatPage, int>(_friendUserId);
    }

    [RelayCommand]
    private async Task OpenFriendProfile(User user)
    {
        await _navigation.NavigateToAsync<FriendProfilePage, int>(user.Id);
    }
}