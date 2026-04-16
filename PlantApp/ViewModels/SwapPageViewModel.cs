using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views.Popups;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlantApp.ViewModels
{
    public partial class SwapPageViewModel : ObservableObject
    {
        private readonly ISwapService _swapService;

        private readonly INavigationService _navigationService;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly AuthService _authService;

        [ObservableProperty]
        private string? desiredText;

        private int _page = 0;
        private const int PageSize = 20;

        [ObservableProperty]
        private bool isFullMode;


        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isInitialLoading;

        [ObservableProperty]
        private bool isLoadingMore;

        public ObservableCollection<SwapOffer> Offers { get; } = new();
          
        public SwapPageViewModel(
            INavigationService navigationService,
            IDbContextFactory<AppDbContext> dbContextFactory, 
            AuthService authService,
            ISwapService swapService)
        {
            _swapService = swapService;
            _navigationService = navigationService;
            _dbContextFactory = dbContextFactory; 
            _authService = authService;
        }

 
        public async Task LoadOffers()
        {
            IsLoading = true;

            System.Diagnostics.Debug.WriteLine($"UI OFFERS COUNT: {Offers.Count}");

            try
            { 
                var items = await _swapService.GetAllOffersAsync(true, 0, PageSize);
                Offers.Clear();

                using var db = _dbContextFactory.CreateDbContext();

                foreach (var item in items)
                {
                    var plant = db.UserPlants
                        .Include(p => p.Plant)
                        .FirstOrDefault(x => x.Id == item.UserPlantId);

                    if (plant != null)
                    {
                        item.ImageUrl = plant.ImageUrl;

                        item.Plant = plant; // ВАЖНО!!!
                    }
                    else
                    {
                        // создаем фейковый plant чтобы XAML не умер
                        item.Plant = new UserPlant
                        {
                            CustomName = "Неизвестное растение",
                            Description = item.DesiredPlantDescription,
                            Plant = new Plant
                            {
                                NamePlant = "Неизвестно"
                            }
                        };

                        item.ImageUrl = "background_listik_profile.png";
                    }

                    Offers.Add(item);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }


        private bool _isInitialized;

        [RelayCommand]
        public async Task LoadAsync()
        {
            System.Diagnostics.Debug.WriteLine("LOAD ASYNC CALLED");
            if (IsInitialLoading || _isInitialized) return;

            IsInitialLoading = true;
            _isInitialized = true;

            try
            {
                Offers.Clear();
                _page = 0;
                IsFullMode = false;

                var items = await _swapService.GetAllOffersAsync(true, 0, PageSize);

                System.Diagnostics.Debug.WriteLine($"OFFERS COUNT: {items.Count}");

                using var db = _dbContextFactory.CreateDbContext();

                foreach (var item in items)
                {
                    var plant = db.UserPlants
                        .Include(p => p.Plant)
                        .FirstOrDefault(x => x.Id == item.UserPlantId);

                    if (plant != null)
                    {
                        item.ImageUrl = plant.ImageUrl;

                        item.Plant = plant; // ВАЖНО!!!
                    }
                    else
                    {
                        // создаем фейковый plant чтобы XAML не умер
                        item.Plant = new UserPlant
                        {
                            CustomName = "Неизвестное растение",
                            Description = item.DesiredPlantDescription,
                            Plant = new Plant
                            {
                                NamePlant = "Неизвестно"
                            }
                        };

                        item.ImageUrl = "background_listik_profile.png";
                    }

                    Offers.Add(item);
                }
            }
            finally
            {
                IsInitialLoading = false;
            }
        }

        [RelayCommand]
        public async Task LoadMore()
        {
            if (IsLoadingMore) return;

            IsLoadingMore = true;

            _page++;

            var items = await _swapService.GetAllOffersAsync(
                onlyPreview: false,
                skip: _page * PageSize,
                take: PageSize);

            using var db = _dbContextFactory.CreateDbContext();

            foreach (var item in items)
            {
                var plant = db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.Id == item.UserPlantId);

                if (plant != null)
                {
                    item.ImageUrl = plant.ImageUrl;

                    item.Plant = plant; // ВАЖНО!!!
                }
                else
                {
                    // создаем фейковый plant чтобы XAML не умер
                    item.Plant = new UserPlant
                    {
                        CustomName = "Неизвестное растение",
                        Description = item.DesiredPlantDescription,
                        Plant = new Plant
                        {
                            NamePlant = "Неизвестно"
                        }
                    };

                    item.ImageUrl = "background_listik_profile.png";
                }

                Offers.Add(item);
            }

            IsLoadingMore = false;
        }

        [RelayCommand]
        public async Task ShowAll()
        {
            IsFullMode = true;
            Offers.Clear();

            var items = await _swapService.GetAllOffersAsync(false, 0, PageSize);

            using var db = _dbContextFactory.CreateDbContext();

            foreach (var item in items)
            {
                var plant = db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.Id == item.UserPlantId);

                if (plant != null)
                {
                    item.ImageUrl = plant.ImageUrl;

                    item.Plant = plant; // ВАЖНО!!!
                }
                else
                {
                    // создаем фейковый plant чтобы XAML не умер
                    item.Plant = new UserPlant
                    {
                        CustomName = "Неизвестное растение",
                        Description = item.DesiredPlantDescription,
                        Plant = new Plant
                        {
                            NamePlant = "Неизвестно"
                        }
                    };

                    item.ImageUrl = "background_listik_profile.png";
                }

                Offers.Add(item);
            }
        }

        [RelayCommand]
        private async Task OpenCreateOfferPopup()
        {
            var vm = App.Current.Handler.MauiContext.Services
                .GetRequiredService<CreateSwapOfferPopupViewModel>();

            var popup = new CreateSwapOfferPopup(vm);

            await Application.Current.MainPage.ShowPopupAsync(popup);

            // после закрытия обновляем список
            await LoadAsync();
        }

        [RelayCommand]
        public async Task SendRequest(SwapOffer offer)
        {
            int myPlantId = 1;

            await _swapService.SendRequestAsync(
                offer.Id,
                _authService.GetUserId(), // pass int user id
                myPlantId);
        }
    }
}