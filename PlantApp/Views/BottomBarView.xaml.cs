using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlantApp.Views;

namespace PlantApp.Views;

public partial class BottomBarView : ContentView
{
    private readonly INavigationService _navigation;

    public BottomBarView()
    {
        InitializeComponent();
        _navigation = Application.Current.Handler.MauiContext
            .Services.GetService<INavigationService>();
    }

    async void HomeClicked(object sender, EventArgs e)
        => await _navigation.NavigateToAsync<MainPage>();

    async void ChatClicked(object sender, EventArgs e)
        => await _navigation.NavigateToAsync<ChatPage>();

    async void CalendarClicked(object sender, EventArgs e)
        => await _navigation.NavigateToAsync<CalendarPage>();

    async void ProfileClicked(object sender, EventArgs e)
        => await _navigation.NavigateToAsync<ProfilePage>();
}