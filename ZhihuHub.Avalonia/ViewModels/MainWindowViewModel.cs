using System.Reactive;
using ReactiveUI;
using ZhihuHub.Core.Services;

namespace ZhihuHub.Avalonia.ViewModels;

/// <summary>
/// 主窗口 ViewModel
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private readonly IZhihuCliService _cliService;
    private ViewModelBase _currentView;
    private string _statusText = "就绪";

    public MainWindowViewModel(IZhihuCliService cliService)
    {
        _cliService = cliService;

        // 初始化视图
        _currentView = new HomeViewModel();

        // 导航命令
        NavigateToHomeCommand = ReactiveCommand.Create(NavigateToHome);
        NavigateToSearchCommand = ReactiveCommand.Create(NavigateToSearch);
        NavigateToHotCommand = ReactiveCommand.Create(NavigateToHot);
        NavigateToSettingsCommand = ReactiveCommand.Create(NavigateToSettings);

        // 延迟检查认证，避免构造函数中的线程问题
        global::Avalonia.Threading.Dispatcher.UIThread.Post(() => CheckAuthenticationAsync());
    }

    /// <summary>
    /// 当前视图
    /// </summary>
    public ViewModelBase CurrentView
    {
        get => _currentView;
        private set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    /// <summary>
    /// 状态栏文本
    /// </summary>
    public string StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }

    // 导航命令
    public ReactiveCommand<Unit, Unit> NavigateToHomeCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToSearchCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToHotCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToSettingsCommand { get; }

    private void NavigateToHome()
    {
        CurrentView = new HomeViewModel();
        UpdateStatus("首页");
    }

    private void NavigateToSearch()
    {
        CurrentView = new SearchViewModel(_cliService);
        UpdateStatus("搜索");
    }

    private void NavigateToHot()
    {
        CurrentView = new HotListViewModel(_cliService);
        UpdateStatus("热榜");
    }

    private void NavigateToSettings()
    {
        CurrentView = new SettingsViewModel(_cliService);
        UpdateStatus("设置");
    }

    private void UpdateStatus(string message)
    {
        StatusText = $"{message} | {DateTime.Now:HH:mm:ss}";
    }

    private async void CheckAuthenticationAsync()
    {
        try
        {
            UpdateStatus("检查认证状态...");
            var status = await _cliService.GetStatusAsync();

            if (status == null)
            {
                UpdateStatus("CLI 连接失败");
            }
            else if (!status.Auth.Configured)
            {
                UpdateStatus("需要配置认证");
            }
            else
            {
                UpdateStatus("认证已配置");
            }
        }
        catch
        {
            UpdateStatus("检查认证失败");
        }
    }
}
