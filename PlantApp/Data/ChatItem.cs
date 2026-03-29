public class ChatItem
{
    public int? UserId { get; set; } // null = AI
    public string Title { get; set; }
    public string LastMessage { get; set; }
    public DateTime? LastMessageTime { get; set; }

    public bool IsAI => UserId == null;
}