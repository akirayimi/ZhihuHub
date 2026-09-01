# 知乎 CLI 图形化客户端开发计划

## 项目概述

**项目名称**: ZhihuHub Desktop  
**技术栈**: .NET 8.0 + Windows Forms  
**目标**: 为知乎开放平台 CLI 提供现代化、易用的图形界面客户端

---

## 一、核心功能模块

### 1.1 认证与配置模块
- **Access Secret 管理**
  - 首次启动引导配置
  - 安全存储与加密（使用 Windows DPAPI）
  - 验证状态检查与刷新
  - 多账号管理（可选，Phase 2）
  
- **CLI 状态管理**
  - 自动检测 CLI 安装状态
  - 版本更新提示
  - 二进制路径配置

### 1.2 搜索模块
- **知乎搜索**
  - 搜索框 + 结果列表
  - 支持指定结果数量（1-10）
  - 结果展示：标题、作者、摘要、缩略图
  - 点击跳转原文链接
  - 复制链接/标题功能
  
- **全网搜索**
  - 搜索框 + 高级筛选
  - 索引库选择（all/realtime/static）
  - 结果数量（1-20）
  - 结果展示：标题、来源、摘要
  
- **搜索历史**
  - 本地缓存最近搜索
  - 快速重新搜索

### 1.3 热榜模块
- **热榜列表**
  - 实时获取知乎热榜（最多30条）
  - 展示：排名、标题、热度、缩略图、摘要
  - 刷新按钮
  - 点击查看详情/跳转链接
  
- **热榜趋势**（可选，Phase 2）
  - 本地记录热榜变化
  - 可视化趋势图表

### 1.4 知乎直答模块
- **智能问答界面**
  - 问题输入框（支持多行）
  - 模型选择：zhida-fast-1p5、zhida-thinking-1p5、zhida-agent
  - 输出格式：文本模式（默认）、JSON、SSE 流式
  - 流式响应实时展示
  - 答案历史记录
  - 复制/导出答案

### 1.5 我的知乎模块
- **我的创作**
  - 类型筛选（all/article/answer/pin/column）
  - 排序方式（时间/热度）
  - 分页加载（每页20-50条）
  - 展示：标题、类型、创建时间、摘要
  
- **我的关注**
  - 关注列表（分页）
  - 用户信息：昵称、简介、头像
  
- **我的收藏**
  - 最近收藏（最多50条）
  - 收藏夹列表
  - 指定收藏夹内容浏览（分页）
  - 收藏内容：标题、作者、收藏时间

### 1.6 知识库模块
- **知识库列表**
  - 作用域筛选（all/created/subscribed）
  - 展示：名称、ID、创建者、类型
  
- **知识库内容浏览**
  - 选择知识库后展示条目列表
  - 游标分页加载（每页最多20条）
  - 展示：标题、类型、上传时间
  
- **知识库搜索**
  - 搜索框 + 作用域选择
  - 支持多个知识库 ID 组合搜索
  - 结果展示（最多10条）
  
- **文件上传**
  - 文件选择器（支持拖拽）
  - 文件大小限制检查（100MB）
  - 上传进度条
  - 成功/失败反馈

### 1.7 额度查询模块
- **额度仪表盘**
  - 展示所有 API 额度：
    - global_search（全网搜索）
    - zhihu_search（知乎搜索）
    - hot_list（热榜）
    - user_data（用户数据）
    - zhida_openai（知乎直答）
    - knowledge（知识库）
    - tools（小工具）
  - 每项显示：总额度、已用、剩余、使用率进度条
  - 自动刷新（可配置间隔）
  - 手动刷新按钮

---

## 二、UI 设计方案

### 2.1 主窗口布局（Modern Fluent 风格）

```
┌─────────────────────────────────────────────────────────┐
│  ZhihuHub Desktop              [─] [□] [×]              │
├──────────┬──────────────────────────────────────────────┤
│          │  内容区域                                     │
│  导航栏   │                                              │
│          │                                              │
│  🏠 首页  │                                              │
│  🔍 搜索  │                                              │
│  🔥 热榜  │                                              │
│  💬 直答  │                                              │
│  👤 我的  │                                              │
│  📚 知识库│                                              │
│  📊 额度  │                                              │
│  ⚙️ 设置  │                                              │
│          │                                              │
│          │                                              │
├──────────┴──────────────────────────────────────────────┤
│  状态栏： CLI 状态 | 认证状态 | 最后更新时间              │
└─────────────────────────────────────────────────────────┘
```

### 2.2 配色方案

**主色调（现代蓝）**:
- Primary: `#0084FF` (知乎蓝调整)
- Primary Light: `#3FA1FF`
- Primary Dark: `#0066CC`

**辅助色**:
- Success: `#28A745`
- Warning: `#FFC107`
- Danger: `#DC3545`
- Info: `#17A2B8`

**背景色**:
- Background: `#F5F7FA`
- Card Background: `#FFFFFF`
- Sidebar: `#2C3E50`
- Sidebar Active: `#34495E`

**文字色**:
- Primary Text: `#2C3E50`
- Secondary Text: `#7F8C8D`
- Link: `#0084FF`

### 2.3 关键组件设计

#### 搜索结果卡片
```
┌─────────────────────────────────────────────┐
│ [缩略图]  标题                               │
│           作者 · 时间                        │
│           摘要内容预览...                    │
│           [📋 复制] [🔗 打开链接]            │
└─────────────────────────────────────────────┘
```

#### 热榜条目
```
┌─────────────────────────────────────────────┐
│ [1] [缩略图] 标题                            │
│              摘要预览...                     │
│              [🔥 查看详情]                   │
└─────────────────────────────────────────────┘
```

#### 直答对话框
```
┌─────────────────────────────────────────────┐
│ 模型: [zhida-fast-1p5 ▼]  格式: [文本 ▼]   │
├─────────────────────────────────────────────┤
│ 你的问题:                                    │
│ ┌─────────────────────────────────────────┐ │
│ │                                         │ │
│ └─────────────────────────────────────────┘ │
│                              [发送]          │
├─────────────────────────────────────────────┤
│ 答案:                                        │
│ ┌─────────────────────────────────────────┐ │
│ │ (流式显示答案内容...)                    │ │
│ │                                         │ │
│ └─────────────────────────────────────────┘ │
│                     [📋 复制] [💾 保存]      │
└─────────────────────────────────────────────┘
```

---

## 三、技术架构

### 3.1 项目结构

```
ZhihuHub/
├── ZhihuHub.sln
├── ZhihuHub.Core/                    # 核心业务逻辑层
│   ├── Models/                       # 数据模型
│   │   ├── SearchResult.cs
│   │   ├── HotItem.cs
│   │   ├── UserContent.cs
│   │   ├── KnowledgeBase.cs
│   │   └── QuotaInfo.cs
│   ├── Services/                     # 业务服务
│   │   ├── IZhihuCliService.cs      # CLI 调用接口
│   │   ├── ZhihuCliService.cs       # CLI 调用实现
│   │   ├── IAuthService.cs          # 认证服务接口
│   │   ├── AuthService.cs           # 认证服务实现
│   │   └── CacheService.cs          # 缓存服务
│   ├── Config/                       # 配置管理
│   │   ├── AppConfig.cs
│   │   └── CliConfig.cs
│   └── Utils/                        # 工具类
│       ├── ProcessHelper.cs         # 进程调用辅助
│       ├── JsonHelper.cs            # JSON 处理
│       └── SecureStorageHelper.cs   # 安全存储
│
├── ZhihuHub.UI/                      # WinForms UI 层
│   ├── Forms/                        # 窗体
│   │   ├── MainForm.cs              # 主窗口
│   │   ├── AuthSetupForm.cs         # 首次配置
│   │   └── SettingsForm.cs          # 设置页面
│   ├── Controls/                     # 自定义控件
│   │   ├── NavigationPanel.cs       # 导航面板
│   │   ├── SearchPanel.cs           # 搜索面板
│   │   ├── HotListPanel.cs          # 热榜面板
│   │   ├── AnswerPanel.cs           # 直答面板
│   │   ├── UserPanel.cs             # 用户面板
│   │   ├── KnowledgePanel.cs        # 知识库面板
│   │   ├── QuotaPanel.cs            # 额度面板
│   │   ├── ResultCard.cs            # 结果卡片
│   │   └── LoadingSpinner.cs        # 加载动画
│   ├── Themes/                       # 主题样式
│   │   ├── ModernTheme.cs
│   │   └── ColorScheme.cs
│   └── Resources/                    # 资源文件
│       ├── Icons/                    # 图标
│       └── Fonts/                    # 字体
│
└── ZhihuHub.Tests/                   # 单元测试
    ├── Services/
    └── Utils/
```

### 3.2 核心类设计

#### IZhihuCliService 接口

```csharp
public interface IZhihuCliService
{
    // 搜索
    Task<SearchResult> SearchZhihuAsync(string query, int count = 10);
    Task<SearchResult> SearchGlobalAsync(string query, int count = 10, string searchDb = "all");
    
    // 热榜
    Task<HotListResult> GetHotListAsync(int limit = 20);
    
    // 直答
    Task<AnswerResult> GetAnswerAsync(string query, string model = "zhida-fast-1p5", bool stream = false);
    IAsyncEnumerable<string> GetAnswerStreamAsync(string query, string model = "zhida-fast-1p5");
    
    // 用户数据
    Task<UserContentsResult> GetMyContentsAsync(string type = "all", int limit = 20, int offset = 0);
    Task<FolloweesResult> GetMyFolloweesAsync(int limit = 20, int offset = 0);
    Task<FavoritesResult> GetRecentFavoritesAsync(int limit = 20);
    Task<FavoriteListsResult> GetFavoriteListsAsync(int limit = 20);
    Task<FavoriteItemsResult> GetFavoriteItemsAsync(string urlToken, int limit = 20, int offset = 0);
    
    // 知识库
    Task<KnowledgeBasesResult> GetKnowledgeBasesAsync(string scope = "all");
    Task<KnowledgeItemsResult> GetKnowledgeItemsAsync(string baseId, int limit = 20, string cursor = null);
    Task<KnowledgeSearchResult> SearchKnowledgeAsync(string query, string scope = "personal", int limit = 10);
    Task<UploadResult> UploadFileAsync(string filePath, IProgress<int> progress = null);
    
    // 额度
    Task<QuotaResult> GetQuotaAsync(params string[] apiIds);
    
    // 状态
    Task<StatusResult> GetStatusAsync();
    Task<bool> VerifyAuthAsync();
}
```

#### ProcessHelper 辅助类

```csharp
public static class ProcessHelper
{
    public static async Task<CliResult> ExecuteCliAsync(
        string cliPath, 
        string arguments, 
        string input = null,
        int timeoutSeconds = 30);
        
    public static IAsyncEnumerable<string> ExecuteCliStreamAsync(
        string cliPath, 
        string arguments);
}
```

### 3.3 数据流设计

```
用户操作 → UI 控件 → Service 接口 → CLI 进程调用 → JSON 解析 → 数据模型 → UI 更新
                                                                    ↓
                                                              缓存服务（可选）
```

---

## 四、开发阶段规划

### Phase 1: 基础框架（预计 2-3 天）

**目标**: 搭建项目骨架，完成基础 UI 和 CLI 调用

- [x] 创建解决方案和项目结构
- [x] 实现 ProcessHelper 和 CLI 调用核心
- [x] 设计主窗口布局和导航
- [x] 实现认证配置流程
- [x] 完成搜索模块（知乎搜索 + 全网搜索）
- [x] 完成热榜模块

**交付物**: 
- 可运行的基础客户端
- 支持认证配置
- 支持搜索和热榜功能

### Phase 2: 核心功能（预计 3-4 天）

**目标**: 实现直答、用户数据、知识库模块

- [x] 实现知乎直答（文本模式）
- [x] 实现流式响应支持
- [x] 实现我的创作/关注/收藏
- [x] 实现知识库浏览和搜索
- [x] 实现文件上传功能
- [x] 实现额度查询仪表盘

**交付物**:
- 功能完整的客户端
- 支持所有 CLI 核心能力

### Phase 3: UI 优化与体验提升（预计 2-3 天）

**目标**: 打磨 UI，提升用户体验

- [x] 实现 Modern Fluent 主题
- [x] 优化控件动画和过渡效果
- [x] 实现加载状态和错误处理
- [x] 添加快捷键支持
- [x] 实现搜索历史和本地缓存
- [x] 优化响应式布局

**交付物**:
- 现代化 UI 界面
- 流畅的用户体验

### Phase 4: 高级功能（预计 2-3 天，可选）

**目标**: 增强功能和扩展性

- [ ] 多账号管理
- [ ] 导出功能（搜索结果、答案等）
- [ ] 热榜趋势可视化
- [ ] 自定义主题
- [ ] 快捷操作面板
- [ ] 自动更新检查

**交付物**:
- 增强版客户端
- 用户手册

---

## 五、技术要点

### 5.1 CLI 调用规范

**标准调用流程**:
```csharp
var startInfo = new ProcessStartInfo
{
    FileName = cliPath,
    Arguments = $"search zhihu --query \"{query}\" --count {count}",
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true,
    StandardOutputEncoding = Encoding.UTF8
};

using var process = Process.Start(startInfo);
string output = await process.StandardOutput.ReadToEndAsync();
string error = await process.StandardError.ReadToEndAsync();
await process.WaitForExitAsync();

if (process.ExitCode != 0)
{
    throw new CliException(error);
}

return JsonSerializer.Deserialize<TResult>(output);
```

### 5.2 异步与线程安全

- **UI 线程更新**: 使用 `Invoke` 或 `BeginInvoke`
- **异步操作**: 全面使用 `async/await`
- **取消令牌**: 支持长时间操作的取消
- **并发控制**: 使用 `SemaphoreSlim` 限制并发请求

### 5.3 错误处理策略

```csharp
public enum CliErrorCode
{
    AuthRequired,
    AuthInvalid,
    QuotaExceeded,
    RateLimited,
    NetworkError,
    ServerError,
    InvalidParameter,
    Unknown
}

// 统一错误处理
catch (CliException ex)
{
    switch (ex.ErrorCode)
    {
        case CliErrorCode.AuthRequired:
            ShowAuthSetupDialog();
            break;
        case CliErrorCode.QuotaExceeded:
            ShowQuotaWarning();
            break;
        // ...
    }
}
```

### 5.4 安全存储

**Access Secret 加密存储**:
```csharp
// 使用 Windows DPAPI
var encryptedData = ProtectedData.Protect(
    Encoding.UTF8.GetBytes(accessSecret),
    entropy,
    DataProtectionScope.CurrentUser
);
```

### 5.5 性能优化

- **虚拟化列表**: 大量结果使用虚拟滚动
- **图片异步加载**: 缩略图异步下载和缓存
- **结果缓存**: 相同查询短时间内复用结果
- **延迟加载**: 按需加载知识库内容

---

## 六、依赖项与环境

### 6.1 开发环境要求

**本地开发（AI 辅助）**:
- **操作系统**: Windows 11
- **.NET 版本**: 不需要本地安装（使用 GitHub Actions 云端编译）
- **IDE**: 任意文本编辑器即可
- **知乎 CLI**: 0.5.0+

**GitHub Actions 构建环境**:
- **运行器**: windows-latest
- **.NET SDK**: 8.0.x
- **构建输出**: 单文件自包含 exe

### 6.2 NuGet 包依赖

```xml
<ItemGroup>
  <!-- JSON 处理 -->
  <PackageReference Include="System.Text.Json" Version="8.0.*" />
  
  <!-- HTTP 客户端（图片下载） -->
  <PackageReference Include="System.Net.Http" Version="8.0.*" />
  
  <!-- 配置管理 -->
  <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.*" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.*" />
  
  <!-- 依赖注入 -->
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.*" />
  
  <!-- 日志 -->
  <PackageReference Include="Serilog" Version="3.1.*" />
  <PackageReference Include="Serilog.Sinks.File" Version="5.0.*" />
  
  <!-- UI 增强（可选）-->
  <PackageReference Include="MetroFramework" Version="1.4.*" />
  
  <!-- 单元测试 -->
  <PackageReference Include="xunit" Version="2.6.*" />
  <PackageReference Include="Moq" Version="4.20.*" />
</ItemGroup>
```

### 6.3 配置文件示例

**appsettings.json**:
```json
{
  "ZhihuCli": {
    "BinaryPath": "C:\\Users\\akira\\AppData\\Local\\ZhihuCLI\\current\\zhihu-cli.exe",
    "DefaultTimeout": 30,
    "CacheEnabled": true,
    "CacheDuration": 300
  },
  "UI": {
    "Theme": "Modern",
    "Language": "zh-CN",
    "AutoRefreshHot": true,
    "RefreshInterval": 300
  },
  "Logging": {
    "LogLevel": "Information",
    "LogPath": "logs/zhihuhub.log"
  }
}
```

---

## 七、CI/CD 自动化构建

### 7.1 GitHub Actions 工作流

**构建流程**: `.github/workflows/build.yml`

```yaml
name: Build ZhihuHub Desktop

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]
  workflow_dispatch:

jobs:
  build:
    runs-on: windows-latest
    
    steps:
    - name: Checkout code
      uses: actions/checkout@v4
      
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build solution
      run: dotnet build --configuration Release --no-restore
      
    - name: Run tests
      run: dotnet test --configuration Release --no-build --verbosity normal
      
    - name: Publish self-contained executable
      run: |
        dotnet publish ZhihuHub.UI/ZhihuHub.UI.csproj `
          --configuration Release `
          --runtime win-x64 `
          --self-contained true `
          --output ./publish `
          -p:PublishSingleFile=true `
          -p:IncludeNativeLibrariesForSelfExtract=true `
          -p:EnableCompressionInSingleFile=true `
          -p:DebugType=None `
          -p:DebugSymbols=false
          
    - name: Upload build artifact
      uses: actions/upload-artifact@v4
      with:
        name: ZhihuHub-win-x64-${{ github.sha }}
        path: ./publish/ZhihuHub.exe
        retention-days: 30
```

**Release 发布流程**: `.github/workflows/release.yml`

```yaml
name: Release ZhihuHub Desktop

on:
  push:
    tags:
      - 'v*.*.*'

jobs:
  release:
    runs-on: windows-latest
    
    steps:
    - name: Checkout code
      uses: actions/checkout@v4
      
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'
        
    - name: Extract version from tag
      id: get_version
      shell: pwsh
      run: |
        $version = $env:GITHUB_REF -replace 'refs/tags/v', ''
        echo "VERSION=$version" >> $env:GITHUB_OUTPUT
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Publish self-contained executable
      run: |
        dotnet publish ZhihuHub.UI/ZhihuHub.UI.csproj `
          --configuration Release `
          --runtime win-x64 `
          --self-contained true `
          --output ./publish `
          -p:PublishSingleFile=true `
          -p:IncludeNativeLibrariesForSelfExtract=true `
          -p:EnableCompressionInSingleFile=true `
          -p:DebugType=None `
          -p:DebugSymbols=false `
          -p:Version=${{ steps.get_version.outputs.VERSION }}
          
    - name: Create zip package
      shell: pwsh
      run: |
        Compress-Archive -Path ./publish/* -DestinationPath ZhihuHub-${{ steps.get_version.outputs.VERSION }}-win-x64.zip
        
    - name: Calculate SHA256
      shell: pwsh
      run: |
        $hash = Get-FileHash -Algorithm SHA256 ./publish/ZhihuHub.exe
        $hash.Hash | Out-File -FilePath ZhihuHub.exe.sha256 -NoNewline
        
    - name: Create Release
      uses: softprops/action-gh-release@v1
      with:
        files: |
          ZhihuHub-${{ steps.get_version.outputs.VERSION }}-win-x64.zip
          ZhihuHub.exe.sha256
        body: |
          ## ZhihuHub Desktop v${{ steps.get_version.outputs.VERSION }}
          
          ### 下载说明
          - 下载 `ZhihuHub-${{ steps.get_version.outputs.VERSION }}-win-x64.zip`
          - 解压后运行 `ZhihuHub.exe`
          - 首次运行需要配置 Access Secret
          
          ### 系统要求
          - Windows 10/11 x64
          - 无需安装 .NET 运行时（已包含）
          
          ### SHA256 校验
          见附件 `ZhihuHub.exe.sha256`
        draft: false
        prerelease: false
      env:
        GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

### 7.2 项目配置文件

**ZhihuHub.UI.csproj 发布配置**:

```xml
<PropertyGroup>
  <OutputType>WinExe</OutputType>
  <TargetFramework>net8.0-windows</TargetFramework>
  <UseWindowsForms>true</UseWindowsForms>
  <Nullable>enable</Nullable>
  <ApplicationIcon>Resources\app.ico</ApplicationIcon>
  
  <!-- 发布配置 -->
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
  <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
  
  <!-- 优化选项 -->
  <PublishTrimmed>false</PublishTrimmed>
  <TrimMode>partial</TrimMode>
  <InvariantGlobalization>false</InvariantGlobalization>
  
  <!-- 版本信息 -->
  <Version>1.0.0</Version>
  <AssemblyVersion>1.0.0.0</AssemblyVersion>
  <FileVersion>1.0.0.0</FileVersion>
  <Company>ZhihuHub</Company>
  <Product>ZhihuHub Desktop</Product>
  <Copyright>Copyright © 2026</Copyright>
</PropertyGroup>
```

### 7.3 本地开发流程

由于本地没有 .NET SDK，开发流程调整为：

1. **编写代码**：使用任意编辑器（VS Code、Notepad++ 等）编写 C# 代码
2. **提交到 GitHub**：`git push` 到仓库
3. **自动构建**：GitHub Actions 自动编译打包
4. **下载制品**：从 Actions 页面下载 `ZhihuHub.exe`
5. **本地测试**：运行 exe，测试功能
6. **迭代开发**：修复问题，重复 1-5 步骤

**快速触发构建**：
- 方式 1：推送到 `main` 或 `develop` 分支
- 方式 2：在 GitHub Actions 页面手动触发 `workflow_dispatch`

**获取构建产物**：
- **开发版本**：GitHub Actions → 最新 workflow run → Artifacts → 下载 zip
- **正式版本**：打 tag (`git tag v1.0.0 && git push --tags`) → Releases 页面下载

### 7.4 构建优化

**单文件打包大小优化**：
- 启用压缩：`EnableCompressionInSingleFile=true`
- 移除调试符号：`DebugType=None`
- 预计大小：~50-80 MB（包含 .NET 运行时）

**可选的更激进优化**（可能影响兼容性）：
```xml
<PublishTrimmed>true</PublishTrimmed>
<TrimMode>full</TrimMode>
<!-- 预计大小：~30-50 MB，但可能影响反射和动态加载 -->
```

### 7.5 版本管理

**版本号规范**: `v{major}.{minor}.{patch}`

- **major**: 大版本，架构重构或重大功能变更
- **minor**: 小版本，新增功能
- **patch**: 补丁版本，Bug 修复

**示例**:
- `v1.0.0`: 首次正式发布
- `v1.1.0`: 新增多账号管理
- `v1.1.1`: 修复搜索 Bug

**发布流程**:
```bash
# 本地打标签
git tag -a v1.0.0 -m "Release version 1.0.0"

# 推送标签到 GitHub
git push origin v1.0.0

# GitHub Actions 自动构建并创建 Release
```

---

## 八、测试计划

### 8.1 单元测试

- CLI 调用正确性
- JSON 解析准确性
- 错误处理覆盖
- 数据模型验证

**注意**: 单元测试在 GitHub Actions 中自动运行，构建失败会阻止打包

### 8.2 集成测试

- 完整业务流程测试
- 认证流程测试
- 多模块联动测试

### 8.3 UI 测试

- 响应式布局验证
- 多分辨率适配
- 键盘快捷键测试
- 长时间运行稳定性

**本地测试流程**:
1. 从 GitHub Actions 下载 exe
2. 在 Windows 11 环境测试
3. 记录问题并反馈
4. 修复后重新构建

---

## 九、交付清单

### 9.1 核心交付物

1. **源代码仓库**: 完整的 Git 仓库（GitHub）
2. **自动构建**: GitHub Actions 工作流配置
3. **可执行程序**: `ZhihuHub.exe`（从 Actions/Releases 下载）
4. **用户手册**: `README.md` + `UserGuide.md`
5. **开发文档**: 架构设计 + API 文档

### 9.2 GitHub 仓库结构

```
ZhihuHub/
├── .github/
│   └── workflows/
│       ├── build.yml           # 持续集成构建
│       └── release.yml         # 版本发布
├── ZhihuHub.Core/              # 核心业务层
├── ZhihuHub.UI/                # UI 层
├── ZhihuHub.Tests/             # 测试
├── docs/                       # 文档
│   ├── Architecture.md
│   ├── API.md
│   └── UserGuide.md
├── .gitignore
├── LICENSE
├── README.md
└── ZhihuHub.sln

```

### 9.3 README.md 内容

````markdown
# ZhihuHub Desktop

知乎开放平台 CLI 的现代化图形界面客户端

## 功能特性

- 🔍 **智能搜索**: 知乎内容 + 全网搜索
- 🔥 **实时热榜**: 掌握知乎热点动态
- 💬 **知乎直答**: AI 驱动的智能问答
- 👤 **我的知乎**: 创作、关注、收藏管理
- 📚 **知识库**: 浏览、搜索、上传文件
- 📊 **额度监控**: API 用量实时查看

## 快速开始

### 下载安装

1. 前往 [Releases](https://github.com/yourusername/ZhihuHub/releases) 页面
2. 下载最新版本的 `ZhihuHub-x.x.x-win-x64.zip`
3. 解压到任意目录
4. 运行 `ZhihuHub.exe`

### 首次配置

1. 打开程序后会引导你配置 Access Secret
2. 访问 [知乎开放平台](https://developer.zhihu.com/profile)
3. 生成 Access Secret 并粘贴到程序中
4. 完成配置，开始使用

## 系统要求

- Windows 10/11 (64-bit)
- 无需安装 .NET 运行时

## 开发构建

本项目使用 GitHub Actions 自动构建：

```bash
# 触发构建
git push origin main

# 创建发布版本
git tag v1.0.0
git push --tags
```

## 技术栈

- .NET 8.0
- Windows Forms
- 知乎开放平台 CLI 0.5.0

## 许可证

MIT License
````

### 9.4 附加文档

1. **安装指南**: 首次配置步骤（包含在 README 中）
2. **常见问题**: `docs/FAQ.md`
3. **更新日志**: `CHANGELOG.md`
4. **架构文档**: `docs/Architecture.md`

---

## 十、风险与挑战

### 10.1 技术风险

| 风险项 | 影响 | 应对策略 |
|--------|------|----------|
| CLI 输出格式变化 | 高 | 版本锁定 + 兼容层 |
| 大数据量卡顿 | 中 | 虚拟化 + 分页加载 |
| 流式响应处理 | 中 | 异步流 + 缓冲机制 |
| GitHub Actions 构建失败 | 中 | 本地验证语法 + 测试覆盖 |
| 无本地 .NET 环境调试 | 中 | 依赖日志 + 云端构建反馈 |

### 10.2 产品风险

| 风险项 | 影响 | 应对策略 |
|--------|------|----------|
| 用户体验不佳 | 高 | 迭代优化 + 用户反馈 |
| 功能理解偏差 | 中 | 需求澄清 + Demo 确认 |
| 性能不达标 | 中 | 性能测试 + 优化 |

### 10.3 开发流程风险

**无本地 .NET 环境的影响**:
- ⚠️ **无法本地编译验证**: 需要依赖 GitHub Actions 反馈（约等待 3-5 分钟）
- ⚠️ **调试困难**: 无法使用断点调试，只能通过日志排查
- ⚠️ **迭代周期变长**: 每次修改都需要提交 → 构建 → 下载 → 测试

**应对措施**:
1. **代码审查**: AI 辅助静态检查，减少语法错误
2. **完善日志**: 详细的日志输出，便于问题定位
3. **分批提交**: 每次提交前仔细检查，减少无效构建
4. **本地语法检查**: 使用在线 C# 语法检查器
5. **模块化开发**: 小步快跑，每次只修改一个小模块

---

## 十一、后续扩展方向

### 10.1 短期（3 个月内）

- macOS 和 Linux 跨平台支持（Avalonia UI）
- 浏览器插件（Chrome/Edge）
- 移动端伴侣 APP

### 10.2 中期（6 个月内）

- AI 助手集成（对话式操作）
- 数据分析与可视化
- 批量操作与自动化
- API 速率优化建议

### 10.3 长期（1 年内）

- 团队协作功能
- 云端同步
- 插件生态系统
- 开发者 SDK

---

## 总结

本计划涵盖了一个功能完整、UI 现代化的知乎 CLI 图形化客户端的全部开发内容。

**预计总开发时间**: 9-13 天（按 Phase 1-3 计算）

**核心优势**:
1. ✅ **功能完整**: 覆盖 CLI 所有能力（搜索、热榜、直答、用户数据、知识库、额度）
2. ✅ **UI 现代化**: Modern Fluent 风格，符合 2026 年设计趋势
3. ✅ **架构清晰**: 三层分层设计，易于维护和扩展
4. ✅ **体验流畅**: 全异步处理，流式响应，响应迅速
5. ✅ **安全可靠**: DPAPI 加密存储，完善错误处理
6. ✅ **云端构建**: GitHub Actions 自动化打包，无需本地 .NET 环境

**开发模式特点**:
- 📝 **编写代码**: 使用任意编辑器，AI 辅助静态检查
- ☁️ **云端编译**: GitHub Actions 自动构建（3-5 分钟）
- 📦 **下载测试**: 直接下载 exe，本地运行验证
- 🔄 **快速迭代**: 提交 → 构建 → 测试 → 优化

**下一步行动**:
1. ✅ 审阅开发计划（当前步骤）
2. 📂 初始化 Git 仓库和项目结构
3. ⚙️ 配置 GitHub Actions 工作流
4. 💻 开始 Phase 1 开发（基础框架）

**请审阅此计划并确认：**
- [ ] 功能范围是否符合预期？
- [ ] 开发阶段划分是否合理？
- [ ] 云端构建方案是否可行？
- [ ] 是否需要调整或补充？

**确认后我将立即开始创建项目结构和 GitHub Actions 配置！**
