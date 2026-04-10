using PlantApp.Data;
using PlantApp.ViewModels;

namespace PlantApp.Views.Popups;

public partial class UserPlantDetailsPopup : CommunityToolkit.Maui.Views.Popup
{
    public UserPlantDetailsPopup(UserPlantDetailsPopupViewModel vm, UserPlant plant, UserProfile owner)
    {
        InitializeComponent();

        vm.Plant = plant;
        vm.Owner = owner;

        BindingContext = vm;
    }
}