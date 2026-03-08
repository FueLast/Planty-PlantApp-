using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PlantApp.Services;
using PlantApp.Views;

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
}