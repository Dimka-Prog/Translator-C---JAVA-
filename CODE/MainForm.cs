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
            hasUnsavedChanges = false;
            highlighted = false;

            cSharpCustomRichTextBox.getInnerTextBox().TextChanged += new EventHandler(ehCSharpCustomTextBox_TextChanged);
            consoleCustomRichTextBox.getInnerTextBox().TextChanged += new EventHandler(ehConsoleCustomTextBox_TextChanged);

            cSharpCustomRichTextBox.getInnerTextBox().SelectionChanged += new EventHandler(ehCSharpCustomRichTextBox_SelectionChanged);
            javaCustomRichTextBox.getInnerTextBox().SelectionChanged += new EventHandler(ehJavaCustomRichTextBox_SelectionChanged);

            translateCustomButton.Enabled = false;
            clearInputCustomButton.Enabled = false;
            clearConsoleCustomButton.Enabled = false;
            saveCustomButton.Enabled = false;

            mainMenuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomMenuStripColorTable());
        }

        private bool hasUnsavedChanges;
        private bool highlighted;

        private void ehCSharpCustomTextBox_TextChanged(object sender, EventArgs e)
        {
            if(highlighted)
            {
                cSharpCustomRichTextBox.removeHighlight();
                highlighted = false;
            }

            if (cSharpCustomRichTextBox.getInnerTextBox().Text.Length == 0 ||
                cSharpCustomRichTextBox.isInPlaceholderMode())
            {
                clearInputCustomButton.Enabled = false;
                translateCustomButton.Enabled = false;
            }
            else
            {
                clearInputCustomButton.Enabled = true;
                translateCustomButton.Enabled = true;
            }

            clearInputCustomButton.Invalidate();
            translateCustomButton.Invalidate();
        }

        private void ehConsoleCustomTextBox_TextChanged(object sender, EventArgs e)
        {
            if (consoleCustomRichTextBox.getInnerTextBox().Text.Length == 0 ||
                consoleCustomRichTextBox.isInPlaceholderMode())
            {
                clearConsoleCustomButton.Enabled = false;
            }
            else
            {
                clearConsoleCustomButton.Enabled = true;
            }

            clearConsoleCustomButton.Invalidate();
        }

        private void ehCSharpCustomRichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            int line = cSharpCustomRichTextBox.getInnerTextBox().
                       GetLineFromCharIndex(cSharpCustomRichTextBox.
                                            getInnerTextBox().
                                            SelectionStart);
            int column = cSharpCustomRichTextBox.getInnerTextBox().SelectionStart -
                         cSharpCustomRichTextBox.getInnerTextBox().GetFirstCharIndexFromLine(line);

            this.cSharpLineColumnNumbersText.Text = "Строка: " + (line + 1) + ", столбец: " + (column + 1);
        }

        private void ehJavaCustomRichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            int line = javaCustomRichTextBox.getInnerTextBox().
                       GetLineFromCharIndex(javaCustomRichTextBox.
                                            getInnerTextBox().
                                            SelectionStart);
            int column = javaCustomRichTextBox.getInnerTextBox().SelectionStart -
                         javaCustomRichTextBox.getInnerTextBox().GetFirstCharIndexFromLine(line);

            this.javaLineColumnNumbersText.Text = "Строка: " + (line + 1) + ", столбец: " + (column + 1);
        }

        private void loadFromFile()
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(this.openFileDialog.FileName);

                if (lines.Length != 0)
                {
                    bool isWhitespace = true;

                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            isWhitespace = false;
                            cSharpCustomRichTextBox.setText(lines, Color.Black);
                            javaCustomRichTextBox.getInnerTextBox().Clear();
                            saveCustomButton.Enabled = false;
                            saveMenuItem.Enabled = false;
                            break;
                        }
                    }

                    if (isWhitespace)
                    {
                        CustomMessageBox customMessageBox = new CustomMessageBox("Загрузка кода из файла",
                                                           "Выбранный файл не содержит печатные символы.",
                                                           CustomMessageBox.Icons.EXCLAMATION,
                                                           CustomMessageBox.Buttons.OK);
                        customMessageBox.ShowDialog();
                        customMessageBox.Dispose();
                    }
                }
                else
                {
                    CustomMessageBox customMessageBox = new CustomMessageBox("Загрузка кода из файла",
                                                           "Выбранный файл пустой.",
                                                           CustomMessageBox.Icons.EXCLAMATION,
                                                           CustomMessageBox.Buttons.OK);
                    customMessageBox.ShowDialog();
                    customMessageBox.Dispose();
                }
            }
        }

        private bool saveToFile()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                hasUnsavedChanges = false;
                this.Text = "C# to Java Translator";

                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                foreach (string line in javaCustomRichTextBox.getInnerTextBox().Lines)
                    streamWriter.WriteLine(line);
                streamWriter.Close();
                return true;
            }

            return false;
        }

        private void openCustomButton_Click(object sender, EventArgs e)
        {
            if(hasUnsavedChanges)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox("Есть несохранённые изменения",
                                                           "Сохранить транслированный код?",
                                                           CustomMessageBox.Icons.INFORMATION,
                                                           CustomMessageBox.Buttons.YES_NO_CANCEL);
                DialogResult result = customMessageBox.ShowDialog();
                customMessageBox.Dispose();

                if (result == DialogResult.Yes)
                {
                    if(saveToFile())
                    {
                        loadFromFile();
                    }
                }
                else if (result == DialogResult.No)
                {
                    hasUnsavedChanges = false;
                    this.Text = "C# to Java Translator";
                    cSharpCustomRichTextBox.getInnerTextBox().Clear();
                    javaCustomRichTextBox.getInnerTextBox().Clear();
                    loadFromFile();
                }
            }
            else
            {
                loadFromFile();
            }

            openCustomButton.Invalidate();
        }

        private void clearInputCustomButton_Click(object sender, EventArgs e)
        {
            if (hasUnsavedChanges)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox("Есть несохранённые изменения",
                                                           "Сохранить транслированный код?",
                                                           CustomMessageBox.Icons.INFORMATION,
                                                           CustomMessageBox.Buttons.YES_NO_CANCEL);
                DialogResult result = customMessageBox.ShowDialog();
                customMessageBox.Dispose();

                if (result == DialogResult.Yes)
                {
                    if (saveToFile())
                    {
                        cSharpCustomRichTextBox.getInnerTextBox().Clear();
                        saveCustomButton.Enabled = false;
                        saveMenuItem.Enabled = false;
                        javaCustomRichTextBox.getInnerTextBox().Clear();
                        hasUnsavedChanges = false;
                        this.Text = "C# to Java Translator";
                    }
                }
                else if (result == DialogResult.No)
                {
                    cSharpCustomRichTextBox.getInnerTextBox().Clear();
                    saveCustomButton.Enabled = false;
                    saveMenuItem.Enabled = false;
                    javaCustomRichTextBox.getInnerTextBox().Clear();
                    hasUnsavedChanges = false;
                    this.Text = "C# to Java Translator";
                }
            }
            else
            {
                cSharpCustomRichTextBox.getInnerTextBox().Clear();
                saveCustomButton.Enabled = false;
                saveMenuItem.Enabled = false;
                javaCustomRichTextBox.getInnerTextBox().Clear();
                hasUnsavedChanges = false;
                this.Text = "C# to Java Translator";
            }
        }

        private void translateCustomButton_Click(object sender, EventArgs e)
        {
            if (this.cleanLogsCustomCheckBox.Checked)
            {
                this.consoleCustomRichTextBox.getInnerTextBox().Clear();
            }

            this.consoleCustomRichTextBox.appendText("[INFO] : " + System.DateTime.Now + "- трансляция начата.\n", Color.Black);
            javaCustomRichTextBox.getInnerTextBox().Clear();

            TranslationResultBus translationResultBus = 
                new TranslationResultBus(this.consoleCustomRichTextBox);

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(translationResultBus);
            Token[] tokenArr = lexicalAnalyzer.parse(this.cSharpCustomRichTextBox).ToArray();

            if(tokenArr.Length > 0)
            {
                SyntaxAnalyzer syntaxAnalyzer = new SyntaxAnalyzer(translationResultBus);
                SyntaxTree syntaxTree = syntaxAnalyzer.parse(ref tokenArr);

                if (translationResultBus.getErrorCount() == 0)
                {
                    SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(syntaxTree, translationResultBus);
                    semanticAnalyzer.semanticAnalysis();
                }

                if (translationResultBus.getErrorCount() == 0)
                {
                    CodeGenerator codeGenerator = new CodeGenerator(syntaxTree, translationResultBus);
                    javaCustomRichTextBox.setText(codeGenerator.generateCode().ToArray(), Color.Black);
                    saveCustomButton.Enabled = true;
                    saveMenuItem.Enabled = true;
                    hasUnsavedChanges = true;
                    this.Text = "C# to Java Translator - есть несохранённые изменения";
                }
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
            saveToFile();
            this.saveCustomButton.Invalidate();
        }

        private void clearConsoleCustomButton_Click(object sender, EventArgs e)
        {
            this.consoleCustomRichTextBox.getInnerTextBox().Clear();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (hasUnsavedChanges)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox("Есть несохранённые изменения",
                                                           "Сохранить транслированный код?",
                                                           CustomMessageBox.Icons.INFORMATION,
                                                           CustomMessageBox.Buttons.YES_NO_CANCEL);
                DialogResult result = customMessageBox.ShowDialog();
                customMessageBox.Dispose();

                if (result == DialogResult.Yes)
                {
                    if (saveToFile())
                    {
                        loadFromFile();
                    }
                }
                else if (result == DialogResult.No)
                {
                    hasUnsavedChanges = false;
                    saveCustomButton.Enabled = false;
                    saveMenuItem.Enabled = false;
                    this.Text = "C# to Java Translator";
                    cSharpCustomRichTextBox.getInnerTextBox().Clear();
                    javaCustomRichTextBox.getInnerTextBox().Clear();
                    loadFromFile();
                }
            }
            else
            {
                loadFromFile();
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            saveToFile();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasUnsavedChanges)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox("Есть несохранённые изменения",
                                                           "Сохранить транслированный код?",
                                                           CustomMessageBox.Icons.INFORMATION,
                                                           CustomMessageBox.Buttons.YES_NO_CANCEL);
                DialogResult result = customMessageBox.ShowDialog();
                customMessageBox.Dispose();

                if (result == DialogResult.Yes)
                {
                    if(!saveToFile())
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
