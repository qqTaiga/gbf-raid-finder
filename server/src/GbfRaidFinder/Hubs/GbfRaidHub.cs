using GbfRaidFinder.Services;
using Microsoft.AspNetCore.SignalR;

namespace GbfRaidFinder.Hubs;

public class GbfRaidHub : Hub<IGbfRaidHub>
{
    private readonly IInMemBossesService _inMemService;

    public GbfRaidHub(IInMemBossesService inMemService)
    {
        _inMemService = inMemService;
    }

    public async Task JoinRaid(string perceptualHash)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, perceptualHash);
        try
        {
            if (!_inMemService.Bosses.ContainsKey(perceptualHash))
                return;

            var codes = _inMemService.Bosses[perceptualHash].RaidCodes.ToArray();
            await Clients.Client(Context.ConnectionId)
                .ReceivePreviousRaidCodes(perceptualHash, codes);
        }
        catch (FormatException)
        {
            await Clients.Caller.OnFailure($"Cannot parse {perceptualHash} to ulong");
        }
    }

    public async Task LeaveRaid(string perceptualHash)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, perceptualHash);
}
