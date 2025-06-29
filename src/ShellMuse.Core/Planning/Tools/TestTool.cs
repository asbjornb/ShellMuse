using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Core.Planning.Tools;

public class TestTool : ITool
{
    private readonly SandboxRunner _runner;
    private readonly string _repoPath;

    public TestTool(SandboxRunner runner, string repoPath)
    {
        _runner = runner;
        _repoPath = repoPath;
    }

    public Task<string> RunAsync(
        System.Text.Json.JsonElement args,
        CancellationToken cancellationToken = default
    )
    {
        return _runner.ExecAsync(_repoPath, "dotnet test --no-build --nologo", cancellationToken);
    }
}
