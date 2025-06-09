using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public class BranchTool : ITool
{
    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var name = args.GetProperty("name").GetString() ?? "muse-branch";
        return ProcessUtil.RunAsync("git", $"switch -c {name}", cancellationToken);
    }
}
