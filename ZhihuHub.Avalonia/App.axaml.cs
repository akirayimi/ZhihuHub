using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ZhihuHub.Avalonia.ViewModels;
using ZhihuHub.Avalonia.Views;
using ZhihuHub.Core.Config;
using ZhihuHub.Core.Services;

namespace ZhihuHub.Avalonia;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 配置服务
            var appConfig = new AppConfig
            {
                CliPath = AppConfig.GetDefaultCliPath(),
                DefaultTimeout = 30,
                CacheEnabled = true,
                CacheDuration = 300
            };

            var cliService = new ZhihuCliService(appConfig);

            // 创建主窗口
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(cliService)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
