using Avalonia;
using System;

namespace ZhihuHub.Avalonia;

class Program
{
    // Avalonia 配置，不要在这里使用任何第三方代码
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia 配置
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
