using CommunityToolkit.Maui.Views;
using PlantApp.ViewModels;
using System.Threading.Tasks;

namespace PlantApp.Views.Popups;

public partial class AddPlantPopup : CommunityToolkit.Maui.Views.Popup
{
    private readonly TaskCompletionSource<bool> _result = new();
    public Task<bool> Result => _result.Task;

    public AddPlantPopup(AddPlantPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; 
    }

    // Compatibility helper: set the result and close the popup
    public void Close(bool result)
    {
        // TrySetResult in case Close is called multiple times
        _result.TrySetResult(result);
        // Close the underlying popup page asynchronously
        _ = CloseAsync();
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as AddPlantPopupViewModel;
        try
        {
            await vm.SavePlant();
            // ВАЖНО: Метод Close() позволяет передать результат
            this.Close(true);
        }
        catch (Exception ex)
        { 
            await Application.Current.MainPage.DisplayAlert("ошибка", "не удалось сохранить", "ok");
            this.Close(false);
        }
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        this.Close(false);
    }



    // Кнопка "Добавить фото" уже привязана к команде в VM, поэтому здесь нет кода.
}
