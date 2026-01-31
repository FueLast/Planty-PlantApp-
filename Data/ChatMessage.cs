using SQLite;

namespace PlantApp.Data
{
    public class ChatMessage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public DateTime Date { get; set; }

        public int UserProfileId { get; set; }

        [Ignore]
        public UserProfile UserProfile { get; set; }


        public string Profile_Image { get; set; }
    }
}
