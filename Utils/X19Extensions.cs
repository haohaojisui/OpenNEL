/*
<OpenNEL>
Copyright (C) <2025>  <OpenNEL>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System.Threading.Tasks;
using Codexus.OpenSDK;
using Codexus.OpenSDK.Entities.X19;

namespace OpenNEL.Utils;

public static class X19Extensions
{
    public static async Task<string> ApiRaw(this X19AuthenticationOtp otp, string url, string body)
    {
        var response = await X19.ApiPostAsync(url, body, otp.EntityId, otp.Token);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
}
