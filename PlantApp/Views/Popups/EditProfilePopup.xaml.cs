using CommunityToolkit.Maui.Views;

namespace PlantApp.Views.Popups;

public partial class EditProfilePopup : Popup
{
    public EditProfilePopup(EditProfilePopupViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;

        vm.SetPopup(this);
    }
}