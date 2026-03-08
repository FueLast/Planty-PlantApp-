// Базовые пространства имён
// MAUI / UI
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Controls.Shapes;
using PlantApp.Data;
using CommunityToolkit.Mvvm.ComponentModel;
// ViewModel и модели
using PlantApp.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks; 


namespace PlantApp.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Просим ViewModel загрузить данные
        // Page знает КОГДА грузить
        await _vm.LoadPopularPlantsAsync();

        // UI-логика построения сетки
        // НЕ бизнес-логика и НЕ работа с БД
        loadings(_vm.PopularPlants);
    }
    private void loadings(IEnumerable<Plant> plants)
    {
        PopularPlantsLayout.Children.Clear();   

        var randomizedPlants = plants
            .OrderBy(_ => Guid.NewGuid()) //мешаем изображения
            .Take(7);

        foreach (var plant in randomizedPlants)
        {
            var image = new Image
            {
                Source = plant.PlantImage,
                Aspect = Aspect.AspectFill,
                HeightRequest = 150,
                WidthRequest = 150
            };

            var border = new Border
            {
                Content = image,
                StrokeShape = new RoundRectangle { CornerRadius = 20 },
                BackgroundColor = Colors.White
            };

            PopularPlantsLayout.Children.Add(border);

        }

    }



}
