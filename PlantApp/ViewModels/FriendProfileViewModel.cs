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

    // ---------------- Профиль ----------------

    [ObservableProperty]
    private UserProfile profile;

    public bool HasBio => !string.IsNullOrWhiteSpace(Profile?.Bio);

    // ---------------- FRIEND STATE ----------------

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

    // ---------------- PLANTS ----------------

    public ObservableCollection<UserPlant> AllPlants { get; set; } = new();
    public ObservableCollection<UserPlant> VisiblePlants { get; set; } = new();

    private int plantsCount = 5;

    public bool CanShowMorePlants => AllPlants.Count > VisiblePlants.Count;

    // ---------------- FRIENDS ----------------

    public ObservableCollection<User> AllFriends { get; set; } = new();
    public ObservableCollection<User> VisibleFriends { get; set; } = new();

    private int friendsCount = 5;

    public bool CanShowMoreFriends => AllFriends.Count > VisibleFriends.Count;

    // ---------------- LOAD ----------------

    public async Task Load(int userId)
    {
        _currentUserId = _authService.GetUserId();
        _friendUserId = userId;

        using var db = await _factory.CreateDbContextAsync();

        // профиль
        Profile = await db.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);

        OnPropertyChanged(nameof(HasBio));

        // ---------------- FRIEND STATUS ----------------

        var friendIds = await _friendService.GetFriendIdsAsync(_currentUserId);

        IsFriend = friendIds.Contains(userId);
        IsRequestSent = IsFriend; // пока упрощенно

        OnPropertyChanged(nameof(FriendButtonText));
        OnPropertyChanged(nameof(FriendButtonColor));
        OnPropertyChanged(nameof(FriendButtonTextColor));

        // ---------------- PLANTS ----------------

        var plants = await _plantService.GetUserPlants(userId); // 👈 ВАЖНО

        AllPlants = new ObservableCollection<UserPlant>(plants);
        UpdatePlants();

        // ---------------- FRIENDS ----------------

        var friends = await _friendService.GetFriendsAsync(userId);

        AllFriends = new ObservableCollection<User>(friends);
        UpdateFriends();
    }

    // ---------------- PLANTS LOGIC ----------------

    [RelayCommand]
    private void ShowMorePlants()
    {
        plantsCount += 5;
        UpdatePlants();
    }

    private void UpdatePlants()
    {
        VisiblePlants = new ObservableCollection<UserPlant>(
            AllPlants.Take(plantsCount));

        OnPropertyChanged(nameof(CanShowMorePlants));
    }

    // ---------------- FRIENDS LOGIC ----------------

    [RelayCommand]
    private void ShowMoreFriends()
    {
        friendsCount += 5;
        UpdateFriends();
    }

    private void UpdateFriends()
    {
        VisibleFriends = new ObservableCollection<User>(
            AllFriends.Take(friendsCount));

        OnPropertyChanged(nameof(CanShowMoreFriends));
    }

    // ---------------- ACTIONS ----------------

    [RelayCommand]
    private async Task AddFriend()
    {
        if (IsFriend)
        {
            // TODO: удалить друга
            return;
        }

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