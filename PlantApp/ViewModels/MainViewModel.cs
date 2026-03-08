using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data; 
using PlantApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlantApp.Views;


namespace PlantApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly PlantService _plantService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Plant> popularPlants;


        public MainViewModel(
            PlantService plantService,
            INavigationService navigationService)
        {
            _plantService = plantService;
            _navigationService = navigationService;

            OnEncyclopediaCommand = new AsyncRelayCommand(OpenEncyclopedia);
            OnGPTCommand = new AsyncRelayCommand(OpenGPT);
            OnRemindersCommand = new AsyncRelayCommand(OpenReminders);
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





    }

}
