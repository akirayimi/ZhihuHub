# Scoop 开发环境配置指南

## ✅ 已安装的工具

通过 Scoop 安装的开发工具：

| 工具 | 版本 | 用途 |
|------|------|------|
| **.NET 8 SDK** | 8.0.424 | 编译和运行 Avalonia 项目 |
| **Git** | 2.54.0 | 版本控制 |
| **Visual Studio Code** | 1.135.0 | 代码编辑器（推荐） |
| **VC++ Redistributable 2022** | 14.51.36247.0 | Avalonia UI 运行时依赖 |
| **知乎 CLI** | 0.5.0+ | API 调用（已预装） |

---

## 🚀 快速开始

### 1. 克隆或进入项目目录
```powershell
cd C:\Users\akira\project\zhihu
```

### 2. 恢复 NuGet 依赖
```powershell
dotnet restore
```

### 3. 运行 Avalonia 版本（调试）
```powershell
dotnet run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj
```

### 4. 发布单文件可执行文件
```powershell
dotnet publish ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output ./publish `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:EnableCompressionInSingleFile=true
```

生成的 exe 文件：`./publish/ZhihuHub.exe`

---

## 📦 Scoop 常用命令

### 更新工具
```powershell
# 更新所有 bucket（软件源）
scoop update

# 更新特定工具
scoop update dotnet8-sdk

# 更新所有已安装的工具
scoop update *
```

### 管理工具
```powershell
# 查看已安装的工具
scoop list

# 卸载工具
scoop uninstall vscode

# 搜索工具
scoop search nodejs

# 查看工具信息
scoop info dotnet8-sdk
```

### 管理 Bucket
```powershell
# 列出已添加的 bucket
scoop bucket list

# 添加常用 bucket
scoop bucket add extras      # GUI 应用
scoop bucket add versions    # 历史版本
scoop bucket add nerd-fonts  # 字体
```

---

## 🔧 VS Code 配置（可选）

### ⚠️ 重要：首次使用前必须执行

由于可能存在旧的 .NET SDK 残留，**首次使用前必须设置环境变量**：

**方式 1: 使用启动脚本（推荐）**
```powershell
# 直接运行项目启动脚本
.\run-dev.ps1
```

**方式 2: 手动设置环境变量**
```powershell
# 设置环境变量（每次打开新的 PowerShell 窗口都需要执行）
$dotnetPath = "$env:USERPROFILE\scoop\apps\dotnet8-sdk\current"
$env:DOTNET_ROOT = $dotnetPath
$env:MSBuildSDKsPath = Join-Path $dotnetPath "sdk\8.0.424\Sdks"
$env:Path = "$dotnetPath;$env:Path"

# 验证
dotnet --version  # 应该显示 8.0.424
```

**方式 3: 永久设置（重启后生效）**
```powershell
# 清理旧环境变量
[Environment]::SetEnvironmentVariable("DOTNET_ROOT", $null, "User")
[Environment]::SetEnvironmentVariable("MSBuildSDKsPath", $null, "User")

# 设置新环境变量
$dotnetPath = "$env:USERPROFILE\scoop\apps\dotnet8-sdk\current"
[Environment]::SetEnvironmentVariable("DOTNET_ROOT", $dotnetPath, "User")

# 重启 PowerShell 后生效
```

### 安装推荐扩展
在 VS Code 中打开项目，建议安装以下扩展：

1. **C# Dev Kit** - Microsoft 官方 C# 支持
2. **AvaloniaUI** - Avalonia XAML 智能提示
3. **GitLens** - 增强的 Git 功能

### 快捷安装命令
```powershell
code --install-extension ms-dotnettools.csdevkit
code --install-extension AvaloniaTeam.vscode-avalonia
code --install-extension eamodio.gitlens
```

### 打开项目
```powershell
# 在 VS Code 中打开项目
code .
```

---

## 🛠️ 开发工作流

### 方式 1: 本地开发（推荐用于调试）
```powershell
# 1. 恢复依赖
dotnet restore

# 2. 运行项目（热重载支持）
dotnet watch run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj

# 3. 编译检查
dotnet build --configuration Release
```

### 方式 2: GitHub Actions（推荐用于发布）
```powershell
# 1. 提交代码
git add .
git commit -m "feat: your changes"
git push origin main

# 2. 等待自动构建（3-5 分钟）
# 访问: https://github.com/akirayimi/ZhihuHub/actions

# 3. 下载 Artifacts
# ZhihuHub-Avalonia-win-x64-{sha}.zip
```

---

## 📊 项目结构

```
ZhihuHub/
├── ZhihuHub.Core/          # 核心业务逻辑（CLI 调用、数据模型）
├── ZhihuHub.Avalonia/      # Avalonia UI 项目（推荐）
├── ZhihuHub.UI/            # Windows Forms 项目（已弃用）
└── .github/workflows/      # GitHub Actions 自动构建
```

---

## 🎯 故障排查

### 问题 1: dotnet 命令找不到
```powershell
# 刷新环境变量
refreshenv

# 或者重启 PowerShell
```

### 问题 2: Avalonia 编译错误
```powershell
# 清理并重新构建
dotnet clean
dotnet restore
dotnet build
```

### 问题 3: 知乎 CLI 连接失败
```powershell
# 检查 CLI 状态
& "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe" status

# 配置 Access Secret
& "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe" auth set-secret <your-secret>
```

### 问题 4: Scoop 更新失败
```powershell
# 强制更新 Scoop 自身
scoop update

# 重置 bucket
scoop bucket rm main
scoop bucket add main
```

---

## 🔄 环境维护

### 定期更新（每月）
```powershell
# 1. 更新 Scoop 和 bucket
scoop update

# 2. 更新所有工具
scoop update *

# 3. 清理旧版本
scoop cleanup *

# 4. 检查并修复
scoop checkup
```

### 清理缓存
```powershell
# 清理下载缓存
scoop cache rm *

# 查看缓存大小
scoop cache show
```

---

## 📚 参考资源

- **Scoop 官网**: https://scoop.sh/
- **Scoop GitHub**: https://github.com/ScoopInstaller/Scoop
- **.NET 8 文档**: https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8/
- **Avalonia UI 文档**: https://docs.avaloniaui.net/
- **知乎开放平台**: https://developer.zhihu.com/

---

## ✨ 下一步

1. ✅ 运行项目：`dotnet run --project ZhihuHub.Avalonia/ZhihuHub.Avalonia.csproj`
2. ✅ 在高 DPI 显示器上测试 UI 效果
3. ✅ 配置 Access Secret（首次运行时引导）
4. ✅ 测试搜索、热榜等功能

---

**开发环境配置完成！** 🎉

如有问题，请查看 [DEPLOYMENT.md](./DEPLOYMENT.md) 或 [README.md](./README.md)。
