using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CreateSwapOfferPopupViewModel : ObservableObject
{
    private readonly ISwapService _swapService;
    private readonly AuthService _authService;

    public Action? CloseAction;

    [ObservableProperty]
    private string desiredText;

    [ObservableProperty]
    private string imagePreview;

    public ObservableCollection<UserPlant> MyPlants { get; } = new();

    [ObservableProperty]
    private UserPlant selectedPlant;

    public ObservableCollection<Plant> Plants { get; } = new();
     
    private readonly PlantService _plantService;

    public bool HasImage => !string.IsNullOrEmpty(ImagePreview);
    private readonly AppDbContext _db;


    public CreateSwapOfferPopupViewModel(
        ISwapService swapService,
        AuthService authService,
        PlantService plantService,
        AppDbContext db)
    {
        _swapService = swapService;
        _authService = authService;
        _plantService = plantService;
        _db = db;

        LoadPlants();
    }

    public async Task LoadPlants()
    {
        var plants = await _db.UserPlants
            .Include(x => x.Plant)
            .Where(x => x.UserId == _authService.GetUserId())
            .ToListAsync();

        MyPlants.Clear();

        foreach (var p in plants)
            MyPlants.Add(p);
    }

    [RelayCommand]
    private async Task PickImage()
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result == null) return;

        var path = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

        using var stream = await result.OpenReadAsync();
        using var file = File.OpenWrite(path);
        await stream.CopyToAsync(file);

        ImagePreview = path;

        OnPropertyChanged(nameof(HasImage));
    }

    [RelayCommand]
    public async Task Create()
    {
        if (SelectedPlant == null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка", "Выберите растение", "OK");
            return;
        }

        // ПРОВЕРЯЕМ что SupabaseId есть
        if (!SelectedPlant.SupabaseId.HasValue)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                "Растение не синхронизировано с сервером. " +
                "Попробуйте удалить и добавить растение заново.",
                "OK");
            return;
        }

        System.Diagnostics.Debug.WriteLine($"SupabaseId для свопа: {SelectedPlant.SupabaseId}");

        await _swapService.CreateOfferAsync(
            _authService.GetUserUuid(),
            SelectedPlant.SupabaseId.Value, // только Supabase Id!
            DesiredText
        );

        CloseAction?.Invoke();
    }
}