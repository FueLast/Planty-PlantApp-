using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PlantApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        public ICommand OnEncyclopediaCommand { get; }
        public ICommand OnGPTCommand { get; }
        public ICommand OnRemindersCommand { get; }
        public ICommand OnFavoritesCommand { get; }
        public ICommand OnHomePageCommand { get; }
        public ICommand OnChatPageCommand { get; }
        public ICommand OnCalendarPageCommand { get; }
        public ICommand OnProfilePageCommand { get; }


        public MainViewModel()
        {
            OnEncyclopediaCommand = new AsyncRelayCommand(ExecuteEncyclopediaPage);
            OnGPTCommand = new AsyncRelayCommand(ExecuteGPTPage);
            OnRemindersCommand = new AsyncRelayCommand(ExecuteRemindersPage);
            OnFavoritesCommand = new AsyncRelayCommand(ExecuteFavoritesPage);

            //OnHomePageCommand = new AsyncRelayCommand(ExecuteHomePage);
            //OnChatPageCommand = new AsyncRelayCommand(ExecuteChatPage);
            //OnCalendarPageCommand = new AsyncRelayCommand(ExecuteCalendarPage);
            //OnProfilePageCommand = new AsyncRelayCommand(ExecuteProfilePage);

        }

        public async Task ExecuteEncyclopediaPage()
        {
            await Shell.Current.GoToAsync("EncyclopediaPage");
        }

        public async Task ExecuteGPTPage()
        {
             await Shell.Current.GoToAsync("GPTPage");
        }

        public async Task ExecuteRemindersPage()
        {
            await Shell.Current.GoToAsync("RemindersPage");
        }

        public async Task ExecuteFavoritesPage()
        {
            await Shell.Current.GoToAsync("FavoritesPage");
        }

//нижняя часть приложения (TabBar)

        //public async Task ExecuteHomePage()
        //{
        //    var CurrentPageTitle = Shell.Current.CurrentPage.Title;
        //    if (CurrentPageTitle == "MainPage")
        //    {
        //        return;
        //    }
        //    await Shell.Current.GoToAsync("///MainPage");
        //}

        //public async Task ExecuteChatPage()
        //{
        //    var CurrentPageTitle = Shell.Current.CurrentPage.Title;
        //    if (CurrentPageTitle == "ChatPage")
        //    {
        //        return;
        //    }
        //    await Shell.Current.GoToAsync("/ChatPage");
        //}

        //public async Task ExecuteCalendarPage()
        //{
        //    var CurrentPageTitle = Shell.Current.CurrentPage.Title;
        //    if (CurrentPageTitle == "CalendarPage")
        //    {
        //        return;
        //    }
        //    await Shell.Current.GoToAsync("/CalendarPage");
        //}

        //public async Task ExecuteProfilePage()
        //{
        //    var CurrentPageTitle = Shell.Current.CurrentPage.Title;
        //    if (CurrentPageTitle == "ProfilePage")
        //    {
        //        return;
        //    }
        //    await Shell.Current.GoToAsync("/ProfilePage");
        //}




    }

}
