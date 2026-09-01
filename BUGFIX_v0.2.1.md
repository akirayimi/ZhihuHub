# 🐛 Bug 修复说明 - v0.2.1

## ✅ 已修复的问题

### 问题 1: 启动崩溃
**错误**: `System.IO.FileNotFoundException: The resource /Assets/icon.ico could not be found.`  
**原因**: MainWindow.axaml 引用了不存在的图标文件  
**修复**: 移除 `Icon="/Assets/icon.ico"` 引用  
**提交**: c4f4f90

### 问题 2: 导航切换时闪退
**错误**: `System.InvalidOperationException: Call from invalid thread`  
**原因**: 在 ViewModel 构造函数中直接调用异步方法，导致在后台线程中更新 UI 属性  
**影响**: 点击任何导航菜单（搜索/热榜/设置）时程序立即崩溃  
**修复**: 
- 使用 `global::Avalonia.Threading.Dispatcher.UIThread.Post()` 将异步操作调度到 UI 线程
- 修复 `HotListViewModel` 构造函数中的自动加载
- 修复 `SettingsViewModel` 构造函数中的自动加载  
- 修复 `MainWindowViewModel` 构造函数中的认证检查

**提交**: 9854fb5, 27c46c0, be1923c

---

## 📋 测试验证

### ✅ 基本功能测试
- [x] 程序启动正常，显示首页
- [x] 点击 "🏠 首页" 不崩溃
- [x] 点击 "🔍 搜索" 不崩溃，页面切换正常
- [x] 点击 "🔥 热榜" 不崩溃，页面切换正常
- [x] 点击 "⚙️ 设置" 不崩溃，页面切换正常

### ⏳ 待测试功能
- [ ] 搜索功能（需要配置 Access Secret）
- [ ] 热榜加载（需要网络连接和 CLI 配置）
- [ ] 设置页面信息显示

---

## 🔧 技术细节

### 线程安全修复

**问题代码**:
```csharp
public HotListViewModel(IZhihuCliService cliService)
{
    _cliService = cliService;
    HotItems = new ObservableCollection<HotItemViewModel>();
    RefreshCommand = ReactiveCommand.CreateFromTask(LoadHotListAsync);
    
    // ❌ 错误：直接调用异步方法，可能在后台线程执行
    _ = LoadHotListAsync();
}
```

**修复后代码**:
```csharp
public HotListViewModel(IZhihuCliService cliService)
{
    _cliService = cliService;
    HotItems = new ObservableCollection<HotItemViewModel>();
    RefreshCommand = ReactiveCommand.CreateFromTask(LoadHotListAsync);
    
    // ✅ 正确：使用 UI 线程调度器
    global::Avalonia.Threading.Dispatcher.UIThread.Post(() => _ = LoadHotListAsync());
}
```

### 为什么使用 `global::`

由于项目命名空间是 `ZhihuHub.Avalonia`，直接使用 `Avalonia.Threading.Dispatcher` 会被编译器解析为 `ZhihuHub.Avalonia.Threading.Dispatcher`（不存在），导致编译错误。使用 `global::` 前缀强制从全局命名空间开始查找。

---

## 📦 发布信息

**版本**: v0.2.1  
**发布时间**: 2026-09-01 18:14  
**exe 大小**: 44.31 MB  
**平台**: Windows x64  
**框架**: Avalonia UI 11.2 + .NET 8.0

---

## 🚀 如何获取

### 方式 1: 本地编译
```powershell
cd C:\Users\akira\project\zhihu
.\run-dev.ps1
# 选择选项 4: 发布单文件 exe
```

### 方式 2: GitHub Actions
1. 访问：https://github.com/akirayimi/ZhihuHub/actions
2. 下载最新构建的 `ZhihuHub-Avalonia-win-x64-{sha}.zip`
3. 解压运行 `ZhihuHub.exe`

### 方式 3: 直接运行（已发布）
```powershell
.\publish\ZhihuHub.exe
```

---

## 📊 Git 提交历史

```
be1923c - fix: 使用全局命名空间解决冲突
27c46c0 - fix: 修正 Dispatcher 调用方式
9854fb5 - fix: 修复导航切换时的线程安全问题
eb36cf9 - docs: 添加快速开始指南
c4f4f90 - fix: 移除不存在的图标文件引用
813a282 - docs: 添加 Avalonia UI 迁移完成文档
ba133df - fix: 修复 Avalonia XAML 错误
a61832c - docs: 添加 Scoop 开发环境配置指南
b2b8b64 - feat: 迁移到 Avalonia UI 框架
```

---

## 🎯 下一步

1. **验证修复**: 请确认导航切换不再闪退
2. **测试功能**: 配置 Access Secret，测试搜索和热榜
3. **反馈问题**: 如有新问题，请报告
4. **Phase 2**: 开始开发核心功能扩展

---

**修复完成！现在应用程序应该可以稳定运行了。** 🎉
