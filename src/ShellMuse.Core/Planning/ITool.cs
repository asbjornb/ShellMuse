using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public interface ITool
{
    Task<string> RunAsync(
        JsonElement args,
        CancellationToken cancellationToken = default,
        Action<string>? outputLogger = null
    );
}
