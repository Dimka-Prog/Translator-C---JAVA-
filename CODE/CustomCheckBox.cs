using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace CSharpToJavaTranslator
{
    public class CustomCheckBox : CheckBox
    {
        public CustomCheckBox()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(100, 40);

            this.currentBorderColor = this.borderColorUnfocused;

            this.MouseDown += new MouseEventHandler(this.CustomCheckBox_MouseDown);
            this.MouseEnter += new EventHandler(this.CustomCheckBox_MouseEnter);
            this.MouseLeave += new EventHandler(this.CustomCheckBox_MouseLeave);
            this.MouseUp += new MouseEventHandler(this.CustomCheckBox_MouseUp);
            this.EnabledChanged += new EventHandler(this.CustomCheckBox_EnabledChanged);
            this.GotFocus += new EventHandler(this.CustomCheckBox_GotFocus);
            this.LostFocus += new EventHandler(this.CustomCheckBox_LostFocus);
        }

        private Color backgroundColorFocused;
        private Color backgroundColorUnfocused;
        private Color backgroundColorMouseDown;
        private Color backgroundColorDisabled;
        private Color textColorEnabled;
        private Color textColorDisabled;
        private Color borderColorFocused;
        private Color borderColorUnfocused;
        private Color borderColorMouseDown;
        private Color borderColorDisabled;
        private int borderWidth;
        private Color currentBorderColor;

        [Category("Дополнительные настройки")]
        public Color BackgroundColorFocused
        {
            get
            { return this.backgroundColorFocused; }
            set
            {
                this.backgroundColorFocused = value;
            }
        }

        [Category("Дополнительные настройки")]
        public Color BackgroundColorUnfocused
        {
            get
            { return this.backgroundColorUnfocused; }
            set
            {
                this.backgroundColorUnfocused = value;
                this.BackColor = value;
                this.Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public Color BackgroundColorMouseDown
        {
            get
            { return this.backgroundColorMouseDown; }
            set
            { this.backgroundColorMouseDown = value; }
        }

        [Category("Дополнительные настройки")]
        public Color BackgroundColorDisabled
        {
            get
            { return this.backgroundColorDisabled; }
            set
            { this.backgroundColorDisabled = value; }
        }

        [Category("Дополнительные настройки")]
        public Color TextColorEnabled
        {
            get
            { return this.textColorEnabled; }
            set
            {
                this.textColorEnabled = value;
            }
        }

        [Category("Дополнительные настройки")]
        public Color TextColorDisabled
        {
            get
            { return this.textColorDisabled; }
            set
            {
                this.textColorDisabled = value;
            }
        }

        [Category("Дополнительные настройки")]
        public Color BorderColorFocused
        {
            get
            { return this.borderColorFocused; }
            set
            {
                this.borderColorFocused = value;
            }
        }

        [Category("Дополнительные настройки")]
        public Color BorderColorUnfocused
        {
            get
            { return this.borderColorUnfocused; }
            set
            {
                this.borderColorUnfocused = value;
                this.currentBorderColor = value;
                this.Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public Color BorderColorMouseDown
        {
            get
            { return this.borderColorMouseDown; }
            set
            { this.borderColorMouseDown = value; }
        }

        [Category("Дополнительные настройки")]
        public Color BorderColorDisabled
        {
            get
            { return this.borderColorDisabled; }
            set
            { this.borderColorDisabled = value; }
        }

        [Category("Дополнительные настройки")]
        public int BorderWidth
        {
            get
            { return this.borderWidth; }
            set
            {
                this.borderWidth = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush br = new SolidBrush(this.BackColor))
            using (Brush backgroundBr = new SolidBrush(Color.WhiteSmoke))
            using (Font f = new Font("Segoe UI", 10, FontStyle.Regular))
            using (Brush sbr = new SolidBrush(this.ForeColor))
            using (Pen fp = new Pen(Brushes.Green, 2))
            using (Pen p = new Pen(new SolidBrush(this.currentBorderColor), this.borderWidth))
            {
                e.Graphics.FillRectangle(backgroundBr, 0, 0, this.Width, this.Height);
                e.Graphics.FillRectangle(br, 0, this.Height / 2 - 10, 20, 20);

                SizeF s = e.Graphics.MeasureString(this.Text, f);
                e.Graphics.DrawString(this.Text, f, sbr, 25, (this.Height - s.Height) / 2);

                if (this.Checked)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(fp, 2, this.Height / 2 - 3, 9, this.Height / 2 + 7);
                    e.Graphics.DrawLine(fp, 8, this.Height / 2 + 7, 17, this.Height / 2 - 8);
                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                }

                e.Graphics.DrawRectangle(p, this.borderWidth / 2, this.Height / 2 - 10 + this.borderWidth / 2,
                                         20 - this.borderWidth,
                                         20 - this.borderWidth);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(CustomCheckBox_BackColorChangedEh);
        }

        private void CustomCheckBox_BackColorChangedEh(object Sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }

        private void CustomCheckBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = this.backgroundColorMouseDown;
            this.currentBorderColor = this.borderColorMouseDown;
        }

        private void CustomCheckBox_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = this.backgroundColorFocused;
            this.currentBorderColor = this.borderColorFocused;
        }

        private void CustomCheckBox_MouseLeave(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = this.backgroundColorUnfocused;
                this.currentBorderColor = this.borderColorUnfocused;
            }
        }

        private void CustomCheckBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = this.backgroundColorFocused;
                this.currentBorderColor = this.borderColorFocused;
            }
        }

        private void CustomCheckBox_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = this.backgroundColorUnfocused;
                this.ForeColor = this.textColorEnabled;
                this.currentBorderColor = this.borderColorUnfocused;
            }
            else
            {
                this.BackColor = this.backgroundColorDisabled;
                this.ForeColor = this.textColorDisabled;
                this.currentBorderColor = this.borderColorDisabled;
            }

            this.Invalidate();
        }

        private void CustomCheckBox_GotFocus(object sender, EventArgs e)
        {
            this.BackColor = this.backgroundColorFocused;
            this.currentBorderColor = this.BorderColorFocused;

            this.Invalidate();
        }

        private void CustomCheckBox_LostFocus(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = this.backgroundColorUnfocused;
                this.ForeColor = this.textColorEnabled;
                this.currentBorderColor = this.borderColorUnfocused;
            }
            else
            {
                this.BackColor = this.backgroundColorDisabled;
                this.ForeColor = this.textColorDisabled;
                this.currentBorderColor = this.borderColorDisabled;
            }

            this.Invalidate();
        }
    }
}
