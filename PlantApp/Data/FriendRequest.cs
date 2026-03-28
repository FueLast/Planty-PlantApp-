using PlantApp.Data;

public class FriendRequest
{
    public int Id { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; }

    public int ReceiverId { get; set; }
    public User Receiver { get; set; }

    public FriendRequestStatus Status { get; set; } // Pending / Accepted / Rejected

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}