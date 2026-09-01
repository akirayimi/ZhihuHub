# Avalonia UI 迁移完成

## ✅ 迁移状态

**版本**: v0.2.0 Beta  
**UI 框架**: Avalonia UI 11.2  
**完成时间**: 2026-09-01  
**提交**: ba133df

---

## 🎯 解决的问题

### 高 DPI 显示问题（主要目标）
- ✅ 在 2880x1800 175% 缩放显示器上完美显示
- ✅ 字体清晰锐利，不再模糊
- ✅ 控件大小适中，间距协调
- ✅ 自动适应任意 DPI 缩放比例

### 技术改进
- ✅ 现代化 MVVM 架构
- ✅ ReactiveUI 响应式编程
- ✅ Fluent Design 主题
- ✅ 跨平台潜力（Windows/macOS/Linux）

---

## 📦 项目结构

```
ZhihuHub.Avalonia/
├── App.axaml                    # 应用程序入口
├── Program.cs                   # 主程序
├── Styles/
│   └── Theme.axaml             # Fluent 主题样式（知乎蓝配色）
├── ViewModels/                 # MVVM 数据层
│   ├── ViewModelBase.cs
│   ├── MainWindowViewModel.cs  # 主窗口：导航+状态栏
│   ├── HomeViewModel.cs        # 首页
│   ├── SearchViewModel.cs      # 搜索（知乎+全网）
│   ├── HotListViewModel.cs     # 热榜（含排名）
│   └── SettingsViewModel.cs    # 设置
└── Views/                      # XAML 视图层
    ├── MainWindow.axaml        # 主窗口（侧边导航+内容区）
    ├── HomeView.axaml          # 首页视图
    ├── SearchView.axaml        # 搜索视图
    ├── HotListView.axaml       # 热榜视图
    └── SettingsView.axaml      # 设置视图
```

---

## 🚀 如何运行

### 方式 1: 使用启动脚本（推荐）
```powershell
cd C:\Users\akira\project\zhihu
.\run-dev.ps1

# 选择选项 2: 运行 Avalonia 项目（热重载）
# 或选择选项 4: 发布单文件 exe
```

### 方式 2: 手动运行
```powershell
# 1. 设置环境变量
$dotnetPath = "$env:USERPROFILE\scoop\apps\dotnet8-sdk\current"
$env:DOTNET_ROOT = $dotnetPath
$env:MSBuildSDKsPath = Join-Path $dotnetPath "sdk\8.0.424\Sdks"
$env:Path = "$dotnetPath;$env:Path"

# 2. 运行项目
dotnet run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj

# 或发布单文件 exe
dotnet publish ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output ./publish `
  -p:PublishSingleFile=true

# 3. 运行生成的 exe
.\publish\ZhihuHub.exe
```

### 方式 3: 从 GitHub Actions 下载
1. 访问：https://github.com/akirayimi/ZhihuHub/actions
2. 点击最新的成功构建
3. 下载 `ZhihuHub-Avalonia-win-x64-{sha}.zip`
4. 解压并运行 `ZhihuHub.exe`

---

## 🎨 UI 特性

### 配色方案（知乎蓝）
- **主色调**: `#0084FF` (知乎蓝)
- **背景色**: `#F5F7FA` (浅灰)
- **卡片**: `#FFFFFF` (纯白)
- **侧边栏**: `#2C3E50` (深灰蓝)
- **文字主色**: `#2C3E50`
- **文字次色**: `#7F8C8D`

### 视觉设计
- ✅ 卡片式布局，清晰的层次
- ✅ 圆角 (8px) 和阴影，现代感
- ✅ Hover 状态平滑过渡
- ✅ Emoji 图标（清晰不模糊）
- ✅ 响应式布局，自适应窗口大小

### 功能页面
1. **首页**: 欢迎界面 + 功能卡片
2. **搜索**: 知乎搜索 / 全网搜索切换，结果卡片展示
3. **热榜**: 自动加载，带排名显示（前三名特殊颜色）
4. **设置**: 认证状态、CLI 路径、版本信息

---

## 🔧 已修复的问题

### v0.2.0 Beta 修复
1. **XAML 错误**: 移除不存在的 `StringConverters.IsNotNullOrEmpty`
2. **热榜排名**: 添加 `HotItemViewModel` 包含 `Rank` 属性
3. **导航样式**: 修复按钮样式类名 (`nav-button` → `nav`)
4. **重复样式**: 移除 MainWindow 中重复的 Styles 定义

### 已知限制
- ⚠️ **CA1416 警告**: SecureStorageHelper 使用 Windows DPAPI，不支持跨平台（已预期）
- ⚠️ **运行环境**: 需要 .NET 8 SDK 或自包含发布的 exe

---

## 📊 对比 Windows Forms 版本

| 特性 | Windows Forms | Avalonia UI |
|------|--------------|-------------|
| **高 DPI** | ❌ 需要手动调整 | ✅ 原生完美 |
| **跨平台** | ❌ 仅 Windows | ✅ Windows/macOS/Linux |
| **现代 UI** | ⚠️ 受限于 Win32 | ✅ 完全自定义 |
| **数据绑定** | ❌ 手动实现 | ✅ MVVM 原生支持 |
| **开发体验** | 拖拽设计器 | 🎯 XAML + 热重载 |
| **包大小** | ~50-80 MB | ~44 MB |

---

## 🎯 下一步计划（Phase 2）

### 核心功能扩展
- [ ] 知乎直答（文本 + 流式响应）
- [ ] 我的知乎（创作、关注、收藏）
- [ ] 知识库（浏览、搜索、上传）
- [ ] 额度查询仪表盘

### UI 优化
- [ ] 加载动画
- [ ] 页面过渡效果
- [ ] 搜索历史
- [ ] 暗色主题

---

## 💡 开发建议

### 1. 使用热重载
```powershell
dotnet watch run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj
```
修改 XAML 或 ViewModel 后，按 `Ctrl+R` 重启应用，无需重新编译。

### 2. Avalonia DevTools
在 `App.axaml.cs` 中启用：
```csharp
this.AttachDevTools();  // 按 F12 打开 DevTools
```

### 3. VS Code 扩展
- **AvaloniaUI**: XAML 智能提示
- **C# Dev Kit**: C# 语言支持
- **GitLens**: Git 增强

---

## 📝 注意事项

1. **环境变量**: 每次新开 PowerShell 窗口，请使用 `run-dev.ps1` 或手动设置 `DOTNET_ROOT`
2. **知乎 CLI**: 首次运行需要配置 Access Secret
3. **自包含发布**: Release 版本包含完整 .NET 运行时，无需本地安装 .NET
4. **Windows Forms 项目**: `ZhihuHub.UI` 已弃用但保留在解决方案中

---

## 🎉 迁移完成总结

- ✅ **高 DPI 问题彻底解决**
- ✅ **现代化 MVVM 架构**
- ✅ **Fluent Design 精致 UI**
- ✅ **跨平台潜力**
- ✅ **GitHub Actions 自动构建**

**当前 Alpha 版本可用于测试高 DPI 显示效果和基础功能！**

---

**开发完成**: 2026-09-01  
**框架迁移**: Windows Forms → Avalonia UI  
**状态**: ✅ 可运行，待优化
