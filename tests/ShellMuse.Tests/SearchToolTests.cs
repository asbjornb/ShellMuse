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
        try
        {
            Environment.SetEnvironmentVariable("PATH", string.Empty);
            var tool = new SearchTool();
            var args = JsonDocument.Parse("{\"query\":\"foo\"}").RootElement;
            var result = await tool.RunAsync(args);
            Assert.Contains("ripgrep", result, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            Environment.SetEnvironmentVariable("PATH", originalPath);
        }
    }
}
