using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data; 
using PlantApp.Services;
using PlantApp.Views;
using PlantApp.Views.AdditionalViews;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace PlantApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly PlantService _plantService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Plant> popularPlants;

        public ObservableCollection<Plant> SearchResults { get; set; } = new();
        public IAsyncRelayCommand<Plant> OpenPlantDetailsCommand { get; } //command для search

        public MainViewModel(
            PlantService plantService,
            INavigationService navigationService)
        {
            _plantService = plantService;
            _navigationService = navigationService;

            OnEncyclopediaCommand = new AsyncRelayCommand(OpenEncyclopedia);
            OnGPTCommand = new AsyncRelayCommand(OpenGPT);
            OnRemindersCommand = new AsyncRelayCommand(OpenReminders);
            OnFavoritesCommand = new AsyncRelayCommand(OpenFavorites);

            OpenPlantDetailsCommand = new AsyncRelayCommand<Plant>(OpenPlantDetails);
        }

        public async Task LoadPopularPlantsAsync()
        {
            var plants = await _plantService.GetPlantsAsync();

            PopularPlants = new ObservableCollection<Plant>(
                plants.OrderBy(_ => Guid.NewGuid()).Take(7)
            );
        }

        public ICommand OnEncyclopediaCommand { get; }
        public ICommand OnGPTCommand { get; }
        public ICommand OnRemindersCommand { get; }
        public ICommand OnFavoritesCommand { get; }
        public ICommand OnHomePageCommand { get; }
        public ICommand OnChatPageCommand { get; }
        public ICommand OnCalendarPageCommand { get; }
        public ICommand OnProfilePageCommand { get; }

        private async Task OpenEncyclopedia()
        {
            await _navigationService.NavigateToAsync<EncyclopediaPage>();
        } 

        private async Task OpenGPT()
        {
            await _navigationService.NavigateToAsync<GPTPage>();
        }

        private async Task OpenReminders()
        {
            await _navigationService.NavigateToAsync<RemindersPage>();
        }
        private async Task OpenFavorites()
        {
            await _navigationService.NavigateToAsync<FavoritesPage>();
        }

        [ObservableProperty]
        private string searchPlant;

        private CancellationTokenSource _searchCts;

        partial void OnSearchPlantChanged(string value)
        {
            // отменяю предыдущий поиск
            _searchCts?.Cancel();

            _searchCts = new CancellationTokenSource();

            _ = DebouncedSearch(value, _searchCts.Token);
        }

        private async Task DebouncedSearch(string query, CancellationToken token)
        {
            try
            {
                // жду 300мс чтобы пользователь закончил ввод
                await Task.Delay(300, token);

                if (string.IsNullOrWhiteSpace(query))
                {
                    SearchResults.Clear();
                    return;
                }

                // не начинаю поиск если символов меньше 3
                if (query.Length < 3)
                {
                    SearchResults.Clear();
                    return;
                }

                var plants = await _plantService.SearchPlants(query);

                SearchResults.Clear();

                foreach (var plant in plants)
                    SearchResults.Add(plant);
            }
            catch (TaskCanceledException)
            {
                // ничего не делаю если поиск отменился
            }
        }

        private async Task OpenPlantDetails(Plant plant)
        {
            if (plant == null)
                return;

            SearchPlant = "";
            SearchResults.Clear();

            await _navigationService.NavigateToAsync<PlantDetailsPage, Plant>(plant);
        }

        private async Task Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                SearchResults.Clear();
                return;
            }

            var plants = await _plantService.SearchPlants(query);

            SearchResults.Clear();

            foreach (var plant in plants)
                SearchResults.Add(plant);
        }



    }

}
