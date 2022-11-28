using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace CSharpToJavaTranslator
{
    public class CustomButton : Button
    {
        public CustomButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(100, 40);

            this.currentBorderColor = this.borderColorUnfocused;

            this.MouseDown += new MouseEventHandler(this.CustomButton_MouseDown);
            this.MouseEnter += new EventHandler(this.CustomButton_MouseEnter);
            this.MouseLeave += new EventHandler(this.CustomButton_MouseLeave);
            this.MouseUp += new MouseEventHandler(this.CustomButton_MouseUp);
            this.EnabledChanged += new EventHandler(this.CustomButton_EnabledChanged);
            this.GotFocus += new EventHandler(this.CustomButton_GotFocus);
            this.LostFocus += new EventHandler(this.CustomButton_LostFocus);
        }

        private Color backgroundColorFocused;
        private Color backgroundColorUnfocused;
        private Color backgroundColorMouseDown;
        private Color backgroundColorDisabled;
        private Color textColorFocused;
        private Color textColorUnfocused;
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
        public Color TextColorFocused
        {
            get 
            { return this.textColorFocused; }
            set
            { this.textColorFocused = value; }
        }

        [Category("Дополнительные настройки")]
        public Color TextColorUnfocused
        {
            get
            { return this.textColorUnfocused; }
            set
            { 
                this.textColorUnfocused = value;
                this.ForeColor = value;
                this.Invalidate();
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

        private GraphicsPath getRegionPath(RectangleF rf, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rf.X, rf.Y, radius, radius, 180, 90);
            path.AddArc(rf.Width - radius, rf.Y, radius, radius, 270, 90);
            path.AddArc(rf.Width - radius, rf.Height - radius, radius, radius, 0, 90);
            path.AddArc(rf.X, rf.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clear(this.Parent.BackColor);
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Скруглённая кнопка
            //RectangleF rf = new RectangleF(0, 0, this.Width - 6, this.Height - 6);
            //using (Brush br = new SolidBrush(this.BackColor))
            //{
            //    e.Graphics.FillPath(br, this.GetRegionPath(rf, 30));
            //}

            using (Brush br = new SolidBrush(this.BackColor))
            using (Font f = new Font("Sans Serif", 8, FontStyle.Regular))
            using (Brush sbr = new SolidBrush(this.ForeColor))
            using (Pen p = new Pen(new SolidBrush(this.currentBorderColor), this.borderWidth))
            {
                e.Graphics.FillRectangle(br, 0, 0, this.Width, this.Height);

                SizeF s = e.Graphics.MeasureString(this.Text, f);
                e.Graphics.DrawString(this.Text, f, sbr, (this.Width - s.Width) / 2, (this.Height - s.Height) / 2);

                e.Graphics.DrawRectangle(p, this.borderWidth / 2, this.borderWidth / 2, 
                                         this.Width - this.borderWidth, 
                                         this.Height - this.borderWidth);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(CustomButton_BackColorChangedEh);
        }

        private void CustomButton_BackColorChangedEh(object Sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }

        private void CustomButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = this.backgroundColorMouseDown;
            this.currentBorderColor = this.borderColorMouseDown;
        }

        private void CustomButton_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = this.backgroundColorFocused;
            this.ForeColor = this.textColorFocused;
            this.currentBorderColor = this.borderColorFocused;
        }

        private void CustomButton_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = this.backgroundColorUnfocused;
            this.ForeColor = this.textColorUnfocused;
            this.currentBorderColor = this.borderColorUnfocused;
        }

        private void CustomButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackColor = this.backgroundColorFocused;
            this.currentBorderColor = this.borderColorFocused;
        }

        private void CustomButton_EnabledChanged(object sender, EventArgs e)
        {
            if(this.Enabled)
            {
                this.BackColor = this.backgroundColorUnfocused;
                this.ForeColor = this.textColorUnfocused;
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

        private void CustomButton_GotFocus(object sender, EventArgs e)
        {
            this.BackColor = this.backgroundColorFocused;
            this.ForeColor = this.textColorFocused;
            this.currentBorderColor = this.BorderColorFocused;

            this.Invalidate();
        }

        private void CustomButton_LostFocus(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = this.backgroundColorUnfocused;
                this.ForeColor = this.textColorUnfocused;
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
