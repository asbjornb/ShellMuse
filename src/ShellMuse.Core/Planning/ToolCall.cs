using System;
using System.Text.Json;

namespace ShellMuse.Core.Planning;

public record ToolCall(Tool Tool, JsonElement Args)
{
    public static bool TryParse(string json, out ToolCall? call)
    {
        call = null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("tool", out var toolProp))
                return false;
            var name = toolProp.GetString() ?? string.Empty;
            if (!Enum.TryParse<Tool>(name, true, out var tool))
                return false;
            var args = root.TryGetProperty("args", out var a)
                ? JsonDocument.Parse(a.GetRawText()).RootElement : default;

            call = new ToolCall(tool, args);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
