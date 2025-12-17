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
using System.Text.Json.Serialization;

namespace OpenNEL.Entities.Web.NEL;

public class EntityQueryGameSessions
{
	[JsonPropertyName("id")]
	public required string Id { get; set; }

	[JsonPropertyName("server_name")]
	public required string ServerName { get; set; }

	[JsonPropertyName("guid")]
	public required string Guid { get; set; }

	[JsonPropertyName("character_name")]
	public required string CharacterName { get; set; }

	[JsonPropertyName("server_version")]
	public required string ServerVersion { get; set; }

	[JsonPropertyName("status_text")]
	public required string StatusText { get; set; }

	[JsonPropertyName("type")]
	public required string Type { get; set; }

	[JsonPropertyName("progress_value")]
	public required int ProgressValue { get; set; }

	[JsonPropertyName("local_address")]
	public string LocalAddress { get; set; } = string.Empty;
}
