
namespace CSharpToJavaTranslator
{
    partial class CustomRichTextBox
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.innerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // innerRichTextBox
            // 
            this.innerRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.innerRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerRichTextBox.Location = new System.Drawing.Point(7, 7);
            this.innerRichTextBox.Name = "innerRichTextBox";
            this.innerRichTextBox.Size = new System.Drawing.Size(136, 136);
            this.innerRichTextBox.TabIndex = 0;
            this.innerRichTextBox.Text = "";
            this.innerRichTextBox.WordWrap = false;
            this.innerRichTextBox.Enter += new System.EventHandler(this.innerRichTextBox_Enter);
            this.innerRichTextBox.Leave += new System.EventHandler(this.innerRichTextBox_Leave);
            // 
            // CustomRichTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.innerRichTextBox);
            this.Name = "CustomRichTextBox";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Resize += new System.EventHandler(this.CustomRichTextBox_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox innerRichTextBox;
    }
}
