using Microsoft.Extensions.DependencyInjection;

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