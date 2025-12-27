using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class GradientPanel : Panel
{
    public Color TopColor { get; set; } = Color.RoyalBlue;
    public Color BottomColor { get; set; } = Color.Navy;
    public float Angle { get; set; } = 45F;

    public string Title { get; set; } = "Tiêu đề";
    public string Value { get; set; } = "0";
    public Image Icon { get; set; }

    public GradientPanel()
    {
        this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        this.UpdateStyles();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, this.TopColor, this.BottomColor, this.Angle))
        {
            e.Graphics.FillRectangle(lgb, this.ClientRectangle);
        }

        if (Icon != null)
        {
            e.Graphics.DrawImage(Icon, 15, 15, 40, 40);
        }
        Font fontTieuDe = new Font("Segoe UI", 12, FontStyle.Bold);
        Font fontSo = new Font("Segoe UI", 20, FontStyle.Bold);

        float titleX = 80;
        float titleY = 28; 

        e.Graphics.DrawString(Title, fontTieuDe, Brushes.WhiteSmoke, new PointF(titleX, titleY));

        SizeF titleSize = e.Graphics.MeasureString(Title, fontTieuDe);

        float numberX = titleX + titleSize.Width + 10;

        float numberY = 20;

        e.Graphics.DrawString(Value, fontSo, Brushes.White, new PointF(numberX, numberY));
    }
}