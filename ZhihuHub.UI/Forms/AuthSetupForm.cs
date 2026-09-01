using ZhihuHub.UI.Themes;
using ZhihuHub.Core.Services;

namespace ZhihuHub.UI.Forms;

/// <summary>
/// 认证配置窗体
/// </summary>
public class AuthSetupForm : Form
{
    private readonly IZhihuCliService _cliService;
    private TextBox _secretTextBox = null!;
    private Button _submitButton = null!;
    private Button _cancelButton = null!;
    private Label _statusLabel = null!;

    public bool AuthConfigured { get; private set; }

    public AuthSetupForm(IZhihuCliService cliService)
    {
        _cliService = cliService;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "配置 Access Secret";
        Size = new Size(600, 400);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = ColorScheme.Background;

        // 标题
        var titleLabel = new Label
        {
            Text = "配置知乎开放平台 Access Secret",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(30, 30)
        };
        Controls.Add(titleLabel);

        // 说明文字
        var instructionLabel = new Label
        {
            Text = "请按照以下步骤获取 Access Secret：\n\n" +
                   "1. 访问知乎开放平台个人中心\n" +
                   "2. 登录你的知乎账号\n" +
                   "3. 生成或复制已有的 Access Secret\n" +
                   "4. 将 Access Secret 粘贴到下方输入框",
            Font = new Font("Segoe UI", 10F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = false,
            Size = new Size(540, 120),
            Location = new Point(30, 70)
        };
        Controls.Add(instructionLabel);

        // 打开链接按钮
        var openLinkButton = new Button
        {
            Text = "🔗 打开知乎开放平台",
            Location = new Point(30, 200),
            Width = 180
        };
        openLinkButton.Click += (s, e) =>
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://developer.zhihu.com/profile",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开链接: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };
        ModernTheme.ApplyButtonStyle(openLinkButton);
        Controls.Add(openLinkButton);

        // Access Secret 输入标签
        var secretLabel = new Label
        {
            Text = "Access Secret:",
            Font = new Font("Segoe UI", 10F),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(30, 250)
        };
        Controls.Add(secretLabel);

        // Access Secret 输入框
        _secretTextBox = new TextBox
        {
            Font = new Font("Segoe UI", 10F),
            Location = new Point(30, 275),
            Width = 540,
            UseSystemPasswordChar = true
        };
        ModernTheme.ApplyTextBoxStyle(_secretTextBox);
        Controls.Add(_secretTextBox);

        // 状态标签
        _statusLabel = new Label
        {
            Text = "",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(30, 315)
        };
        Controls.Add(_statusLabel);

        // 提交按钮
        _submitButton = new Button
        {
            Text = "提交",
            Location = new Point(370, 315),
            Width = 100
        };
        _submitButton.Click += OnSubmitClick;
        ModernTheme.ApplyButtonStyle(_submitButton, isPrimary: true);
        Controls.Add(_submitButton);

        // 取消按钮
        _cancelButton = new Button
        {
            Text = "取消",
            Location = new Point(480, 315),
            Width = 90
        };
        _cancelButton.Click += (s, e) => Close();
        ModernTheme.ApplyButtonStyle(_cancelButton);
        Controls.Add(_cancelButton);
    }

    private async void OnSubmitClick(object? sender, EventArgs e)
    {
        var secret = _secretTextBox.Text.Trim();

        if (string.IsNullOrEmpty(secret))
        {
            MessageBox.Show("请输入 Access Secret", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _submitButton.Enabled = false;
        _secretTextBox.Enabled = false;
        _statusLabel.Text = "配置中...";
        _statusLabel.ForeColor = ColorScheme.Info;

        try
        {
            // 配置 Access Secret
            var success = await _cliService.SetAccessSecretAsync(secret);

            if (success)
            {
                // 验证配置
                var isValid = await _cliService.VerifyAuthAsync();

                if (isValid)
                {
                    _statusLabel.Text = "✅ 配置成功！";
                    _statusLabel.ForeColor = ColorScheme.Success;
                    AuthConfigured = true;

                    MessageBox.Show("Access Secret 配置成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    _statusLabel.Text = "❌ 验证失败";
                    _statusLabel.ForeColor = ColorScheme.Danger;
                    MessageBox.Show("Access Secret 无效，请检查后重试", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                _statusLabel.Text = "❌ 配置失败";
                _statusLabel.ForeColor = ColorScheme.Danger;
                MessageBox.Show("配置失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "❌ 出错";
            _statusLabel.ForeColor = ColorScheme.Danger;
            MessageBox.Show($"配置出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _submitButton.Enabled = true;
            _secretTextBox.Enabled = true;
        }
    }
}
