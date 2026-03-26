namespace PlantApp.Data
{
    public class Chat
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public List<ChatMessage> Messages { get; set; } = new();
    }
}