using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Helpers;
using PlantApp.Services;
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

        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public UserChatViewModel(
            RealtimeChatService chatService,
            AuthService authService,
            IDbContextFactory<AppDbContext> dbFactory) 
        {
            _chatService = chatService;
            _authService = authService;
            _dbFactory = dbFactory;
            SendCommand = new AsyncRelayCommand(SendAsync);
        }

        public async Task Init(int friendId)
        {
            var myId = _authService.GetUserUuid();

            using var db = await _dbFactory.CreateDbContextAsync();
            var friend = await db.Users.FindAsync(friendId);
            var friendUuid = friend?.SupabaseUuid ?? friendId.ToString(); // fallback

            _chatId = ChatHelper.GetChatId(myId, friendUuid);
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            if (string.IsNullOrEmpty(_chatId)) return;

            var msgs = await _chatService.GetMessagesAsync(_chatId);
            Messages.Clear();

            var myId = _authService.GetUserUuid(); // string

            foreach (var m in msgs)
            {
                m.IsMine = m.SenderId == myId; // string == string  
                Messages.Add(m);
            }
        }

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText)) return;
            if (string.IsNullOrEmpty(_chatId)) return;

            var myId = _authService.GetUserUuid(); // string uuid

            var msg = new RealtimeMessage
            {
                ChatId = _chatId,
                SenderId = myId, // string  
                Content = MessageText,
                CreatedAt = DateTime.Now,
                IsMine = true
            };

            Messages.Add(msg);

            await _chatService.SendMessageAsync(
                _chatId,
                MessageText,
                myId// string ✅
            );

            MessageText = string.Empty;
        }


    }
}