using System;
using System.Text.Json;
using System.Threading.Tasks;
using ShellMuse.Core.Planning.Tools;
using Xunit;

namespace ShellMuse.Tests;

public class SearchToolTests
{
    [Fact]
    public async Task ReturnsMessageWhenRipgrepMissing()
    {
        var originalPath = Environment.GetEnvironmentVariable("PATH");
        var originalCustom = Environment.GetEnvironmentVariable("SHELLMUSE_RIPGREP");
        try
        {
            Environment.SetEnvironmentVariable("PATH", string.Empty);
            Environment.SetEnvironmentVariable("SHELLMUSE_RIPGREP", null);
            var tool = new SearchTool();
            var args = JsonDocument.Parse("{\"query\":\"foo\"}").RootElement;
            var result = await tool.RunAsync(args);
            Assert.Contains("ripgrep", result, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            Environment.SetEnvironmentVariable("PATH", originalPath);
            Environment.SetEnvironmentVariable("SHELLMUSE_RIPGREP", originalCustom);
        }
    }

    [Fact]
    public async Task UsesCustomRipgrepPath()
    {
        var original = Environment.GetEnvironmentVariable("SHELLMUSE_RIPGREP");
        try
        {
            Environment.SetEnvironmentVariable("SHELLMUSE_RIPGREP", "echo");
            var tool = new SearchTool();
            var args = JsonDocument.Parse("{\"query\":\"foo\"}").RootElement;
            var result = await tool.RunAsync(args);
            Assert.Contains("foo", result);
        }
        finally
        {
            Environment.SetEnvironmentVariable("SHELLMUSE_RIPGREP", original);
        }
    }
}
