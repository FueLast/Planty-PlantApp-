using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using System.Security.Cryptography;
using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterPageViewModel _viewmodel;
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        _viewmodel = viewModel;
        BindingContext = _viewmodel;
    }
}