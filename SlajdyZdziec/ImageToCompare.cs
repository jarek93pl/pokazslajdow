using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
namespace SlajdyZdziec
{
    public class ImageToCompare
    {
        List<Vector<short>> vectors = new List<Vector<short>>();
        public enum TypeConvert { Bright, RGB };
        TypeConvert typeConvert;
        public ImageToCompare(Bitmap bitmap, Size size, TypeConvert typeConvert)
        {
            this.typeConvert = typeConvert;
            LoadVector(new Bitmap(bitmap, size),size);
        }

        private void LoadVector(Bitmap bitmap, Size size)
        {
            switch (typeConvert)
            {
                case TypeConvert.Bright:
                    break;
                case TypeConvert.RGB:
                    break;
                default:
                    break;
            }
        }
    }
}
