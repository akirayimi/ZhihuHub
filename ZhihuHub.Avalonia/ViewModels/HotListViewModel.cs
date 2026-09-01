using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ZhihuHub.Core.Models;
using ZhihuHub.Core.Services;

namespace ZhihuHub.Avalonia.ViewModels;

/// <summary>
/// 热榜 ViewModel
/// </summary>
public class HotListViewModel : ViewModelBase
{
    private readonly IZhihuCliService _cliService;
    private string _statusMessage = string.Empty;
    private bool _isLoading = false;
    private string _lastUpdateTime = string.Empty;

    public HotListViewModel(IZhihuCliService cliService)
    {
        _cliService = cliService;
        HotItems = new ObservableCollection<HotItem>();

        RefreshCommand = ReactiveCommand.CreateFromTask(LoadHotListAsync);

        // 自动加载
        _ = LoadHotListAsync();
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public string LastUpdateTime
    {
        get => _lastUpdateTime;
        set => this.RaiseAndSetIfChanged(ref _lastUpdateTime, value);
    }

    public ObservableCollection<HotItem> HotItems { get; }

    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    private async Task LoadHotListAsync()
    {
        IsLoading = true;
        StatusMessage = "加载中...";
        HotItems.Clear();

        try
        {
            var result = await _cliService.GetHotListAsync(20);

            if (result?.Code == 0 && result.Data?.Items != null)
            {
                foreach (var item in result.Data.Items)
                {
                    HotItems.Add(item);
                }

                StatusMessage = $"共 {result.Data.Total} 条热榜";
                LastUpdateTime = $"最后更新: {DateTime.Now:HH:mm:ss}";
            }
            else
            {
                StatusMessage = "加载失败";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"加载出错: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
