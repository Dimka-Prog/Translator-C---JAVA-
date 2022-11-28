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
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            this.hasUnsavedChanges = false;

            this.cSharpCustomRichTextBox.getInnerTextBox().TextChanged += new EventHandler(Form1_TextChanged);
            this.translateCustomButton.Enabled = false;
            this.clearCustomButton.Enabled = false;

            this.mainMenuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomMenuStripColorTable());
        }

        private bool hasUnsavedChanges;

        private void Form1_TextChanged(object sender, EventArgs e)
        {
            if(this.cSharpCustomRichTextBox.getInnerTextBox().Text.Length == 0 ||
               this.cSharpCustomRichTextBox.isInPlaceholderMode())
            {
                this.clearCustomButton.Enabled = false;
            }
            else
            {
                this.clearCustomButton.Enabled = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openCustomButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
            this.openCustomButton.Invalidate();
        }

        private void saveCustomButton_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.ShowDialog();
            this.saveCustomButton.Invalidate();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.ShowDialog();
        }
    }
}
