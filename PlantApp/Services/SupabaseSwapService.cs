using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using PlantApp.Data;

namespace PlantApp.Services;

public class SupabaseSwapService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public SupabaseSwapService(HttpClient http, IConfiguration config)
    {
        _http = http;

        var baseUrl = config["Supabase:BaseUrl"];
        _baseUrl = $"{baseUrl}/rest/v1/swap_offers";

        // для отладки:
        System.Diagnostics.Debug.WriteLine($"[SupabaseSwapService] BaseUrl = {_baseUrl}");

        var apiKey = config["Supabase:ApiKey"]; 

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("apikey", apiKey);
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        _http.DefaultRequestHeaders.Add("Prefer", "return=representation");
    }

    public async Task CreateOfferAsync(SwapOffer offer)
    {
        Console.WriteLine($"CHECK USER IN SUPABASE: {offer.OwnerId}");
        Console.WriteLine("=== CREATE OFFER START ===");
        Console.WriteLine($"OwnerId: {offer.OwnerId}");
        Console.WriteLine($"UserPlantId: {offer.UserPlantId}");
        Console.WriteLine($"Desired: {offer.DesiredPlantDescription}"); 

        var payload = new
        {
            owner_id = offer.OwnerId,
            user_plant_id = offer.UserPlantId,
            desired_plant_description = offer.DesiredPlantDescription,
            created_at = DateTime.UtcNow
    };

        var response = await _http.PostAsJsonAsync(_baseUrl, payload);

        var error = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = $"SUPABASE ERROR: {error}";
            System.Diagnostics.Debug.WriteLine(message);

            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                message,
                "OK");

            return; // важно
        }
    }
     

    public async Task<List<SwapOffer>> GetOffersAsync()
    {
        var url = $"{_baseUrl}?order=created_at.desc";

        var result = await _http.GetFromJsonAsync<List<SwapOfferDto>>(url);

        return result?.Select(x => new SwapOffer
        {
            Id = (int)x.id,
            OwnerId = x.owner_id,
            UserPlantId = x.user_plant_id,
            DesiredPlantDescription = x.desired_plant_description,
            CreatedAt = x.created_at,
            OwnerName = x.owner_name
        }).ToList() ?? new List<SwapOffer>();
    }
}