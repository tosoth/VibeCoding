using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

// 双滑块控件：一条轨道上有“开始”和“结束”两个滑块
class RangeSlider : Control
{
    const int Pad = 12;
    double max = 1, lo = 0, hi = 1;
    int drag = 0; // 1=开始滑块 2=结束滑块
    public event EventHandler Changed;

    public RangeSlider()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        Height = 40;
    }

    public double Maximum
    {
        get { return max; }
        set { max = value > 0 ? value : 1; lo = 0; hi = max; Invalidate(); Fire(); }
    }
    public double Lo { get { return lo; } }
    public double Hi { get { return hi; } }
    public void SetLo(double v) { lo = Math.Max(0, Math.Min(v, hi)); Invalidate(); Fire(); }
    public void SetHi(double v) { hi = Math.Min(max, Math.Max(v, lo)); Invalidate(); Fire(); }

    void Fire() { if (Changed != null) Changed(this, EventArgs.Empty); }
    float X(double v) { return (float)(Pad + (Width - 2 * Pad) * v / max); }
    double V(int x)
    {
        double v = (x - Pad) * max / Math.Max(1, Width - 2 * Pad);
        return Math.Max(0, Math.Min(max, v));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.Clear(BackColor);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float y = Height / 2f;
        using (Pen track = new Pen(Color.Silver, 6))
            g.DrawLine(track, X(0), y, X(max), y);
        using (Pen sel = new Pen(Color.SteelBlue, 6))
            g.DrawLine(sel, X(lo), y, X(hi), y);
        DrawThumb(g, X(lo), y);
        DrawThumb(g, X(hi), y);
    }

    void DrawThumb(Graphics g, float x, float y)
    {
        RectangleF r = new RectangleF(x - 8, y - 8, 16, 16);
        g.FillEllipse(Brushes.White, r);
        using (Pen p = new Pen(Color.SteelBlue, 2)) g.DrawEllipse(p, r);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Focus(); // 让正在编辑的时间框先提交
        double dl = Math.Abs(e.X - X(lo)), dh = Math.Abs(e.X - X(hi));
        if (Math.Abs(dl - dh) < 0.5) drag = e.X > X(lo) ? 2 : 1;
        else drag = dl < dh ? 1 : 2;
        MoveTo(e.X);
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (drag != 0) MoveTo(e.X);
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        drag = 0;
    }

    void MoveTo(int x)
    {
        double v = V(x);
        if (drag == 1) lo = Math.Min(v, hi);
        else if (drag == 2) hi = Math.Max(v, lo);
        Invalidate();
        Fire();
    }
}

class MainForm : Form
{
    Button btnOpen, btnGo;
    TextBox txtIn, txtOut;
    RangeSlider slider;
    TextBox txtStart, txtEnd;
    Label lblLen, lblStatus;
    CheckBox chkExact;
    double duration = 0;

    public MainForm()
    {
        Text = "视频截取工具 (ffmpeg)";
        Font = new Font("Microsoft YaHei UI", 9f);
        ClientSize = new Size(640, 245);
        MinimumSize = new Size(520, 284);
        StartPosition = FormStartPosition.CenterScreen;

        btnOpen = new Button { Text = "选择文件...", Location = new Point(15, 14), Size = new Size(110, 28) };
        btnOpen.Click += OnOpen;

        txtIn = new TextBox { ReadOnly = true, Location = new Point(135, 17), Size = new Size(490, 24),
                              Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

        slider = new RangeSlider { Location = new Point(15, 60), Size = new Size(610, 40),
                                   Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
        slider.Changed += OnSliderChanged;

        Label lblS = new Label { Text = "开始", Location = new Point(15, 108), AutoSize = true };
        txtStart = MakeTimeBox(new Point(52, 105), HorizontalAlignment.Left, AnchorStyles.Top | AnchorStyles.Left);
        lblLen = new Label { Location = new Point(215, 105), Size = new Size(210, 20), TextAlign = ContentAlignment.TopCenter,
                             Text = "时长 00:00:00.000", Anchor = AnchorStyles.Top };
        Label lblE = new Label { Text = "结束", Location = new Point(487, 108), AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right };
        txtEnd = MakeTimeBox(new Point(525, 105), HorizontalAlignment.Right, AnchorStyles.Top | AnchorStyles.Right);

        Label lblOut = new Label { Text = "输出文件:", Location = new Point(15, 143), AutoSize = true };
        txtOut = new TextBox { Location = new Point(90, 140), Size = new Size(535, 24),
                               Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

        chkExact = new CheckBox { Text = "精确到帧（重新编码，较慢，体积会变）", Location = new Point(15, 176), AutoSize = true };
        btnGo = new Button { Text = "生成", Location = new Point(525, 172), Size = new Size(100, 30), Enabled = false,
                             Anchor = AnchorStyles.Top | AnchorStyles.Right };
        btnGo.Click += OnGo;

        lblStatus = new Label { Location = new Point(15, 215), Size = new Size(610, 20), Text = "请先选择文件",
                                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

        Controls.AddRange(new Control[] { btnOpen, txtIn, slider, lblS, txtStart, lblLen, lblE, txtEnd, lblOut, txtOut, chkExact, btnGo, lblStatus });
    }

    static string Fmt(double s)
    {
        TimeSpan ts = TimeSpan.FromSeconds(s);
        return string.Format("{0:00}:{1:00}:{2:00}.{3:000}", (int)ts.TotalHours, ts.Minutes, ts.Seconds, ts.Milliseconds);
    }

    static string Sec(double s) { return s.ToString("F3", CultureInfo.InvariantCulture); }

    void OnSliderChanged(object sender, EventArgs e)
    {
        txtStart.Text = Fmt(slider.Lo);
        txtEnd.Text = Fmt(slider.Hi);
        lblLen.Text = "时长 " + Fmt(slider.Hi - slider.Lo);
    }

    TextBox MakeTimeBox(Point loc, HorizontalAlignment align, AnchorStyles anchor)
    {
        TextBox tb = new TextBox { Location = loc, Size = new Size(100, 24), TextAlign = align, Anchor = anchor, Text = "00:00:00.000" };
        tb.GotFocus += delegate { tb.SelectAll(); };
        tb.KeyDown += OnTimeKeyDown;
        tb.Leave += OnTimeLeave;
        new ToolTip().SetToolTip(tb, "点击后输入时间，如 1:08:58、05:30.5、90，回车确认");
        return tb;
    }

    void OnTimeKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Commit((TextBox)sender); }
        else if (e.KeyCode == Keys.Escape) { e.SuppressKeyPress = true; OnSliderChanged(null, EventArgs.Empty); }
    }

    void OnTimeLeave(object sender, EventArgs e) { Commit((TextBox)sender); }

    // 提交输入的时间；无效输入则还原为当前滑块值
    void Commit(TextBox tb)
    {
        double v = ParseTime(tb.Text);
        if (v >= 0 && duration > 0)
        {
            if (tb == txtStart) slider.SetLo(v); else slider.SetHi(v);
        }
        OnSliderChanged(null, EventArgs.Empty);
    }

    // 支持 时:分:秒.毫秒 / 分:秒 / 纯秒数
    static double ParseTime(string s)
    {
        string[] parts = s.Trim().Replace('：', ':').Replace(',', '.').Split(':');
        if (parts.Length < 1 || parts.Length > 3) return -1;
        double total = 0;
        for (int i = 0; i < parts.Length; i++)
        {
            double x;
            if (!double.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out x) || x < 0) return -1;
            total = total * 60 + x;
        }
        return total;
    }

    void OnOpen(object sender, EventArgs e)
    {
        using (OpenFileDialog dlg = new OpenFileDialog())
        {
            dlg.Filter = "视频/音频文件|*.mp4;*.mkv;*.mov;*.avi;*.flv;*.ts;*.webm;*.wmv;*.m4v;*.m4a;*.mp3|所有文件|*.*";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            double d = Probe(dlg.FileName);
            if (d <= 0) return;

            duration = d;
            txtIn.Text = dlg.FileName;
            slider.Maximum = d;
            txtOut.Text = NextOutName(dlg.FileName);
            btnGo.Enabled = true;
            lblStatus.Text = "已读取时长 " + Fmt(d) + "，拖动滑块选择范围";
        }
    }

    // 用 ffprobe 读取时长（秒）
    double Probe(string path)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo("ffprobe",
                "-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"" + path + "\"");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            using (Process p = Process.Start(psi))
            {
                string o = p.StandardOutput.ReadToEnd();
                p.StandardError.ReadToEnd();
                p.WaitForExit();
                double d;
                if (double.TryParse(o.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out d)) return d;
            }
            MessageBox.Show("无法读取该文件的时长。", "错误");
        }
        catch (Win32Exception)
        {
            MessageBox.Show("找不到 ffprobe。请把 ffmpeg.exe 和 ffprobe.exe 放到本程序同一目录，或加入系统 PATH。", "错误");
        }
        return 0;
    }

    // 源文件_cut1.mp4, _cut2.mp4 ... 取第一个不存在的
    static string NextOutName(string src)
    {
        string dir = Path.GetDirectoryName(src);
        string name = Path.GetFileNameWithoutExtension(src);
        for (int i = 1; ; i++)
        {
            string cand = Path.Combine(dir, name + "_cut" + i + ".mp4");
            if (!File.Exists(cand)) return cand;
        }
    }

    void OnGo(object sender, EventArgs e)
    {
        string input = txtIn.Text;
        string outPath = txtOut.Text.Trim();
        double start = slider.Lo, len = slider.Hi - slider.Lo;

        if (len < 0.1) { MessageBox.Show("截取时长太短。", "提示"); return; }
        if (outPath.Length == 0) { MessageBox.Show("请填写输出文件名。", "提示"); return; }
        if (string.Equals(Path.GetFullPath(input), Path.GetFullPath(outPath), StringComparison.OrdinalIgnoreCase))
        { MessageBox.Show("输出文件不能与源文件相同。", "提示"); return; }
        if (File.Exists(outPath) &&
            MessageBox.Show("输出文件已存在，是否覆盖？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        string args = "-y -ss " + Sec(start) + " -i \"" + input + "\" -t " + Sec(len) + " " +
                      (chkExact.Checked ? "" : "-c copy -avoid_negative_ts make_zero ") +
                      "\"" + outPath + "\"";

        btnGo.Enabled = false;
        lblStatus.Text = "处理中...";

        Thread t = new Thread(() =>
        {
            int code = -1;
            string err = "";
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("ffmpeg", args);
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardError = true;
                using (Process p = Process.Start(psi))
                {
                    err = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                    code = p.ExitCode;
                }
            }
            catch (Exception ex) { err = ex.Message; }

            BeginInvoke(new Action(() =>
            {
                btnGo.Enabled = true;
                if (code == 0)
                {
                    lblStatus.Text = "完成: " + outPath;
                    txtOut.Text = NextOutName(input); // 为下一段准备新文件名
                }
                else
                {
                    lblStatus.Text = "失败";
                    if (err.Length > 600) err = err.Substring(err.Length - 600);
                    MessageBox.Show("ffmpeg 出错:\n" + err, "错误");
                }
            }));
        });
        t.IsBackground = true;
        t.Start();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}
