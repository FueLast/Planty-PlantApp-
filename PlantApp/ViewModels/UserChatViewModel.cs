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
        private int _chatId;

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
            var myUuid = _authService.GetUserUuid();

            using var db = await _dbFactory.CreateDbContextAsync();
            var friend = await db.Users.FindAsync(friendId);
            var friendUuid = friend?.SupabaseUuid;

            if (string.IsNullOrEmpty(friendUuid))
            {
                Debug.WriteLine("friend uuid not found");
                return;
            }

            // получаем числовой id чата из Supabase
            _chatId = await _chatService.GetOrCreateChatAsync(myUuid, friendUuid);
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            if (_chatId == 0) return;

            var msgs = await _chatService.GetMessagesAsync(_chatId);
            Messages.Clear();

            var myUuid = _authService.GetUserUuid();

            foreach (var m in msgs)
            {
                m.IsMine = m.SenderId == myUuid;
                Messages.Add(m);
            }
        }

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText)) return;
            if (_chatId == 0) return;

            var myUuid = _authService.GetUserUuid();

            var msg = new RealtimeMessage
            {
                ChatId = _chatId,
                SenderId = myUuid,
                Content = MessageText,
                CreatedAt = DateTime.Now,
                IsMine = true
            };

            Messages.Add(msg);

            await _chatService.SendMessageAsync(_chatId, MessageText, myUuid);

            MessageText = string.Empty;
        }


    }
}