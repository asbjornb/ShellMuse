using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public class SearchTool : ITool
{
    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var query = args.GetProperty("query").GetString() ?? string.Empty;
        var path = args.TryGetProperty("path", out var p) ? p.GetString() ?? "." : ".";
        return ProcessUtil.RunAsync("rg", $"{query} {path}", cancellationToken);
    }
}
