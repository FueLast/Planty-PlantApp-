using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Services;
using PlantApp.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PlantApp.ViewModels
{
    public partial class UserChatViewModel : ObservableObject
    {
        private readonly RealtimeChatService _chatService;
        private readonly AuthService _authService;

        private CancellationTokenSource _cts;
        private string _chatId;

        [ObservableProperty]
        private ObservableCollection<RealtimeMessage> messages = new();

        [ObservableProperty]
        private string messageText;

        public IAsyncRelayCommand SendCommand { get; }

        public UserChatViewModel(
            RealtimeChatService chatService,
            AuthService authService)
        {
            _chatService = chatService;
            _authService = authService;

            SendCommand = new AsyncRelayCommand(SendAsync);
        }

        public async Task Init(int friendId)
        {
            var myId = _authService.GetUserId();
            _chatId = ChatHelper.GetChatId(myId, friendId);

            await LoadMessages();
            StartListening();
        }

        private async Task LoadMessages()
        {
            if (string.IsNullOrEmpty(_chatId))
                return;

            var msgs = await _chatService.GetMessagesAsync(_chatId);

            Messages.Clear();

            var myId = _authService.GetUserId();

            foreach (var m in msgs)
            {
                m.IsMine = m.SenderId == myId;
                Messages.Add(m);
            }
        }

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;

            var myId = _authService.GetUserId();

            await _chatService.SendMessageAsync(
                _chatId,
                MessageText,
                myId
            );

            MessageText = string.Empty;
        }

        private void StartListening()
        {
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    await LoadMessages();
                    await Task.Delay(2000);
                }
            });
        }

        public void StopListening()
        {
            _cts?.Cancel();
        }
    }
}