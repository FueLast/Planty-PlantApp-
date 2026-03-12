using PlantApp.Data;

public class UserPlant
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlantId { get; set; }

    public string CustomName { get; set; }

    public string Description { get; set; }

    public int AgeDays { get; set; }

    public string ImagePath { get; set; }

    public DateTime? LastWatered { get; set; }

    public Plant Plant { get; set; }

    // название растения из энциклопедии
    public string PlantName => Plant?.NamePlant;

    // текст последнего полива
    public string LastWateredText =>
        LastWatered == null
            ? "еще не поливалось"
            : $"полив: {LastWatered.Value:dd.MM}";
}