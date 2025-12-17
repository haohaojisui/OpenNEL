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
using System;
using Codexus.Cipher.Protocol;

namespace OpenNEL.type;
using System.Collections.Concurrent;

internal static class AppState
{
    private static Com4399? _com4399;
    public static Com4399 Com4399 => _com4399 ??= new Com4399();

    private static G79? _g79;
    public static G79 G79 => _g79 ??= new G79();

    private static WPFLauncher? _x19;
    public static WPFLauncher X19 => _x19 ??= new WPFLauncher();
    
    public static Services? Services;
    public static ConcurrentDictionary<string, bool> WaitRestartPlugins { get; } = new();
    public static bool Debug;
    public static bool Pre = AppInfo.AppVersion.Contains("pre", StringComparison.OrdinalIgnoreCase);
}
