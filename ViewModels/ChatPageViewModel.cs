using CommunityToolkit.Mvvm.ComponentModel;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels
{
    public partial class ChatPageViewModel : ObservableObject
    {
        private readonly ChatService _chatService;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> messages;

        [ObservableProperty]
        private string messageText;

        public ChatPageViewModel(ChatService chatService)
        {
            _chatService = chatService;
            Messages = new ObservableCollection<ChatMessage>(); // 🔴 ВАЖНО
        }

        public async Task LoadMessagesAsync()
        {
            var messagesFromDb = await _chatService.GetUsersAsync();

            foreach (var message in messagesFromDb)
            {
                message.UserProfile = new UserProfile
                {
                    Id = message.UserProfileId,
                    UserName = "PlantBot"
                };
            }

            Messages = new ObservableCollection<ChatMessage>(messagesFromDb);
        }

    }
}
