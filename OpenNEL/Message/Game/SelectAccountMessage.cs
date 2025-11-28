using OpenNEL.Network;
using OpenNEL.type;

using System.Text.Json;

namespace OpenNEL.Message.Game;

internal class SelectAccountMessage : IWsMessage
{
    public string Type => "select_account";
    public async Task<object?> ProcessAsync(JsonElement root)
    {
        var id = root.TryGetProperty("entityId", out var idProp2) ? idProp2.GetString() : null;
        if (string.IsNullOrWhiteSpace(id) || !AppState.Auths.ContainsKey(id))
        {
            return new { type = "notlogin" };
        }
        AppState.SelectedAccountId = id;
        return new { type = "selected_account", entityId = id };
    }
}
