using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.ViewModels;
using PlantApp.Views;
using PlantApp.Views.AdditionalViews;
using Microsoft.EntityFrameworkCore;

namespace PlantApp
{
    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //путь к Sqlite
            builder.Services.AddSingleton<PlantService>(sp => // синглтон потому что
            {                                           // один объект на всё приложение
                var plantdbPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "plants.db"
                    );
                return new PlantService(plantdbPath);
            }
            );

            //string plantdbPath = Path.Combine(FileSystem.AppDataDirectory, "plants.db");
            string chatdbPath = Path.Combine(FileSystem.AppDataDirectory, "chat.db");

            //if (File.Exists(plantdbPath))
            //{
            //    File.Delete(plantdbPath);
            //}

            string userDbPath = Path.Combine(FileSystem.AppDataDirectory, "users.db");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Filename={userDbPath}"));


            //builder.Services.AddSingleton(new PlantService(plantDbPath));
            builder.Services.AddSingleton(new ChatService(chatdbPath));
            builder.Services.AddSingleton<INavigationService, NavigationService>();



            // Pages
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<PlantDetailsPage>();


            //BottomBar
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<CalendarPage>();
            builder.Services.AddTransient<ProfilePage>();
            //Pages Form Main Page
            builder.Services.AddTransient<GPTPage>();
            builder.Services.AddTransient<EncyclopediaPage>();
            builder.Services.AddTransient<RemindersPage>();
            builder.Services.AddTransient<FavoritesPage>();

            //ViewModels 
            builder.Services.AddTransient<LoginPageViewModel>(); 
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ChatPageViewModel>();
            builder.Services.AddTransient<EncyclopediaViewModel>();
            builder.Services.AddTransient<FavoritesPageViewModel>();

            //Data & Service
            builder.Services.AddSingleton<SecurityService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddScoped<FavoriteService>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();

        }

    }
}
