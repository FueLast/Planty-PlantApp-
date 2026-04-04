using PlantApp.Data;
using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageViewModel _viewModel;

    public ProfilePage(ProfilePageViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadProfile();
    }

    private async void OnFriendSelected(object sender, SelectionChangedEventArgs e)
    {
        var user = e.CurrentSelection.FirstOrDefault() as User;

        if (user == null)
            return;

        if (BindingContext is ProfilePageViewModel vm)
            await vm.OpenProfileCommand.ExecuteAsync(user);

        ((CollectionView)sender).SelectedItem = null; // сброс выбора
    }

}