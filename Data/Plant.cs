using SQLite;

namespace PlantApp.Data
{
    public class Plant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NamePlant { get; set; }
        public string DescriptionPlant { get; set; }
        public string PlantCare { get; set; }
        public string PlantImage { get; set; }
        public double EstimatedHeight { get; set; }
    }

}
