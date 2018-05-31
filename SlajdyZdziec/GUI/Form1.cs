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
using SlajdyZdziec.BaseLogic;
using System.Diagnostics;
using SlajdyZdziec.GUI.ImageInImage;

namespace SlajdyZdziec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<ImageUrl> imageUrls
        {
            get
            {
                return listBox1.Items.OfType<ImageUrl>().ToList();
            }
        }
        private void otwórzWieleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.AddRange(FileHelper.GetFile(new System.IO.DirectoryInfo(folderBrowserDialog1.SelectedPath), SetingProvaider.FileExtension).AsParallel().Select(X => new ImageUrl(X)).ToArray());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is ImageUrl url)
            {
                pictureBox1.Image = url.Bitmap(new Size (300,300));
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<LogicAndImage<ImageToCompare, ImageUrl>> list = new List<LogicAndImage<ImageToCompare, ImageUrl>>();
            list.AddRange(imageUrls.Select(X => new LogicAndImage<ImageToCompare, ImageUrl>()
            { Bitmap = X, Logic = new ImageToCompare(X.Bitmap(new Size(30,30)), new Size(30, 30),true) }));
            (LogicAndImage<ImageToCompare, ImageUrl> Left, LogicAndImage<ImageToCompare, ImageUrl> Right) Record = (null, null);
            long MinDifrent = long.MaxValue;
            Stopwatch stoper = Stopwatch.StartNew();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j)
                    {
                        var curent = (list[i], list[j]);
                        long CurentDifrent = ImageToCompare.GetDifrent(curent);
                        if (CurentDifrent < MinDifrent)
                        {
                            MinDifrent = CurentDifrent;
                            Record = curent;
                        }
                    }
                }
            }

            MessageBox.Show($"{stoper.ElapsedMilliseconds} {Record.Left.Bitmap.file.FullName}\n {Record.Right.Bitmap.file.FullName}");
        }

        private void calsageWitchPhotosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColageImageForm clif = new ColageImageForm(imageUrls);
            clif.ShowDialog();
        }
    }
}
