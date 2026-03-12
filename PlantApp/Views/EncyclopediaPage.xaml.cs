// Базовые пространства имён
using Microsoft.EntityFrameworkCore;
// MAUI / UI
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PlantApp.Data;
using PlantApp.Services;
// ViewModel и модели
using PlantApp.ViewModels;
using PlantApp.Views.AdditionalViews;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PlantApp.Views;

public partial class EncyclopediaPage : ContentPage
{
// ViewModel внедряется через DI (НЕ создаётся вручную)
    private readonly EncyclopediaViewModel _viewModel;
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly AuthService _authService;
    // Конструктор страницы
    // ViewModel приходит из DI-контейнера
    public EncyclopediaPage(
        EncyclopediaViewModel viewModel,
        IDbContextFactory<AppDbContext> factory,
        AuthService authService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _factory = factory;
        _authService = authService;

        BindingContext = _viewModel;
    }

    // Жизненный цикл страницы
    // Вызывается каждый раз, когда страница появляется на экране
    protected override async void OnAppearing()
    {
        base.OnAppearing();

// Просим ViewModel загрузить данные
// Page знает КОГДА грузить
        await _viewModel.LoadPlantsAsync();

// UI-логика построения сетки
// НЕ бизнес-логика и НЕ работа с БД
        LikePinterestGrid(_viewModel.Plants);
    }

// Чисто UI-метод
// Формирует "пинтерест-подобную" сетку изображений
    private void LikePinterestGrid(IEnumerable<Plant> plants)
    {
// Очищаем колонки перед новой отрисовкой
        Column0Layout.Children.Clear();
        Column1Layout.Children.Clear();
        Column2Layout.Children.Clear();

// Присваиваем каждому обьекту из plants свое значние GUID и рандомном сортируем 
        var randomizedPlants = plants
        .OrderBy(_ => Guid.NewGuid()) //мешаем изображения
        .ToList();

// Проходим по всем растениям


        foreach (var plant in randomizedPlants)
        {
            // Создаём изображение растения
            var image = new Image
            {
                Source = plant.PlantImage, // путь к картинке
                Aspect = Aspect.AspectFill // обрезка по контейнеру
            };


            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new PlantDetailsPage(plant, _factory, _authService));
            };

            // Оборачиваем изображение в Border
            // (удобно для скруглений, теней и т.д.)
            var border = new Border
            {
                Content = image,
            };

            border.GestureRecognizers.Add(tapGesture);

            //чекаем сколько данных в колумнах (единиц)
            int count1 = Column0Layout.Children.Count;
            int count2 = Column1Layout.Children.Count;
            int count3 = Column2Layout.Children.Count;
            var precise_distrib = (count1, count2, count3) switch //проверка
            {
                var (c1, c2, c3) when c1 <= c2 && c1 <= c3 => Column0Layout,
                var (c1, c2, c3) when c2 <= c3 => Column1Layout,
                _ => Column2Layout
            };
            // Добавляем карточку в UI
            precise_distrib.Children.Add(border);
        }

    }

    private void TapGesture_Tapped(object? sender, TappedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
