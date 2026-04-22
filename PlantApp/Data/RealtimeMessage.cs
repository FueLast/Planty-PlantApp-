using System.Text.Json.Serialization;

public class RealtimeMessage
{
    [JsonIgnore]
    public long Id { get; set; }

    [JsonPropertyName("chat_id")]
    public string ChatId { get; set; }

    [JsonPropertyName("sender_id")]
    public string SenderId { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public bool IsMine { get; set; }
}