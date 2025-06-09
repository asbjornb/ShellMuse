using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class ReadFileTool : ITool
{
    public Task<string> RunAsync(
        JsonElement args,
        CancellationToken cancellationToken = default,
        Action<string>? outputLogger = null
    )
    {
        var path = args.GetProperty("path").GetString() ?? string.Empty;
        var result = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        outputLogger?.Invoke($"read {path}");
        return Task.FromResult(result);
    }
}
