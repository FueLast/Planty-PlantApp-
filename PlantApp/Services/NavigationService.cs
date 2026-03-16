using Microsoft.Extensions.DependencyInjection;
using PlantApp.Data;

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

    // навигация с передачей параметра
    public async Task NavigateToAsync<TPage, TParameter>(TParameter parameter)
        where TPage : Page
    {
        var page = _serviceProvider.GetRequiredService<TPage>();

        if (page.BindingContext is IInitialize<TParameter> vm)
            vm.Initialize(parameter);

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