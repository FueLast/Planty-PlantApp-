 

namespace PlantApp.Data
{
    public class ChatMessage
    { 
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public DateTime Date { get; set; }

        public int UserProfileId { get; set; }
         
        public UserProfile UserProfile { get; set; }


        public string Profile_Image { get; set; }
    }
}
