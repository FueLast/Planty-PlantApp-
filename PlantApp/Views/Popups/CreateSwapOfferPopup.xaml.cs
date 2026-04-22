using CommunityToolkit.Maui.Views;
using PlantApp.ViewModels;

namespace PlantApp.Views.Popups;

public partial class CreateSwapOfferPopup : Popup
{
    public CreateSwapOfferPopup(CreateSwapOfferPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

       
        vm.CloseAction = async () => await this.CloseAsync();
    }
}