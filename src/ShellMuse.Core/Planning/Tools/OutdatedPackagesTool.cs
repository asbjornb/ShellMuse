using System;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Core.Planning.Tools;

public class OutdatedPackagesTool : ITool
{
    private readonly SandboxRunner _runner;
    private readonly string _repoPath;

    public OutdatedPackagesTool(SandboxRunner runner, string repoPath)
    {
        _runner = runner;
        _repoPath = repoPath;
    }

    public Task<string> RunAsync(
        System.Text.Json.JsonElement args,
        CancellationToken cancellationToken = default,
        Action<string>? outputLogger = null
    )
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        return _runner.ExecAsync(
            _repoPath,
            "dotnet list package --outdated",
            cts.Token,
            outputLogger
        );
    }
}
