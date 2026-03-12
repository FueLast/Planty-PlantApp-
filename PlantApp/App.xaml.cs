using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Views;

namespace PlantApp
{
    public partial class App : Application
    {
        public App(AppDbContext db)
        {
            InitializeComponent(); 

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var page = activationState?.Context.Services.GetRequiredService<LoginPage>();

            var navigationPage = new NavigationPage(page);

            var window = new Window(navigationPage)
            {
                Width = 500,
                Height = 800
            };

            return window;
        }
    }
}