namespace ShellMuse.Core.Config;

public record AppConfig
{
    public string Model { get; init; } = "gpt-4o";
    public double Temperature { get; init; } = 0.2;
    public int MaxTokens { get; init; } = 2048;
    public string DockerImage { get; init; } = "ghcr.io/shellmuse/runtime:dotnet-slim";
    public string OpenAIApiKey { get; init; } = string.Empty;
    public bool UseLocalLlm { get; init; } = true;
}
