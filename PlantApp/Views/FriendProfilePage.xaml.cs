using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class FriendProfilePage : ContentPage
{
    public FriendProfilePage(FriendProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}