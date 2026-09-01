using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ZhihuHub.Core.Models;
using ZhihuHub.Core.Services;

namespace ZhihuHub.Avalonia.ViewModels;

/// <summary>
/// 搜索 ViewModel
/// </summary>
public class SearchViewModel : ViewModelBase
{
    private readonly IZhihuCliService _cliService;
    private string _searchQuery = string.Empty;
    private int _selectedSearchType = 0; // 0: 知乎搜索, 1: 全网搜索
    private string _statusMessage = string.Empty;
    private bool _isSearching = false;

    public SearchViewModel(IZhihuCliService cliService)
    {
        _cliService = cliService;
        SearchResults = new ObservableCollection<SearchItem>();

        SearchCommand = ReactiveCommand.CreateFromTask(ExecuteSearchAsync);
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
    }

    public int SelectedSearchType
    {
        get => _selectedSearchType;
        set => this.RaiseAndSetIfChanged(ref _selectedSearchType, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public bool IsSearching
    {
        get => _isSearching;
        set => this.RaiseAndSetIfChanged(ref _isSearching, value);
    }

    public ObservableCollection<SearchItem> SearchResults { get; }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    private async Task ExecuteSearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            StatusMessage = "请输入搜索关键词";
            return;
        }

        IsSearching = true;
        StatusMessage = "搜索中...";
        SearchResults.Clear();

        try
        {
            SearchResult? result = null;

            if (SelectedSearchType == 0)
            {
                result = await _cliService.SearchZhihuAsync(SearchQuery, 10);
            }
            else
            {
                result = await _cliService.SearchGlobalAsync(SearchQuery, 10);
            }

            if (result?.Code == 0 && result.Data?.Items != null)
            {
                foreach (var item in result.Data.Items)
                {
                    SearchResults.Add(item);
                }

                StatusMessage = $"找到 {result.Data.Total} 条结果";
            }
            else
            {
                StatusMessage = "搜索失败";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"搜索出错: {ex.Message}";
        }
        finally
        {
            IsSearching = false;
        }
    }
}
