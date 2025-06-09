using System;
using System.Text.Json;

namespace ShellMuse.Core.Planning;

public record ToolCall(Tool Tool, JsonElement Args)
{
    public static bool TryParse(string json, out ToolCall? call) => TryParse(json, out call, out _);

    public static bool TryParse(string json, out ToolCall? call, out string? error)
    {
        call = null;
        error = null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("tool", out var toolProp))
            {
                error = "Missing 'tool' property";
                return false;
            }
            var name = toolProp.GetString() ?? string.Empty;
            if (!Enum.TryParse<Tool>(name, true, out var tool))
            {
                error = $"Unknown tool '{name}'";
                return false;
            }
            var args = root.TryGetProperty("args", out var a)
                ? JsonDocument.Parse(a.GetRawText()).RootElement
                : default;

            call = new ToolCall(tool, args);
            return true;
        }
        catch (JsonException je)
        {
            error = je.Message;
            return false;
        }
    }
}
