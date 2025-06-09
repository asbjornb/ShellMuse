using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ShellMuse.Core.Planning;
using ShellMuse.Core.Providers;
using Xunit;

namespace ShellMuse.Tests;

public class PlannerTests
{
    [Fact]
    public void ToolCallParses()
    {
        var json = "{\"tool\":\"search\",\"args\":{\"query\":\"foo\"}}";
        Assert.True(ToolCall.TryParse(json, out var call));
        Assert.Equal(Tool.Search, call!.Tool);
        Assert.Equal("foo", call.Value.Args.GetProperty("query").GetString());
    }

    [Fact]
    public async Task UnknownToolThrows()
    {
        var provider = new SequenceProvider("{\"tool\":\"bad\"}");
        var palette = new ToolPalette(new (Tool, ITool)[] { (Tool.Search, new StubTool()) });
        var planner = new Planner(provider, palette);
        await Assert.ThrowsAsync<InvalidOperationException>(() => planner.RunAsync("do"));
    }

    [Fact]
    public async Task RespectsStepLimit()
    {
        var provider = new SequenceProvider(
            "{\"tool\":\"search\",\"args\":{}}",
            "{\"tool\":\"search\",\"args\":{}}",
            "{\"tool\":\"finish\"}"
        );
        var stub = new StubTool();
        var palette = new ToolPalette(new (Tool, ITool)[]
        {
            (Tool.Search, stub),
            (Tool.Finish, new FinishTool())
        });
        var planner = new Planner(provider, palette, 2);
        await planner.RunAsync("task");
        Assert.Equal(2, stub.Count);
    }

    private class StubTool : ITool
    {
        public int Count { get; private set; }
        public Task<string> RunAsync(JsonElement args, CancellationToken cancellationToken = default)
        {
            Count++;
            return Task.FromResult("ok");
        }
    }

    private class SequenceProvider : IChatProvider
    {
        private readonly Queue<string> _responses;
        public SequenceProvider(params string[] responses) => _responses = new Queue<string>(responses);
        public async IAsyncEnumerable<string> StreamChatAsync(string prompt, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (_responses.Count == 0)
                yield break;
            yield return _responses.Dequeue();
            await Task.CompletedTask;
        }
    }
}
