namespace ZhihuHub.Core.Models;

/// <summary>
/// CLI 调用结果
/// </summary>
public class CliResult
{
    public bool Success { get; set; }
    public string Output { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public int ExitCode { get; set; }
}

/// <summary>
/// CLI 状态信息
/// </summary>
public class StatusResult
{
    public bool Ok { get; set; }
    public bool Installed { get; set; }
    public AuthInfo Auth { get; set; } = new();
    public CliInfo Cli { get; set; } = new();
    public string NextAction { get; set; } = string.Empty;
}

public class AuthInfo
{
    public bool Configured { get; set; }
}

public class CliInfo
{
    public string BinaryPath { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public bool Compatible { get; set; }
}
