using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSharpToJavaTranslator
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            this.hasUnsavedChanges = false;
            this.highlighted = false;

            this.cSharpCustomRichTextBox.getInnerTextBox().TextChanged += new EventHandler(ehCSharpCustomTextBox_TextChanged);
            this.consoleCustomRichTextBox.getInnerTextBox().TextChanged += new EventHandler(ehConsoleCustomTextBox_TextChanged);

            this.translateCustomButton.Enabled = false;
            this.clearInputCustomButton.Enabled = false;
            this.clearConsoleCustomButton.Enabled = false;

            this.mainMenuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomMenuStripColorTable());
        }

        private bool hasUnsavedChanges;
        private bool highlighted;
        private void ehCSharpCustomTextBox_TextChanged(object sender, EventArgs e)
        {
            if(highlighted)
            {
                this.cSharpCustomRichTextBox.removeHighlight();
                this.highlighted = false;
            }

            if (this.cSharpCustomRichTextBox.getInnerTextBox().Text.Length == 0 ||
                this.cSharpCustomRichTextBox.isInPlaceholderMode())
            {
                this.clearInputCustomButton.Enabled = false;
                this.translateCustomButton.Enabled = false;
            }
            else
            {
                this.clearInputCustomButton.Enabled = true;
                this.translateCustomButton.Enabled = true;
            }

            this.clearInputCustomButton.Invalidate();
            this.translateCustomButton.Invalidate();
        }

        private void ehConsoleCustomTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.consoleCustomRichTextBox.getInnerTextBox().Text.Length == 0 ||
                this.consoleCustomRichTextBox.isInPlaceholderMode())
            {
                this.clearConsoleCustomButton.Enabled = false;
            }
            else
            {
                this.clearConsoleCustomButton.Enabled = true;
            }

            this.clearConsoleCustomButton.Invalidate();
        }

        private void openCustomButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(this.openFileDialog.FileName);
                
                if(lines.Length != 0)
                {
                    bool isWhitespace = true;

                    foreach(string line in lines)
                    {
                        if(!string.IsNullOrWhiteSpace(line))
                        {
                            isWhitespace = false;
                            this.cSharpCustomRichTextBox.setText(lines, Color.Black);
                            break;
                        }
                    }

                    if(isWhitespace)
                    {
                        MessageBox.Show("Файл пустой или не содержит печатные символы.");
                    }
                }
                else
                {
                    MessageBox.Show("Файл пустой или не содержит печатные символы.");
                }
            }

            this.openCustomButton.Invalidate();
        }

        private void clearInputCustomButton_Click(object sender, EventArgs e)
        {
            this.cSharpCustomRichTextBox.getInnerTextBox().Clear();
        }

        private void translateCustomButton_Click(object sender, EventArgs e)
        {
            TranslationResultBus translationResultBus = 
                new TranslationResultBus(this.consoleCustomRichTextBox);

            LexicalAnalyzer lexAn = new LexicalAnalyzer(this.consoleCustomRichTextBox,
                                                        translationResultBus);
            Token[] tokenArr = lexAn.parse(this.cSharpCustomRichTextBox).ToArray();

            if(tokenArr.Length > 0)
            {
                SyntaxAnalyzer syntAn = new SyntaxAnalyzer(this.consoleCustomRichTextBox,
                                                       translationResultBus);
                SyntaxTree syntTree = syntAn.parse(ref tokenArr);
            }
            else
            {
                this.consoleCustomRichTextBox.appendText("[INFO] : отсутствуют лексемы в выходном потоке лексического анализатора, дальнейший анализ отменён.\n", Color.Black);
            }

            translationResultBus.summarizeTranslation();
            translationResultBus.highlight(this.cSharpCustomRichTextBox);
            this.highlighted = true;
        }

        private void saveCustomButton_Click(object sender, EventArgs e)
        {
            if(this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.hasUnsavedChanges = false;

                StreamWriter streamWriter = new StreamWriter(this.saveFileDialog.FileName);
                foreach (string line in this.javaCustomRichTextBox.getInnerTextBox().Lines)
                    streamWriter.WriteLine(line);
                streamWriter.Close();
            }

            this.saveCustomButton.Invalidate();
        }

        private void clearConsoleCustomButton_Click(object sender, EventArgs e)
        {
            this.consoleCustomRichTextBox.getInnerTextBox().Clear();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            //if(this.openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string[] lines = File.ReadAllLines(this.openFileDialog.FileName);
            //    MessageBox.Show(lines.Length + "", "");
            //}
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.ShowDialog();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
