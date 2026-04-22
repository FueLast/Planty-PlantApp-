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

        public string MessageType { get; set; } = "text"; // text | plant

        public int? PlantId { get; set; }
        public string? PlantName { get; set; }
        public string? PlantImage { get; set; }
    }
}