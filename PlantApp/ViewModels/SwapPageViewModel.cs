using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.Views;
using PlantApp.Views.AdditionalViews;
using PlantApp.Views.Popups;
using Supabase.Gotrue;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlantApp.ViewModels
{
    public partial class SwapPageViewModel : ObservableObject
    {
        private readonly ISwapService _swapService;
        private readonly INavigationService _navigationService;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthService _authService;

        [ObservableProperty]
        private string? desiredText;

        private int _page = 0;
        private const int PageSize = 20;

        [ObservableProperty]
        private bool isFullMode;

        [ObservableProperty]
        private MyPlantsPopup myplants;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isInitialLoading;

        [ObservableProperty]
        private bool isLoadingMore;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private int totalCount;

        private bool isSkeletonVisible;
        public bool IsSkeletonVisible
        {
            get => isSkeletonVisible;
            set => SetProperty(ref isSkeletonVisible, value);
        }

        public ObservableCollection<int> Pages { get; } = new();
        public ObservableCollection<SwapOffer> Offers { get; } = new();

        public SwapPageViewModel(
            INavigationService navigationService,
            IDbContextFactory<AppDbContext> dbContextFactory,
            AuthService authService,
            ISwapService swapService,
            IServiceProvider serviceProvider)
        {
            _swapService = swapService;
            _navigationService = navigationService;
            _dbContextFactory = dbContextFactory;
            _authService = authService;
            _serviceProvider = serviceProvider;
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
                        .FirstOrDefault(x => x.SupabaseId == item.UserPlantId);

                    if (plant != null)
                    {
                        item.ImageUrl ??= plant.ImageUrl;
                        item.Plant = plant;
                    }
                    else
                    {
                        item.Plant = new UserPlant
                        {
                            CustomName = "Неизвестное растение",
                            Description = item.DesiredPlantDescription,
                            Plant = new Plant { NamePlant = "Неизвестно" }
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
            if (IsInitialLoading || _isInitialized) return;

            IsInitialLoading = true;
            _isInitialized = true;
            IsSkeletonVisible = true;

            try
            {
                System.Diagnostics.Debug.WriteLine("LOAD ASYNC CALLED");

                Offers.Clear();
                _page = 0;
                IsFullMode = false;

                var items = await _swapService.GetAllOffersAsync(true, 0, PageSize);

                using var db = _dbContextFactory.CreateDbContext();

                // блок поиска растения
                foreach (var item in items)
                {
                    // ищем по SupabaseId, а не по локальному Id
                    var plant = db.UserPlants
                        .Include(p => p.Plant)
                        .FirstOrDefault(x => x.SupabaseId == item.UserPlantId);

                    if (plant != null)
                    {
                        item.ImageUrl ??= plant.ImageUrl;
                        item.Plant = plant;
                    }
                    else
                    {
                        item.Plant = new UserPlant
                        {
                            CustomName = "Неизвестное растение",
                            Description = item.DesiredPlantDescription,
                            Plant = new Plant { NamePlant = "Неизвестно" }
                        };
                        item.ImageUrl = "background_listik_profile.png";
                    }

                    Offers.Add(item);
                }
            }
            finally
            {
                IsSkeletonVisible = false;
                IsInitialLoading = false;
            }
        }

        [RelayCommand]
        public async Task LoadMore()
        {
            if (IsLoadingMore) return;

            IsLoadingMore = true;
            _page++;

            var items = await _swapService.GetAllOffersAsync(false, _page * PageSize, PageSize);

            using var db = _dbContextFactory.CreateDbContext();

            foreach (var item in items)
            {
                var plant = db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.SupabaseId == item.UserPlantId);

                if (plant != null)
                {
                    item.ImageUrl = plant.ImageUrl;
                    item.Plant = plant;
                }
                else
                {
                    item.Plant = new UserPlant
                    {
                        CustomName = "Неизвестное растение",
                        Description = item.DesiredPlantDescription,
                        Plant = new Plant { NamePlant = "Неизвестно" }
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
            TotalCount = await _swapService.GetOffersCountAsync();
            TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);

            Pages.Clear();
            for (int i = 1; i <= TotalPages; i++)
                Pages.Add(i);

            IsFullMode = true;
            Offers.Clear();

            var items = await _swapService.GetAllOffersAsync(false, 0, PageSize);

            using var db = _dbContextFactory.CreateDbContext();

            foreach (var item in items)
            {
                var plant = db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.SupabaseId == item.UserPlantId);

                if (plant != null)
                {
                    item.ImageUrl = plant.ImageUrl;
                    item.Plant = plant;
                }
                else
                {
                    item.Plant = new UserPlant
                    {
                        CustomName = "Неизвестное растение",
                        Description = item.DesiredPlantDescription,
                        Plant = new Plant { NamePlant = "Неизвестно" }
                    };
                    item.ImageUrl = "background_listik_profile.png";
                }

                Offers.Add(item);
            }
        }

        [RelayCommand]
        public async Task GoToPage(int page)
        {
            if (page == CurrentPage) return;

            CurrentPage = page;
            Offers.Clear();

            var items = await _swapService.GetAllOffersAsync(
                false,
                (page - 1) * PageSize,
                PageSize);

            using var db = _dbContextFactory.CreateDbContext();

            foreach (var item in items)
            {
                var plant = db.UserPlants
                    .Include(p => p.Plant)
                    .FirstOrDefault(x => x.SupabaseId == item.UserPlantId);

                item.Plant = plant ?? new UserPlant
                {
                    CustomName = "Неизвестное растение",
                    Plant = new Plant { NamePlant = "Неизвестно" }
                };

                Offers.Add(item);
            }
        }

        [RelayCommand]
        public async Task SendRequest(SwapOffer offer)
        {
            int userId = _authService.GetUserId();

            using var db = _dbContextFactory.CreateDbContext();

            var myPlant = db.UserPlants
                .FirstOrDefault(p => p.UserId == userId);

            if (myPlant == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка",
                    "У вас нет растений",
                    "Ок");
                return;
            }

            await _swapService.SendRequestAsync(
                offer.Id,
                userId, 
                myPlant.Id);

            var page = _serviceProvider.GetRequiredService<ChatPage>();

            if (page.BindingContext is ChatPageViewModel vm)
                await vm.Load(offer.OwnerId);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        [RelayCommand]
        private async Task OpenCreateOfferPopup()
        {
            var vm = App.Current.Handler.MauiContext.Services
                .GetRequiredService<CreateSwapOfferPopupViewModel>();
            var popup = new CreateSwapOfferPopup(vm);
            await Application.Current.MainPage.ShowPopupAsync(popup);

            _isInitialized = false;
            await LoadOffers(); // вызываем LoadOffers
        }

        [RelayCommand]
        public async Task OpenSelectPlant(SwapOffer offer)
        {
            //нельзя отвечать на свой же оффер
            var myUuid = _authService.GetUserUuid();

            if (offer.OwnerId == myUuid)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Это ваш оффер",
                    "Нельзя откликнуться на собственное предложение",
                    "OK");
                return;
            }

            var popup = _serviceProvider.GetRequiredService<MyPlantsPopup>();

            if (popup.BindingContext is MyPlantsPopupViewModel vm)
                await vm.LoadForSwap(offer);

            var result = await Application.Current.MainPage.ShowPopupAsync(popup);

            if (result is UserPlant selectedPlant)
            {
                if (!selectedPlant.SupabaseId.HasValue)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ошибка",
                        "Растение не синхронизировано с сервером. " +
                        "Удалите и добавьте растение заново.",
                        "OK");
                    return;
                }

                await _swapService.SendRequestAsync(
                    offer.Id,
                    _authService.GetUserId(),
                    selectedPlant.SupabaseId.Value);

                var page = _serviceProvider.GetRequiredService<ChatPage>();

                await Application.Current.MainPage.Navigation.PushAsync(page);

                if (page.BindingContext is ChatPageViewModel vmChat)
                {
                    await vmChat.Load(offer.OwnerId);
                    await Task.Delay(100);

                    // сначала отправляем текстовое приветствие
                    var greeting = $"🌿 Привет! Меня заинтересовал твой оффер. Давай обменяемся?";
                    await vmChat.SendSystemMessage(greeting);

                    // потом карточку растения
                    await vmChat.SendPlantMessage(selectedPlant);
                }
            }
        }
    }
}