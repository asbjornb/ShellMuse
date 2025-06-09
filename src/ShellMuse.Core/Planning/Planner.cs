using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShellMuse.Core.Providers;

namespace ShellMuse.Core.Planning;

public class Planner
{
    private readonly IChatProvider _provider;
    private readonly ToolPalette _palette;
    private readonly int _maxSteps;

    public Planner(IChatProvider provider, ToolPalette palette, int maxSteps = 10)
    {
        _provider = provider;
        _palette = palette;
        _maxSteps = maxSteps;
    }

    public async Task RunAsync(string task, string contextInfo = "", CancellationToken cancellationToken = default)
    {
        var context = new StringBuilder();
        if (!string.IsNullOrEmpty(contextInfo))
            context.AppendLine(contextInfo);
        context.Append(task);
        for (int step = 0; step < _maxSteps; step++)
        {
            var toolJson = await RequestToolAsync(context.ToString(), cancellationToken);
            if (!ToolCall.TryParse(toolJson, out var call) || call == null)
                throw new InvalidOperationException($"Invalid tool call at step {step}: {toolJson}");

            try
            {
                var result = await _palette.ExecuteAsync(call, cancellationToken);
                context.Append("\n");
                context.Append(result);
                if (call.Tool == Tool.Finish)
                    break;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Tool {call.Tool} failed: {ex.Message}", ex);
            }
        }
    }

    private async Task<string> RequestToolAsync(string prompt, CancellationToken ct)
    {
        var sb = new StringBuilder();
        await foreach (var chunk in _provider.StreamChatAsync(prompt, ct))
            sb.Append(chunk);
        return sb.ToString();
    }
}
