using OpenNEL.network;
using System.Text;
using System.Text.Json;
using Codexus.Development.SDK.Manager;
using Serilog;

namespace OpenNEL.HandleWebSocket.Plugin;

internal class RestartGatewayHandler : IWsHandler
{
    public string Type => "restart";
    public async Task ProcessAsync(System.Net.WebSockets.WebSocket ws, JsonElement root)
    {
        Log.Information("重启网关");
        PluginManager.RestartGateway();
        var ack = JsonSerializer.Serialize(new { type = "restart_ack" });
        await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(ack)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
    }
}