using System;
using System.IO;
using ShellMuse.Core.Config;
using Xunit;

namespace ShellMuse.Tests;

public class ConfigTests
{
    [Fact]
    public void EnvOverridesAppSettings()
    {
        var tmpDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tmpDir);
        File.WriteAllText(Path.Combine(tmpDir, "appsettings.json"), "{\"Model\":\"from-file\"}");

        Environment.SetEnvironmentVariable("SHELLMUSE_MODEL", "from-env");

        var config = ConfigLoader.Load(tmpDir);

        Assert.Equal("from-env", config.Model);

        Environment.SetEnvironmentVariable("SHELLMUSE_MODEL", null);
    }
}

