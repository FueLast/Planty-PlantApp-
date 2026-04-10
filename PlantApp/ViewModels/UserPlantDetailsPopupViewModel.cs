using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using CommunityToolkit.Maui.Views;

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

    public string CreatedAtText =>
        Plant?.CreatedAt == default
            ? ""
            : $"Добавлено {Plant.CreatedAt:dd.MM.yyyy}";

    public string AgeText =>
        string.IsNullOrWhiteSpace(Plant?.AgeDays)
            ? ""
            : $"Возраст: {Plant.AgeDays} дней";

    [RelayCommand]
    private async Task OpenOwnerProfile()
    {
        if (Owner == null)
            return;

        await _navigation.NavigateToAsync<FriendProfilePage, int>(Owner.UserId);
    }

    [RelayCommand]
    private async Task Close(Popup popup)
    {
        // закрываю попап через toolkit
        await popup.CloseAsync();
    }

    [RelayCommand]
    private async Task OpenChat()
    {
        if (Owner == null)
            return;

        // задел под будущее — сразу норм архитектура
        await _navigation.NavigateToAsync<UserChatPage, int>(Owner.UserId);
    }
}