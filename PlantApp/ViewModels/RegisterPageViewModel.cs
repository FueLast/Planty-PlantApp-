using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using System.Windows.Input;

public partial class RegisterPageViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly SecurityService _securityService;
    private readonly INavigationService _navigationService;
    private readonly AuthService _authService;
    private readonly IConfiguration _config;

    [ObservableProperty]
    private string login;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string city;

    [ObservableProperty]
    private string age;

    public IRelayCommand RegisterCommand { get; }

    public ICommand GoToLoginCommand => new Command(async () =>
    {
        await _navigationService.NavigateToAsync<LoginPage>();
    });

    public RegisterPageViewModel(
        AppDbContext db,
        SecurityService securityService,
        INavigationService navigationService,
        AuthService authService,
        IConfiguration config)
    {
        _db = db;
        _securityService = securityService;
        _navigationService = navigationService;
        _authService = authService;
        _config = config;

        RegisterCommand = new RelayCommand(async () => await Register());
    }

    private async Task Register()
    {
        // проверяем обязательные поля
        if (string.IsNullOrWhiteSpace(Login) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(Username))
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка", "заполните обязательные поля", "ok");
            return;
        }

        if (Password.Length < 6)
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка", "пароль должен содержать минимум 6 символов", "ok");
            return;
        }

        if (Password != ConfirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка", "пароли не совпадают", "ok");
            return;
        }

        // проверяем уникальность логина в SQLite
        var existingUser = await _db.Users
            .FirstOrDefaultAsync(u => u.Login == Login);

        if (existingUser != null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка", "логин уже занят", "ok");
            return;
        }

        // создаем хеш пароля
        _securityService.CreatePasswordHash(Password, out var hash, out var salt);

        int? parsedAge = null;
        if (int.TryParse(Age, out var ageValue))
            parsedAge = ageValue;

        // регистрируем / восстанавливаем пользователя в Supabase Auth
        string supabaseUuid;
        try
        {
            var supabase = new Supabase.Client(
                _config["Supabase:BaseUrl"],
                _config["Supabase:ApiKeyAnonPK"]
            );
            await supabase.InitializeAsync();

            var email = $"{Login.ToLower().Trim()}@plantapp.com";

            Supabase.Gotrue.Session session = null;

            try
            {
                // сначала пробуем зарегистрировать
                session = await supabase.Auth.SignUp(email, Password);

                System.Diagnostics.Debug.WriteLine("SUPABASE: SignUp успешен");
            }
            catch (Exception signUpEx) when (
                signUpEx.Message.Contains("user_already_exists") ||
                signUpEx.Message.Contains("User already registered") ||
                signUpEx.Message.Contains("already registered"))
            {
                // пользователь уже есть в Supabase Auth
                // пробуем войти с теми же данными (восстановление после дропа SQLite)
                System.Diagnostics.Debug.WriteLine(
                    "SUPABASE: пользователь уже существует, пробуем SignIn...");

                try
                {
                    session = await supabase.Auth.SignIn(email, Password);

                    System.Diagnostics.Debug.WriteLine("SUPABASE: SignIn успешен, UUID восстановлен");
                }
                catch (Exception signInEx)
                {
                    // пароль не совпадает → логин занят другим человеком
                    System.Diagnostics.Debug.WriteLine(
                        $"SUPABASE: SignIn failed: {signInEx.Message}");

                    await Application.Current.MainPage.DisplayAlert(
                        "ошибка",
                        "этот логин уже используется. попробуйте другой.",
                        "ok");
                    return;
                }
            }

            if (session?.User == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "ошибка",
                    "не удалось создать аккаунт в Supabase. попробуйте другой логин.",
                    "ok");
                return;
            }

            supabaseUuid = session.User.Id;
            System.Diagnostics.Debug.WriteLine($"SUPABASE UUID: {supabaseUuid}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Supabase signup failed: {ex.Message}");

            string errorMessage;

            if (ex.Message.Contains("over_email_send_rate_limit") ||
                ex.Message.Contains("rate limit"))
            {
                errorMessage = "слишком много регистраций подряд. подождите немного и попробуйте снова.";
            }
            else if (ex.Message.Contains("email_address_invalid"))
            {
                errorMessage = "некорректный формат логина.";
            }
            else if (ex.Message.Contains("weak_password"))
            {
                errorMessage = "пароль слишком слабый. минимум 6 символов.";
            }
            else
            {
                errorMessage = $"ошибка: {ex.Message}";
            }

            await Application.Current.MainPage.DisplayAlert(
                "ошибка регистрации", errorMessage, "ok");
            return;
        }

        // сохраняем пользователя в SQLite
        var random = new Random();

        var user = new User
        {
            Login = Login,
            PasswordHash = hash,
            PasswordSalt = salt,
            SupabaseUuid = supabaseUuid,
            Profile = new UserProfile
            {
                UserName = Username,
                City = City,
                Age = parsedAge,
                AvatarId = random.Next(1, 13)
            }
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _authService.SetUser(user);
        Preferences.Set("user_id", user.Id);

        await Application.Current.MainPage.DisplayAlert(
            "успех", "регистрация выполнена", "ok");

        await _navigationService.NavigateToAsync<MainPage>();
    }
}