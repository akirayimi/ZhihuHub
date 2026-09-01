namespace ZhihuHub.UI.Themes;

/// <summary>
/// 现代化配色方案
/// </summary>
public static class ColorScheme
{
    // 主色调（知乎蓝）
    public static readonly Color Primary = Color.FromArgb(0, 132, 255);
    public static readonly Color PrimaryLight = Color.FromArgb(63, 161, 255);
    public static readonly Color PrimaryDark = Color.FromArgb(0, 102, 204);
    public static readonly Color PrimaryHover = Color.FromArgb(51, 153, 255);

    // 状态色
    public static readonly Color Success = Color.FromArgb(40, 167, 69);
    public static readonly Color Warning = Color.FromArgb(255, 193, 7);
    public static readonly Color Danger = Color.FromArgb(220, 53, 69);
    public static readonly Color Info = Color.FromArgb(23, 162, 184);

    // 背景色
    public static readonly Color Background = Color.FromArgb(245, 247, 250);
    public static readonly Color CardBackground = Color.White;
    public static readonly Color Sidebar = Color.FromArgb(44, 62, 80);
    public static readonly Color SidebarActive = Color.FromArgb(52, 73, 94);
    public static readonly Color SidebarHover = Color.FromArgb(58, 80, 102);

    // 文字色
    public static readonly Color TextPrimary = Color.FromArgb(44, 62, 80);
    public static readonly Color TextSecondary = Color.FromArgb(127, 140, 141);
    public static readonly Color TextLight = Color.White;
    public static readonly Color TextLink = Primary;

    // 边框色
    public static readonly Color Border = Color.FromArgb(220, 223, 230);
    public static readonly Color BorderLight = Color.FromArgb(233, 236, 239);

    // 阴影色
    public static readonly Color Shadow = Color.FromArgb(50, 0, 0, 0);
}
