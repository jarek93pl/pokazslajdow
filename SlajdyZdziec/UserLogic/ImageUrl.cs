using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using SlajdyZdziec.BaseLogic;

namespace SlajdyZdziec.UserLogic
{
    public class ImageUrl
    {

        public List<GraphicProcesing.Parameters> GraphicParameters { get; set; }
        public float factorTocompare = 1.0f;
        public FileInfo file;
        public float PropabilityAccept = 1;
        public ImageUrl(FileInfo file)
        {
            this.file = file;
        }
        public Bitmap Bitmap(Size s)
        {
            return new Bitmap(new Bitmap(file.FullName), s);

        }
        public override string ToString()
        {
            return file.Name;
        }

        internal bool CanApplyForThis(PartImage bitmap)
        {
            return StaticRander.GetNumber(bitmap.PointInImage.X, bitmap.PointInImage.Y) < PropabilityAccept;
        }
    }
}
