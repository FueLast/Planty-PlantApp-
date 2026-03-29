using PlantApp.ViewModels;

namespace PlantApp.Views;

public partial class ChatListPage : ContentPage
{
    public ChatListPage(ChatListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ChatListViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private void OnChatSelected(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as ChatItem;

        if (item == null) return;

        if (BindingContext is ChatListViewModel vm)
        {
            vm.OpenChatCommand.Execute(item);
        }

        ((CollectionView)sender).SelectedItem = null;
    }

}