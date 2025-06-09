using System;
using System.Net.Http;
using System.Threading.Tasks;
using ShellMuse.Core.Config;
using ShellMuse.Core.Git;
using ShellMuse.Core.Planning;
using ShellMuse.Core.Providers;
using ShellMuse.Core.Rules;
using ShellMuse.Core.Sandbox;

namespace ShellMuse.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            if (args.Length == 0 || args[0] == "ask")
            {
                var start = args.Length > 0 && args[0] == "ask" ? 1 : 0;
                if (args.Length <= start)
                {
                    Usage();
                    return 1;
                }
                var prompt = args[start];
                var config = ConfigLoader.Load();
                using var http = new HttpClient();
                var provider = new OpenAIChatProvider(http, config);
                await foreach (var chunk in provider.StreamChatAsync(prompt))
                    Console.Write(chunk);
                Console.WriteLine();
                return 0;
            }
            else if (args[0] == "run")
            {
                if (args.Length < 2)
                {
                    Usage();
                    return 1;
                }
                var task = args[1];
                var runOpts = ParseRun(args, 2);
                var config = ConfigLoader.Load();
                using var http = new HttpClient();
                var provider = new OpenAIChatProvider(http, config);
                var repoPath = Environment.CurrentDirectory;
                var runner = new SandboxRunner(config.DockerImage);
                var palette = DefaultPalette(runner, repoPath);
                var planner = new Planner(provider, palette, runOpts.MaxSteps);
                var rules = RulesLoader.Load(repoPath);
                var contextInfo = await GitContext.CaptureAsync();
                await planner.RunAsync(task, rules + "\n" + contextInfo, Console.WriteLine);
                return 0;
            }
            else
            {
                Usage();
                return 1;
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    private static void Usage()
    {
        Console.WriteLine("shellmuse [ask] <prompt>");
        Console.WriteLine("shellmuse run <task> [--max-cost N] [--max-steps N] [-v]");
    }

    private record RunOptions(double MaxCost, int MaxSteps);

    private static RunOptions ParseRun(string[] args, int start)
    {
        int index = start;
        double maxCost = 1.0;
        int maxSteps = 8;
        for (int i = index; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--max-cost"
                    when i + 1 < args.Length && double.TryParse(args[i + 1], out var c):
                    maxCost = c;
                    i++;
                    break;
                case "--max-steps" when i + 1 < args.Length && int.TryParse(args[i + 1], out var s):
                    maxSteps = s;
                    i++;
                    break;
                case "-v":
                    break;
                default:
                    break;
            }
        }
        return new RunOptions(maxCost, maxSteps);
    }

    private static ToolPalette DefaultPalette(SandboxRunner runner, string repoPath)
    {
        return new ToolPalette(
            new (Tool, ITool)[]
            {
                (Tool.Search, new SearchTool()),
                (Tool.ReadFile, new ReadFileTool()),
                (Tool.WriteFile, new WriteFileTool()),
                (Tool.Build, new BuildTool(runner, repoPath)),
                (Tool.Test, new TestTool(runner, repoPath)),
                (Tool.Commit, new CommitTool(runner, repoPath)),
                (Tool.Branch, new BranchTool(runner, repoPath)),
                (Tool.Finish, new FinishTool()),
            }
        );
    }
}
