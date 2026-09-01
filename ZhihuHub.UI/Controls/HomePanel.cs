using ZhihuHub.UI.Themes;

namespace ZhihuHub.UI.Controls;

/// <summary>
/// 首页面板
/// </summary>
public class HomePanel : Panel
{
    public HomePanel()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        BackColor = ColorScheme.Background;
        Dock = DockStyle.Fill;

        // 欢迎标题
        var titleLabel = new Label
        {
            Text = "欢迎使用 ZhihuHub Desktop",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(50, 80)
        };
        Controls.Add(titleLabel);

        // 副标题
        var subtitleLabel = new Label
        {
            Text = "知乎开放平台 CLI 的现代化图形界面客户端",
            Font = new Font("Segoe UI", 12F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(50, 130)
        };
        Controls.Add(subtitleLabel);

        // 功能卡片容器
        var featuresPanel = new FlowLayoutPanel
        {
            Location = new Point(50, 200),
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };
        Controls.Add(featuresPanel);

        // 功能卡片
        var features = new[]
        {
            ("🔍", "智能搜索", "搜索知乎内容和全网资源"),
            ("🔥", "实时热榜", "掌握知乎热点动态"),
            ("👤", "我的知乎", "管理创作、关注和收藏"),
            ("📚", "知识库", "浏览和搜索知识库")
        };

        foreach (var (icon, title, desc) in features)
        {
            var card = CreateFeatureCard(icon, title, desc);
            featuresPanel.Controls.Add(card);
        }

        // 版本信息
        var versionLabel = new Label
        {
            Text = "版本 0.1.0 Alpha",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(50, Height - 50)
        };
        Controls.Add(versionLabel);
    }

    private Panel CreateFeatureCard(string icon, string title, string description)
    {
        var card = new Panel
        {
            Width = 200,
            Height = 150,
            BackColor = ColorScheme.CardBackground,
            Margin = new Padding(0, 0, 20, 20),
            Padding = new Padding(20)
        };

        var iconLabel = new Label
        {
            Text = icon,
            Font = new Font("Segoe UI Emoji", 32F),
            AutoSize = true,
            Location = new Point(20, 20)
        };
        card.Controls.Add(iconLabel);

        var titleLabel = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(20, 80)
        };
        card.Controls.Add(titleLabel);

        var descLabel = new Label
        {
            Text = description,
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = false,
            Width = 160,
            Height = 40,
            Location = new Point(20, 105)
        };
        card.Controls.Add(descLabel);

        // 添加边框
        card.Paint += (s, e) =>
        {
            using var pen = new Pen(ColorScheme.BorderLight, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        return card;
    }
}
