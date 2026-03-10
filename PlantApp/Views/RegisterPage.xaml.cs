namespace PlantApp.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }
}