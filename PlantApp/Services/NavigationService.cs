using Microsoft.Extensions.DependencyInjection;
using PlantApp.Views;

namespace PlantApp.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task NavigateToAsync<TPage>() where TPage : Page
    {
        var page = _serviceProvider.GetRequiredService<TPage>();

        await Application.Current.MainPage.Navigation.PushAsync(page);
    }

    //навигация с параметром
    public async Task NavigateToAsync<TPage, TParameter>(TParameter parameter)
        where TPage : Page
    {
        var page = _serviceProvider.GetRequiredService<TPage>();

        // 1. если ViewModel поддерживает Initialize
        if (page.BindingContext is IInitialize<TParameter> vm)
        {
            vm.Initialize(parameter);
        }

        // 2. если это чат - прокидываем friendId
        if (page is UserChatPage chatPage && parameter is int id)
        {
            chatPage.SetFriendId(id);
        }

        //PushAsync только 1 раз
        await Application.Current.MainPage.Navigation.PushAsync(page);
    }

    public async Task GoBackAsync()
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    public void NavigateToRoot<TPage>() where TPage : Page
    {
        var page = _serviceProvider.GetRequiredService<TPage>();

        Application.Current.MainPage = new NavigationPage(page);
    }
}