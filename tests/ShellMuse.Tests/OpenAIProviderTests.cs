using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Config;
using ShellMuse.Core.Providers;
using Xunit;

namespace ShellMuse.Tests;

public class OpenAIProviderTests
{
    [Fact]
    public async Task StreamsCompletion()
    {
        var handler = new StubHandler();
        var client = new HttpClient(handler);
        var config = new AppConfig { OpenAIApiKey = "test" };
        var provider = new OpenAIChatProvider(client, config);

        var chunks = new List<string>();
        await foreach (var c in provider.StreamChatAsync("hello"))
            chunks.Add(c);

        Assert.Equal(new[] { "He", "llo" }, chunks);
    }

    private class StubHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            const string data =
                "data: {\"choices\":[{\"delta\":{\"content\":\"He\"}}]}\n\n"
                + "data: {\"choices\":[{\"delta\":{\"content\":\"llo\"}}]}\n\n"
                + "data: [DONE]\n\n";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(data),
            };
            return Task.FromResult(response);
        }
    }
}
