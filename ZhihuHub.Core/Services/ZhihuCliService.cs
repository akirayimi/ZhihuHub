using System.Text.Json;
using ZhihuHub.Core.Config;
using ZhihuHub.Core.Models;
using ZhihuHub.Core.Utils;

namespace ZhihuHub.Core.Services;

/// <summary>
/// 知乎 CLI 服务实现
/// </summary>
public class ZhihuCliService : IZhihuCliService
{
    private readonly string _cliPath;
    private readonly int _defaultTimeout;

    public ZhihuCliService(AppConfig config)
    {
        _cliPath = string.IsNullOrEmpty(config.CliPath)
            ? AppConfig.GetDefaultCliPath()
            : config.CliPath;
        _defaultTimeout = config.DefaultTimeout;
    }

    public async Task<StatusResult?> GetStatusAsync()
    {
        var (success, output, error) = await ProcessHelper.ExecuteCliAsync(
            _cliPath,
            "status",
            timeoutSeconds: 10);

        if (!success)
            return null;

        try
        {
            return JsonSerializer.Deserialize<StatusResult>(output, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> SetAccessSecretAsync(string secret)
    {
        var (success, _, _) = await ProcessHelper.ExecuteCliAsync(
            _cliPath,
            "auth set --secret-stdin",
            input: secret,
            timeoutSeconds: 10);

        return success;
    }

    public async Task<bool> VerifyAuthAsync()
    {
        var (success, output, _) = await ProcessHelper.ExecuteCliAsync(
            _cliPath,
            "auth status --verify",
            timeoutSeconds: 10);

        if (!success)
            return false;

        try
        {
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(output);
            return result?.ContainsKey("ok") == true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<SearchResult?> SearchZhihuAsync(string query, int count = 10)
    {
        var args = $"search zhihu --query \"{EscapeArgument(query)}\" --count {count}";
        var (success, output, _) = await ProcessHelper.ExecuteCliAsync(
            _cliPath,
            args,
            timeoutSeconds: _defaultTimeout);

        if (!success)
            return null;

        try
        {
            return JsonSerializer.Deserialize<SearchResult>(output, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }

    public async Task<SearchResult?> SearchGlobalAsync(string query, int count = 10, string searchDb = "all")
    {
        var args = $"search global --query \"{EscapeArgument(query)}\" --count {count} --search-db {searchDb}";
        var (success, output, _) = await ProcessHelper.ExecuteCliAsync(
            _cliPath,
            args,
            timeoutSeconds: _defaultTimeout);

        if (!success)
            return null;

        try
        {
            return JsonSerializer.Deserialize<SearchResult>(output, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }

    public async Task<HotListResult?> GetHotListAsync(int limit = 20)
    {
        var args = $"hot --limit {limit}";
        var (success, output, _) = await ProcessHelper.ExecuteCliAsync(
            _cliPath,
            args,
            timeoutSeconds: _defaultTimeout);

        if (!success)
            return null;

        try
        {
            return JsonSerializer.Deserialize<HotListResult>(output, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 转义命令行参数中的特殊字符
    /// </summary>
    private static string EscapeArgument(string arg)
    {
        return arg.Replace("\"", "\\\"");
    }
}
