using OpenNEL.network;
using System.Text.Json;
using Codexus.Development.SDK.Manager;
using OpenNEL.type;

namespace OpenNEL.HandleWebSocket.Plugin;

internal class ListInstalledPluginsHandler : IWsHandler
{
    public string Type => "list_installed_plugins";
    public async Task<object?> ProcessAsync(JsonElement root)
    {
        var items = PluginManager.Instance.Plugins.Values.Select(plugin => new {
            identifier = plugin.Id,
            name = plugin.Name,
            version = plugin.Version,
            description = plugin.Description,
            author = plugin.Author,
            status = plugin.Status,
            waitingRestart = AppState.WaitRestartPlugins.ContainsKey(plugin.Id)
        }).ToArray();
        return new { type = "installed_plugins", items };
    }
}
