using Microsoft.AspNetCore.SignalR;

namespace GbfRaidFinder.Hubs;

public class GbfRaidHub : Hub
{
    public async Task JoinRaidRoomAsync(string name)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, name);
        await Clients.Group(name).SendAsync("messageReceived", "a");
    }
}
