# 🚀 ZhihuHub Desktop - 快速开始

## ✅ 最新修复 (v0.2.0)

**问题**: 启动崩溃 `System.IO.FileNotFoundException: /Assets/icon.ico`  
**修复**: 已移除不存在的图标引用  
**状态**: ✅ 可以正常启动

---

## 🎯 运行方式

### 方式 1: 直接运行 exe（推荐）

```powershell
# 进入项目目录
cd C:\Users\akira\project\zhihu

# 运行已发布的 exe
.\publish\ZhihuHub.exe
```

**如果没有 publish 文件夹**，先发布：
```powershell
# 使用启动脚本
.\run-dev.ps1
# 选择选项 4: 发布单文件 exe
```

---

### 方式 2: 使用启动脚本

```powershell
.\run-dev.ps1

# 菜单选项:
#   1. 恢复依赖 (首次使用)
#   2. 运行项目（热重载，开发用）
#   3. 编译 Release 版本
#   4. 发布单文件 exe  ← 推荐
#   5. 在 VS Code 中打开
```

---

### 方式 3: 从 GitHub Actions 下载

1. 访问：https://github.com/akirayimi/ZhihuHub/actions
2. 点击最新的成功构建（绿色勾号 ✅）
3. 下载 `ZhihuHub-Avalonia-win-x64-{sha}.zip`
4. 解压并运行 `ZhihuHub.exe`

---

## 🔍 验证清单

启动后，您应该看到：

### 窗口布局
- ✅ 窗口标题："ZhihuHub Desktop"
- ✅ 左侧深蓝色侧边栏（宽 200px）
- ✅ 右侧浅灰色主内容区
- ✅ 底部状态栏（白色）

### 侧边导航（4 个按钮）
- 🏠 首页
- 🔍 搜索
- 🔥 热榜
- ⚙️ 设置

### 高 DPI 效果（2880x1800 175%）
- ✅ 字体清晰锐利，不模糊
- ✅ Emoji 图标清晰
- ✅ 控件大小适中
- ✅ 间距比例协调

---

## 🎨 首页内容

您应该看到：

1. **欢迎标题**: "欢迎使用 ZhihuHub Desktop"（大字体，深色）
2. **副标题**: "知乎开放平台 CLI 的现代化图形界面客户端"（中等字体，灰色）
3. **功能卡片**（4 个白色卡片）:
   - 🔍 智能搜索
   - 🔥 实时热榜
   - 👤 我的知乎
   - 📚 知识库
4. **版本信息**: "版本 0.2.0 Beta (Avalonia UI)"（小字体，底部）

---

## 🧪 功能测试

### 1. 测试搜索功能
1. 点击侧边栏 "🔍 搜索"
2. 选择搜索类型（知乎搜索 / 全网搜索）
3. 输入关键词，例如："人工智能"
4. 点击 "🔍 搜索" 按钮
5. 应该看到搜索结果卡片

### 2. 测试热榜功能
1. 点击侧边栏 "🔥 热榜"
2. 应该自动加载知乎热榜
3. 查看排名显示（前三名有特殊颜色）
4. 点击 "🔄 刷新" 按钮更新

### 3. 测试设置
1. 点击侧边栏 "⚙️ 设置"
2. 查看认证状态
3. 查看 CLI 路径和版本

---

## ❌ 常见问题

### 问题 1: 双击 exe 无反应

**可能原因**:
- 缺少 VC++ Redistributable 2022
- 防火墙/杀毒软件拦截

**解决方案**:
```powershell
# 检查是否安装 VC++ Redistributable
scoop list vcredist2022

# 如果没有，安装
scoop install extras/vcredist2022
```

### 问题 2: 窗口闪退

**排查步骤**:
```powershell
# 1. 查看 Windows 事件日志
Get-EventLog -LogName Application -Source ".NET Runtime" -Newest 1 | Select-Object Message

# 2. 在 PowerShell 中运行，查看错误信息
.\publish\ZhihuHub.exe
```

### 问题 3: 搜索/热榜加载失败

**可能原因**:
- 知乎 CLI 未安装或配置
- Access Secret 未配置
- 网络连接问题

**解决方案**:
1. 检查 CLI 是否安装：
```powershell
Test-Path "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe"
```

2. 配置 Access Secret（首次使用）:
   - 点击 "设置" 页面
   - 如果提示需要配置，按照引导操作

---

## 💻 开发模式（热重载）

如果您想边修改代码边查看效果：

```powershell
# 方式 1: 使用启动脚本
.\run-dev.ps1
# 选择选项 2

# 方式 2: 手动运行
$dotnetPath = "$env:USERPROFILE\scoop\apps\dotnet8-sdk\current"
$env:DOTNET_ROOT = $dotnetPath
$env:Path = "$dotnetPath;$env:Path"

dotnet watch run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj
```

修改 XAML 或 C# 代码后，按 `Ctrl+R` 重启应用。

---

## 📦 重新发布

如果修改了代码，需要重新发布 exe：

```powershell
# 使用启动脚本
.\run-dev.ps1
# 选择选项 4

# 或者手动命令
$dotnetPath = "$env:USERPROFILE\scoop\apps\dotnet8-sdk\current"
$env:DOTNET_ROOT = $dotnetPath
$env:Path = "$dotnetPath;$env:Path"

dotnet publish ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output ./publish `
  -p:PublishSingleFile=true
```

---

## 📊 性能指标

- **启动时间**: < 2 秒
- **内存占用**: 50-100 MB
- **exe 大小**: ~44 MB
- **窗口大小**: 1200x800（可调整，最小 1000x600）

---

## 🎯 下一步

1. ✅ 运行程序，查看高 DPI 效果
2. ✅ 测试搜索和热榜功能
3. ✅ 配置 Access Secret（如果需要）
4. ✅ 反馈任何显示或功能问题

---

**祝使用愉快！** 🎉

如有问题，请查看:
- `SCOOP_SETUP.md` - Scoop 环境配置
- `AVALONIA_MIGRATION.md` - 迁移详情
- `DEPLOYMENT.md` - 部署说明
