using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CSharpToJavaTranslator
{
    public partial class CustomMessageBox : Form
    {
        public enum Icons
        {
            INFORMATION,
            EXCLAMATION
        }

        public enum Buttons
        {
            OK,
            YES_NO_CANCEL
        }

        public CustomMessageBox(string title, string message, Icons icon, Buttons buttons)
        {
            InitializeComponent();

            this.Text = title;
            this.messageLabel.Text = message;

            iconPictureBox.Image = new Bitmap(119, 119);
            Graphics g = Graphics.FromImage(iconPictureBox.Image);
            if (icon == Icons.INFORMATION)
            {
                Brush b = new SolidBrush(Color.Silver);
                Pen p = new Pen(b, 2);
                
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawEllipse(p, 5, 5, 80, 80);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString("?", new Font("Segoe UI", 50), b, 20, 0);
                
                b.Dispose();
                p.Dispose();
            }
            else
            {
                Brush b = new SolidBrush(Color.Orange);
                Pen p = new Pen(b, 2);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(p, 5, 85, 40, 10);
                g.DrawLine(p, 40, 10, 80, 85);
                g.DrawLine(p, 80, 85, 5, 85);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString("!", new Font("Segoe UI", 50), b, 20, 0);

                b.Dispose();
                p.Dispose();
            }
            g.Dispose();

            if (buttons == Buttons.OK)
            {
                CustomButton okButton = makeWhiteButton();
                okButton.Text = "OK";
                okButton.Width = 130;
                okButton.Height = 30;
                okButton.DialogResult = DialogResult.OK;
                flowLayoutPanel.Controls.Add(okButton);
            }
            else
            {
                CustomButton cancelButton = makeWhiteButton();
                cancelButton.Text = "Отмена";
                cancelButton.Width = 130;
                cancelButton.Height = 30;
                cancelButton.DialogResult = DialogResult.Cancel;
                flowLayoutPanel.Controls.Add(cancelButton);

                CustomButton noButton = makeRedButton();
                noButton.Text = "Нет";
                noButton.Width = 130;
                noButton.Height = 30;
                noButton.DialogResult = DialogResult.No;
                flowLayoutPanel.Controls.Add(noButton);

                CustomButton yesButton = makeGreenButton();
                yesButton.Text = "Да";
                yesButton.Width = 130;
                yesButton.Height = 30;
                yesButton.DialogResult = DialogResult.Yes;
                flowLayoutPanel.Controls.Add(yesButton);
            }

            messageLabel.Focus();
        }

        private CustomButton makeGreenButton()
        {
            CustomButton greenButton = new CustomButton();

            greenButton.BackgroundColorFocused = Color.FromArgb(111, 221, 151);
            greenButton.BackgroundColorMouseDown = Color.FromArgb(43, 176, 96);
            greenButton.BackgroundColorUnfocused = Color.FromArgb(50, 205, 112);
            greenButton.BorderColorFocused = Color.Green;
            greenButton.BorderColorMouseDown = Color.FromArgb(31, 126, 29);
            greenButton.BorderColorUnfocused = Color.FromArgb(37, 152, 83);
            greenButton.BorderWidth = 1;
            greenButton.TextColorFocused = Color.Black;
            greenButton.TextColorUnfocused = Color.Black;

            return greenButton;
        }

        private CustomButton makeWhiteButton()
        {
            CustomButton whiteButton = new CustomButton();

            whiteButton.BackgroundColorFocused = Color.FromArgb(111, 221, 151);
            whiteButton.BackgroundColorMouseDown = Color.FromArgb(43, 176, 96);
            whiteButton.BackgroundColorUnfocused = Color.White;
            whiteButton.BorderColorFocused = Color.Green;
            whiteButton.BorderColorMouseDown = Color.FromArgb(31, 126, 29);
            whiteButton.BorderColorUnfocused = Color.Silver;
            whiteButton.BorderWidth = 1;
            whiteButton.TextColorFocused = Color.Black;
            whiteButton.TextColorUnfocused = Color.Black;

            return whiteButton;
        }

        private CustomButton makeRedButton()
        {
            CustomButton redButton = new CustomButton();

            redButton.BackgroundColorFocused = Color.FromArgb(255, 100, 100);
            redButton.BackgroundColorMouseDown = Color.FromArgb(255, 40, 40);
            redButton.BackgroundColorUnfocused = Color.White;
            redButton.BorderColorFocused = Color.FromArgb(220, 0, 0);
            redButton.BorderColorMouseDown = Color.FromArgb(170, 0, 0);
            redButton.BorderColorUnfocused = Color.Silver;
            redButton.BorderWidth = 1;
            redButton.TextColorFocused = Color.White;
            redButton.TextColorUnfocused = Color.Black;

            return redButton;
        }
    }
}
