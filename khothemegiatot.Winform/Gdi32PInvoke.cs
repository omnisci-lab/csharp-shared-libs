using System.Runtime.InteropServices;

namespace OmniSciLab.WinForm;

public class Gdi32PInvoke
{
    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    public static extern nint CreateRoundRectRgn
    (
        int nLeftRect,     // x-coordinate of upper-left corner
        int nTopRect,      // y-coordinate of upper-left corner
        int nRightRect,    // x-coordinate of lower-right corner
        int nBottomRect,   // y-coordinate of lower-right corner
        int nWidthEllipse, // width of ellipse
        int nHeightEllipse // height of ellipse
    );
}
