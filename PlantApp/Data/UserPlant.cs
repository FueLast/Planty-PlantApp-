using PlantApp.Data;

public class UserPlant
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PlantId { get; set; }
    public string CustomName { get; set; }
    public string Description { get; set; }
    public string AgeDays { get; set; }
    public string ImageUrl { get; set; }
    public DateTime? LastWatered { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Plant Plant { get; set; }

    public int? SupabaseId { get; set; } 

    public string PlantName => Plant?.NamePlant;
    public string LastWateredText =>
        LastWatered == null
            ? "еще не поливалось"
            : $"полив: {LastWatered.Value:dd.MM}";
}