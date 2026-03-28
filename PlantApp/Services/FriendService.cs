using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

public class FriendService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public FriendService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    // отправка заявки
    public async Task SendRequestAsync(int senderId, int receiverId)
    {
        using var db = await _factory.CreateDbContextAsync();

        var exists = await db.FriendRequests.AnyAsync(x =>
            (x.SenderId == senderId && x.ReceiverId == receiverId) ||
            (x.SenderId == receiverId && x.ReceiverId == senderId));

        if (exists) return;

        //db.FriendRequests.Add(new FriendRequest
        //{
        //    SenderId = senderId,
        //    ReceiverId = receiverId,
        //    Status = FriendRequestStatus.Pending
        //});
        db.FriendRequests.Add(new FriendRequest
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = FriendRequestStatus.Accepted // ВАЖНО
        });

        await db.SaveChangesAsync();
    }

    // принять заявку
    public async Task AcceptRequestAsync(int requestId)
    {
        using var db = await _factory.CreateDbContextAsync();

        var request = await db.FriendRequests.FindAsync(requestId);

        if (request == null) return;

        request.Status = FriendRequestStatus.Accepted;

        await db.SaveChangesAsync();
    }

    // получить друзей пользователя
    public async Task<List<User>> GetFriendsAsync(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        var friends = await db.FriendRequests
            .Include(x => x.Sender).ThenInclude(s => s.Profile)
            .Include(x => x.Receiver).ThenInclude(r => r.Profile)
            .Where(x => x.Status == FriendRequestStatus.Accepted &&
                       (x.SenderId == userId || x.ReceiverId == userId))
            .Select(x => x.SenderId == userId ? x.Receiver : x.Sender)
            .ToListAsync();

        return friends;
    }

    //проверка при поиске
    public async Task<HashSet<int>> GetFriendIdsAsync(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        return await db.FriendRequests
            .Where(x => x.Status == FriendRequestStatus.Accepted &&
                       (x.SenderId == userId || x.ReceiverId == userId))
            .Select(x => x.SenderId == userId ? x.ReceiverId : x.SenderId)
            .ToHashSetAsync();
    }

    // входящие заявки
    public async Task<List<FriendRequest>> GetIncomingRequestsAsync(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        return await db.FriendRequests
            .Include(x => x.Sender)
            .Where(x => x.ReceiverId == userId && x.Status == FriendRequestStatus.Pending)
            .ToListAsync();
    }
}