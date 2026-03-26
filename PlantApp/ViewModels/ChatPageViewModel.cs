using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels
{
    public partial class ChatPageViewModel : ObservableObject
    {
        private readonly AIChatService _chatService;
        private readonly AIService _aiService;
        private readonly AuthService _authService;

        private Chat _chat;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> messages = new();

        [ObservableProperty]
        private string messageText;

        public IAsyncRelayCommand SendCommand { get; }

        public ChatPageViewModel(
            AIChatService chatService,
            AuthService authService,
            AIService aiService)
        {
            _chatService = chatService;
            _authService = authService;
            _aiService = aiService;

            SendCommand = new AsyncRelayCommand(SendAsync);
        }

        public async Task LoadMessagesAsync()
        {
            var user = _authService.CurrentUser;
            if (user == null) return;

            _chat = await _chatService.GetOrCreateChatAsync(user.Id);

            var messagesFromDb = await _chatService.GetMessagesAsync(_chat.Id);

            if (!messagesFromDb.Any())
            {
                var welcome = new ChatMessage
                {
                    ChatId = _chat.Id,
                    Role = "assistant",
                    Content = "Привет! Я твой помощник 🌿",
                    CreatedAt = DateTime.Now
                };

                await _chatService.AddMessageAsync(welcome);
                messagesFromDb.Add(welcome);
            }

            // не пересоздаём коллекцию!
            Messages.Clear();

            foreach (var msg in messagesFromDb)
                Messages.Add(msg);
        }

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;

            var user = _authService.CurrentUser;
            if (user == null) return;

            if (_chat == null)
                await LoadMessagesAsync();

            var userMsg = new ChatMessage
            {
                ChatId = _chat.Id,
                Role = "user",
                Content = MessageText,
                CreatedAt = DateTime.Now
            };

            Messages.Add(userMsg);
            await _chatService.AddMessageAsync(userMsg);

            var aiResponse = await _aiService.SendAsync(MessageText, user);

            var botMsg = new ChatMessage
            {
                ChatId = _chat.Id,
                Role = "assistant",
                Content = aiResponse,
                CreatedAt = DateTime.Now
            };

            Messages.Add(botMsg);
            await _chatService.AddMessageAsync(botMsg);

            MessageText = string.Empty;
        }
    }
}