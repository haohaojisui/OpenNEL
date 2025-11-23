using System.Text.Json;
using System.Net.WebSockets;

namespace OpenNEL.network;

internal interface IWsHandler
{
    string Type { get; }
    Task ProcessAsync(WebSocket ws, JsonElement root);
}
