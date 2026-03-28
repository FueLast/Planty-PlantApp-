public class RealtimeMessage
{
    public long Id { get; set; }
    public string ChatId { get; set; }

    public int SenderId { get; set; }
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}