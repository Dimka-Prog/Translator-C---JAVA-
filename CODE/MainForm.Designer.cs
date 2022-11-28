
namespace CSharpToJavaTranslator
{
    partial class mainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sep1MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотрСправкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.cSharpCustomRichTextBox = new CSharpToJavaTranslator.CustomRichTextBox();
            this.javaCustomRichTextBox = new CSharpToJavaTranslator.CustomRichTextBox();
            this.saveCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.translateCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.clearCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.consoleCustomRichTextBox = new CSharpToJavaTranslator.CustomRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.BackColor = System.Drawing.Color.DimGray;
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 30);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainSplitContainer.Panel2.Controls.Add(this.consoleCustomRichTextBox);
            this.mainSplitContainer.Size = new System.Drawing.Size(852, 473);
            this.mainSplitContainer.SplitterDistance = 259;
            this.mainSplitContainer.TabIndex = 0;
            this.mainSplitContainer.TabStop = false;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel.Controls.Add(this.openCustomButton, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.cSharpCustomRichTextBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.javaCustomRichTextBox, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.saveCustomButton, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.translateCustomButton, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.clearCustomButton, 2, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(852, 259);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(852, 30);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.saveMenuItem,
            this.sep1MenuItem,
            this.exitMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 26);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(175, 26);
            this.openMenuItem.Text = "Открыть...";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(175, 26);
            this.saveMenuItem.Text = "Сохранить...";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // sep1MenuItem
            // 
            this.sep1MenuItem.Name = "sep1MenuItem";
            this.sep1MenuItem.Size = new System.Drawing.Size(172, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(175, 26);
            this.exitMenuItem.Text = "Выход";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.просмотрСправкиToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(81, 26);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // просмотрСправкиToolStripMenuItem
            // 
            this.просмотрСправкиToolStripMenuItem.Name = "просмотрСправкиToolStripMenuItem";
            this.просмотрСправкиToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.просмотрСправкиToolStripMenuItem.Text = "Просмотр справки";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Файлы C#|*.cs|Текстовые файлы|*.txt";
            this.openFileDialog.Title = "Выберите файл для загрузки кода";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Файлы Java|*.java|Текстовые файлы|*.txt";
            this.saveFileDialog.Title = "Выберите файл для сохранения кода";
            // 
            // openCustomButton
            // 
            this.openCustomButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.openCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.openCustomButton.BackgroundColorFocused = System.Drawing.Color.LimeGreen;
            this.openCustomButton.BackgroundColorMouseDown = System.Drawing.Color.Green;
            this.openCustomButton.BackgroundColorUnfocused = System.Drawing.Color.DarkSeaGreen;
            this.openCustomButton.BorderColorDisabled = System.Drawing.Color.Gray;
            this.openCustomButton.BorderColorFocused = System.Drawing.Color.Green;
            this.openCustomButton.BorderColorMouseDown = System.Drawing.Color.Black;
            this.openCustomButton.BorderColorUnfocused = System.Drawing.Color.Green;
            this.openCustomButton.BorderWidth = 2;
            this.openCustomButton.FlatAppearance.BorderSize = 0;
            this.openCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openCustomButton.ForeColor = System.Drawing.Color.Black;
            this.openCustomButton.Location = new System.Drawing.Point(3, 209);
            this.openCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.openCustomButton.Name = "openCustomButton";
            this.openCustomButton.Size = new System.Drawing.Size(138, 40);
            this.openCustomButton.TabIndex = 4;
            this.openCustomButton.Text = "Открыть...";
            this.openCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.openCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.openCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.openCustomButton.UseVisualStyleBackColor = false;
            this.openCustomButton.Click += new System.EventHandler(this.openCustomButton_Click);
            // 
            // cSharpCustomRichTextBox
            // 
            this.cSharpCustomRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.cSharpCustomRichTextBox.BackgroundColor = System.Drawing.SystemColors.Window;
            this.cSharpCustomRichTextBox.BorderColorFocused = System.Drawing.Color.LimeGreen;
            this.cSharpCustomRichTextBox.BorderColorUnfocused = System.Drawing.Color.DimGray;
            this.tableLayoutPanel.SetColumnSpan(this.cSharpCustomRichTextBox, 2);
            this.cSharpCustomRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSharpCustomRichTextBox.Editable = true;
            this.cSharpCustomRichTextBox.Font = new System.Drawing.Font("Consolas", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cSharpCustomRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.cSharpCustomRichTextBox.Name = "cSharpCustomRichTextBox";
            this.cSharpCustomRichTextBox.Padding = new System.Windows.Forms.Padding(5);
            this.cSharpCustomRichTextBox.PlaceholderText = "Введите сюда код на C#...";
            this.cSharpCustomRichTextBox.Size = new System.Drawing.Size(420, 193);
            this.cSharpCustomRichTextBox.TabIndex = 2;
            // 
            // javaCustomRichTextBox
            // 
            this.javaCustomRichTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.javaCustomRichTextBox.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.javaCustomRichTextBox.BorderColorFocused = System.Drawing.Color.LimeGreen;
            this.javaCustomRichTextBox.BorderColorUnfocused = System.Drawing.Color.DimGray;
            this.tableLayoutPanel.SetColumnSpan(this.javaCustomRichTextBox, 3);
            this.javaCustomRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.javaCustomRichTextBox.Editable = false;
            this.javaCustomRichTextBox.Font = new System.Drawing.Font("Consolas", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.javaCustomRichTextBox.Location = new System.Drawing.Point(429, 3);
            this.javaCustomRichTextBox.Name = "javaCustomRichTextBox";
            this.javaCustomRichTextBox.Padding = new System.Windows.Forms.Padding(5);
            this.javaCustomRichTextBox.PlaceholderText = "Здесь появится код на Java...";
            this.javaCustomRichTextBox.Size = new System.Drawing.Size(420, 193);
            this.javaCustomRichTextBox.TabIndex = 3;
            this.javaCustomRichTextBox.TabStop = false;
            // 
            // saveCustomButton
            // 
            this.saveCustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCustomButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.saveCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.saveCustomButton.BackgroundColorFocused = System.Drawing.Color.LimeGreen;
            this.saveCustomButton.BackgroundColorMouseDown = System.Drawing.Color.Green;
            this.saveCustomButton.BackgroundColorUnfocused = System.Drawing.Color.DarkSeaGreen;
            this.saveCustomButton.BorderColorDisabled = System.Drawing.Color.Empty;
            this.saveCustomButton.BorderColorFocused = System.Drawing.Color.Empty;
            this.saveCustomButton.BorderColorMouseDown = System.Drawing.Color.Empty;
            this.saveCustomButton.BorderColorUnfocused = System.Drawing.Color.Empty;
            this.saveCustomButton.BorderWidth = 0;
            this.saveCustomButton.FlatAppearance.BorderSize = 0;
            this.saveCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveCustomButton.ForeColor = System.Drawing.Color.Black;
            this.saveCustomButton.Location = new System.Drawing.Point(717, 209);
            this.saveCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.saveCustomButton.Name = "saveCustomButton";
            this.saveCustomButton.Size = new System.Drawing.Size(132, 40);
            this.saveCustomButton.TabIndex = 7;
            this.saveCustomButton.Text = "Сохранить...";
            this.saveCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.saveCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.saveCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.saveCustomButton.UseVisualStyleBackColor = false;
            this.saveCustomButton.Click += new System.EventHandler(this.saveCustomButton_Click);
            // 
            // translateCustomButton
            // 
            this.translateCustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.translateCustomButton.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.translateCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.translateCustomButton.BackgroundColorFocused = System.Drawing.Color.LimeGreen;
            this.translateCustomButton.BackgroundColorMouseDown = System.Drawing.Color.Green;
            this.translateCustomButton.BackgroundColorUnfocused = System.Drawing.Color.DarkSeaGreen;
            this.translateCustomButton.BorderColorDisabled = System.Drawing.Color.Empty;
            this.translateCustomButton.BorderColorFocused = System.Drawing.Color.Empty;
            this.translateCustomButton.BorderColorMouseDown = System.Drawing.Color.Empty;
            this.translateCustomButton.BorderColorUnfocused = System.Drawing.Color.Empty;
            this.translateCustomButton.BorderWidth = 0;
            this.translateCustomButton.FlatAppearance.BorderSize = 0;
            this.translateCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.translateCustomButton.ForeColor = System.Drawing.Color.Black;
            this.translateCustomButton.Location = new System.Drawing.Point(285, 209);
            this.translateCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.translateCustomButton.Name = "translateCustomButton";
            this.translateCustomButton.Size = new System.Drawing.Size(138, 40);
            this.translateCustomButton.TabIndex = 5;
            this.translateCustomButton.Text = "Транслировать";
            this.translateCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.translateCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.translateCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.translateCustomButton.UseVisualStyleBackColor = false;
            // 
            // clearCustomButton
            // 
            this.clearCustomButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(155)))), ((int)(((byte)(157)))));
            this.clearCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.clearCustomButton.BackgroundColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.clearCustomButton.BackgroundColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clearCustomButton.BackgroundColorUnfocused = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(155)))), ((int)(((byte)(157)))));
            this.clearCustomButton.BorderColorDisabled = System.Drawing.Color.Empty;
            this.clearCustomButton.BorderColorFocused = System.Drawing.Color.Empty;
            this.clearCustomButton.BorderColorMouseDown = System.Drawing.Color.Empty;
            this.clearCustomButton.BorderColorUnfocused = System.Drawing.Color.Empty;
            this.clearCustomButton.BorderWidth = 0;
            this.clearCustomButton.FlatAppearance.BorderSize = 0;
            this.clearCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearCustomButton.ForeColor = System.Drawing.Color.Black;
            this.clearCustomButton.Location = new System.Drawing.Point(429, 209);
            this.clearCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.clearCustomButton.Name = "clearCustomButton";
            this.clearCustomButton.Size = new System.Drawing.Size(132, 40);
            this.clearCustomButton.TabIndex = 6;
            this.clearCustomButton.Text = "Очистить консоль";
            this.clearCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.clearCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.clearCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.clearCustomButton.UseVisualStyleBackColor = false;
            // 
            // consoleCustomRichTextBox
            // 
            this.consoleCustomRichTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.consoleCustomRichTextBox.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.consoleCustomRichTextBox.BorderColorFocused = System.Drawing.Color.LimeGreen;
            this.consoleCustomRichTextBox.BorderColorUnfocused = System.Drawing.Color.DimGray;
            this.consoleCustomRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleCustomRichTextBox.Editable = false;
            this.consoleCustomRichTextBox.Font = new System.Drawing.Font("Consolas", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.consoleCustomRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.consoleCustomRichTextBox.Name = "consoleCustomRichTextBox";
            this.consoleCustomRichTextBox.Padding = new System.Windows.Forms.Padding(5);
            this.consoleCustomRichTextBox.PlaceholderText = "Здесь появятся сообщения от транслятора...";
            this.consoleCustomRichTextBox.Size = new System.Drawing.Size(852, 210);
            this.consoleCustomRichTextBox.TabIndex = 8;
            this.consoleCustomRichTextBox.TabStop = false;
            // 
            // mainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(852, 503);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "mainForm";
            this.Text = "C# to Java Translator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private CustomRichTextBox javaCustomRichTextBox;
        private CustomRichTextBox cSharpCustomRichTextBox;
        private CustomRichTextBox consoleCustomRichTextBox;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripSeparator sep1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотрСправкиToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private CustomButton customButton21;
        private CustomButton saveCustomButton;
        private CustomButton translateCustomButton;
        private CustomButton openCustomButton;
        private CustomButton clearCustomButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

