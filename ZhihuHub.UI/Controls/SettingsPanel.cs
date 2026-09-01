using ZhihuHub.UI.Themes;
using ZhihuHub.Core.Services;

namespace ZhihuHub.UI.Controls;

/// <summary>
/// 设置面板
/// </summary>
public class SettingsPanel : Panel
{
    private readonly IZhihuCliService _cliService;
    private Label _authStatusLabel = null!;
    private Button _verifyAuthButton = null!;
    private TextBox _cliPathTextBox = null!;

    public SettingsPanel(IZhihuCliService cliService)
    {
        _cliService = cliService;
        InitializeComponents();
        LoadSettings();
    }

    private void InitializeComponents()
    {
        BackColor = ColorScheme.Background;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        // 标题
        var titleLabel = new Label
        {
            Text = "设置",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(20, 20)
        };
        Controls.Add(titleLabel);

        // 认证状态区域
        var authGroupBox = new GroupBox
        {
            Text = "认证状态",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Location = new Point(20, 70),
            Size = new Size(600, 120),
            ForeColor = ColorScheme.TextPrimary
        };
        Controls.Add(authGroupBox);

        _authStatusLabel = new Label
        {
            Text = "检查中...",
            Font = new Font("Segoe UI", 10F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(20, 30)
        };
        authGroupBox.Controls.Add(_authStatusLabel);

        _verifyAuthButton = new Button
        {
            Text = "验证认证",
            Location = new Point(20, 60),
            Width = 120
        };
        _verifyAuthButton.Click += OnVerifyAuthClick;
        ModernTheme.ApplyButtonStyle(_verifyAuthButton);
        authGroupBox.Controls.Add(_verifyAuthButton);

        // CLI 配置区域
        var cliGroupBox = new GroupBox
        {
            Text = "CLI 配置",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Location = new Point(20, 210),
            Size = new Size(600, 100),
            ForeColor = ColorScheme.TextPrimary
        };
        Controls.Add(cliGroupBox);

        var cliPathLabel = new Label
        {
            Text = "CLI 路径:",
            Font = new Font("Segoe UI", 10F),
            Location = new Point(20, 35),
            AutoSize = true
        };
        cliGroupBox.Controls.Add(cliPathLabel);

        _cliPathTextBox = new TextBox
        {
            ReadOnly = true,
            Font = new Font("Segoe UI", 9F),
            Location = new Point(100, 32),
            Width = 480,
            BackColor = Color.WhiteSmoke
        };
        cliGroupBox.Controls.Add(_cliPathTextBox);

        // 关于区域
        var aboutGroupBox = new GroupBox
        {
            Text = "关于",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Location = new Point(20, 330),
            Size = new Size(600, 120),
            ForeColor = ColorScheme.TextPrimary
        };
        Controls.Add(aboutGroupBox);

        var aboutLabel = new Label
        {
            Text = "ZhihuHub Desktop v0.1.0 Alpha\n知乎开放平台 CLI 图形界面客户端\n\nCopyright © 2026",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            Location = new Point(20, 30),
            AutoSize = true
        };
        aboutGroupBox.Controls.Add(aboutLabel);
    }

    private async void LoadSettings()
    {
        try
        {
            var status = await _cliService.GetStatusAsync();
            if (status != null)
            {
                _cliPathTextBox.Text = status.Cli.BinaryPath;

                if (status.Auth.Configured)
                {
                    _authStatusLabel.Text = "✅ 已配置认证";
                    _authStatusLabel.ForeColor = ColorScheme.Success;
                }
                else
                {
                    _authStatusLabel.Text = "⚠️ 未配置认证";
                    _authStatusLabel.ForeColor = ColorScheme.Warning;
                }
            }
        }
        catch
        {
            _authStatusLabel.Text = "❌ 无法获取状态";
            _authStatusLabel.ForeColor = ColorScheme.Danger;
        }
    }

    private async void OnVerifyAuthClick(object? sender, EventArgs e)
    {
        _verifyAuthButton.Enabled = false;
        _authStatusLabel.Text = "验证中...";

        try
        {
            var isValid = await _cliService.VerifyAuthAsync();
            if (isValid)
            {
                _authStatusLabel.Text = "✅ 认证有效";
                _authStatusLabel.ForeColor = ColorScheme.Success;
                MessageBox.Show("认证验证成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _authStatusLabel.Text = "❌ 认证无效";
                _authStatusLabel.ForeColor = ColorScheme.Danger;
                MessageBox.Show("认证验证失败，请重新配置 Access Secret", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            _authStatusLabel.Text = "❌ 验证出错";
            _authStatusLabel.ForeColor = ColorScheme.Danger;
            MessageBox.Show($"验证出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _verifyAuthButton.Enabled = true;
        }
    }
}
