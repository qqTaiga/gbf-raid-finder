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
            var perceptualHashUlong = ulong.Parse(perceptualHash);

            var codes = _inMemService.Bosses[perceptualHashUlong].RaidCodes.ToArray();
            await Clients.Client(Context.ConnectionId)
                .ReceivePreviousRaidCodes(perceptualHash, codes);
        }
        catch (FormatException)
        {
            await Clients.Caller.OnFailure($"Cannot parse {perceptualHash} to ulong");
        }
    }

    public async Task LeaveRaid(string perceptualHash)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, perceptualHash);
    }

    public async Task PushRaidCode(string perceptualHash, string raidCode)
    {
        await Clients.Group(perceptualHash).ReceiveRaidCode(perceptualHash, raidCode);
    }
}
