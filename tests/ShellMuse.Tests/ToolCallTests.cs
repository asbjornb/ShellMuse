using System.Text.Json;
using ShellMuse.Core.Planning;
using Xunit;

namespace ShellMuse.Tests;

public class ToolCallTests
{
    [Fact]
    public void MissingToolPropertyFails()
    {
        var json = "{\"args\":{}}";
        var ok = ToolCall.TryParse(json, out var call, out var error);
        Assert.False(ok);
        Assert.Null(call);
        Assert.Equal("Missing 'tool' property", error);
    }

    [Fact]
    public void UnknownToolReturnsFalse()
    {
        var json = "{\"tool\":\"bogus\"}";
        var ok = ToolCall.TryParse(json, out var call, out var error);
        Assert.False(ok);
        Assert.Null(call);
        Assert.Equal("Unknown tool 'bogus'", error);
    }

    [Fact]
    public void MalformedJsonSurfacesError()
    {
        var json = "{\"tool\":\"search\""; // missing closing brace
        var ok = ToolCall.TryParse(json, out var call, out var error);
        Assert.False(ok);
        Assert.Null(call);
        Assert.False(string.IsNullOrEmpty(error));
    }
}
