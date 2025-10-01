using System.Text.Json.Serialization;

namespace SelfResearch.Financial.API.Feature.Wallet;

[JsonConverter(typeof(JsonStringEnumConverter<WalletStateEnumDto>))]
public enum WalletStateEnumDto
{
    Active = 1,
    Suspended = 2,
    Closed = 3
}