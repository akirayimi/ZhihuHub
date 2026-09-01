using ZhihuHub.UI.Themes;
using ZhihuHub.UI.Controls;
using ZhihuHub.Core.Services;
using ZhihuHub.Core.Config;

namespace ZhihuHub.UI.Forms;

/// <summary>
/// 主窗体
/// </summary>
public class MainForm : Form
{
    private readonly IZhihuCliService _cliService;
    private NavigationPanel _navigationPanel = null!;
    private Panel _contentPanel = null!;
    private Panel _statusBar = null!;
    private Label _statusLabel = null!;

    private Panel? _currentContentPanel;

    public MainForm(IZhihuCliService cliService)
    {
        _cliService = cliService;
        InitializeComponents();
        CheckAuthentication();
    }

    private void InitializeComponents()
    {
        Text = "ZhihuHub Desktop";
        Size = new Size(1200, 800);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 600);
        BackColor = ColorScheme.Background;

        // 导航面板
        _navigationPanel = new NavigationPanel();
        _navigationPanel.NavigationChanged += OnNavigationChanged;
        Controls.Add(_navigationPanel);

        // 内容面板
        _contentPanel = new Panel
        {
            Location = new Point(200, 0),
            Width = Width - 200,
            Height = Height - 70,
            BackColor = ColorScheme.Background,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };
        Controls.Add(_contentPanel);

        // 状态栏
        _statusBar = new Panel
        {
            Location = new Point(200, Height - 70),
            Width = Width - 200,
            Height = 40,
            BackColor = ColorScheme.CardBackground,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };
        Controls.Add(_statusBar);

        _statusLabel = new Label
        {
            Text = "就绪",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(20, 12)
        };
        _statusBar.Controls.Add(_statusLabel);

        // 状态栏边框
        _statusBar.Paint += (s, e) =>
        {
            using var pen = new Pen(ColorScheme.Border, 1);
            e.Graphics.DrawLine(pen, 0, 0, _statusBar.Width, 0);
        };

        // 默认显示首页
        ShowPanel(new HomePanel());
    }

    private void OnNavigationChanged(object? sender, string tag)
    {
        Panel? panel = tag switch
        {
            "Home" => new HomePanel(),
            "Search" => new SearchPanel(_cliService),
            "Hot" => new HotListPanel(_cliService),
            "Settings" => new SettingsPanel(_cliService),
            _ => null
        };

        if (panel != null)
        {
            ShowPanel(panel);
            UpdateStatus($"切换到: {GetNavigationName(tag)}");
        }
    }

    private void ShowPanel(Panel panel)
    {
        // 移除当前面板
        if (_currentContentPanel != null)
        {
            _contentPanel.Controls.Remove(_currentContentPanel);
            _currentContentPanel.Dispose();
        }

        // 添加新面板
        panel.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(panel);
        _currentContentPanel = panel;
    }

    private string GetNavigationName(string tag)
    {
        return tag switch
        {
            "Home" => "首页",
            "Search" => "搜索",
            "Hot" => "热榜",
            "Settings" => "设置",
            _ => tag
        };
    }

    private void UpdateStatus(string message)
    {
        _statusLabel.Text = $"{message} | {DateTime.Now:HH:mm:ss}";
    }

    private async void CheckAuthentication()
    {
        try
        {
            UpdateStatus("检查认证状态...");

            var status = await _cliService.GetStatusAsync();

            if (status == null)
            {
                MessageBox.Show("无法连接到知乎 CLI，请检查是否已正确安装", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("CLI 连接失败");
                return;
            }

            if (!status.Auth.Configured)
            {
                UpdateStatus("需要配置认证");

                var result = MessageBox.Show(
                    "尚未配置 Access Secret，是否现在配置？",
                    "需要配置",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var authForm = new AuthSetupForm(_cliService);
                    authForm.ShowDialog(this);

                    if (authForm.AuthConfigured)
                    {
                        UpdateStatus("认证配置成功");
                        MessageBox.Show("认证配置成功，现在可以使用所有功能了！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus("未配置认证");
                    }
                }
            }
            else
            {
                UpdateStatus("认证已配置");
            }
        }
        catch (Exception ex)
        {
            UpdateStatus("检查认证失败");
            MessageBox.Show($"检查认证状态时出错: {ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
