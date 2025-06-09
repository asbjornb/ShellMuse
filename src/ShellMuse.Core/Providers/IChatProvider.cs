namespace ShellMuse.Core.Providers;

public interface IChatProvider
{
    IAsyncEnumerable<string> StreamChatAsync(string prompt, CancellationToken cancellationToken = default);
}
