using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class UserChatPage : ContentPage
{
    private readonly UserChatViewModel _vm;

    public UserChatPage(UserChatViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // friendId приходит через NavigationService
        if (BindingContext is UserChatViewModel vm && _friendId != 0)
        {
            await vm.Init(_friendId);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void OnEnterPressed(object sender, EventArgs e)
    {
        _vm.SendCommand.Execute(null);
    }

    // поле для передачи параметра
    private int _friendId;

    public void SetFriendId(int id)
    {
        _friendId = id;
    }
}