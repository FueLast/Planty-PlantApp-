using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class SwapPage : ContentPage
{
    public SwapPage(SwapPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}