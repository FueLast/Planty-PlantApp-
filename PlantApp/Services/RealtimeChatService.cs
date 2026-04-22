using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PlantApp.Services
{
    public class RealtimeChatService
    {
        private readonly HttpClient _http;
        private readonly string _messagesUrl;
        private readonly string _chatsUrl;
        private readonly string _apiKey;

        public RealtimeChatService(HttpClient http, IConfiguration config)
        {
            _http = http;

            _apiKey = config["Supabase:ApiKey"] ?? string.Empty;
            var baseUrl = config["Supabase:BaseUrl"] ?? string.Empty;

            _messagesUrl = $"{baseUrl}/rest/v1/messages";
            _chatsUrl = $"{baseUrl}/rest/v1/chats";

            _http.DefaultRequestHeaders.Add("apikey", _apiKey);
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _http.DefaultRequestHeaders.Add("Prefer", "return=representation");
        }

        // ───── GetOrCreateChat ─────
        public async Task<int> GetOrCreateChatAsync(string user1Uuid, string user2Uuid)
        {
            var url = $"{_chatsUrl}" +
                      $"?or=(and(user1_id.eq.{user1Uuid},user2_id.eq.{user2Uuid})," +
                      $"and(user1_id.eq.{user2Uuid},user2_id.eq.{user1Uuid}))&limit=1";

            try
            {
                var existing = await _http.GetFromJsonAsync<List<SupabaseChatDto>>(url);

                if (existing != null && existing.Count > 0)
                {
                    Debug.WriteLine($"[Chat] найден чат id={existing[0].id}");
                    return existing[0].id;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Chat] GetChat error: {ex.Message}");
            }

            var payload = new
            {
                user1_id = user1Uuid,
                user2_id = user2Uuid
            };

            var response = await _http.PostAsJsonAsync(_chatsUrl, payload);
            var body = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"[Chat] CreateChat status={response.StatusCode}, body={body}");

            var result = await response.Content.ReadFromJsonAsync<List<SupabaseChatDto>>();

            if (result == null || result.Count == 0)
                throw new Exception($"Не удалось создать чат. Ответ: {body}");

            Debug.WriteLine($"[Chat] создан чат id={result[0].id}");
            return result[0].id;
        }

        // ───── SendMessage ─────
        public async Task SendMessageAsync(int chatId, string content, string senderUuid)
        {
            var payload = new
            {
                chat_id = chatId,
                sender_id = senderUuid,
                content = content
            };

            Debug.WriteLine($"[Chat] Send chat_id={chatId} sender={senderUuid}");

            var response = await _http.PostAsJsonAsync(_messagesUrl, payload);
            var body = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"[Chat] Send status={response.StatusCode}, body={body}");
        }

        // ───── GetMessages ─────
        public async Task<List<RealtimeMessage>> GetMessagesAsync(int chatId)
        {
            try
            {
                var url = $"{_messagesUrl}?chat_id=eq.{chatId}&order=created_at.asc";
                var result = await _http.GetFromJsonAsync<List<RealtimeMessage>>(url);

                Debug.WriteLine($"[Chat] GetMessages count={result?.Count}");

                return result ?? new List<RealtimeMessage>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Chat] GetMessages error: {ex.Message}");
                return new List<RealtimeMessage>();
            }
        }
    }

    public class SupabaseChatDto
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("user1_id")]
        public string? user1_id { get; set; }

        [JsonPropertyName("user2_id")]
        public string? user2_id { get; set; }
    }
}