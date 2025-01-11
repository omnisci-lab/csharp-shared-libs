namespace khothemegiatot.Winform;

public static class ActionExtensions
{
    public static void HandleWithMessageBox(this Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}