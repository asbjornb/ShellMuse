using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Sandbox;

public class SandboxRunner
{
    private readonly string _image;
    public SandboxRunner(string image) => _image = image;

    public Task<string> ExecAsync(string repoPath, string command, CancellationToken ct = default)
    {
        var args = $"run --rm --network none --read-only --cap-drop ALL --pids-limit 200 --tmpfs /tmp -v \"{repoPath}\":/workspace -w /workspace {_image} /bin/sh -c \"{command}\"";
        return Planning.ProcessUtil.RunAsync("docker", args, ct);
    }
}
