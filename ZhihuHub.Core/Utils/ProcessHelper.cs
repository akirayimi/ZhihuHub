using System.Diagnostics;
using System.Text;

namespace ZhihuHub.Core.Utils;

/// <summary>
/// 进程执行辅助类
/// </summary>
public static class ProcessHelper
{
    /// <summary>
    /// 执行 CLI 命令
    /// </summary>
    public static async Task<(bool Success, string Output, string Error)> ExecuteCliAsync(
        string cliPath,
        string arguments,
        string? input = null,
        int timeoutSeconds = 30)
    {
        if (!File.Exists(cliPath))
        {
            return (false, string.Empty, $"CLI not found at: {cliPath}");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = cliPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = !string.IsNullOrEmpty(input),
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        try
        {
            using var process = new Process { StartInfo = startInfo };
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                    outputBuilder.AppendLine(e.Data);
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                    errorBuilder.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // 如果有输入，写入标准输入
            if (!string.IsNullOrEmpty(input))
            {
                await process.StandardInput.WriteLineAsync(input);
                await process.StandardInput.FlushAsync();
                process.StandardInput.Close();
            }

            // 等待进程完成，带超时
            var completed = await process.WaitForExitAsync(
                TimeSpan.FromSeconds(timeoutSeconds));

            if (!completed)
            {
                process.Kill(true);
                return (false, string.Empty, "Process timeout");
            }

            var output = outputBuilder.ToString().TrimEnd();
            var error = errorBuilder.ToString().TrimEnd();
            var success = process.ExitCode == 0;

            return (success, output, error);
        }
        catch (Exception ex)
        {
            return (false, string.Empty, $"Process execution failed: {ex.Message}");
        }
    }

    /// <summary>
    /// 等待进程退出（带超时）
    /// </summary>
    private static async Task<bool> WaitForExitAsync(this Process process, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        try
        {
            await process.WaitForExitAsync(cts.Token);
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }
}
