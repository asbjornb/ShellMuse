using System;
using ShellMuse.Core.Config;

namespace ShellMuse.Cli;

public static class Program
{
    public static int Main(string[] args)
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
            Console.WriteLine($"ASK '{prompt}' (model {config.Model})");
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
            Console.WriteLine($"RUN '{task}' (model {config.Model}, maxCost {runOpts.MaxCost}, maxSteps {runOpts.MaxSteps})");
            return 0;
        }
        else
        {
            Usage();
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
                case "--max-cost" when i + 1 < args.Length && double.TryParse(args[i + 1], out var c):
                    maxCost = c; i++; break;
                case "--max-steps" when i + 1 < args.Length && int.TryParse(args[i + 1], out var s):
                    maxSteps = s; i++; break;
                case "-v":
                    break;
                default:
                    break;
            }
        }
        return new RunOptions(maxCost, maxSteps);
    }
}
