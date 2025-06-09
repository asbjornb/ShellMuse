using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class SearchTool : ITool
{
    public async Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var query = args.GetProperty("query").GetString() ?? string.Empty;
        var path = args.TryGetProperty("path", out var p) ? p.GetString() ?? "." : ".";
        var command = "rg";
        try
        {
            return await ProcessUtil.RunAsync(command, $"{query} {path}", cancellationToken);
        }
        catch (Win32Exception)
        {
            return "ripgrep (rg) failed to start. Is it installed and on your PATH?";
        }
    }
}
