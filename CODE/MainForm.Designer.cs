
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.javaLineColumnNumbersText = new System.Windows.Forms.Label();
            this.cSharpLineColumnNumbersText = new System.Windows.Forms.Label();
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
            this.clearInputCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.cSharpCustomRichTextBox = new CSharpToJavaTranslator.CustomRichTextBox();
            this.javaCustomRichTextBox = new CSharpToJavaTranslator.CustomRichTextBox();
            this.clearConsoleCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.saveCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.openCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.translateCustomButton = new CSharpToJavaTranslator.CustomButton();
            this.consoleCustomRichTextBox = new CSharpToJavaTranslator.CustomRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.BackColor = System.Drawing.Color.Gray;
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 30);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainSplitContainer.Panel1.Controls.Add(this.clearInputCustomButton);
            this.mainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.mainSplitContainer.Panel1.Controls.Add(this.clearConsoleCustomButton);
            this.mainSplitContainer.Panel1.Controls.Add(this.saveCustomButton);
            this.mainSplitContainer.Panel1.Controls.Add(this.openCustomButton);
            this.mainSplitContainer.Panel1.Controls.Add(this.translateCustomButton);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainSplitContainer.Panel2.Controls.Add(this.consoleCustomRichTextBox);
            this.mainSplitContainer.Size = new System.Drawing.Size(852, 473);
            this.mainSplitContainer.SplitterDistance = 393;
            this.mainSplitContainer.TabIndex = 0;
            this.mainSplitContainer.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.javaLineColumnNumbersText, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cSharpCustomRichTextBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.javaCustomRichTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cSharpLineColumnNumbersText, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 46);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(852, 299);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // javaLineColumnNumbersText
            // 
            this.javaLineColumnNumbersText.AutoSize = true;
            this.javaLineColumnNumbersText.BackColor = System.Drawing.Color.WhiteSmoke;
            this.javaLineColumnNumbersText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.javaLineColumnNumbersText.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.javaLineColumnNumbersText.Location = new System.Drawing.Point(429, 279);
            this.javaLineColumnNumbersText.Name = "javaLineColumnNumbersText";
            this.javaLineColumnNumbersText.Size = new System.Drawing.Size(420, 20);
            this.javaLineColumnNumbersText.TabIndex = 5;
            this.javaLineColumnNumbersText.Text = "Строка, столбец";
            // 
            // cSharpLineColumnNumbersText
            // 
            this.cSharpLineColumnNumbersText.AutoSize = true;
            this.cSharpLineColumnNumbersText.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cSharpLineColumnNumbersText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSharpLineColumnNumbersText.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cSharpLineColumnNumbersText.Location = new System.Drawing.Point(3, 279);
            this.cSharpLineColumnNumbersText.Name = "cSharpLineColumnNumbersText";
            this.cSharpLineColumnNumbersText.Size = new System.Drawing.Size(420, 20);
            this.cSharpLineColumnNumbersText.TabIndex = 4;
            this.cSharpLineColumnNumbersText.Text = "Строка, столбец";
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
            // clearInputCustomButton
            // 
            this.clearInputCustomButton.BackColor = System.Drawing.Color.White;
            this.clearInputCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.clearInputCustomButton.BackgroundColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.clearInputCustomButton.BackgroundColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.clearInputCustomButton.BackgroundColorUnfocused = System.Drawing.Color.White;
            this.clearInputCustomButton.BorderColorDisabled = System.Drawing.Color.Gray;
            this.clearInputCustomButton.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clearInputCustomButton.BorderColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clearInputCustomButton.BorderColorUnfocused = System.Drawing.Color.Silver;
            this.clearInputCustomButton.BorderWidth = 1;
            this.clearInputCustomButton.FlatAppearance.BorderSize = 0;
            this.clearInputCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearInputCustomButton.ForeColor = System.Drawing.Color.Black;
            this.clearInputCustomButton.Location = new System.Drawing.Point(139, 10);
            this.clearInputCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.clearInputCustomButton.Name = "clearInputCustomButton";
            this.clearInputCustomButton.Size = new System.Drawing.Size(130, 30);
            this.clearInputCustomButton.TabIndex = 9;
            this.clearInputCustomButton.Text = "Очистить ввод";
            this.clearInputCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.clearInputCustomButton.TextColorFocused = System.Drawing.Color.White;
            this.clearInputCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.clearInputCustomButton.UseVisualStyleBackColor = false;
            this.clearInputCustomButton.Click += new System.EventHandler(this.clearInputCustomButton_Click);
            // 
            // cSharpCustomRichTextBox
            // 
            this.cSharpCustomRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.cSharpCustomRichTextBox.BackgroundColor = System.Drawing.SystemColors.Window;
            this.cSharpCustomRichTextBox.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(176)))), ((int)(((byte)(96)))));
            this.cSharpCustomRichTextBox.BorderColorUnfocused = System.Drawing.Color.Gray;
            this.cSharpCustomRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSharpCustomRichTextBox.Editable = true;
            this.cSharpCustomRichTextBox.Font = new System.Drawing.Font("Consolas", 14F);
            this.cSharpCustomRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.cSharpCustomRichTextBox.Name = "cSharpCustomRichTextBox";
            this.cSharpCustomRichTextBox.Padding = new System.Windows.Forms.Padding(5);
            this.cSharpCustomRichTextBox.PlaceholderText = "Введите сюда код на C#...";
            this.cSharpCustomRichTextBox.Size = new System.Drawing.Size(420, 273);
            this.cSharpCustomRichTextBox.TabIndex = 2;
            // 
            // javaCustomRichTextBox
            // 
            this.javaCustomRichTextBox.BackColor = System.Drawing.Color.White;
            this.javaCustomRichTextBox.BackgroundColor = System.Drawing.Color.White;
            this.javaCustomRichTextBox.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(176)))), ((int)(((byte)(96)))));
            this.javaCustomRichTextBox.BorderColorUnfocused = System.Drawing.Color.Gray;
            this.javaCustomRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.javaCustomRichTextBox.Editable = false;
            this.javaCustomRichTextBox.Font = new System.Drawing.Font("Consolas", 14F);
            this.javaCustomRichTextBox.Location = new System.Drawing.Point(429, 3);
            this.javaCustomRichTextBox.Name = "javaCustomRichTextBox";
            this.javaCustomRichTextBox.Padding = new System.Windows.Forms.Padding(5);
            this.javaCustomRichTextBox.PlaceholderText = "Здесь появится код на Java...";
            this.javaCustomRichTextBox.Size = new System.Drawing.Size(420, 273);
            this.javaCustomRichTextBox.TabIndex = 3;
            this.javaCustomRichTextBox.TabStop = false;
            // 
            // clearConsoleCustomButton
            // 
            this.clearConsoleCustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearConsoleCustomButton.BackColor = System.Drawing.Color.White;
            this.clearConsoleCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.clearConsoleCustomButton.BackgroundColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.clearConsoleCustomButton.BackgroundColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.clearConsoleCustomButton.BackgroundColorUnfocused = System.Drawing.Color.White;
            this.clearConsoleCustomButton.BorderColorDisabled = System.Drawing.Color.Gray;
            this.clearConsoleCustomButton.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clearConsoleCustomButton.BorderColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clearConsoleCustomButton.BorderColorUnfocused = System.Drawing.Color.Silver;
            this.clearConsoleCustomButton.BorderWidth = 1;
            this.clearConsoleCustomButton.FlatAppearance.BorderSize = 0;
            this.clearConsoleCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearConsoleCustomButton.ForeColor = System.Drawing.Color.Black;
            this.clearConsoleCustomButton.Location = new System.Drawing.Point(3, 355);
            this.clearConsoleCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.clearConsoleCustomButton.Name = "clearConsoleCustomButton";
            this.clearConsoleCustomButton.Size = new System.Drawing.Size(130, 30);
            this.clearConsoleCustomButton.TabIndex = 6;
            this.clearConsoleCustomButton.Text = "Очистить консоль";
            this.clearConsoleCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.clearConsoleCustomButton.TextColorFocused = System.Drawing.Color.White;
            this.clearConsoleCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.clearConsoleCustomButton.UseVisualStyleBackColor = false;
            this.clearConsoleCustomButton.Click += new System.EventHandler(this.clearConsoleCustomButton_Click);
            // 
            // saveCustomButton
            // 
            this.saveCustomButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(112)))));
            this.saveCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.saveCustomButton.BackgroundColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(221)))), ((int)(((byte)(151)))));
            this.saveCustomButton.BackgroundColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(176)))), ((int)(((byte)(96)))));
            this.saveCustomButton.BackgroundColorUnfocused = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(112)))));
            this.saveCustomButton.BorderColorDisabled = System.Drawing.Color.Gray;
            this.saveCustomButton.BorderColorFocused = System.Drawing.Color.Green;
            this.saveCustomButton.BorderColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(126)))), ((int)(((byte)(29)))));
            this.saveCustomButton.BorderColorUnfocused = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(152)))), ((int)(((byte)(83)))));
            this.saveCustomButton.BorderWidth = 1;
            this.saveCustomButton.Enabled = false;
            this.saveCustomButton.FlatAppearance.BorderSize = 0;
            this.saveCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveCustomButton.ForeColor = System.Drawing.Color.Black;
            this.saveCustomButton.Location = new System.Drawing.Point(429, 10);
            this.saveCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.saveCustomButton.Name = "saveCustomButton";
            this.saveCustomButton.Size = new System.Drawing.Size(130, 30);
            this.saveCustomButton.TabIndex = 7;
            this.saveCustomButton.Text = "Сохранить...";
            this.saveCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.saveCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.saveCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.saveCustomButton.UseVisualStyleBackColor = false;
            this.saveCustomButton.Click += new System.EventHandler(this.saveCustomButton_Click);
            // 
            // openCustomButton
            // 
            this.openCustomButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(112)))));
            this.openCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.openCustomButton.BackgroundColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(221)))), ((int)(((byte)(151)))));
            this.openCustomButton.BackgroundColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(176)))), ((int)(((byte)(96)))));
            this.openCustomButton.BackgroundColorUnfocused = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(112)))));
            this.openCustomButton.BorderColorDisabled = System.Drawing.Color.Gray;
            this.openCustomButton.BorderColorFocused = System.Drawing.Color.Green;
            this.openCustomButton.BorderColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(126)))), ((int)(((byte)(29)))));
            this.openCustomButton.BorderColorUnfocused = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(152)))), ((int)(((byte)(83)))));
            this.openCustomButton.BorderWidth = 1;
            this.openCustomButton.FlatAppearance.BorderSize = 0;
            this.openCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openCustomButton.ForeColor = System.Drawing.Color.Black;
            this.openCustomButton.Location = new System.Drawing.Point(3, 10);
            this.openCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.openCustomButton.Name = "openCustomButton";
            this.openCustomButton.Size = new System.Drawing.Size(130, 30);
            this.openCustomButton.TabIndex = 4;
            this.openCustomButton.Text = "Открыть...";
            this.openCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.openCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.openCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.openCustomButton.UseVisualStyleBackColor = false;
            this.openCustomButton.Click += new System.EventHandler(this.openCustomButton_Click);
            // 
            // translateCustomButton
            // 
            this.translateCustomButton.BackColor = System.Drawing.Color.White;
            this.translateCustomButton.BackgroundColorDisabled = System.Drawing.Color.Gainsboro;
            this.translateCustomButton.BackgroundColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(221)))), ((int)(((byte)(151)))));
            this.translateCustomButton.BackgroundColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(176)))), ((int)(((byte)(96)))));
            this.translateCustomButton.BackgroundColorUnfocused = System.Drawing.Color.White;
            this.translateCustomButton.BorderColorDisabled = System.Drawing.Color.Gray;
            this.translateCustomButton.BorderColorFocused = System.Drawing.Color.Green;
            this.translateCustomButton.BorderColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(126)))), ((int)(((byte)(29)))));
            this.translateCustomButton.BorderColorUnfocused = System.Drawing.Color.Silver;
            this.translateCustomButton.BorderWidth = 1;
            this.translateCustomButton.FlatAppearance.BorderSize = 0;
            this.translateCustomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.translateCustomButton.ForeColor = System.Drawing.Color.Black;
            this.translateCustomButton.Location = new System.Drawing.Point(293, 10);
            this.translateCustomButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.translateCustomButton.Name = "translateCustomButton";
            this.translateCustomButton.Size = new System.Drawing.Size(130, 30);
            this.translateCustomButton.TabIndex = 5;
            this.translateCustomButton.Text = "Транслировать";
            this.translateCustomButton.TextColorDisabled = System.Drawing.Color.Gray;
            this.translateCustomButton.TextColorFocused = System.Drawing.Color.Black;
            this.translateCustomButton.TextColorUnfocused = System.Drawing.Color.Black;
            this.translateCustomButton.UseVisualStyleBackColor = false;
            this.translateCustomButton.Click += new System.EventHandler(this.translateCustomButton_Click);
            // 
            // consoleCustomRichTextBox
            // 
            this.consoleCustomRichTextBox.BackColor = System.Drawing.Color.White;
            this.consoleCustomRichTextBox.BackgroundColor = System.Drawing.Color.White;
            this.consoleCustomRichTextBox.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(176)))), ((int)(((byte)(96)))));
            this.consoleCustomRichTextBox.BorderColorUnfocused = System.Drawing.Color.Gray;
            this.consoleCustomRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleCustomRichTextBox.Editable = false;
            this.consoleCustomRichTextBox.Font = new System.Drawing.Font("Consolas", 14F);
            this.consoleCustomRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.consoleCustomRichTextBox.Name = "consoleCustomRichTextBox";
            this.consoleCustomRichTextBox.Padding = new System.Windows.Forms.Padding(5);
            this.consoleCustomRichTextBox.PlaceholderText = "Здесь появятся сообщения от транслятора...";
            this.consoleCustomRichTextBox.Size = new System.Drawing.Size(852, 76);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private CustomButton saveCustomButton;
        private CustomButton translateCustomButton;
        private CustomButton openCustomButton;
        private CustomButton clearConsoleCustomButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CustomButton clearInputCustomButton;
        private System.Windows.Forms.Label javaLineColumnNumbersText;
        private System.Windows.Forms.Label cSharpLineColumnNumbersText;
    }
}

