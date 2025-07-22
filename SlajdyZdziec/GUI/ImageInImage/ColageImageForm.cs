using SlajdyZdziec.ImagesInImage;
using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlajdyZdziec.GUI.ImageInImage
{
    public partial class ColageImageForm : Form
    {
        Bitmap bitmap;
        public ColageImageForm()
        {
            InitializeComponent();
        }
        List<ImageUrl> urls;
        public ColageImageForm(List<ImageUrl> urls) : this()
        {
            this.urls = urls;
        }

        private void poleVektor1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = new Bitmap(bitmap, pictureBox1.Width, pictureBox1.Height);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("This method is not implemented yet.");
            //Dispatcher.GetMultiImage(bitmap, projection.WartośćSize, SizePartImage.WartośćSize,SizeToCompare.WartośćSize, urls).Save("t.jpg");
            //Process.Start("t.jpg");
        }

        private void poleVektor1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
