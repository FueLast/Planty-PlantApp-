using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using System.Windows.Input; 

public partial class RegisterPageViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly SecurityService _securityService;
    private readonly INavigationService _navigationService;

    // основные поля регистрации

    [ObservableProperty]
    private string login;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;

    [ObservableProperty]
    private string username;

    // дополнительные необязательные поля

    [ObservableProperty]
    private string city;

    [ObservableProperty]
    private string age;

    public IRelayCommand RegisterCommand { get; }

    // переход на страницу логина
    public ICommand GoToLoginCommand => new Command(async () =>
    {
        await _navigationService.NavigateToAsync<LoginPage>();
    });

    public RegisterPageViewModel(
        AppDbContext db,
        SecurityService securityService,
        INavigationService navigationService)
    {
        _db = db;
        _securityService = securityService;
        _navigationService = navigationService;

        // команда регистрации
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
                "ошибка",
                "заполните обязательные поля",
                "ok");

            return;
        }

        // проверяем совпадение паролей
        if (Password != ConfirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка",
                "пароли не совпадают",
                "ok");

            return;
        }

        // проверяем уникальность логина
        var existingUser = await _db.Users
            .FirstOrDefaultAsync(u => u.Login == Login);

        if (existingUser != null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка",
                "логин уже занят",
                "ok");

            return;
        }

        // создаем хеш пароля
        _securityService.CreatePasswordHash(Password, out var hash, out var salt);

        // пытаемся распарсить возраст
        int? parsedAge = null;
        if (int.TryParse(Age, out var ageValue))
        {
            parsedAge = ageValue;
        }
          

        //для рандом аватарки
        var random = new Random();

        var user = new User        // создаем нового пользователя
        {
            Login = Login,
            PasswordHash = hash,
            PasswordSalt = salt,
            // Создаем вложенный объект профиля
            Profile = new UserProfile
            {
                UserName = Username, // Теперь эти данные попадают по адресу
                City = City,
                Age = parsedAge,
                AvatarId = random.Next(1, 13) // <-- сюда
            }
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
         

        await Application.Current.MainPage.DisplayAlert(
            "успех",
            "регистрация выполнена",
            "ok");

        // после регистрации переходим на главную страницу
        await _navigationService.NavigateToAsync<MainPage>();
    }
}