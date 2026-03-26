using System.ComponentModel;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PlantApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Services;
using PlantApp.Data;
using SQLite;

namespace PlantApp.Views;
public partial class GPTPage : ContentPage
{
    private readonly ChatPageViewModel _vm;

    public GPTPage(ChatPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; 
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadMessagesAsync();
    }

    private void OnEnterPressed(object sender, EventArgs e)
    {
        if (BindingContext is ChatPageViewModel vm && vm.SendCommand.CanExecute(null))
        {
            vm.SendCommand.Execute(null);
        }
    }

}

