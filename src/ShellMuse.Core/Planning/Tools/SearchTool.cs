using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class SearchTool : ITool
{
    private static string ResolveRgCommand()
    {
        var custom = Environment.GetEnvironmentVariable("SHELLMUSE_RIPGREP");
        return string.IsNullOrWhiteSpace(custom) ? "rg" : custom;
    }

    public async Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var query = args.GetProperty("query").GetString() ?? string.Empty;
        var path = args.TryGetProperty("path", out var p) ? p.GetString() ?? "." : ".";
        var command = ResolveRgCommand();
        try
        {
            return await ProcessUtil.RunAsync(command, $"{query} {path}", cancellationToken);
        }
        catch (Win32Exception) when (!File.Exists(command))
        {
            return $"ripgrep not found (tried '{command}'). Install ripgrep or set SHELLMUSE_RIPGREP.";
        }
        catch (Win32Exception)
        {
            return "ripgrep (rg) failed to start.";
        }
    }
}
