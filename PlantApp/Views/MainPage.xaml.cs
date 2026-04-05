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
    }



}
