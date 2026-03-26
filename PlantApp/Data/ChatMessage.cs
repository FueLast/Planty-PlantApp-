namespace PlantApp.Data
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatId { get; set; }

        public string Role { get; set; } // user / assistant
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public Chat Chat { get; set; }
    }
}