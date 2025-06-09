using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Core.Planning;

public class BranchTool : ITool
{
    private readonly SandboxRunner _runner;
    private readonly string _repoPath;

    public BranchTool(SandboxRunner runner, string repoPath)
    {
        _runner = runner;
        _repoPath = repoPath;
    }

    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var name = args.GetProperty("name").GetString() ?? "muse-branch";
        return _runner.ExecAsync(_repoPath, $"git switch -c {name}", cancellationToken);
    }
}
