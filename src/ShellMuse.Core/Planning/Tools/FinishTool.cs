using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning.Tools;

public class FinishTool : ITool
{
    public Task<string> RunAsync(
        JsonElement args,
        CancellationToken cancellationToken = default,
        Action<string>? outputLogger = null
    )
    {
        outputLogger?.Invoke("finished");
        return Task.FromResult("done");
    }
}
