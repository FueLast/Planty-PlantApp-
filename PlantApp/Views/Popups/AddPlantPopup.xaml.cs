using CommunityToolkit.Maui.Views;

namespace PlantApp.Views.Popups;

public partial class AddPlantPopup : Popup
{
    public AddPlantPopup()
    {
        InitializeComponent();
    }

    void OnCancelClicked(object sender, EventArgs e)
    {
        // закрываем popup
        CloseAsync();
    }

    void OnSaveClicked(object sender, EventArgs e)
    {
        // пока просто закрываем окно
        // позже сюда добавим сохранение растения

        CloseAsync();
    }

    async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();

            if (result == null)
                return;

            var path = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

            using var stream = await result.OpenReadAsync();
            using var fileStream = File.OpenWrite(path);

            await stream.CopyToAsync(fileStream);

            // пока просто сообщение, позже привяжем к модели
            await Application.Current.MainPage.DisplayAlert(
                "фото добавлено",
                "изображение успешно выбрано",
                "ok");
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert(
                "ошибка",
                "не удалось выбрать фото",
                "ok");
        }
    }
}