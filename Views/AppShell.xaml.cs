using PlantApp.Views;

namespace PlantApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("GPTPage", typeof(GPTPage));
            Routing.RegisterRoute("EncyclopediaPage", typeof(EncyclopediaPage));
            Routing.RegisterRoute("RemindersPage", typeof(RemindersPage));
            Routing.RegisterRoute("FavoritesPage", typeof(FavoritesPage));

            Routing.RegisterRoute("Mainpage", typeof(MainPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("CalendarPage", typeof(CalendarPage));
            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        }

    }
}
