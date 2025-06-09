using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public class TestTool : ITool
{
    public Task<string> RunAsync(System.Text.Json.JsonElement args, CancellationToken cancellationToken = default)
    {
        return ProcessUtil.RunAsync("dotnet", "test --no-build --nologo", cancellationToken);
    }
}
