using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Core.Planning.Tools;

public class CommitTool : ITool
{
    private readonly SandboxRunner _runner;
    private readonly string _repoPath;

    public CommitTool(SandboxRunner runner, string repoPath)
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
        var message = args.GetProperty("message").GetString() ?? "commit";
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        return _runner.ExecAsync(
            _repoPath,
            $"git commit -am \"{message}\"",
            cts.Token,
            outputLogger
        );
    }
}
