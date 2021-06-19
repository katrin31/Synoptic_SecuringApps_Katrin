using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Steganography
{
    public partial class Steganography : Form
    {
        private Bitmap bmp = null;
        private string extractedText = string.Empty;

        public Steganography()
        {
            InitializeComponent();
        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;

            string text = dataTextBox.Text;

            if (text.Equals(""))
            {
                MessageBox.Show("The text you want to hide can't be empty", "Warning");

                return;
            }

            if (encryptCheckBox.Checked)
            {
                    text = Crypto.EncryptStringAES(text, passwordTextBox.Text);
       
            }

            bmp = SteganographyHelper.embedText(text, bmp);

            MessageBox.Show("Your text was hidden in the image successfully!", "Done");

            notesLabel.Text = "Notes: don't forget to save your new image.";
            notesLabel.ForeColor = Color.OrangeRed;
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;

            string extractedText = SteganographyHelper.extractText(bmp);

            if (encryptCheckBox.Checked)
            {
                try
                {
                    extractedText = Crypto.DecryptStringAES(extractedText, passwordTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Wrong password", "Error");

                    return;
                }
            }

            dataTextBox.Text = extractedText;
        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image File|*.bmp";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                string file = open_dialog.FileName;
                FileInfo fi = new FileInfo(file);
                int fileSize = (int)fi.Length / (1024 * 1024);

                 if (fileSize > 10)
                {
                   MessageBox.Show("The photo must be smaller than 10mb!", "Warning");
                }
                else
                {
                  imagePictureBox.Image = Image.FromFile(open_dialog.FileName);
                }
               
            }
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Bitmap Image|*.bmp";

            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                  bmp.Save(save_dialog.FileName, ImageFormat.Bmp);          
            }

        }
        

      
       
    }
} 