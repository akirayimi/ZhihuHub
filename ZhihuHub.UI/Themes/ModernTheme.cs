namespace ZhihuHub.UI.Themes;

/// <summary>
/// 现代主题工具类
/// </summary>
public static class ModernTheme
{
    /// <summary>
    /// 应用现代扁平按钮样式
    /// </summary>
    public static void ApplyButtonStyle(Button button, bool isPrimary = false)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;
        button.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        button.Height = 36;
        button.Padding = new Padding(16, 0, 16, 0);

        if (isPrimary)
        {
            button.BackColor = ColorScheme.Primary;
            button.ForeColor = ColorScheme.TextLight;
            button.FlatAppearance.MouseOverBackColor = ColorScheme.PrimaryHover;
            button.FlatAppearance.MouseDownBackColor = ColorScheme.PrimaryDark;
        }
        else
        {
            button.BackColor = ColorScheme.Background;
            button.ForeColor = ColorScheme.TextPrimary;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = ColorScheme.Border;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 242, 245);
        }
    }

    /// <summary>
    /// 应用文本框样式
    /// </summary>
    public static void ApplyTextBoxStyle(TextBox textBox)
    {
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Font = new Font("Segoe UI", 10F);
        textBox.Height = 32;
        textBox.BackColor = ColorScheme.CardBackground;
        textBox.ForeColor = ColorScheme.TextPrimary;
    }

    /// <summary>
    /// 应用面板样式
    /// </summary>
    public static void ApplyPanelStyle(Panel panel, bool isCard = false)
    {
        panel.BackColor = isCard ? ColorScheme.CardBackground : ColorScheme.Background;

        if (isCard)
        {
            panel.Padding = new Padding(16);
        }
    }

    /// <summary>
    /// 应用标签样式
    /// </summary>
    public static void ApplyLabelStyle(Label label, bool isTitle = false, bool isSecondary = false)
    {
        label.Font = isTitle
            ? new Font("Segoe UI", 12F, FontStyle.Bold)
            : new Font("Segoe UI", 9F);

        label.ForeColor = isSecondary ? ColorScheme.TextSecondary : ColorScheme.TextPrimary;
        label.BackColor = Color.Transparent;
    }

    /// <summary>
    /// 绘制卡片阴影
    /// </summary>
    public static void DrawCardShadow(Graphics g, Rectangle bounds)
    {
        using var shadowBrush = new SolidBrush(ColorScheme.Shadow);
        g.FillRectangle(shadowBrush, bounds.X + 2, bounds.Y + 2, bounds.Width, bounds.Height);
    }

    /// <summary>
    /// 创建圆角路径
    /// </summary>
    public static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
        path.CloseFigure();
        return path;
    }
}
