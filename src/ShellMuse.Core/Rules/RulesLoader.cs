using System.IO;

namespace ShellMuse.Core.Rules;

public static class RulesLoader
{
    public static string Load(string repoPath)
    {
        var path = Path.Combine(repoPath, ".muse-rules.md");
        return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
    }
}
