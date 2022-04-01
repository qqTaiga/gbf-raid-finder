using GbfRaidFinder.Models;

namespace GbfRaidFinder.Hubs;

public interface IGbfRaidHub
{
    Task ReceiveRaidCode(string perceptualHash, GbfRaidCode code);

    Task ReceivePreviousRaidCodes(string perceptualHash, GbfRaidCode[] codes);

    Task OnFailure(string message);
}
