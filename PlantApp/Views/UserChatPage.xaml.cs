using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class UserChatPage : ContentPage
{
    public UserChatPage(UserChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is UserChatViewModel vm)
        {
            vm.StopListening();
        }
    }

    private void OnEnterPressed(object sender, EventArgs e)
    {
        if (BindingContext is UserChatViewModel vm)
        {
            vm.SendCommand.Execute(null);
        }
    }
}