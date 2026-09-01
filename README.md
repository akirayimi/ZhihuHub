# ZhihuHub Desktop

知乎开放平台 CLI 的现代化图形界面客户端

![Version](https://img.shields.io/badge/version-0.1.0--alpha-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![Platform](https://img.shields.io/badge/platform-Windows-0078D6)

## 🎯 项目状态

**当前版本**: 0.1.0 Alpha  
**开发阶段**: Phase 1 基础框架完成

## ✨ 功能特性

### 已实现 (Phase 1)
- ✅ **认证管理**: Access Secret 配置和验证
- ✅ **智能搜索**: 知乎内容搜索 + 全网搜索
- ✅ **实时热榜**: 获取知乎热榜，实时更新
- ✅ **现代 UI**: Modern Fluent 风格设计
- ✅ **状态管理**: 实时状态显示和错误处理

### 待实现 (Phase 2+)
- ⏳ **知乎直答**: AI 驱动的智能问答
- ⏳ **我的知乎**: 创作、关注、收藏管理
- ⏳ **知识库**: 浏览、搜索、上传文件
- ⏳ **额度监控**: API 用量实时查看

## 📦 快速开始

### 系统要求

- Windows 10/11 (64-bit)
- 知乎 CLI 0.5.0+ (首次运行时自动引导安装)
- 无需安装 .NET 运行时（自包含发布）

### 下载安装

1. 前往 [GitHub Actions](../../actions) 页面
2. 找到最新的成功构建
3. 下载 **Artifacts** 中的 `ZhihuHub-win-x64-xxx.zip`
4. 解压到任意目录
5. 运行 `ZhihuHub.exe`

### 首次配置

1. 打开程序后会提示配置 Access Secret
2. 点击"是"，然后点击"打开知乎开放平台"按钮
3. 在浏览器中登录并生成 Access Secret
4. 复制 Access Secret 并粘贴到程序中
5. 点击"提交"完成配置

## 🚀 开发构建

### 本地开发

本项目使用 **GitHub Actions 云端构建**，无需本地安装 .NET SDK。

**开发流程**:
```bash
# 1. 克隆仓库
git clone <your-repo-url>
cd zhihu

# 2. 修改代码（使用任意编辑器）

# 3. 提交并推送
git add .
git commit -m "Your changes"
git push origin main

# 4. GitHub Actions 自动构建（3-5 分钟）

# 5. 下载构建产物
# 访问 GitHub Actions 页面 → 最新 workflow run → Artifacts
```

### 手动触发构建

访问 GitHub Actions 页面，点击 "Build ZhihuHub Desktop"，然后点击 "Run workflow"。

### 创建发布版本

```bash
# 打标签
git tag -a v0.1.0 -m "Release version 0.1.0 Alpha"

# 推送标签
git push origin v0.1.0

# GitHub Actions 自动构建并创建 Release
```

## 🏗️ 项目结构

```
ZhihuHub/
├── .github/
│   └── workflows/
│       └── build.yml           # CI/CD 构建配置
├── ZhihuHub.Core/              # 核心业务层
│   ├── Models/                 # 数据模型
│   ├── Services/               # 业务服务
│   ├── Config/                 # 配置管理
│   └── Utils/                  # 工具类
├── ZhihuHub.UI/                # UI 层
│   ├── Forms/                  # 窗体
│   ├── Controls/               # 自定义控件
│   └── Themes/                 # 主题样式
├── .gitignore
├── README.md
└── ZhihuHub.sln                # 解决方案文件
```

## 🎨 技术栈

- **.NET 8.0**: 现代化的跨平台框架
- **Windows Forms**: 原生 Windows UI
- **知乎开放平台 CLI 0.5.0**: 后端 API 调用
- **GitHub Actions**: 自动化构建和发布

## 🎨 设计理念

### 配色方案

- **主色调**: 知乎蓝 `#0084FF`
- **背景色**: 浅灰 `#F5F7FA`
- **卡片背景**: 纯白 `#FFFFFF`
- **侧边栏**: 深灰蓝 `#2C3E50`

### UI 风格

- Modern Fluent 设计语言
- 扁平化按钮和卡片
- 圆润的边角和阴影
- 清晰的层次结构

## 📝 更新日志

### v0.1.0-alpha (2026-09-01)

**Phase 1 基础框架完成**

- ✅ 初始化项目结构
- ✅ 实现认证配置流程
- ✅ 实现知乎搜索和全网搜索
- ✅ 实现热榜展示
- ✅ 设计现代化 UI 主题
- ✅ 配置 GitHub Actions 自动构建

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

MIT License

## 🔗 相关链接

- [知乎开放平台](https://developer.zhihu.com/)
- [知乎 CLI 文档](https://developer.zhihu.com/docs/cli)
- [开发计划](./知乎CLI客户端开发计划.md)

## ⚠️ 注意事项

1. **Alpha 版本**: 当前为早期测试版本，功能尚不完整
2. **API 额度**: 请注意知乎开放平台的 API 调用额度限制
3. **安全存储**: Access Secret 使用 Windows DPAPI 加密存储
4. **网络连接**: 需要稳定的网络连接访问知乎 API

## 💡 常见问题

**Q: 无法连接到知乎 CLI？**  
A: 请确保知乎 CLI 已正确安装在默认位置 `%LOCALAPPDATA%\ZhihuCLI\current\zhihu-cli.exe`

**Q: 搜索或热榜加载失败？**  
A: 请检查网络连接，确认 Access Secret 配置正确，并查看 API 额度是否充足

**Q: 如何更新到新版本？**  
A: 从 GitHub Releases 下载最新版本，覆盖旧文件即可（配置会保留）

---

**Made with ❤️ for Zhihu Developers**
