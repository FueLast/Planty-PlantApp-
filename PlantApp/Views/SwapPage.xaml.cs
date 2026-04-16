using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class SwapPage : ContentPage
{
    private readonly SwapPageViewModel _vm;

    public SwapPage(SwapPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
        System.Diagnostics.Debug.WriteLine("BINDING CONTEXT SET");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // защита от повторной загрузки
        if (_vm.Offers.Count == 0)
        {
            await _vm.LoadAsync();
        }
    }
}