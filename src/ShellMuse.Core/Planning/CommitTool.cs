using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public class CommitTool : ITool
{
    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var message = args.GetProperty("message").GetString() ?? "commit";
        return ProcessUtil.RunAsync("git", $"commit -am \"{message}\"", cancellationToken);
    }
}
