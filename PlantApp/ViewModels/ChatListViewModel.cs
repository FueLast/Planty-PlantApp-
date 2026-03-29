using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Services;
using PlantApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.ViewModels
{
    public partial class ChatListViewModel : ObservableObject
    {
        private readonly FriendService _friendService;
        private readonly AuthService _authService;
        private readonly INavigationService _navigation;

        [ObservableProperty]
        private ObservableCollection<ChatItem> chats = new();

        public ChatListViewModel(
            FriendService friendService,
            AuthService authService,
            INavigationService navigation)
        {
            _friendService = friendService;
            _authService = authService;
            _navigation = navigation;
        }

        public async Task LoadAsync()
        {
            Chats.Clear();

            // YandexAI чат (ВСЕГДА ПЕРВЫЙ)
            Chats.Add(new ChatItem
            {
                Title = "🌿 Yandex AI",
                LastMessage = "Помощник по растениям",
                LastMessageTime = DateTime.Now
            });

            var userId = _authService.GetUserId();
            var friends = await _friendService.GetFriendsAsync(userId);

            foreach (var f in friends)
            {
                Chats.Add(new ChatItem
                {
                    UserId = f.Id,
                    Title = f.Profile?.UserName ?? f.Login,
                    LastMessage = "Напиши сообщение...",
                    LastMessageTime = DateTime.Now
                });
            }
        }

        [RelayCommand]
        private async Task OpenChat(ChatItem item)
        {
            if (item.IsAI)
            {
                await _navigation.NavigateToAsync<ChatPage>(); // AI
            }
            else
            {
                await _navigation.NavigateToAsync<UserChatPage, int>(item.UserId.Value); // ВАЖНО
            }
        }

    }
}
