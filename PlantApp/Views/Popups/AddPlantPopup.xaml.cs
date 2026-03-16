using CommunityToolkit.Maui.Views;
using PlantApp.ViewModels;

namespace PlantApp.Views.Popups;

public partial class AddPlantPopup : Popup
{
    private readonly TaskCompletionSource<bool> _result = new();
    public Task<bool> Result => _result.Task;

    public AddPlantPopup(AddPlantPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; 
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        _result.TrySetResult(false);
        await CloseAsync();
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as AddPlantPopupViewModel;
        try
        {
            await vm.SavePlant();
            _result.TrySetResult(true);
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert("ошибка", "не удалось сохранить", "ok");
            _result.TrySetResult(false);
        }
        await CloseAsync();
    }
     
    // Кнопка "Добавить фото" уже привязана к команде в VM, поэтому здесь нет кода.
}
