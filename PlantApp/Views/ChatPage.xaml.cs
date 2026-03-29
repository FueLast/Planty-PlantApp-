using System.ComponentModel;
using SQLite;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class ChatPage : ContentPage
{
    public ChatPage(ChatPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if(BindingContext is ChatPageViewModel viewModel)
        {
            await viewModel.LoadMessagesAsync();
        }
    }

    private void OnEnterPressed(object sender, EventArgs e)
    {
        if (BindingContext is ChatPageViewModel vm)
        {
            vm.SendCommand.Execute(null);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is UserChatViewModel vm)
        {
            vm.StopListening();
        }
    }

}