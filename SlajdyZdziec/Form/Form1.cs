using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlajdyZdziec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public IEnumerable<ImageUrl> imageUrls
        {
            get
            {
                return (IEnumerable<ImageUrl>)listBox1.Items;
            }
        }
        private void otwórzWieleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.AddRange(FileHelper.GetFile(new System.IO.DirectoryInfo(folderBrowserDialog1.SelectedPath), SetingProvaider.FileExtension).Select(X => new ImageUrl(X)).ToArray());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is ImageUrl url)
            {
                pictureBox1.Image = url.Bitmap;
            }
        }
    }
}
