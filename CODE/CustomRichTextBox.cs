using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpToJavaTranslator
{
    public partial class CustomRichTextBox : UserControl
    {
        public CustomRichTextBox()
        {
            InitializeComponent();
            borderColorFocused = Color.Blue;
            borderColorUnfocused = Color.Black;
            isPlaceholder = true;
            innerRichTextBox.ForeColor = Color.Gray;
        }

        private Color borderColorFocused;
        private Color borderColorUnfocused;
        private String placeholderText;
        private bool isPlaceholder;

        [Category("Дополнительные настройки")]
        public Color BorderColorFocused
        {
            get
            { return borderColorFocused; }
            set
            {
                borderColorFocused = value;
                Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public Color BorderColorUnfocused
        {
            get
            { return borderColorUnfocused; }
            set
            {
                borderColorUnfocused = value;
                Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public Color BackgroundColor
        {
            get
            { return innerRichTextBox.BackColor; }
            set
            {
                BackColor = value;
                innerRichTextBox.BackColor = value;
                Invalidate();
            }
        }

        [Category("Дополнительные настройки")]
        public bool Editable
        {
            get { return !innerRichTextBox.ReadOnly; }
            set
            {
                innerRichTextBox.ReadOnly = !value;
            }
        }

        [Category("Дополнительные настройки")]
        public String PlaceholderText
        {
            get
            { return placeholderText; }
            set
            {
                placeholderText = value;
                innerRichTextBox.Text = value;
                Invalidate();
            }
        }

        public RichTextBox getInnerTextBox()
        {
            return innerRichTextBox;
        }

        public bool isInPlaceholderMode()
        {
            return isPlaceholder;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Pen borderPen;

            if (innerRichTextBox.Focused)
            {
                borderPen = new Pen(borderColorFocused, 1);
            }
            else
            {
                borderPen = new Pen(borderColorUnfocused, 1);
            }

            g.DrawRectangle(borderPen, 1, 1, Width - 2, Height - 2);
            borderPen.Dispose();
        }

        private void innerRichTextBox_Enter(object sender, EventArgs e)
        {
            if (isPlaceholder)
            {
                innerRichTextBox.Text = "";
                isPlaceholder = false;
                innerRichTextBox.ForeColor = Color.Black;
            }
            Invalidate();
        }

        private void innerRichTextBox_Leave(object sender, EventArgs e)
        {
            if (innerRichTextBox.Text.Length == 0)
            {
                isPlaceholder = true;
                innerRichTextBox.Text = placeholderText;
                innerRichTextBox.ForeColor = Color.Gray;
            }
            Invalidate();
        }

        private void CustomRichTextBox_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void highlightText(int startPosition, int length, Color color)
        {
            innerRichTextBox.SelectionStart = startPosition;
            innerRichTextBox.SelectionLength = length;
            innerRichTextBox.SelectionBackColor = color;
            innerRichTextBox.SelectionLength = 0;
        }

        public void removeHighlight()
        {
            int i = innerRichTextBox.SelectionStart;
            innerRichTextBox.SelectAll();
            innerRichTextBox.SelectionBackColor = Color.White;
            innerRichTextBox.DeselectAll();
            innerRichTextBox.SelectionStart = i;
        }

        public void removeFontFormatting()
        {
            int i = innerRichTextBox.SelectionStart;
            innerRichTextBox.SelectAll();
            innerRichTextBox.SelectionBackColor = Color.White;
            innerRichTextBox.SelectionFont = new Font("Consolas", 14);
            innerRichTextBox.SelectionColor = Color.Black;
            innerRichTextBox.DeselectAll();
            innerRichTextBox.SelectionStart = i;
        }

        public void appendText(string line, Color color)
        {
            if(isPlaceholder)
            {
                isPlaceholder = false;
                innerRichTextBox.Clear();
            }

            innerRichTextBox.SelectionStart = innerRichTextBox.Text.Length;
            innerRichTextBox.SelectionLength = 0;
            innerRichTextBox.SelectionColor = color;
            innerRichTextBox.AppendText(line);
        }

        public void setText(string[] lines, Color color)
        {
            if (isPlaceholder)
            {
                isPlaceholder = false;
            }
            
            innerRichTextBox.Clear();
            innerRichTextBox.ForeColor = color;

            innerRichTextBox.Lines = lines;

            innerRichTextBox.SelectionStart = 0;
            innerRichTextBox.SelectionLength = 0;
        }
        
        private void innerRichTextBox_TextChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void innerRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool ctrlV = (e.Modifiers == Keys.Control && e.KeyCode == Keys.V);
            bool shiftInsert = (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Insert);

            if (ctrlV || shiftInsert)
            {
                innerRichTextBox.Paste();
                removeFontFormatting();
                e.Handled = true;
            }
        }
    }
}
