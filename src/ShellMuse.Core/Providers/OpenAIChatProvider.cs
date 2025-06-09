using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using ShellMuse.Core.Config;

namespace ShellMuse.Core.Providers;

public class OpenAIChatProvider : IChatProvider
{
    private readonly HttpClient _client;
    private readonly AppConfig _config;

    public OpenAIChatProvider(HttpClient client, AppConfig config)
    {
        _client = client;
        _config = config;
    }

    public async IAsyncEnumerable<string> StreamChatAsync(
        string prompt,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default
    )
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.openai.com/v1/chat/completions"
        );
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _config.OpenAIApiKey
        );
        var payload = new
        {
            model = _config.Model,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are a coding agent. Respond ONLY with JSON matching {\"tool\":string, \"args\":object}. Available tools: search, read_file, write_file, build, test, commit, branch, finish.",
                },
                new { role = "user", content = prompt },
            },
            temperature = _config.Temperature,
            max_tokens = _config.MaxTokens,
            stream = true,
        };
        request.Content = JsonContent.Create(payload);

        using var response = await _client.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line == null)
                continue;
            if (!line.StartsWith("data:"))
                continue;
            var data = line[5..].Trim();
            if (data == "[DONE]")
                yield break;

            using var doc = JsonDocument.Parse(data);
            foreach (var choice in doc.RootElement.GetProperty("choices").EnumerateArray())
            {
                if (
                    choice.TryGetProperty("delta", out var delta)
                    && delta.TryGetProperty("content", out var content)
                )
                {
                    var text = content.GetString();
                    if (!string.IsNullOrEmpty(text))
                        yield return text!;
                }
            }
        }
    }
}
