using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel;
using PlantApp.Views;

namespace PlantApp.ViewModels;

public partial class AddFriendViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly FriendService _friendService;

    private int _currentUserId;

    public ObservableCollection<User> Users { get; set; } = new();

    [ObservableProperty]
    private string searchText;

    private readonly IServiceProvider _serviceProvider;

    public AddFriendViewModel(
        UserService userService,
        FriendService friendService,
        IServiceProvider serviceProvider)
    {
        _userService = userService;
        _friendService = friendService;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private async Task OpenProfile(User user)
    {
        var page = _serviceProvider.GetRequiredService<FriendProfilePage>();

        if (page.BindingContext is FriendProfileViewModel vm)
            await vm.Load(user.Id);

        await Application.Current.MainPage.Navigation.PushAsync(page);
    }

    public void Init(int currentUserId)
    {
        _currentUserId = currentUserId;
    }


    [RelayCommand]
    private async Task Search()
    {
        var result = await _userService.SearchUsersAsync(SearchText);

        var friendIds = await _friendService.GetFriendIdsAsync(_currentUserId);

        Users.Clear();

        foreach (var user in result)
        {
            if (user.Id == _currentUserId)
                continue;

            // если уже друг → сразу галочка
            user.IsRequestSent = friendIds.Contains(user.Id);

            Users.Add(user);
        }
    }


    [RelayCommand]
    private async Task AddFriend(User user)
    {
        await _friendService.SendRequestAsync(_currentUserId, user.Id);

        user.IsRequestSent = true;

        // перерисовка конкретного элемента
        var index = Users.IndexOf(user);
        if (index >= 0)
        {
            Users.RemoveAt(index);
            Users.Insert(index, user);
        }
    }
}