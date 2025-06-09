using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Core.Planning.Tools;

public class BranchTool : ITool
{
    private readonly SandboxRunner _runner;
    private readonly string _repoPath;

    public BranchTool(SandboxRunner runner, string repoPath)
    {
        _runner = runner;
        _repoPath = repoPath;
    }

    public Task<string> RunAsync(
        JsonElement args,
        CancellationToken cancellationToken = default,
        Action<string>? outputLogger = null
    )
    {
        var name = args.GetProperty("name").GetString() ?? "muse-branch";
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        return _runner.ExecAsync(
            _repoPath,
            $"git switch -c {name}",
            cts.Token,
            outputLogger
        );
    }
}
