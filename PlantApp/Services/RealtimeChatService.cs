using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;

namespace PlantApp.Services
{
    public class RealtimeChatService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public RealtimeChatService(HttpClient http, IConfiguration config)
        {
            _http = http;

            var apiKey = config["SupabaseChatApiKey"];

            _baseUrl = config["Supabase:BaseUrl"];

            if (string.IsNullOrEmpty(_baseUrl))
            {
                throw new Exception("Supabase BaseUrl is missing in configuration!");
            }

            // настраиваем заголовки один раз при создании сервиса
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("apikey", apiKey);
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task SendMessageAsync(RealtimeMessage message)
        {
            await _http.PostAsJsonAsync(_baseUrl, message);
        }

        public async Task<List<RealtimeMessage>> GetMessagesAsync(string chatId)
        {
            // формируем строку запроса с параметрами фильтрации
            var url = $"{_baseUrl}?chat_id=eq.{chatId}&order=created_at.asc";

            return await _http.GetFromJsonAsync<List<RealtimeMessage>>(url)
                   ?? new List<RealtimeMessage>();
        }
    }
}