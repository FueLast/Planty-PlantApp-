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

        private Chat _chat;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> messages = new();

        [ObservableProperty]
        private string messageText;

        private string _otherUserId = string.Empty;// swap, для прогрузки собеседника
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

            _chat = await GetOrCreateChatAsync(_authService.GetUserUuid()); // string uuid

            var msgs = await _realtimeChatService.GetMessagesAsync(_chat.Id.ToString());
            Messages.Clear();

            foreach (var m in msgs)
            {
                var msg = new ChatMessage
                {
                    ChatId = int.Parse(m.ChatId),
                    Role = m.SenderId == _authService.GetUserUuid() ? "user" : "assistant", // string == string
                    CreatedAt = m.CreatedAt
                };

                if (m.Content.StartsWith("PLANT|"))
                {
                    var parts = m.Content.Split('|');
                    msg.MessageType = "plant";
                    msg.PlantId = int.Parse(parts[1]);
                    msg.PlantName = parts[2];
                    msg.PlantImage = parts[3];
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

            await _realtimeChatService.SendMessageAsync(
                _chat.Id.ToString(),
                MessageText,
                _authService.GetUserUuid() // string uuid 
            );

            if (string.IsNullOrEmpty(_otherUserId)) // AI чат
            {
                var aiResponse = await _aiService.SendAsync(MessageText, user);
                Messages.Add(new ChatMessage
                {
                    ChatId = _chat.Id,
                    Role = "assistant",
                    Content = aiResponse,
                    CreatedAt = DateTime.Now
                });
            }
        }

        public async Task Load(string otherUserId)  // string  
        {
            _otherUserId = otherUserId;
            await LoadMessagesAsync();
        }

        private async Task<Chat> GetOrCreateChatAsync(string otherUserId) // string  
        {
            var currentUserId = _authService.GetUserUuid(); // string uuid
            var chatId = ChatHelper.GetChatId(currentUserId, otherUserId); // string, string  

            return new Chat
            {
                Id = Math.Abs(chatId.GetHashCode())
            };
        }

        public async Task SendSystemMessage(string text)
        {
            // Устанавливаем текст и переиспользуем существующую логику отправки
            MessageText = text;
            await SendAsync();
        }

        [RelayCommand]
        public async Task OpenPlant(int plantId)
        {
            var plant = await _plantService.GetUserPlantById(plantId);
            if (plant == null) return;

            // берем реальную сущность Plant
            var plantEntity = plant.Plant;

            // достаем VM из DI
            var vm = App.Current.Handler.MauiContext.Services
                .GetRequiredService<PlantDetailsViewModel>();

            // инициализация
            vm.Initialize(plantEntity);

            var page = new PlantDetailsPage(vm);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task SendPlantMessage(UserPlant plant)
        {
            var user = _authService.CurrentUser;
            if (user == null) return;

            if (_chat == null)
            {
                if (string.IsNullOrEmpty(_otherUserId)) // string проверка (было == 0)
                    return;

                _chat = await GetOrCreateChatAsync(_otherUserId);
            }

            var content = $"PLANT|{plant.Id}|{plant.CustomName}|{plant.ImageUrl}";

            var msg = new ChatMessage
            {
                ChatId = _chat.Id,
                Role = "user",
                MessageType = "plant",
                PlantId = plant.Id,
                PlantName = plant.CustomName,
                PlantImage = plant.ImageUrl,
                CreatedAt = DateTime.Now
            };

            Messages.Add(msg);

            await _realtimeChatService.SendMessageAsync(
                _chat.Id.ToString(),
                content,
                _authService.GetUserUuid() // string uuid  
            );
        }


    }
}