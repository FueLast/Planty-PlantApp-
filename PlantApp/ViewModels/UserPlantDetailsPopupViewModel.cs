using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Views;
using PlantApp.Services;

public partial class UserPlantDetailsPopupViewModel : ObservableObject
{
    private readonly INavigationService _navigation;

    public UserPlantDetailsPopupViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }

    [ObservableProperty]
    private UserPlant plant;

    [ObservableProperty]
    private UserProfile owner;

    // текст "добавлено ..."
    public string CreatedAtText =>
        Plant?.CreatedAt == default
            ? ""
            : $"добавлено {Plant.CreatedAt:dd.MM.yyyy}";

    [RelayCommand]
    private async Task OpenOwnerProfile()
    {
        if (Owner == null)
            return;

        // сразу навигация на профиль
        await _navigation.NavigateToAsync<FriendProfilePage, int>(Owner.UserId);
    }
}