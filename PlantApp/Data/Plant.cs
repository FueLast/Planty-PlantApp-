 

namespace PlantApp.Data
{
    public class Plant
    { 
        public int Id { get; set; }

        public string NamePlant { get; set; }
        public string DescriptionPlant { get; set; }
        public string PlantCare { get; set; }
        public string PlantImage { get; set; }

        // уход растения
        public string Care_Light { get; set; }
        public string Care_Watering { get; set; }
        public string Care_Temperature { get; set; }
        public string Care_Complexity { get; set; }

        //особенность растения
        public string Feature_One { get; set; }
        public string Feature_Two { get; set; }
        public string Feature_Three { get; set; }
        public string Feature_Four { get; set; }

        public double EstimatedHeight { get; set; }

        public string SearchNames { get; set; } //используется для поиска в MainPage 
    }

}
