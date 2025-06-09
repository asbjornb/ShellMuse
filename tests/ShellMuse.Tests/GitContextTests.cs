using System.IO;
using ShellMuse.Core.Git;
using Xunit;

namespace ShellMuse.Tests;

public class GitContextTests
{
    [Fact]
    public void CapturesBranch()
    {
        var dir = Directory.CreateDirectory(
            Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
        );
        var repo = dir.FullName;
        ShellMuse
            .Core.Planning.ProcessUtil.RunAsync("git", $"init {repo}")
            .GetAwaiter()
            .GetResult();
        Directory.SetCurrentDirectory(repo);
        ShellMuse
            .Core.Planning.ProcessUtil.RunAsync(
                "git",
                "-c user.email=a@a -c user.name=a commit --allow-empty -m init"
            )
            .GetAwaiter()
            .GetResult();
        var branch = GitContext.GetBranchAsync().GetAwaiter().GetResult();
        Assert.Contains("master", branch.Trim());
    }

    [Fact]
    public void CaptureIncludesUncommittedChanges()
    {
        var dir = Directory.CreateDirectory(
            Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
        );
        var repo = dir.FullName;
        ShellMuse
            .Core.Planning.ProcessUtil.RunAsync("git", $"init {repo}")
            .GetAwaiter()
            .GetResult();
        Directory.SetCurrentDirectory(repo);
        ShellMuse
            .Core.Planning.ProcessUtil.RunAsync(
                "git",
                "-c user.email=a@a -c user.name=a commit --allow-empty -m init"
            )
            .GetAwaiter()
            .GetResult();

        File.WriteAllText("file.txt", "hello");
        ShellMuse
            .Core.Planning.ProcessUtil.RunAsync("git", "add file.txt")
            .GetAwaiter()
            .GetResult();
        ShellMuse
            .Core.Planning.ProcessUtil.RunAsync(
                "git",
                "-c user.email=a@a -c user.name=a commit -m add"
            )
            .GetAwaiter()
            .GetResult();

        File.WriteAllText("file.txt", "changed");

        var context = GitContext.CaptureAsync().GetAwaiter().GetResult();

        Assert.Contains("master", context);
        Assert.Contains("changed", context);
        Assert.Contains("file.txt", context);
    }
}
