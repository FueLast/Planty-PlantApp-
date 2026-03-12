using PlantApp.Data;

public class FavoritePlant
{
    public int UserId { get; set; }

    public int PlantId { get; set; }

    public Plant Plant { get; set; }
}