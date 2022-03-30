using GbfRaidFinder.Models;

namespace GbfRaidFinder.Hubs;

public interface IGbfRaidHub
{
    Task ReceiveRaidCode(string perceptualHash, string code);

    Task ReceivePreviousRaidCodes(string perceptualHash, GbfRaidCode[] codes);

    Task OnFailure(string message);
}
