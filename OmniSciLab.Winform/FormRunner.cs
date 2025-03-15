using khothemegiatot.Winform;
using OmniSciLab.License;

namespace OmniSciLab.WinForm;

public static class FormRunner
{
    public static void RunAfterLicenseCheck<TForm>(TForm form) where TForm : Form
    {
        if (!File.Exists("license.dat"))
            Application.Run(new LicenseForm());
        else
        {
            if (LicenseChecker.ValidateLicense().GetAwaiter().GetResult())
                Application.Run(form);
            else
                Application.Exit();
        }
    }
}
