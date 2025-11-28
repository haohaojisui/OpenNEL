using OpenNEL.Network;
using OpenNEL.type;
using System.Text.Json;

namespace OpenNEL.Message.Game;

internal class ListAccountsMessage : IWsMessage
{
    public string Type => "list_accounts";
    public async Task<object?> ProcessAsync(JsonElement root)
    {
        var items = AppState.Accounts.Select(kv => new { entityId = kv.Key, channel = kv.Value }).ToArray();
        return new { type = "accounts", items };
    }
}
