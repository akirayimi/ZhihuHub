namespace ZhihuHub.Core.Config;

/// <summary>
/// 应用配置
/// </summary>
public class AppConfig
{
    public string CliPath { get; set; } = string.Empty;
    public int DefaultTimeout { get; set; } = 30;
    public bool CacheEnabled { get; set; } = true;
    public int CacheDuration { get; set; } = 300;

    /// <summary>
    /// 从环境或默认位置获取 CLI 路径
    /// </summary>
    public static string GetDefaultCliPath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, "ZhihuCLI", "current", "zhihu-cli.exe");
    }
}
