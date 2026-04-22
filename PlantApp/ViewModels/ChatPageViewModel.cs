using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Data;
using PlantApp.Helpers;
using PlantApp.Services;
using PlantApp.Views.AdditionalViews;
using System.Collections.ObjectModel;

namespace PlantApp.ViewModels
{
    public partial class ChatPageViewModel : ObservableObject
    {
        private readonly RealtimeChatService _realtimeChatService;
        private readonly AIService _aiService;
        private readonly AuthService _authService;
        private readonly PlantService _plantService;

        // числовой id чата из Supabase
        private int _chatId;
        private string _otherUserUuid = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> messages = new();

        [ObservableProperty]
        private string messageText;

        public IAsyncRelayCommand SendCommand { get; }

        public ChatPageViewModel(
            RealtimeChatService realtimeChatService,
            AuthService authService,
            AIService aiService,
            PlantService plantService)
        {
            _realtimeChatService = realtimeChatService;
            _authService = authService;
            _aiService = aiService;
            _plantService = plantService;

            SendCommand = new AsyncRelayCommand(SendAsync);
        }

        public async Task LoadMessagesAsync()
        {
            var user = _authService.CurrentUser;
            if (user == null) return;

            // получаем числовой id чата
            _chatId = await _realtimeChatService.GetOrCreateChatAsync(
                _authService.GetUserUuid(),
                _otherUserUuid);

            var msgs = await _realtimeChatService.GetMessagesAsync(_chatId);
            Messages.Clear();

            foreach (var m in msgs)
            {
                var msg = new ChatMessage
                {
                    ChatId = m.ChatId,
                    Role = m.SenderId == _authService.GetUserUuid() ? "user" : "assistant",
                    CreatedAt = m.CreatedAt
                };

                if (m.Content != null && m.Content.StartsWith("PLANT|"))
                {
                    var parts = m.Content.Split('|');
                    msg.MessageType = "plant";
                    msg.PlantId = int.TryParse(parts.ElementAtOrDefault(1), out var pid) ? pid : 0;
                    msg.PlantName = parts.ElementAtOrDefault(2);
                    msg.PlantImage = parts.ElementAtOrDefault(3);
                }
                else
                {
                    msg.MessageType = "text";
                    msg.Content = m.Content;
                }

                Messages.Add(msg);
            }
        }

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText)) return;

            var user = _authService.CurrentUser;
            if (user == null) return;

            if (_chatId == 0)
                await LoadMessagesAsync();

            var userMsg = new ChatMessage
            {
                ChatId = _chatId,
                Role = "user",
                Content = MessageText,
                CreatedAt = DateTime.Now
            };

            Messages.Add(userMsg);

            await _realtimeChatService.SendMessageAsync(
                _chatId,
                MessageText,
                _authService.GetUserUuid());

            // AI чат — только если нет собеседника
            if (string.IsNullOrEmpty(_otherUserUuid))
            {
                var aiResponse = await _aiService.SendAsync(MessageText, user);
                Messages.Add(new ChatMessage
                {
                    ChatId = _chatId,
                    Role = "assistant",
                    Content = aiResponse,
                    CreatedAt = DateTime.Now
                });
            }

            MessageText = string.Empty;
        }

        // вызывается из SwapPageViewModel при открытии чата
        public async Task Load(string otherUserUuid)
        {
            _otherUserUuid = otherUserUuid;
            await LoadMessagesAsync();
        }

        public async Task SendSystemMessage(string text)
        {
            MessageText = text;
            await SendAsync();
        }

        [RelayCommand]
        public async Task OpenPlant(int plantId)
        {
            var plant = await _plantService.GetUserPlantById(plantId);
            if (plant == null) return;

            var plantEntity = plant.Plant;

            var vm = App.Current.Handler.MauiContext.Services
                .GetRequiredService<PlantDetailsViewModel>();

            vm.Initialize(plantEntity);

            var page = new PlantDetailsPage(vm);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task SendPlantMessage(UserPlant plant)
        {
            var user = _authService.CurrentUser;
            if (user == null) return;

            if (_chatId == 0)
            {
                if (string.IsNullOrEmpty(_otherUserUuid))
                    return;

                await LoadMessagesAsync();
            }

            var content = $"PLANT|{plant.Id}|{plant.CustomName}|{plant.ImageUrl}";

            var msg = new ChatMessage
            {
                ChatId = _chatId,
                Role = "user",
                MessageType = "plant",
                PlantId = plant.Id,
                PlantName = plant.CustomName,
                PlantImage = plant.ImageUrl,
                CreatedAt = DateTime.Now
            };

            Messages.Add(msg);

            await _realtimeChatService.SendMessageAsync(
                _chatId,
                content,
                _authService.GetUserUuid());
        }
    }
}