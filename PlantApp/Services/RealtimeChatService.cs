using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using System.Diagnostics;

namespace PlantApp.Services
{
    public class RealtimeChatService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public RealtimeChatService(HttpClient http, IConfiguration config)
        {
            _http = http;

            _apiKey = config["Supabase:ApiKey"];
            var baseUrl = config["Supabase:BaseUrl"];
            var endpoint = config["Supabase:MessagesEndpoint"];

            _baseUrl = baseUrl + endpoint;

            if (string.IsNullOrEmpty(_baseUrl))
                throw new Exception("Supabase BaseUrl is missing");

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("apikey", _apiKey);
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _http.DefaultRequestHeaders.Add("Prefer", "return=minimal");
        }

        // единый метод отправки
        public async Task SendMessageAsync(string chatId, string content, int senderId)
        {
            var payload = new
            {
                chat_id = chatId,
                sender_id = senderId,
                content = content,
                created_at = DateTime.UtcNow
            };

            Debug.WriteLine($"SENDING: {System.Text.Json.JsonSerializer.Serialize(payload)}");

            var response = await _http.PostAsJsonAsync(_baseUrl, payload);

            var error = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"STATUS: {response.StatusCode}");
            Debug.WriteLine($"ERROR: {error}");
        }

        public async Task<List<RealtimeMessage>> GetMessagesAsync(string chatId)
        {
            var url = $"{_baseUrl}?chat_id=eq.{chatId}&order=created_at.asc";

            return await _http.GetFromJsonAsync<List<RealtimeMessage>>(url)
                   ?? new List<RealtimeMessage>();
        }
    }
}