using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class WriteFileTool : ITool
{
    public Task<string> RunAsync(
        JsonElement args,
        CancellationToken cancellationToken = default,
        Action<string>? outputLogger = null
    )
    {
        var path = args.GetProperty("path").GetString() ?? string.Empty;
        var content = args.GetProperty("content").GetString() ?? string.Empty;
        File.WriteAllText(path, content);
        outputLogger?.Invoke($"wrote {path}");
        return Task.FromResult("written");
    }
}
