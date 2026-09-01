using ZhihuHub.Core.Models;

namespace ZhihuHub.Core.Services;

/// <summary>
/// 知乎 CLI 服务接口
/// </summary>
public interface IZhihuCliService
{
    /// <summary>
    /// 获取 CLI 状态
    /// </summary>
    Task<StatusResult?> GetStatusAsync();

    /// <summary>
    /// 配置 Access Secret
    /// </summary>
    Task<bool> SetAccessSecretAsync(string secret);

    /// <summary>
    /// 验证认证状态
    /// </summary>
    Task<bool> VerifyAuthAsync();

    /// <summary>
    /// 搜索知乎内容
    /// </summary>
    Task<SearchResult?> SearchZhihuAsync(string query, int count = 10);

    /// <summary>
    /// 搜索全网内容
    /// </summary>
    Task<SearchResult?> SearchGlobalAsync(string query, int count = 10, string searchDb = "all");

    /// <summary>
    /// 获取热榜
    /// </summary>
    Task<HotListResult?> GetHotListAsync(int limit = 20);
}
