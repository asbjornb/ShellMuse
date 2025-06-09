using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Core.Planning.Tools;

public class SearchTool : ITool
{
    private readonly SandboxRunner _runner;
    private readonly string _repoPath;

    public SearchTool(SandboxRunner runner, string repoPath)
    {
        _runner = runner;
        _repoPath = repoPath;
    }

    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var query = args.GetProperty("query").GetString() ?? string.Empty;
        var path = args.TryGetProperty("path", out var p) ? p.GetString() ?? "." : ".";
        var command = $"rg {query} {path}";
        return _runner.ExecAsync(_repoPath, command, cancellationToken);
    }
}
