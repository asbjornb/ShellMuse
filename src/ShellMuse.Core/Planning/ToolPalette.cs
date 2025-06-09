using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

public class ToolPalette
{
    private readonly Dictionary<Tool, ITool> _tools;

    public ToolPalette(IEnumerable<(Tool kind, ITool tool)> tools)
    {
        _tools = tools.ToDictionary(t => t.kind, t => t.tool);
    }

    public async Task<string> ExecuteAsync(
        ToolCall call,
        CancellationToken cancellationToken = default
    )
    {
        if (!_tools.TryGetValue(call.Tool, out var tool))
            throw new InvalidOperationException($"Unknown tool {call.Tool}");
        return await tool.RunAsync(call.Args, cancellationToken);
    }
}
