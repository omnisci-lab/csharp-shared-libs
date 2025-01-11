namespace khothemegiatot.Winform
{
    partial class LicenseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtLicense = new TextBox();
            btnCheck = new Button();
            lbNotificationText = new Label();
            SuspendLayout();
            // 
            // txtLicense
            // 
            txtLicense.Location = new Point(12, 45);
            txtLicense.Name = "txtLicense";
            txtLicense.Size = new Size(362, 27);
            txtLicense.TabIndex = 0;
            // 
            // btnCheck
            // 
            btnCheck.Location = new Point(134, 88);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(94, 29);
            btnCheck.TabIndex = 1;
            btnCheck.Text = "Kiểm tra";
            btnCheck.UseVisualStyleBackColor = true;
            btnCheck.Click += btnCheck_Click;
            // 
            // lbNotificationText
            // 
            lbNotificationText.AutoSize = true;
            lbNotificationText.Location = new Point(61, 9);
            lbNotificationText.Name = "lbNotificationText";
            lbNotificationText.Size = new Size(261, 20);
            lbNotificationText.TabIndex = 2;
            lbNotificationText.Text = "Vui lòng nhập License mà bạn đã mua";
            // 
            // LicenseForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(386, 132);
            Controls.Add(lbNotificationText);
            Controls.Add(btnCheck);
            Controls.Add(txtLicense);
            Name = "LicenseForm";
            Text = "License";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLicense;
        private Button btnCheck;
        private Label lbNotificationText;
    }
}