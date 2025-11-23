using Codexus.OpenSDK;
using OpenNEL.type;
using OpenNEL.network;

using System.Text.Json;
using System.Text;
using Serilog;

namespace OpenNEL.HandleWebSocket.Game;

internal class CookieLoginHandler : IWsHandler
{
    public string Type => "cookie_login";
    public async Task ProcessAsync(System.Net.WebSockets.WebSocket ws, JsonElement root)
    {
        var cookie = root.TryGetProperty("cookie", out var c) ? c.GetString() : null;
        if (string.IsNullOrWhiteSpace(cookie))
        {
            var err = JsonSerializer.Serialize(new { type = "login_error", message = "cookie为空" });
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(err)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            return;
        }
        try
        {
            if (AppState.Services == null || AppState.Services.X19 == null)
            {
                var err = JsonSerializer.Serialize(new { type = "login_error", message = "系统未初始化" });
                await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(err)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
                return;
            }
            await AppState.Services.X19.InitializeDeviceAsync();
            var cont = await AppState.Services.X19.ContinueAsync(cookie);
            var authOtp = cont.Item1;
            var channel = cont.Item2;
            await X19.InterconnectionApi.LoginStart(authOtp.EntityId, authOtp.Token);
            Log.Information("Cookie登录成功: {Id}, {Channel}", authOtp.EntityId, channel);
            AppState.Accounts[authOtp.EntityId] = channel;
            AppState.Auths[authOtp.EntityId] = authOtp;
            AppState.SelectedAccountId = authOtp.EntityId;
            var ok = JsonSerializer.Serialize(new { type = "Success_login", entityId = authOtp.EntityId, channel });
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(ok)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "Cookie登录失败");
            var err = JsonSerializer.Serialize(new { type = "login_error", message = "登录失败" });
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(err)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }
    }
}