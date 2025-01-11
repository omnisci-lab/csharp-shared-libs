namespace khothemegiatot.Winform;

public static class TextBoxHelper
{
    public static void RoundForm(this TextBox txtBox, int val = 5)
    {
        txtBox.Region = Region.FromHrgn(Gdi32PInvoke.CreateRoundRectRgn(0, 0, txtBox.Width, txtBox.Height, val, val));
    }
}
