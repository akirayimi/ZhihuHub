using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ZhihuHub.Core.Models;
using ZhihuHub.Core.Services;

namespace ZhihuHub.Avalonia.ViewModels;

/// <summary>
/// 热榜项 ViewModel（包含排名）
/// </summary>
public class HotItemViewModel
{
    public int Rank { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}

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
        HotItems = new ObservableCollection<HotItemViewModel>();

        RefreshCommand = ReactiveCommand.CreateFromTask(LoadHotListAsync);

        // 延迟加载，避免构造函数中的线程问题
        Avalonia.Threading.Dispatcher.UIThread.Post(() => _ = LoadHotListAsync());
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

    public ObservableCollection<HotItemViewModel> HotItems { get; }

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
                int rank = 1;
                foreach (var item in result.Data.Items)
                {
                    HotItems.Add(new HotItemViewModel
                    {
                        Rank = rank++,
                        Title = item.Title,
                        Url = item.Url,
                        Summary = item.Summary
                    });
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
