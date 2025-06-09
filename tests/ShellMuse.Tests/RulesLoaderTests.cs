using System;
using System.IO;
using ShellMuse.Core.Rules;
using Xunit;

namespace ShellMuse.Tests;

public class RulesLoaderTests
{
    [Fact]
    public void LoadsAndHandlesMissingFile()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        var rules = Path.Combine(dir, ".muse-rules.md");
        var agents = Path.Combine(dir, "AGENTS.md");
        File.WriteAllText(rules, "rules");
        File.WriteAllText(agents, "agents");

        Assert.Equal("rules\nagents\n", RulesLoader.Load(dir));

        File.Delete(rules);
        File.Delete(agents);

        Assert.Equal(string.Empty, RulesLoader.Load(dir));
    }
}
