using OpenNEL.network;
using OpenNEL.type;
using System.Text.Json;

namespace OpenNEL.HandleWebSocket.Game;

internal class ListChannelsHandler : IWsHandler
{
    public string Type => "list_channels";
    public async Task<object?> ProcessAsync(JsonElement root)
    {
        var items = AppState.Channels.Values.Select(ch => new {
            serverId = ch.ServerId,
            serverName = ch.ServerName,
            playerId = ch.PlayerId,
            roleName = ch.RoleName,
            tcp = "127.0.0.1:" + ch.LocalPort,
            forward = ch.ForwardHost + ":" + ch.ForwardPort,
            address = ch.Ip + ":" + ch.Port,
            identifier = ch.Identifier.ToString()
        }).ToArray();
        return new { type = "channels", items };
    }
}
