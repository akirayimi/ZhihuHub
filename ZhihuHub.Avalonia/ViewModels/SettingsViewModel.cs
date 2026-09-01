using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ZhihuHub.Core.Services;

namespace ZhihuHub.Avalonia.ViewModels;

/// <summary>
/// 设置 ViewModel
/// </summary>
public class SettingsViewModel : ViewModelBase
{
    private readonly IZhihuCliService _cliService;
    private string _authStatus = "检查中...";
    private string _cliPath = string.Empty;
    private string _cliVersion = string.Empty;
    private bool _isBusy;

    public SettingsViewModel(IZhihuCliService cliService)
    {
        _cliService = cliService;

        VerifyAuthCommand = ReactiveCommand.CreateFromTask(VerifyAuthAsync);

        // 延迟加载，避免构造函数中的线程问题
        Avalonia.Threading.Dispatcher.UIThread.Post(() => _ = LoadStatusAsync());
    }

    public string AuthStatus
    {
        get => _authStatus;
        set => this.RaiseAndSetIfChanged(ref _authStatus, value);
    }

    public string CliPath
    {
        get => _cliPath;
        set => this.RaiseAndSetIfChanged(ref _cliPath, value);
    }

    public string CliVersion
    {
        get => _cliVersion;
        set => this.RaiseAndSetIfChanged(ref _cliVersion, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public string VersionInfo { get; } = "ZhihuHub Desktop v0.2.0 Beta";

    public ReactiveCommand<Unit, Unit> VerifyAuthCommand { get; }

    private async Task LoadStatusAsync()
    {
        IsBusy = true;

        try
        {
            var status = await _cliService.GetStatusAsync();

            if (status == null)
            {
                AuthStatus = "无法获取 CLI 状态";
                return;
            }

            CliPath = status.Cli.BinaryPath;
            CliVersion = status.Cli.CurrentVersion;
            AuthStatus = status.Auth.Configured ? "已配置认证" : "未配置认证";
        }
        catch (Exception ex)
        {
            AuthStatus = $"获取状态出错: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task VerifyAuthAsync()
    {
        IsBusy = true;
        AuthStatus = "验证中...";

        try
        {
            var isValid = await _cliService.VerifyAuthAsync();
            AuthStatus = isValid ? "认证有效" : "认证无效，请重新配置 Access Secret";
        }
        catch (Exception ex)
        {
            AuthStatus = $"验证出错: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
