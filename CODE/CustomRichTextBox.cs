using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpToJavaTranslator
{
    public partial class CustomRichTextBox : UserControl
    {
        public CustomRichTextBox()
        {
            InitializeComponent();
            this.borderColorFocused = Color.Blue;
            this.borderColorUnfocused = Color.Black;
            this.isPlaceholder = true;
            this.innerRichTextBox.ForeColor = Color.Gray;
        }

        private Color borderColorFocused;
        private Color borderColorUnfocused;
        private String placeholderText;
        private bool isPlaceholder;

        [Category("Дополнительные настройки")]
        public Color BorderColorFocused
        {
            get
            { return this.borderColorFocused; }
            set
            {
                this.borderColorFocused = value;
                this.Invalidate();
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
                this.Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public Color BackgroundColor
        {
            get
            { return this.innerRichTextBox.BackColor; }
            set
            {
                this.BackColor = value;
                this.innerRichTextBox.BackColor = value;
                this.Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public bool Editable
        {
            get { return !this.innerRichTextBox.ReadOnly; }
            set
            {
                this.innerRichTextBox.ReadOnly = !value;
            }
        }

        [Category("Дополнительные настройки")]
        public String PlaceholderText
        {
            get
            { return this.placeholderText; }
            set
            {
                this.placeholderText = value;
                this.innerRichTextBox.Text = value;
                this.Invalidate();
            }
        }

        public RichTextBox getInnerTextBox()
        {
            return this.innerRichTextBox;
        }

        public bool isInPlaceholderMode()
        {
            return this.isPlaceholder;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Pen borderPen;

            if (this.innerRichTextBox.Focused)
            {
                borderPen = new Pen(this.borderColorFocused, 1);
            }
            else
            {
                borderPen = new Pen(this.borderColorUnfocused, 1);
            }

            g.DrawRectangle(borderPen, 1, 1, this.Width - 2, this.Height - 2);
            borderPen.Dispose();

            //Brush lineNumberBrush = new SolidBrush(Color.Blue);
            //for(int i = 0; i < this.innerRichTextBox.Lines.Count(); i++)
            //{
            //    g.DrawString();
            //}
            
            //lineNumberBrush.Dispose();
        }

        private void innerRichTextBox_Enter(object sender, EventArgs e)
        {
            if (this.isPlaceholder)
            {
                this.innerRichTextBox.Text = "";
                this.isPlaceholder = false;
                this.innerRichTextBox.ForeColor = Color.Black;
            }
            this.Invalidate();
        }

        private void innerRichTextBox_Leave(object sender, EventArgs e)
        {
            if (this.innerRichTextBox.Text.Length == 0)
            {
                this.isPlaceholder = true;
                this.innerRichTextBox.Text = this.placeholderText;
                this.innerRichTextBox.ForeColor = Color.Gray;
            }
            this.Invalidate();
        }

        private void CustomRichTextBox_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        public void highlightText(int startPosition, int length, Color color)
        {
            this.innerRichTextBox.SelectionStart = startPosition;
            this.innerRichTextBox.SelectionLength = length;
            this.innerRichTextBox.SelectionBackColor = color;
            this.innerRichTextBox.SelectionLength = 0;
        }

        public void removeHighlight()
        {
            this.innerRichTextBox.SelectionStart = 0;
            this.innerRichTextBox.SelectionLength = this.innerRichTextBox.Text.Length;
            this.innerRichTextBox.SelectionBackColor = Color.FromArgb(0, 255, 255, 255);
            this.innerRichTextBox.SelectionLength = 0;
        }

        public void appendText(string line, Color color)
        {
            if(this.isPlaceholder)
            {
                this.isPlaceholder = false;
                this.innerRichTextBox.Clear();
            }

            this.innerRichTextBox.SelectionStart = this.innerRichTextBox.Text.Length;
            this.innerRichTextBox.SelectionLength = 0;
            this.innerRichTextBox.SelectionColor = color;
            this.innerRichTextBox.AppendText(line);
        }

        public void setText(string[] lines, Color color)
        {
            if (this.isPlaceholder)
            {
                this.isPlaceholder = false;
            }

            this.innerRichTextBox.Clear();
            this.innerRichTextBox.ForeColor = color;

            foreach (string line in lines)
            {
                this.innerRichTextBox.AppendText(line + "\n");
            }
        }
        
        private void innerRichTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
