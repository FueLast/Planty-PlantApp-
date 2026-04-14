using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public ObservableCollection<Plant> Plants { get; } = new();

    [ObservableProperty]
    private Plant selectedPlant;

    private readonly PlantService _plantService;

    public bool HasImage => !string.IsNullOrEmpty(ImagePreview);

    public CreateSwapOfferPopupViewModel(
        ISwapService swapService,
        AuthService authService,
        PlantService plantService)
    {
        _swapService = swapService;
        _authService = authService;
        _plantService = plantService;

        LoadPlants();
    }

    private async void LoadPlants()
    {
        var list = await _plantService.GetPlants();

        Plants.Clear();

        foreach (var plant in list)
            Plants.Add(plant);
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
    private async Task Create()
    {
        // пока захардкодим plantId (потом сделаем выбор)
        int plantId = 1;

        await _swapService.CreateOfferAsync(
            _authService.GetUserId(),
            plantId,
            DesiredText);

        CloseAction?.Invoke();
    }
}