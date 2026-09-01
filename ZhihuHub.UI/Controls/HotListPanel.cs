using ZhihuHub.UI.Themes;
using ZhihuHub.Core.Models;
using ZhihuHub.Core.Services;

namespace ZhihuHub.UI.Controls;

/// <summary>
/// 热榜面板
/// </summary>
public class HotListPanel : Panel
{
    private readonly IZhihuCliService _cliService;
    private Button _refreshButton = null!;
    private FlowLayoutPanel _hotListPanel = null!;
    private Label _statusLabel = null!;
    private Label _lastUpdateLabel = null!;

    public HotListPanel(IZhihuCliService cliService)
    {
        _cliService = cliService;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        BackColor = ColorScheme.Background;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        // 标题
        var titleLabel = new Label
        {
            Text = "知乎热榜",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(20, 20)
        };
        Controls.Add(titleLabel);

        // 刷新按钮
        _refreshButton = new Button
        {
            Text = "🔄 刷新",
            Location = new Point(Width - 140, 20),
            Width = 100,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _refreshButton.Click += OnRefreshButtonClick;
        ModernTheme.ApplyButtonStyle(_refreshButton, isPrimary: true);
        Controls.Add(_refreshButton);

        // 最后更新时间
        _lastUpdateLabel = new Label
        {
            Text = "",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(20, 60)
        };
        Controls.Add(_lastUpdateLabel);

        // 状态标签
        _statusLabel = new Label
        {
            Text = "",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(20, 85)
        };
        Controls.Add(_statusLabel);

        // 热榜列表面板
        _hotListPanel = new FlowLayoutPanel
        {
            Location = new Point(20, 115),
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = ColorScheme.Background
        };
        Controls.Add(_hotListPanel);

        // 调整面板大小
        Resize += (s, e) =>
        {
            _hotListPanel.Width = Width - 40;
            _hotListPanel.Height = Height - 135;
        };

        // 自动加载
        LoadHotList();
    }

    private async void OnRefreshButtonClick(object? sender, EventArgs e)
    {
        await LoadHotList();
    }

    private async Task LoadHotList()
    {
        _refreshButton.Enabled = false;
        _statusLabel.Text = "加载中...";
        _hotListPanel.Controls.Clear();

        try
        {
            var result = await _cliService.GetHotListAsync(20);

            if (result?.Code == 0 && result.Data?.Items != null)
            {
                _statusLabel.Text = $"共 {result.Data.Total} 条热榜";
                _lastUpdateLabel.Text = $"最后更新: {DateTime.Now:HH:mm:ss}";

                int rank = 1;
                foreach (var item in result.Data.Items)
                {
                    var card = new HotItemCard(item, rank);
                    card.Width = _hotListPanel.Width - 20;
                    _hotListPanel.Controls.Add(card);
                    rank++;
                }

                if (result.Data.Items.Count == 0)
                {
                    _statusLabel.Text = "暂无热榜数据";
                }
            }
            else
            {
                _statusLabel.Text = "加载失败";
                MessageBox.Show("加载热榜失败，请检查网络连接或稍后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "加载出错";
            MessageBox.Show($"加载出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _refreshButton.Enabled = true;
        }
    }
}

/// <summary>
/// 热榜条目卡片
/// </summary>
public class HotItemCard : Panel
{
    private readonly HotItem _item;
    private readonly int _rank;

    public HotItemCard(HotItem item, int rank)
    {
        _item = item;
        _rank = rank;
        Height = 100;
        BackColor = ColorScheme.CardBackground;
        Margin = new Padding(0, 0, 0, 8);
        Padding = new Padding(15);
        Cursor = Cursors.Hand;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // 排名
        var rankLabel = new Label
        {
            Text = _rank.ToString(),
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = GetRankColor(),
            Location = new Point(15, 15),
            Size = new Size(40, 40),
            TextAlign = ContentAlignment.MiddleCenter
        };
        rankLabel.Click += OnCardClick;
        Controls.Add(rankLabel);

        // 标题
        var titleLabel = new Label
        {
            Text = _item.Title,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            Location = new Point(65, 15),
            AutoSize = false,
            Width = Width - 80,
            Height = 48
        };
        titleLabel.Click += OnCardClick;
        Controls.Add(titleLabel);

        // 摘要
        if (!string.IsNullOrEmpty(_item.Summary))
        {
            var summaryText = _item.Summary.Length > 80 ? _item.Summary.Substring(0, 80) + "..." : _item.Summary;
            var summaryLabel = new Label
            {
                Text = summaryText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorScheme.TextSecondary,
                Location = new Point(65, 65),
                AutoSize = false,
                Width = Width - 80,
                Height = 20
            };
            summaryLabel.Click += OnCardClick;
            Controls.Add(summaryLabel);
        }

        Click += OnCardClick;
    }

    private Color GetRankColor()
    {
        return _rank switch
        {
            1 => Color.FromArgb(255, 87, 51),  // 红色
            2 => Color.FromArgb(255, 140, 0),  // 橙色
            3 => Color.FromArgb(218, 165, 32), // 金色
            _ => ColorScheme.TextSecondary
        };
    }

    private void OnCardClick(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_item.Url))
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _item.Url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开链接: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        BackColor = Color.FromArgb(250, 251, 252);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        BackColor = ColorScheme.CardBackground;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // 绘制边框
        using var pen = new Pen(ColorScheme.BorderLight, 1);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}
