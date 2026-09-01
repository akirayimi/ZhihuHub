using ZhihuHub.UI.Forms;
using ZhihuHub.Core.Services;
using ZhihuHub.Core.Config;

namespace ZhihuHub.UI;

internal static class Program
{
    /// <summary>
    /// 应用程序的主入口点
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        // 配置应用程序
        var appConfig = new AppConfig
        {
            CliPath = AppConfig.GetDefaultCliPath(),
            DefaultTimeout = 30,
            CacheEnabled = true,
            CacheDuration = 300
        };

        // 创建服务
        var cliService = new ZhihuCliService(appConfig);

        // 启动主窗体
        Application.Run(new MainForm(cliService));
    }
}
