using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using PlantApp.Helpers;
using PlantApp.Services;
using PlantApp.Views;
using PlantApp.Views.AdditionalViews;
using PlantApp.Views.Popups;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels
{
    public partial class MyPlantsPopupViewModel : ObservableObject
    {
        private readonly ISwapService _swapService;
        private readonly INavigationService _navigationService;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthService _authService;
        private readonly PlantService _plantService;
        private readonly RealtimeChatService _realtimeChatService;

        private Chat _chat;
        private SwapOffer _currentOffer;

        [ObservableProperty]
        private bool isSelectionMode; 

        public ObservableCollection<UserPlant> Plants { get; } = new();
        public ObservableCollection<UserPlant> VisiblePlants { get; } = new();

        public MyPlantsPopupViewModel(
            INavigationService navigationService,
            IDbContextFactory<AppDbContext> dbContextFactory,
            AuthService authService,
            ISwapService swapService,
            PlantService plantService,
            RealtimeChatService realtimeChatService,
            IServiceProvider serviceProvider)
        {
            _swapService = swapService;
            _navigationService = navigationService;
            _dbContextFactory = dbContextFactory;
            _authService = authService;
            _serviceProvider = serviceProvider;

            _plantService = plantService;
            _realtimeChatService = realtimeChatService;
        }

        public async Task LoadForSwap(SwapOffer offer)
        {
            System.Diagnostics.Debug.WriteLine($"USER PLANT COUNT: {VisiblePlants.Count}");

            _currentOffer = offer;
            IsSelectionMode = true;

            var userId = _authService.GetUserId();
            var plants = await _plantService.GetUserPlants(userId);

            VisiblePlants.Clear();

            foreach (var p in plants)
                VisiblePlants.Add(p);
        }

        [RelayCommand]
        public async Task AddPlant(object popup)
        {
            if (popup is CommunityToolkit.Maui.Views.Popup currentPopup)
                await currentPopup.CloseAsync();

            await Task.Delay(100);

            var secondPopup = _serviceProvider.GetRequiredService<AddPlantPopup>();

            await Application.Current.MainPage.ShowPopupAsync(secondPopup);

            var isSaved = await secondPopup.Result;

            if (isSaved)
            {
                var firstPopup = _serviceProvider.GetRequiredService<MyPlantsPopup>();

                if (firstPopup.BindingContext is MyPlantsPopupViewModel vm)
                {
                    var userId = _authService.GetUserId();
                    var plants = await _plantService.GetUserPlants(userId);

                    vm.VisiblePlants.Clear();
                    foreach (var p in plants)
                        vm.VisiblePlants.Add(p);

                    vm.Plants.Clear();
                    foreach (var p in plants)
                        vm.Plants.Add(p);
                }

                await Application.Current.MainPage.ShowPopupAsync(firstPopup);
            }
        }

        [RelayCommand]
        public async Task SelectPlant(UserPlant plant)
        {
            System.Diagnostics.Debug.WriteLine("PLANT CLICKED: " + plant?.CustomName);

            await Application.Current.MainPage.ClosePopupAsync(plant);
        } 
    }
}