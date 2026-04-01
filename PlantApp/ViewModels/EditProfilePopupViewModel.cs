using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using System.Collections.ObjectModel;

public partial class EditProfilePopupViewModel : ObservableObject
{
    private readonly ProfileService _profileService;
    private readonly AuthService _authService;
    private readonly INavigationService _navigationService;

    private Popup _popup;

    public int ProfileId { get; set; }

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private int selectedAvatarId;

    public ObservableCollection<int> Avatars { get; } =
        new ObservableCollection<int>(Enumerable.Range(1, 12));

    public EditProfilePopupViewModel(
        ProfileService profileService,
        AuthService authService,
        INavigationService navigationService)
    {
        _profileService = profileService;
        _authService = authService;
        _navigationService = navigationService;
    }

    public void SetPopup(Popup popup)
    {
        _popup = popup;
    }

    public void Init(UserProfile profile)
    {
        ProfileId = profile.Id;
        UserName = profile.UserName;
        SelectedAvatarId = profile.AvatarId;
    }

    // ===================== сохранение =====================
    [RelayCommand]
    private async Task Save()
    {
        //валидация
        if (string.IsNullOrWhiteSpace(UserName))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                "Имя пользователя не может быть пустым",
                "ОК");

            return;
        }

        // можно добавить доп. нормализацию
        var trimmedName = UserName.Trim();

        if (trimmedName.Length < 2)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                "Имя должно содержать минимум 2 символа",
                "ОК");

            return;
        }

        // загрузка профиля
        var profile = await _profileService.GetProfileById(ProfileId);

        profile.UserName = trimmedName;
        profile.AvatarId = SelectedAvatarId;

        await _profileService.UpdateProfile(profile);

        // обновляем UI сразу
        ProfileUpdated?.Invoke(profile);

        await _popup.CloseAsync();
    }

    // ===================== отмена =====================
    [RelayCommand]
    private async Task Cancel()
    {
        await _popup.CloseAsync();
    }

    // ===================== выход =====================
    [RelayCommand]
    private async Task Logout()
    {
        _authService.Logout();

        await _popup.CloseAsync();

        await _navigationService.NavigateToAsync<LoginPage>();
    }

    //событие для обновления страницы
    public static Action<UserProfile>? ProfileUpdated;
}