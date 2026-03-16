namespace PlantApp.Services;

public interface INavigationService
{
    Task NavigateToAsync<TPage>() where TPage : Page;

    // навигация с параметром
    Task NavigateToAsync<TPage, TParameter>(TParameter parameter)
        where TPage : Page;

    Task GoBackAsync();

    void NavigateToRoot<TPage>() where TPage : Page;
}