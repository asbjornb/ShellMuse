using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ShellMuse.Core.Planning.Tools;
using Xunit;

namespace ShellMuse.Tests;

public class FileToolsTests
{
    [Fact]
    public async Task WriteFileThenReadBack()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var write = new WriteFileTool();
        var read = new ReadFileTool();
        var writeArgs = JsonDocument.Parse(JsonSerializer.Serialize(new { path, content = "hello" })).RootElement;
        await write.RunAsync(writeArgs);
        var readArgs = JsonDocument.Parse(JsonSerializer.Serialize(new { path })).RootElement;
        var result = await read.RunAsync(readArgs);
        Assert.Equal("hello", result);
        File.Delete(path);
    }

    [Fact]
    public async Task ReadMissingFileReturnsEmpty()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var read = new ReadFileTool();
        var readArgs = JsonDocument.Parse(JsonSerializer.Serialize(new { path })).RootElement;
        var result = await read.RunAsync(readArgs);
        Assert.Equal(string.Empty, result);
    }
}
