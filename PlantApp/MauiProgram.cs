using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.ViewModels;
using PlantApp.Views;
using PlantApp.Views.AdditionalViews;
using PlantApp.Views.Popups;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Http;

namespace PlantApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat");
                    fonts.AddFont("Montserrat-Bold.ttf", "Montserrat");
                    fonts.AddFont("Montserrat-ExtraBold.ttf", "Montserrat");
                    fonts.AddFont("Montserrat-SemiBold.ttf", "Montserrat");
                    fonts.AddFont("Montserrat-Light.ttf", "Montserrat");
                    fonts.AddFont("Montserrat-Thin.ttf", "Montserrat");
                })
                .ConfigureSyncfusionCore();

            // подключаем appsettings.json
            builder.Configuration
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            // база данных
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "plants.db");
                options.UseSqlite($"Filename={dbPath}");
            });

            // сервисы
            builder.Services.AddScoped<PlantService>();
            builder.Services.AddScoped<AIChatService>();
            builder.Services.AddScoped<AIService>();
            builder.Services.AddScoped<FavoriteService>();
            builder.Services.AddScoped<ProfileService>(); 
            builder.Services.AddScoped<FriendService>();
            builder.Services.AddScoped<UserService>(); 


            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<SecurityService>();
            builder.Services.AddSingleton<SupabaseStorageService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>(); 


            // HTTP клиент для AI
            builder.Services.AddHttpClient<AIService>(client =>
            {
                client.DefaultRequestHeaders.Add(
                    "Authorization",
                    "Api-Key " + builder.Configuration["YandexApiKey"]);
            });

            // подключаем HttpClient для чата
            builder.Services.AddHttpClient<RealtimeChatService>(); 

            // подключаем HttpClient для swap'а
            builder.Services.AddHttpClient<SupabaseSwapService>();
            builder.Services.AddScoped<ISwapService, SwapService>();
            builder.Services.AddHttpClient<SupabaseUserPlantService>();


            // страницы
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<PlantDetailsPage>();
            builder.Services.AddTransient<AddFriendPage>();
            builder.Services.AddTransient<FriendProfilePage>();

            builder.Services.AddTransient<AddPlantPopup>();
            builder.Services.AddTransient<EditProfilePopup>();
            builder.Services.AddTransient<UserPlantDetailsPopup>();
            builder.Services.AddTransient<CreateSwapOfferPopup>();
            builder.Services.AddTransient<MyPlantsPopup>();
            //чаты
            builder.Services.AddTransient<UserChatPage>();
            builder.Services.AddTransient<ChatPage>();

            // BottomBar
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ChatListPage>();
            builder.Services.AddTransient<CalendarPage>();
            builder.Services.AddTransient<ProfilePage>();

            // внутренние страницы
            builder.Services.AddTransient<GPTPage>();
            builder.Services.AddTransient<EncyclopediaPage>();
            builder.Services.AddTransient<SwapPage>();
            builder.Services.AddTransient<RemindersPage>(); //скоро удалится
            builder.Services.AddTransient<FavoritesPage>();

            // ViewModels
            builder.Services.AddTransient<FriendProfileViewModel>();
            builder.Services.AddTransient<AddFriendViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<MainViewModel>();  
            builder.Services.AddTransient<EncyclopediaViewModel>();
            builder.Services.AddTransient<FavoritesPageViewModel>();
            builder.Services.AddTransient<ProfilePageViewModel>();
            builder.Services.AddTransient<PlantDetailsViewModel>();
            builder.Services.AddSingleton<SwapPageViewModel>();

            builder.Services.AddTransient<AddPlantPopupViewModel>();
            builder.Services.AddTransient<EditProfilePopupViewModel>();
            builder.Services.AddTransient<UserPlantDetailsPopupViewModel>();
            builder.Services.AddTransient<CreateSwapOfferPopupViewModel>();
            builder.Services.AddTransient<MyPlantsPopupViewModel>();
            //чаты ViewModels
            builder.Services.AddTransient<ChatPageViewModel>();
            builder.Services.AddTransient<UserChatViewModel>();
            builder.Services.AddTransient<ChatListViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
} 