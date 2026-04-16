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
        private readonly AppDbContext _db; 
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
            AppDbContext db, 
            AuthService authService,
            ISwapService swapService)
        {
            _swapService = swapService;
            _navigationService = navigationService;
            _db = db; 
            _authService = authService;
        }

 
        public async Task LoadOffers()
        {
            IsLoading = true;

            try
            {
                var offers = await _swapService.GetAllOffersAsync();

                Offers.Clear();

                foreach (var offer in offers)
                {
                    var plant = _db.UserPlants
                        .Include(p => p.Plant)
                        .FirstOrDefault(x => x.Id == offer.UserPlantId);

                        Console.WriteLine($"PLANT NULL FOR OFFER {offer.Id}");
                        Console.WriteLine($"Offer.UserPlantId = {offer.UserPlantId}");
                        System.Diagnostics.Debug.WriteLine(
    $"OFFER: {offer.Id}, UserPlantId: {offer.UserPlantId}");
                        System.Diagnostics.Debug.WriteLine(
    $"PLANT FOUND: {offer.Plant != null}");
                    
                    if (plant == null)
                    {

                        continue;
                    }

                    offer.ImageUrl = plant.ImageUrl;
                    offer.PlantName = plant.PlantName;
                    offer.Description = plant.Description;

                    Offers.Add(offer);
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

                foreach (var item in items)
                {
                    var plant = _db.UserPlants
                        .Include(p => p.Plant)
                        .FirstOrDefault(x => x.Id == item.UserPlantId);

                    if (plant == null)
                    {
                        Console.WriteLine($"PLANT NULL FOR OFFER {item.Id}");
                        continue;
                    }

                    item.ImageUrl = plant.ImageUrl;
                    item.PlantName = plant.PlantName;
                    item.Description = plant.Description;

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

            foreach (var item in items)
            {
                var plant = _db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.Id == item.UserPlantId);

                if (plant == null)
                {
                    Console.WriteLine($"PLANT NULL FOR OFFER {item.Id}");
                    continue;
                }

                item.ImageUrl = plant.ImageUrl;
                item.PlantName = plant.PlantName;
                item.Description = plant.Description;

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

            foreach (var item in items)
            {
                var plant = _db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.Id == item.UserPlantId);

                if (plant == null)
                {
                    Console.WriteLine($"PLANT NULL FOR OFFER {item.Id}");
                    continue;
                }

                item.ImageUrl = plant.ImageUrl;
                item.PlantName = plant.PlantName;
                item.Description = plant.Description;

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