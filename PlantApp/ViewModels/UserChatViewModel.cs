using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Services;
using PlantApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.ViewModels
{
    public partial class UserChatViewModel : ObservableObject
    {
        private readonly RealtimeChatService _chatService;
        private readonly AuthService _authService;
        private CancellationTokenSource _cts;

        private int _friendId;
        private string _chatId;

        [ObservableProperty]
        private ObservableCollection<RealtimeMessage> messages = new();

        public int MyUserId => _authService.GetUserId();

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
            _friendId = friendId;

            var myId = _authService.GetUserId();
            _chatId = ChatHelper.GetChatId(myId, friendId);

            await LoadMessages();
            StartListening(); 
        }

        private async Task LoadMessages()
        {
            var msgs = await _chatService.GetMessagesAsync(_chatId);

            Messages.Clear();
            foreach (var m in msgs)
                Messages.Add(m);

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

            var msg = new RealtimeMessage
            {
                ChatId = _chatId,
                SenderId = _authService.GetUserId(),
                Content = MessageText,
                CreatedAt = DateTime.Now
            };

            Messages.Add(msg);

            await _chatService.SendMessageAsync(msg);

            MessageText = string.Empty;
        }

        public void StartListening()
        {
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    await LoadMessages();

                    await Task.Delay(2000); // каждые 2 сек
                }
            });
        }

        public void StopListening()
        {
            _cts?.Cancel();
        }

    }
}
