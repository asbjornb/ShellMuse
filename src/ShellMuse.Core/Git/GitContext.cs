using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Git;

public static class GitContext
{
    private static async Task<string> RunGitAsync(string args, CancellationToken ct)
    {
        return await Planning.ProcessUtil.RunAsync("git", args, ct);
    }

    public static Task<string> GetBranchAsync(CancellationToken ct = default) =>
        RunGitAsync("rev-parse --abbrev-ref HEAD", ct);

    public static Task<string> GetDiffAsync(CancellationToken ct = default) =>
        RunGitAsync("diff", ct);

    public static Task<string> GetFileListAsync(CancellationToken ct = default) =>
        RunGitAsync("ls-files", ct);

    public static async Task<string> CaptureAsync(CancellationToken ct = default)
    {
        var branch = (await GetBranchAsync(ct)).Trim();
        var diff = await GetDiffAsync(ct);
        var files = await GetFileListAsync(ct);
        return $"branch: {branch}\nfiles:\n{files}\ndiff:\n{diff}";
    }
}
