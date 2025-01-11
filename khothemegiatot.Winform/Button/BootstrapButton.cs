using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace khothemegiatot.Winform;

public class BootstrapButton : Button
{
    public enum ButtonStyle
    {
        Primary,
        Secondary,
        Success,
        Danger
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ButtonStyle Style { get; set; } = ButtonStyle.Primary;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool IsOutline { get; set; } = false;

    public BootstrapButton()
    {
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 1;
        this.Cursor = Cursors.Hand;
        this.ForeColor = Color.White;
        this.Padding = new Padding(10, 5, 10, 5);
        this.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular);

        ApplyStyle();
    }

    private void ApplyStyle()
    {
        if (IsOutline)
        {
            switch (Style)
            {
                case ButtonStyle.Primary:
                    this.BackColor = Color.Transparent;
                    this.ForeColor = Color.FromArgb(0, 123, 255);
                    this.FlatAppearance.BorderColor = Color.FromArgb(0, 123, 255);
                    break;
                case ButtonStyle.Secondary:
                    this.BackColor = Color.Transparent;
                    this.ForeColor = Color.FromArgb(108, 117, 125);
                    this.FlatAppearance.BorderColor = Color.FromArgb(108, 117, 125);
                    break;
                case ButtonStyle.Success:
                    this.BackColor = Color.Transparent;
                    this.ForeColor = Color.FromArgb(40, 167, 69);
                    this.FlatAppearance.BorderColor = Color.FromArgb(40, 167, 69);
                    break;
                case ButtonStyle.Danger:
                    this.BackColor = Color.Transparent;
                    this.ForeColor = Color.FromArgb(220, 53, 69);
                    this.FlatAppearance.BorderColor = Color.FromArgb(220, 53, 69);
                    break;
                default:
                    this.BackColor = Color.Transparent;
                    this.ForeColor = Color.FromArgb(0, 123, 255);
                    this.FlatAppearance.BorderColor = Color.FromArgb(0, 123, 255);
                    break;
            }
        }
        else
        {
            switch (Style)
            {
                case ButtonStyle.Primary:
                    this.BackColor = Color.FromArgb(0, 123, 255);
                    this.ForeColor = Color.White;
                    this.FlatAppearance.BorderColor = Color.FromArgb(0, 105, 217);
                    break;
                case ButtonStyle.Secondary:
                    this.BackColor = Color.FromArgb(108, 117, 125);
                    this.ForeColor = Color.White;
                    this.FlatAppearance.BorderColor = Color.FromArgb(94, 99, 104);
                    break;
                case ButtonStyle.Success:
                    this.BackColor = Color.FromArgb(40, 167, 69);
                    this.ForeColor = Color.White;
                    this.FlatAppearance.BorderColor = Color.FromArgb(36, 141, 57);
                    break;
                case ButtonStyle.Danger:
                    this.BackColor = Color.FromArgb(220, 53, 69);
                    this.ForeColor = Color.White;
                    this.FlatAppearance.BorderColor = Color.FromArgb(185, 46, 51);
                    break;
                default:
                    this.BackColor = Color.FromArgb(0, 123, 255);
                    this.ForeColor = Color.White;
                    this.FlatAppearance.BorderColor = Color.FromArgb(0, 105, 217);
                    break;
            }
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        if (!IsOutline)
        {
            this.BackColor = ControlPaint.Light(this.BackColor, 0.1f); // Thay đổi màu khi hover
        }
        else
        {
            this.ForeColor = ControlPaint.Light(this.ForeColor, 0.1f); // Thay đổi màu văn bản khi hover
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        ApplyStyle();
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);
        this.BackColor = ControlPaint.Dark(this.BackColor, 0.1f);
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);
        ApplyStyle();
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        this.Region = Region.FromHrgn(Gdi32PInvoke.CreateRoundRectRgn(0, 0, this.Width, this.Height, 2, 2));
        //Graphics g = pevent.Graphics;

        //int radius = 10;
        //using (Pen pen = new Pen(this.FlatAppearance.BorderColor, 2))
        //{
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //    g.DrawArc(pen, 0, 0, radius, radius, 180, 90); // Góc trên trái
        //    g.DrawArc(pen, this.Width - radius - 1, 0, radius, radius, 270, 90); // Góc trên phải
        //    g.DrawArc(pen, this.Width - radius - 1, this.Height - radius - 1, radius, radius, 0, 90); // Góc dưới phải
        //    g.DrawArc(pen, 0, this.Height - radius - 1, radius, radius, 90, 90); // Góc dưới trái
        //}

        //// Vẽ nội dung button
        //using (SolidBrush brush = new SolidBrush(this.ForeColor))
        //{
        //    g.DrawString(this.Text, this.Font, brush, new PointF(this.Width / 2 - g.MeasureString(this.Text, this.Font).Width / 2, this.Height / 2 - g.MeasureString(this.Text, this.Font).Height / 2));
        //}
    }
}
