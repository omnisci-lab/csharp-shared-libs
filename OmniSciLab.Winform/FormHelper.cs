using System.Reflection;

namespace OmniSciLab.WinForm;

public static class FormHelper
{
    public static void RoundForm(this Form frm, int val = 5)
    {
        frm.Region = Region.FromHrgn(Gdi32PInvoke.CreateRoundRectRgn(0, 0, frm.Width, frm.Height, val, val));
    }

    public static void RunWithExceptionHandling(this Form frm)
    {
        var methods = frm.GetType()
                          .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                          .Where(m => m.GetCustomAttributes(typeof(ExceptionHandledAttribute), false).Any());

        foreach (var method in methods)
        {
            var eventInfo = frm.GetType().GetEvent(method.Name);
            if (eventInfo != null)
            {
                // Tạo delegate để bọc phương thức với try-catch
                Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, frm, method);

                // Đăng ký sự kiện với delegate đã bọc xử lý ngoại lệ
                eventInfo.AddEventHandler(frm, handler);
            }
        }
    }
}
