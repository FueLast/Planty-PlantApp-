using PlantApp.Views;
using PlantApp.Services;
using PlantApp.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace PlantApp.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly AppDbContext _db;
        private readonly SecurityService _securityService;
        private readonly AuthService _authService;

        public LoginPageViewModel(
            INavigationService navigationService,
            AppDbContext db,
            SecurityService securityService,
            AuthService authService)
        {
            _navigationService = navigationService;
            _db = db;
            _securityService = securityService;
            _authService = authService;

            LoginCommand = new RelayCommand(async () => await Logins());
        }

        // поля для биндинга из xaml

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        public IRelayCommand LoginCommand { get; }

        // переход на регистрацию
        public ICommand GoToRegisterCommand => new Command(async () =>
        {
            await _navigationService.NavigateToAsync<RegisterPage>();
        });

        private async Task Logins()
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка",
                    "Введите логин и пароль",
                    "OK");

                return;
            }

            // ищем пользователя
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Login == Login);

            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка",
                    "Пользователь не найден",
                    "OK");

                return;
            }

            // проверяем пароль
            var passwordValid = _securityService.VerifyPasswordHash(
                Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!passwordValid)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка",
                    "Неверный пароль",
                    "OK");

                return;
            }

            // сохраняем пользователя в AuthService
            _authService.SetUser(user);

            await Application.Current.MainPage.DisplayAlert(
                "Успех",
                "Вход выполнен",
                "OK");

            await _navigationService.NavigateToAsync<MainPage>();
        }
    }
}