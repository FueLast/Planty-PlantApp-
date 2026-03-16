 

namespace PlantApp.Data
{
    public class Plant
    { 
        public int Id { get; set; }

        public string NamePlant { get; set; }
        public string DescriptionPlant { get; set; }
        public string PlantCare { get; set; }
        public string PlantImage { get; set; }
        public double EstimatedHeight { get; set; }

        public string SearchNames { get; set; } //используется для поиска в MainPage 
    }

}
