using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Providers;
using Xunit;

namespace ShellMuse.Tests;

public class HybridProviderTests
{
    [Fact]
    public async Task UsesOpenAIForComplexPrompt()
    {
        var openai = new StubProvider("openai");
        var ollama = new StubProvider("ollama");
        var provider = new HybridChatProvider(openai, ollama);

        await foreach (var _ in provider.StreamChatAsync("plan the app"))
        {
        }

        Assert.Single(openai.Prompts);
        Assert.Empty(ollama.Prompts);
    }

    [Fact]
    public async Task UsesOllamaForSimplePrompt()
    {
        var openai = new StubProvider("openai");
        var ollama = new StubProvider("ollama");
        var provider = new HybridChatProvider(openai, ollama);

        await foreach (var _ in provider.StreamChatAsync("search foo"))
        {
        }

        Assert.Empty(openai.Prompts);
        Assert.Single(ollama.Prompts);
    }

    private class StubProvider : IChatProvider
    {
        public readonly List<string> Prompts = new();

        public StubProvider(string name) {}

        public async IAsyncEnumerable<string> StreamChatAsync(
            string prompt,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            Prompts.Add(prompt);
            await Task.CompletedTask;
            yield break;
        }
    }
}
