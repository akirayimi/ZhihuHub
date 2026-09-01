namespace ZhihuHub.Avalonia.ViewModels;

/// <summary>
/// 首页 ViewModel
/// </summary>
public class HomeViewModel : ViewModelBase
{
    public string WelcomeMessage { get; } = "欢迎使用 ZhihuHub Desktop";
    public string SubtitleMessage { get; } = "知乎开放平台 CLI 的现代化图形界面客户端";
    public string VersionInfo { get; } = "版本 0.2.0 Beta (Avalonia UI)";
}
