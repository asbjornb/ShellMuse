using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShellMuse.Core.Planning;

internal static class ProcessUtil
{
    public static async Task<string> RunAsync(
        string fileName,
        string arguments,
        CancellationToken ct = default
    )
    {
        var psi = new ProcessStartInfo(fileName, arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var proc = Process.Start(psi)!;
        var sb = new StringBuilder();
        proc.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                sb.AppendLine(e.Data);
        };
        proc.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
                sb.AppendLine(e.Data);
        };
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        await proc.WaitForExitAsync(ct);
        return sb.ToString();
    }
}
