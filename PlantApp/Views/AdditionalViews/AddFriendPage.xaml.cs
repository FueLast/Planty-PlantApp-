using PlantApp.ViewModels;

namespace PlantApp.Views.AdditionalViews;

public partial class AddFriendPage : ContentPage
{
    public AddFriendPage(AddFriendViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}