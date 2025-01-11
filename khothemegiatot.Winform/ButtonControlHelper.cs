using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace khothemegiatot.Winform;

public static class ButtonControlHelper
{
    public static void RoundButton(this Button btn, int borderRadius)
    {
        int diameter = borderRadius * 2;
        GraphicsPath path = new GraphicsPath();

        path.StartFigure();

        // Vẽ các góc bo tròn
        path.AddArc(new Rectangle(0, 0, diameter, diameter), 180, 90);                      // Góc trên trái
        path.AddArc(new Rectangle(btn.Width - diameter, 0, diameter, diameter), 270, 90);   // Góc trên phải
        path.AddArc(new Rectangle(btn.Width - diameter, btn.Height - diameter, diameter, diameter), 0, 90);  // Góc dưới phải
        path.AddArc(new Rectangle(0, btn.Height - diameter, diameter, diameter), 90, 90);   // Góc dưới trái

        path.CloseFigure();
        btn.Region = new Region(path);
    }
}
