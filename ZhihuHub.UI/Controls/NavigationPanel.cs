using ZhihuHub.UI.Themes;

namespace ZhihuHub.UI.Controls;

/// <summary>
/// 导航按钮项
/// </summary>
public class NavigationButton : Button
{
    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            UpdateStyle();
        }
    }

    public string IconText { get; set; } = "";

    public NavigationButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        TextAlign = ContentAlignment.MiddleLeft;
        Font = new Font("Segoe UI", 10F);
        Height = 48;
        Cursor = Cursors.Hand;
        Padding = new Padding(20, 0, 0, 0);
        UpdateStyle();
    }

    private void UpdateStyle()
    {
        if (_isActive)
        {
            BackColor = ColorScheme.SidebarActive;
            ForeColor = ColorScheme.TextLight;
        }
        else
        {
            BackColor = ColorScheme.Sidebar;
            ForeColor = Color.FromArgb(189, 195, 199);
        }

        FlatAppearance.MouseOverBackColor = ColorScheme.SidebarHover;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (!string.IsNullOrEmpty(IconText))
        {
            using var iconFont = new Font("Segoe UI Emoji", 14F);
            var iconSize = e.Graphics.MeasureString(IconText, iconFont);
            var iconX = 20;
            var iconY = (Height - iconSize.Height) / 2;

            e.Graphics.DrawString(IconText, iconFont, new SolidBrush(ForeColor), iconX, iconY);

            // 调整文本位置
            var textFormat = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };

            var textRect = new RectangleF(50, 0, Width - 50, Height);
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textRect, textFormat);
        }
    }
}

/// <summary>
/// 侧边导航面板
/// </summary>
public class NavigationPanel : Panel
{
    private readonly List<NavigationButton> _buttons = new();
    public event EventHandler<string>? NavigationChanged;

    public NavigationPanel()
    {
        BackColor = ColorScheme.Sidebar;
        Dock = DockStyle.Left;
        Width = 200;

        InitializeNavigation();
    }

    private void InitializeNavigation()
    {
        var navigationItems = new[]
        {
            ("🏠", "首页", "Home"),
            ("🔍", "搜索", "Search"),
            ("🔥", "热榜", "Hot"),
            ("⚙️", "设置", "Settings")
        };

        int y = 20;
        foreach (var (icon, text, tag) in navigationItems)
        {
            var button = new NavigationButton
            {
                IconText = icon,
                Text = text,
                Tag = tag,
                Location = new Point(0, y),
                Width = Width
            };

            button.Click += OnNavigationButtonClick;
            _buttons.Add(button);
            Controls.Add(button);

            y += button.Height + 4;
        }

        // 默认激活第一个
        if (_buttons.Count > 0)
            _buttons[0].IsActive = true;
    }

    private void OnNavigationButtonClick(object? sender, EventArgs e)
    {
        if (sender is NavigationButton clickedButton)
        {
            // 取消其他按钮的激活状态
            foreach (var btn in _buttons)
            {
                btn.IsActive = false;
            }

            // 激活当前按钮
            clickedButton.IsActive = true;

            // 触发导航事件
            NavigationChanged?.Invoke(this, clickedButton.Tag?.ToString() ?? "");
        }
    }

    public void SetActiveNavigation(string tag)
    {
        foreach (var btn in _buttons)
        {
            btn.IsActive = btn.Tag?.ToString() == tag;
        }
    }
}
