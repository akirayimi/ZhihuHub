# 快速部署指南

## 📋 部署清单

### 1. GitHub 仓库准备

**必须操作**:
```bash
# 1. 在 GitHub 创建新仓库（名称建议: ZhihuHub）
# 2. 复制仓库 URL

# 3. 添加远程仓库
git remote add origin https://github.com/YOUR_USERNAME/ZhihuHub.git

# 4. 推送代码
git push -u origin main
```

### 2. GitHub Actions 构建

推送后，GitHub Actions 会自动开始构建：

1. 访问仓库的 **Actions** 标签页
2. 查看 "Build ZhihuHub Desktop" 工作流
3. 等待构建完成（约 3-5 分钟）
4. 绿色勾号 ✅ 表示构建成功

### 3. 下载构建产物

**从 Actions 页面下载**:
1. 点击最新的成功构建（绿色勾号）
2. 滚动到页面底部，找到 **Artifacts** 部分
3. 下载 `ZhihuHub-win-x64-[hash].zip`
4. 解压 zip 文件，得到 `ZhihuHub.exe`

### 4. 本地测试

```powershell
# 1. 双击运行 ZhihuHub.exe

# 2. 首次运行会提示配置 Access Secret
#    - 点击"是"
#    - 点击"打开知乎开放平台"
#    - 登录并生成 Access Secret
#    - 复制并粘贴到程序
#    - 点击"提交"

# 3. 测试功能
#    - 点击"搜索"，输入关键词，查看结果
#    - 点击"热榜"，查看知乎热榜
#    - 点击结果卡片，验证链接跳转

# 4. 验证 UI
#    - 检查导航切换是否流畅
#    - 检查搜索结果展示是否正常
#    - 检查热榜排名和颜色
#    - 缩放窗口，验证响应式布局
```

---

## 🔍 验证重点

### 构建验证
- [ ] GitHub Actions 构建成功
- [ ] Artifacts 上传成功
- [ ] 下载的 exe 文件大小正常（50-80 MB）

### 功能验证
- [ ] 程序能正常启动
- [ ] 认证配置流程正常
- [ ] 知乎搜索返回结果
- [ ] 全网搜索返回结果
- [ ] 热榜自动加载
- [ ] 链接跳转正常

### UI 验证
- [ ] 侧边栏导航正常
- [ ] 按钮悬停效果
- [ ] 卡片悬停效果
- [ ] 热榜排名颜色（前三名特殊颜色）
- [ ] 状态栏实时更新
- [ ] 窗口缩放响应式

### 异常处理
- [ ] 空搜索提示
- [ ] 网络错误提示
- [ ] 认证失败提示
- [ ] CLI 不存在提示

---

## 🐛 常见问题排查

### 问题 1: GitHub Actions 构建失败

**排查步骤**:
```bash
# 1. 查看 Actions 日志
# 2. 检查错误信息

# 常见原因：
# - 语法错误（C# 编译错误）
# - 项目引用错误
# - 工作流配置错误

# 解决方案：
# - 修复代码错误
# - 检查 .csproj 配置
# - 验证 build.yml 语法
```

### 问题 2: exe 运行报错

**排查步骤**:
```powershell
# 1. 以管理员身份运行 PowerShell
# 2. 进入 exe 目录
# 3. 运行并查看错误

.\ZhihuHub.exe

# 常见错误：
# - 缺少 .NET 运行时 → 检查是否为自包含发布
# - 权限不足 → 右键"以管理员身份运行"
# - 被杀毒软件拦截 → 添加信任
```

### 问题 3: CLI 连接失败

**排查步骤**:
```powershell
# 1. 检查 CLI 是否安装
Test-Path "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe"

# 2. 手动运行 CLI 测试
& "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe" status

# 3. 如果 CLI 不存在，参考知乎开放平台文档安装
```

### 问题 4: 搜索或热榜加载失败

**排查步骤**:
```powershell
# 1. 检查网络连接
Test-Connection -ComputerName api.zhihu.com -Count 4

# 2. 验证认证状态
& "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe" auth status

# 3. 检查 API 额度
& "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe" quota

# 4. 手动测试 CLI 命令
& "$env:LOCALAPPDATA\ZhihuCLI\current\zhihu-cli.exe" hot --limit 5
```

---

## 📊 性能指标

### 构建性能
- **构建时间**: 3-5 分钟
- **exe 大小**: 50-80 MB（自包含）
- **压缩后**: 20-30 MB（zip）

### 运行性能
- **启动时间**: < 2 秒
- **内存占用**: 50-80 MB（空闲）
- **搜索响应**: 1-3 秒（取决于网络）
- **热榜加载**: 1-2 秒（取决于网络）

---

## 🎯 下一步行动

### 如果构建成功 ✅
1. 下载并测试 exe
2. 验证所有功能点
3. 记录测试结果和问题
4. 准备反馈和改进建议

### 如果构建失败 ❌
1. 查看 Actions 日志
2. 定位具体错误
3. 修复代码问题
4. 重新推送触发构建

### Alpha 测试完成后 🎉
1. 收集反馈
2. 修复 Bug
3. 调整 UI 细节
4. 开始 Phase 2 开发

---

## 📞 技术支持

### 日志位置
- **应用日志**: （暂未实现，Phase 2 添加）
- **GitHub Actions 日志**: Actions 页面查看
- **Windows 事件日志**: 事件查看器 → 应用程序

### 问题报告
如果遇到问题，请提供：
1. 操作系统版本
2. 错误截图或消息
3. 操作步骤
4. CLI 版本（如果已安装）

---

**祝测试顺利！** 🚀
