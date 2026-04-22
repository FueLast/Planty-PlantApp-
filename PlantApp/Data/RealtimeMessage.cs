using System.Text.Json.Serialization;

public class RealtimeMessage
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("chat_id")]
    public int ChatId { get; set; } 

    [JsonPropertyName("sender_id")]
    public string SenderId { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public bool IsMine { get; set; }
}