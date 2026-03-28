using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.ViewModels;

public partial class FriendProfileViewModel : ObservableObject
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    [ObservableProperty]
    private UserProfile profile;

    public FriendProfileViewModel(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task Load(int userId)
    {
        using var db = await _factory.CreateDbContextAsync();

        Profile = await db.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}