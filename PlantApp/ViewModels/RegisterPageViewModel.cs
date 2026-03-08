using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Views;
using PlantApp.ViewModels;
using PlantApp.Services;

public partial class RegisterPageViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly SecurityService _securityService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string login;

    [ObservableProperty]
    private string password;

    public IRelayCommand RegisterCommand { get; } 

    public RegisterPageViewModel(
        AppDbContext db,
        SecurityService securityService,
        INavigationService navigationService)
    {
        _db = db;
        _securityService = securityService;
        _navigationService = navigationService;

        RegisterCommand = new RelayCommand(async () => await Register());
    }

    private async Task Register()
    {
        var existingUser = await _db.Users
            .FirstOrDefaultAsync(u => u.Login == login);

        if (existingUser != null)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Логин уже занят", "OK");
            return;
        }

        _securityService.CreatePasswordHash(password, out var hash, out var salt);

        var user = new User
        {
            Login = login,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        await Application.Current.MainPage.DisplayAlert("Успех", "Регистрация выполнена", "OK");

        await _navigationService.NavigateToAsync<MainPage>();
    }
}