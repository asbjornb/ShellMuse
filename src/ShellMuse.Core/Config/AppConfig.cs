namespace ShellMuse.Core.Config;

public record AppConfig
{
    public string Model { get; init; } = "gpt-4o";
    public double Temperature { get; init; } = 0.2;
    public int MaxTokens { get; init; } = 2048;
    public string DockerImage { get; init; } = "mcr.microsoft.com/dotnet/nightly/sdk:9.0";
    public string OpenAIApiKey { get; init; } = string.Empty;
}
