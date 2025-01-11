using khothemegiatot.License;

namespace khothemegiatot.Winform;

public partial class LicenseForm : Form
{
    public LicenseForm()
    {
        InitializeComponent();
    }

    private async void btnCheck_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtLicense.Text))
            MessageBox.Show("Bạn cần phải nhập email!");

        if (await LicenseChecker.GetAndSaveLicenseAsync(txtLicense.Text))
        {
            this.Close();
            Application.Restart();

            return;
        }

        MessageBox.Show("Không hợp lệ!");
        Application.Exit();
    }
}
