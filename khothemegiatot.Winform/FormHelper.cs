namespace khothemegiatot.Winform;

public static class FormHelper
{
    public static void RoundForm(this Form frm, int val = 5)
    {
        frm.Region = Region.FromHrgn(Gdi32PInvoke.CreateRoundRectRgn(0, 0, frm.Width, frm.Height, val, val));
    }
}
