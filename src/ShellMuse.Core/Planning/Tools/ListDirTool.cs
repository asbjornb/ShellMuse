using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class ListDirTool : ITool
{
    public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
    {
        var path = args.GetProperty("path").GetString() ?? ".";
        if (!Directory.Exists(path))
            return Task.FromResult(string.Empty);
        var entries = Directory.EnumerateFileSystemEntries(path).Select(Path.GetFileName).ToArray();
        return Task.FromResult(string.Join('\n', entries));
    }
}
