namespace OmniSciLab.WinForm;

public static class ButtonControlHelper
{
    public static void RoundButton(this Button btn, int val = 5)
    {
        btn.Region = Region.FromHrgn(Gdi32PInvoke.CreateRoundRectRgn(0, 0, btn.Width, btn.Height, val, val));
    }
}
