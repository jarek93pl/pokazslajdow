using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using SlajdyZdziec.UserLogic;

namespace SlajdyZdziec.BaseLogic
{
    public class ImageToCompare
    {
        List<Vector<short>> vectors = new List<Vector<short>>();
        public enum TypeConvert { Bright, RGB };
        TypeConvert typeConvert;
        public ImageToCompare(Bitmap bitmap, Size size, TypeConvert typeConvert)
        {
            this.typeConvert = typeConvert;
            LoadVector(new Bitmap(bitmap, size), size);
        }
        public ImageToCompare(Bitmap bitmap, Size size) : this(bitmap, size, TypeConvert.Bright)
        {
        }
        private void LoadVector(Bitmap bitmap, Size size)
        {
            switch (typeConvert)
            {
                case TypeConvert.Bright:
                    vectors.AddRange(ImageOperation.GetVector(ImageOperation.LoadMono(bitmap)));
                    break;
                case TypeConvert.RGB:
                    vectors.AddRange(ImageOperation.GetVector(ImageOperation.LoadRGB(bitmap)));
                    break;
                default:
                    break;
            }
        }
        public long GetDifrent(ImageToCompare image)
        {
            long Difrent = 0;
            int i = 0;
            List<Vector<short>> vectorsToCompare = image.vectors;
            while (i < vectors.Count)
            {
                Vector<short> AgregateSum = Vector<short>.Zero;
                for (int j = 0; j < 254 && i < vectors.Count; j++, i++)
                {
                    AgregateSum += Vector.Abs((vectors[i] - vectorsToCompare[i]));
                }

                for (int j = 0; j < Vector<short>.Count; j++)
                {
                    Difrent += AgregateSum[j];
                }
                vectorsToCompare = image.vectors;
            }
            return Difrent;
        }
        public static long GetDifrent<T>(LogicAndImage<ImageToCompare, T> Left, LogicAndImage<ImageToCompare, T> Right) 
            => Left.Logic.GetDifrent(Right.Logic);

        internal static long GetDifrent<T>((LogicAndImage<ImageToCompare, T>, LogicAndImage<ImageToCompare, T>) curent)
            => GetDifrent(curent.Item1, curent.Item2);
    }
}
