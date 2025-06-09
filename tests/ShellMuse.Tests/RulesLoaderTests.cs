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
        var path = Path.Combine(dir, ".muse-rules.md");
        File.WriteAllText(path, "test rules");

        Assert.Equal("test rules", RulesLoader.Load(dir));

        File.Delete(path);

        Assert.Equal(string.Empty, RulesLoader.Load(dir));
    }
}
