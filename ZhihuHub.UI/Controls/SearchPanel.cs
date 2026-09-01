using ZhihuHub.UI.Themes;
using ZhihuHub.Core.Models;
using ZhihuHub.Core.Services;

namespace ZhihuHub.UI.Controls;

/// <summary>
/// 搜索面板
/// </summary>
public class SearchPanel : Panel
{
    private readonly IZhihuCliService _cliService;
    private TextBox _searchBox = null!;
    private Button _searchButton = null!;
    private ComboBox _searchTypeCombo = null!;
    private FlowLayoutPanel _resultsPanel = null!;
    private Label _statusLabel = null!;

    public SearchPanel(IZhihuCliService cliService)
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
            Text = "搜索",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            AutoSize = true,
            Location = new Point(20, 20)
        };
        Controls.Add(titleLabel);

        // 搜索类型选择
        _searchTypeCombo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F),
            Location = new Point(20, 60),
            Width = 120
        };
        _searchTypeCombo.Items.AddRange(new[] { "知乎搜索", "全网搜索" });
        _searchTypeCombo.SelectedIndex = 0;
        Controls.Add(_searchTypeCombo);

        // 搜索框
        _searchBox = new TextBox
        {
            Font = new Font("Segoe UI", 11F),
            Location = new Point(150, 60),
            Width = 400,
            Height = 32
        };
        _searchBox.KeyDown += OnSearchBoxKeyDown;
        ModernTheme.ApplyTextBoxStyle(_searchBox);
        Controls.Add(_searchBox);

        // 搜索按钮
        _searchButton = new Button
        {
            Text = "🔍 搜索",
            Location = new Point(560, 60),
            Width = 100
        };
        _searchButton.Click += OnSearchButtonClick;
        ModernTheme.ApplyButtonStyle(_searchButton, isPrimary: true);
        Controls.Add(_searchButton);

        // 状态标签
        _statusLabel = new Label
        {
            Text = "",
            Font = new Font("Segoe UI", 9F),
            ForeColor = ColorScheme.TextSecondary,
            AutoSize = true,
            Location = new Point(20, 100)
        };
        Controls.Add(_statusLabel);

        // 结果面板
        _resultsPanel = new FlowLayoutPanel
        {
            Location = new Point(20, 130),
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = ColorScheme.Background
        };
        Controls.Add(_resultsPanel);

        // 调整结果面板大小
        Resize += (s, e) =>
        {
            _resultsPanel.Width = Width - 40;
            _resultsPanel.Height = Height - 150;
        };
    }

    private void OnSearchBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            OnSearchButtonClick(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    private async void OnSearchButtonClick(object? sender, EventArgs e)
    {
        var query = _searchBox.Text.Trim();
        if (string.IsNullOrEmpty(query))
        {
            MessageBox.Show("请输入搜索关键词", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _searchButton.Enabled = false;
        _searchBox.Enabled = false;
        _statusLabel.Text = "搜索中...";
        _resultsPanel.Controls.Clear();

        try
        {
            SearchResult? result = null;

            if (_searchTypeCombo.SelectedIndex == 0)
            {
                // 知乎搜索
                result = await _cliService.SearchZhihuAsync(query, 10);
            }
            else
            {
                // 全网搜索
                result = await _cliService.SearchGlobalAsync(query, 10);
            }

            if (result?.Code == 0 && result.Data?.Items != null)
            {
                _statusLabel.Text = $"找到 {result.Data.Total} 条结果";

                foreach (var item in result.Data.Items)
                {
                    var card = new SearchResultCard(item);
                    card.Width = _resultsPanel.Width - 20;
                    _resultsPanel.Controls.Add(card);
                }

                if (result.Data.Items.Count == 0)
                {
                    _statusLabel.Text = "未找到相关结果";
                }
            }
            else
            {
                _statusLabel.Text = "搜索失败";
                MessageBox.Show("搜索失败，请检查网络连接或稍后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "搜索出错";
            MessageBox.Show($"搜索出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _searchButton.Enabled = true;
            _searchBox.Enabled = true;
        }
    }
}

/// <summary>
/// 搜索结果卡片
/// </summary>
public class SearchResultCard : Panel
{
    private readonly SearchItem _item;

    public SearchResultCard(SearchItem item)
    {
        _item = item;
        Height = 120;
        BackColor = ColorScheme.CardBackground;
        Margin = new Padding(0, 0, 0, 10);
        Padding = new Padding(15);
        Cursor = Cursors.Hand;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // 标题
        var titleLabel = new Label
        {
            Text = _item.Title,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorScheme.TextPrimary,
            Location = new Point(15, 15),
            AutoSize = false,
            Width = Width - 30,
            Height = 24
        };
        titleLabel.Click += OnCardClick;
        Controls.Add(titleLabel);

        // 作者
        if (!string.IsNullOrEmpty(_item.AuthorName))
        {
            var authorLabel = new Label
            {
                Text = $"👤 {_item.AuthorName}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorScheme.TextSecondary,
                Location = new Point(15, 45),
                AutoSize = true
            };
            authorLabel.Click += OnCardClick;
            Controls.Add(authorLabel);
        }

        // 摘要
        var summary = !string.IsNullOrEmpty(_item.ContentText) ? _item.ContentText : _item.Summary;
        if (!string.IsNullOrEmpty(summary))
        {
            var summaryLabel = new Label
            {
                Text = summary.Length > 100 ? summary.Substring(0, 100) + "..." : summary,
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorScheme.TextPrimary,
                Location = new Point(15, 70),
                AutoSize = false,
                Width = Width - 30,
                Height = 35
            };
            summaryLabel.Click += OnCardClick;
            Controls.Add(summaryLabel);
        }

        Click += OnCardClick;
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
