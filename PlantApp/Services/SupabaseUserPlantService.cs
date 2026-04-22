using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace PlantApp.Services
{
    public class SupabaseUserPlantService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public SupabaseUserPlantService(HttpClient http, IConfiguration config)
        {
            _http = http;

            var baseUrl = config["Supabase:BaseUrl"];
            _baseUrl = $"{baseUrl}/rest/v1/user_plants";

            var apiKey = config["Supabase:ApiKey"];
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("apikey", apiKey);
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _http.DefaultRequestHeaders.Add("Prefer", "return=representation");
        }

        // возвращает id созданной записи в Supabase
        public async Task<int?> AddAsync(
            string userUuid,
            int plantId,
            string customName,
            string description,
            string imageUrl)
        {
            var payload = new
            {
                user_id = userUuid,
                plant_id = plantId,
                custom_name = customName,
                description = description,
                image_url = imageUrl,
                created_at = DateTime.UtcNow
            };

            var response = await _http.PostAsJsonAsync(_baseUrl, payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"SUPABASE user_plants ERROR: {error}");
                return null;
            }

            var result = await response.Content
                .ReadFromJsonAsync<List<SupabaseUserPlantDto>>();

            return result?.FirstOrDefault()?.id;
        }
    }

    public class SupabaseUserPlantDto
    {
        public int id { get; set; }
    }
}