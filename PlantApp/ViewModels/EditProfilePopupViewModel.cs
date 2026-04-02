using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using System.Collections.ObjectModel;

public partial class EditProfilePopupViewModel : ObservableObject
{
    private readonly ProfileService _profileService;
    private readonly AuthService _authService;
    private readonly INavigationService _navigationService;
     
    private readonly string _baseUrl;
    private readonly string _apiKey;

    private Popup _popup;

    public int ProfileId { get; set; }

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private int selectedAvatarId;

    [ObservableProperty]
    private string bio;

    [ObservableProperty]
    private string avatarUrl;

    public ObservableCollection<int> Avatars { get; } =
        new ObservableCollection<int>(Enumerable.Range(1, 12));

    public EditProfilePopupViewModel(
        ProfileService profileService,
        AuthService authService,
        INavigationService navigationService, 
        IConfiguration config)
    {
        _profileService = profileService;
        _authService = authService;
        _navigationService = navigationService;
         

        _baseUrl = config["Supabase:BaseUrl"];
        _apiKey = config["Supabase:ApiKeyAnonPK"];

        if (string.IsNullOrEmpty(_baseUrl))
            throw new Exception("Supabase BaseUrl is missing");

        if (string.IsNullOrEmpty(_apiKey))
            throw new Exception("Supabase ApiKey is missing");
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
        Bio = profile.Bio;
        AvatarUrl = profile.AvatarUrl;
    }
    // ===================== ХЕЛПЕРЫ =====================
    private Task ShowError(string message)
    {
        return Application.Current.MainPage.DisplayAlert("Ошибка", message, "OK");
    }
    private async Task<string> UploadToSupabase(Stream stream, string fileName)
    {
        try
        {
            var supabase = new Supabase.Client(
                _baseUrl,
                _apiKey
            );

            await supabase.InitializeAsync();

            var bucket = supabase.Storage.From("avatars");

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            var bytes = ms.ToArray();

            var path = fileName; // или "public/" + fileName если есть папка

            await bucket.Upload(bytes, path);

            var url = bucket.GetPublicUrl(path);

            System.Diagnostics.Debug.WriteLine($"UPLOAD SUCCESS: {url}");

            return url;
        }
        catch (Exception ex)
        {
            var fullError =
                $"MESSAGE: {ex.Message}\n" +
                $"INNER: {ex.InnerException?.Message}\n" +
                $"STACK: {ex.StackTrace}";

            System.Diagnostics.Debug.WriteLine(fullError);

            await Application.Current.MainPage.DisplayAlert(
                "UPLOAD ERROR",
                fullError,
                "OK");

            throw;
        }
    }

    // ===================== собственная аватарка =====================
    [RelayCommand]
    private async Task PickPhoto()
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();

            if (result == null)
                return;

            var stream = await result.OpenReadAsync();

            var fileName = $"avatar_{Guid.NewGuid()}.jpg";

            var url = await UploadToSupabase(stream, fileName);

            AvatarUrl = url;

            // если есть кастом аватар → сбрасываем id
            SelectedAvatarId = 0;
        }
        catch (Exception ex)
        {
            await ShowError("Ошибка загрузки фото");
        }
    }

    // ===================== сохранение =====================
    [RelayCommand]
    private async Task Save()
    {
        var name = UserName?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            await ShowError("Имя не может быть пустым");
            return;
        }

        if (name.Length < 2)
        {
            await ShowError("Минимум 2 символа");
            return;
        }

        if (name.Length > 25)
        {
            await ShowError("Максимум 25 символов");
            return;
        }

        if (Bio?.Length > 150)
        {
            await ShowError("Описание слишком длинное (до 150 символов)");
            return;
        }

        var profile = await _profileService.GetProfileById(ProfileId);

        profile.UserName = name;
        profile.AvatarId = SelectedAvatarId;
        profile.Bio = Bio;
        profile.AvatarUrl = AvatarUrl;

        await _profileService.UpdateProfile(profile);

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