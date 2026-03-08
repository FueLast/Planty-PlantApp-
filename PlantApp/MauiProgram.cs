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
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<PlantDetailsPage>();
            builder.Services.AddTransient<EncyclopediaPage>();



            //ViewModels 
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ChatPageViewModel>();
            builder.Services.AddTransient<EncyclopediaViewModel>();

            //Data
            builder.Services.AddSingleton<SecurityService>();
             
             
                
              

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();

        }

    }
}
