using Microsoft.Extensions.Logging.Abstractions;
using PlantApp.Data;
using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginPageViewModel _viewmodel;
    public LoginPage(LoginPageViewModel viewmodel)
	{
        InitializeComponent();
        _viewmodel = viewmodel;
        BindingContext = _viewmodel;

	} 

}
 