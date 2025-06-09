using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public class BuildTool : ITool
{
    public Task<string> RunAsync(System.Text.Json.JsonElement args, CancellationToken cancellationToken = default)
    {
        return ProcessUtil.RunAsync("dotnet", "build --nologo", cancellationToken);
    }
}
