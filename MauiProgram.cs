using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using PlantApp.Services;
using PlantApp.ViewModels;
using PlantApp.Views;

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

            //builder.Services.AddSingleton(new PlantService(plantDbPath));
            builder.Services.AddSingleton(new ChatService(chatdbPath));


            //Pages
            builder.Services.AddTransient<ChatPage>();


            //ViewModels
            builder.Services.AddTransient<ChatPageViewModel>();
            builder.Services.AddTransient<EncyclopediaViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();

        }

    }
}
