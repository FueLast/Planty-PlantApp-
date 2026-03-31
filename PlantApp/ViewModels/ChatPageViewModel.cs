using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Services;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels
{
    public partial class ChatPageViewModel : ObservableObject
    {
        private readonly RealtimeChatService _realtimeChatService;
        private readonly AIService _aiService;
        private readonly AuthService _authService;

        private Chat _chat;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> messages = new();

        [ObservableProperty]
        private string messageText;

        public IAsyncRelayCommand SendCommand { get; }

        public ChatPageViewModel(
            RealtimeChatService realtimeChatService,
            AuthService authService,
            AIService aiService)
        {
            _realtimeChatService = realtimeChatService;
            _authService = authService;
            _aiService = aiService;

            SendCommand = new AsyncRelayCommand(SendAsync);
        }

        public async Task LoadMessagesAsync()
        {
            var user = _authService.CurrentUser;
            if (user == null) return;

            _chat = await GetOrCreateChatAsync(user.Id);

            var msgs = await _realtimeChatService.GetMessagesAsync(_chat.Id.ToString());

            Messages.Clear();

            foreach (var m in msgs)
            {
                Messages.Add(new ChatMessage
                {
                    ChatId = int.Parse(m.ChatId),
                    Role = m.SenderId == user.Id ? "user" : "assistant",
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                });
            }
        }

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;

            var user = _authService.CurrentUser;
            if (user == null) return;

            if (_chat == null)
                await LoadMessagesAsync();

            // USER MESSAGE
            var userMsg = new ChatMessage
            {
                ChatId = _chat.Id,
                Role = "user",
                Content = MessageText,
                CreatedAt = DateTime.Now
            };

            Messages.Add(userMsg);

            //отправка в Supabase
            await _realtimeChatService.SendMessageAsync(
                _chat.Id.ToString(),
                MessageText,
                user.Id
            );

            // AI RESPONSE
            var aiResponse = await _aiService.SendAsync(MessageText, user);

            var botMsg = new ChatMessage
            {
                ChatId = _chat.Id,
                Role = "assistant",
                Content = aiResponse,
                CreatedAt = DateTime.Now
            };

            Messages.Add(botMsg); 

            MessageText = string.Empty;
        }

        private async Task<Chat> GetOrCreateChatAsync(int userId)
        {
            return new Chat
            {
                Id = userId
            };
        }
    }
}