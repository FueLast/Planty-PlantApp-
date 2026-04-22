using Microsoft.Extensions.Configuration;
using PlantApp.Data;
using System.Net.Http.Json;
using System.Text.Json;

namespace PlantApp.Services
{
    public class AIService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public AIService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["YandexApiKey"];
        }

        public async Task<string> SendAsync(string message, User user)
        {
            var prompt = BuildPrompt(message, user);

            var request = new
            {
                modelUri = "gpt://b1g8pi1r4s2sgj3nqtcs/yandexgpt-5-lite/latest",
                completionOptions = new
                {
                    temperature = 0.6,
                    maxTokens = 3000
                },
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        text = prompt
                    }
                }
            };

            try
            {
                var response = await _http.PostAsJsonAsync(
                    "https://llm.api.cloud.yandex.net/foundationModels/v1/completion",
                    request); 

                var jsonString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"Ошибка API: {response.StatusCode}. Детали: {jsonString}";
                }

                using var doc = JsonDocument.Parse(jsonString);

                return doc.RootElement
                    .GetProperty("result")
                    .GetProperty("alternatives")[0]
                    .GetProperty("message")
                    .GetProperty("text")
                    .GetString();
            }
            catch (Exception ex)
            {
                return $"Ошибка AI: {ex.Message}";
            }
        }

        private string BuildPrompt(string message, User user)
        {
            return $@"
Ты — помощник по растениям 🌿

Имя: {user?.Profile?.UserName ?? "Гость"}

Вопрос:
{message}
";
        }
    }
}