namespace ZhihuHub.Core.Models;

/// <summary>
/// 搜索结果
/// </summary>
public class SearchResult
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public SearchData Data { get; set; } = new();
}

public class SearchData
{
    public List<SearchItem> Items { get; set; } = new();
    public int Total { get; set; }
}

public class SearchItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string ContentText { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}

/// <summary>
/// 热榜结果
/// </summary>
public class HotListResult
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public HotListData Data { get; set; } = new();
}

public class HotListData
{
    public int Total { get; set; }
    public List<HotItem> Items { get; set; } = new();
}

public class HotItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}
