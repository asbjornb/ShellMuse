using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class ReadFileTool : ITool
{
    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var path = args.GetProperty("path").GetString() ?? string.Empty;
        return Task.FromResult(File.Exists(path) ? File.ReadAllText(path) : "");
    }
}
