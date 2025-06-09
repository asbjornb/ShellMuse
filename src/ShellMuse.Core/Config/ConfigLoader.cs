using System;
using Microsoft.Extensions.Configuration;

namespace ShellMuse.Core.Config;

public static class ConfigLoader
{
    public static AppConfig Load(string? basePath = null)
    {
        basePath ??= AppContext.BaseDirectory;
        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<AppConfig>(optional: true)
            .AddEnvironmentVariables(prefix: "SHELLMUSE_");
        var configRoot = builder.Build();
        var config = new AppConfig();
        configRoot.Bind(config);
        return config;
    }
}
