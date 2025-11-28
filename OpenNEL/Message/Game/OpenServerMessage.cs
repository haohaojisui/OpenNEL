using System.Text.Json;
using Codexus.Cipher.Entities.WPFLauncher.NetGame;
using Codexus.Cipher.Entities;
using OpenNEL.Network;
using OpenNEL.type;
using OpenNEL.Utils; 
using OpenNEL.Manager;
using Serilog;

namespace OpenNEL.Message.Game;

internal class OpenServerMessage : IWsMessage
{
    public string Type => "open_server";
    public async Task<object?> ProcessAsync(JsonElement root)
    {
        var serverId = root.TryGetProperty("serverId", out var sid) ? sid.GetString() : null;
        var offset = root.TryGetProperty("offset", out var off) && off.ValueKind == JsonValueKind.Number ? off.GetInt32() : 0;
        var length = root.TryGetProperty("length", out var len) && len.ValueKind == JsonValueKind.Number ? len.GetInt32() : 15;
        var last = UserManager.Instance.GetLastAvailableUser();
        if (last == null) return new { type = "notlogin" };
        if (string.IsNullOrWhiteSpace(serverId))
        {
            return new { type = "server_roles_error", message = "参数错误" };
        }
        try
        {
            if(AppState.Debug)Log.Information("打开服务器: serverId={ServerId}, account={AccountId}", serverId, last.UserId);
            var availableNetGames = AppState.X19.GetAvailableNetGames(last.UserId, last.AccessToken, offset, length);
            if (availableNetGames.Data.Length != 0)
            {
                var entities = AppState.X19.QueryNetGameItemByIds(last.UserId, last.AccessToken, availableNetGames.Data.Select(netgame => netgame.EntityId).ToArray());
                for (int i = 0; i < availableNetGames.Data.Length; i++)
                {
                    availableNetGames.Data[i].TitleImageUrl = entities.Data[i].TitleImageUrl;
                }
            }
            var items = availableNetGames.Data.Select(s => new { type = "net_games", entityId = s.EntityId, name = s.Name, imageUrl = s.TitleImageUrl }).ToArray();
            return items;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "获取服务器角色失败: serverId={ServerId}", serverId);
            return new { type = "server_roles_error", message = "获取失败" };
        }
    }
}
