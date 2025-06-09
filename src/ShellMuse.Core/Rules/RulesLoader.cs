using System.IO;

namespace ShellMuse.Core.Rules;

public static class RulesLoader
{
    public static string Load(string repoPath)
    {
        var sb = new System.Text.StringBuilder();
        var rulesPath = Path.Combine(repoPath, ".muse-rules.md");
        if (File.Exists(rulesPath))
            sb.AppendLine(File.ReadAllText(rulesPath));
        var agentsPath = Path.Combine(repoPath, "AGENTS.md");
        if (File.Exists(agentsPath))
            sb.AppendLine(File.ReadAllText(agentsPath));
        return sb.ToString();
    }
}
