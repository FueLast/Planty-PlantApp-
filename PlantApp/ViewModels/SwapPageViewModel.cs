using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Views.Popups;
using PlantApp.Services;
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

        public ObservableCollection<SwapOffer> Offers { get; } = new();

        private int _currentUserId = 1; // временно

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

        [RelayCommand]
        public async Task LoadAsync()
        {
            var items = await _swapService.GetAllOffersAsync(
                onlyPreview: true,
                skip: 0,
                take: PageSize);

            foreach (var item in items)
                Offers.Add(item);
        }

        [RelayCommand]
        public async Task LoadMore()
        {
            _page++;

            var items = await _swapService.GetAllOffersAsync(
                onlyPreview: false,
                skip: _page * PageSize,
                take: PageSize);

            foreach (var item in items)
                Offers.Add(item);
        }

        [RelayCommand]
        public async Task ShowAll()
        { 
            var items = await _swapService.GetAllOffersAsync(
                onlyPreview: false,
                skip: 0,
                take: PageSize);

            foreach (var item in items)
                Offers.Add(item);
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