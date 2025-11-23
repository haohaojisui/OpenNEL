using OpenNEL.network;
using System.Text;
using System.Text.Json;
using Codexus.Development.SDK.Manager;
using OpenNEL.type;
using Serilog;

namespace OpenNEL.HandleWebSocket.Plugin;

internal class UninstallPluginHandler : IWsHandler
{
    public string Type => "uninstall_plugin";
    public async Task ProcessAsync(System.Net.WebSockets.WebSocket ws, JsonElement root)
    {
        var pluginId = root.TryGetProperty("pluginId", out var idEl) ? idEl.GetString() : null;
        if (!string.IsNullOrWhiteSpace(pluginId))
        {
            Log.Information("卸载插件 {PluginId}", pluginId);
            PluginManager.Instance.UninstallPlugin(pluginId);
            AppState.WaitRestartPlugins[pluginId] = true;
        }
        var upd = JsonSerializer.Serialize(new { type = "installed_plugins_updated" });
        await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(upd)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        var items = PluginManager.Instance.Plugins.Values.Select(plugin => new {
            identifier = plugin.Id,
            name = plugin.Name,
            version = plugin.Version,
            description = plugin.Description,
            author = plugin.Author,
            status = plugin.Status
        }).ToArray();
        var msg = JsonSerializer.Serialize(new { type = "installed_plugins", items });
        await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
    }
}