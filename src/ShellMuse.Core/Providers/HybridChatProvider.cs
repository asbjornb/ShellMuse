using System;
using System.Collections.Generic;
using System.Threading;

namespace ShellMuse.Core.Providers;

public class HybridChatProvider : IChatProvider
{
    private readonly IChatProvider _openAi;
    private readonly IChatProvider _ollama;

    public HybridChatProvider(IChatProvider openAi, IChatProvider ollama)
    {
        _openAi = openAi;
        _ollama = ollama;
    }

    public async IAsyncEnumerable<string> StreamChatAsync(
        string prompt,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default
    )
    {
        var provider = ChooseProvider(prompt);
        var name = ReferenceEquals(provider, _ollama) ? "local Ollama" : "OpenAI";
        Console.WriteLine($"Using {name} provider");
        await foreach (var chunk in provider.StreamChatAsync(prompt, cancellationToken))
            yield return chunk;
    }

    private IChatProvider ChooseProvider(string prompt)
    {
        var text = prompt.ToLowerInvariant();
        if (text.Contains("plan") || text.Contains("design") || text.Contains("code") || text.Contains("implement"))
            return _openAi;
        return _ollama;
    }
}
