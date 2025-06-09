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
        try
        {
            return await ProcessUtil.RunAsync("rg", $"{query} {path}", cancellationToken);
        }
        catch (Win32Exception)
        {
            return "ripgrep (rg) not found. Please install ripgrep to enable the search tool.";
        }
    }
}
