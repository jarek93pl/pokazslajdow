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
        byte[] arrey;
        public enum TypeConvert { Bright, RGB };
        bool UseAvx;
        TypeConvert typeConvert;
        public ImageToCompare()
        {

        }
        public ImageToCompare(Bitmap bitmap, Size size, TypeConvert typeConvert, bool UseAvx = true)
        {
            this.UseAvx = UseAvx;
            this.typeConvert = typeConvert;
            Load(bitmap, size, UseAvx);
        }

        private void Load(Bitmap bitmap, Size size, bool UseAvx)
        {
            Bitmap toDispose;
            if (UseAvx)
            {
                LoadVector(toDispose = new Bitmap(bitmap, size), size);
            }
            else
            {
                Load(toDispose = new Bitmap(bitmap, size), size);
            }
            toDispose.Dispose();
        }

        public ImageToCompare(Bitmap bitmap, Size size, bool UseAvx = true) : this(bitmap, size, TypeConvert.RGB, UseAvx)
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
        private void Load(Bitmap bitmap, Size size)
        {
            switch (typeConvert)
            {
                case TypeConvert.Bright:
                    arrey = ImageOperation.LoadMono(bitmap);
                    break;
                case TypeConvert.RGB:
                    arrey = ImageOperation.LoadRGB(bitmap);
                    break;
                default:
                    break;
            }
        }
        public long GetDifrent(ImageToCompare image)
        {
            if (UseAvx)
            {
                return DifrentAvx(image);
            }
            else
            {
                return DifrentArrey(image);
            }
        }
        private long DifrentArrey(ImageToCompare image)
        {
            long Difrent = 0;
            byte[] toComper = image.arrey;
            for (int i = 0; i < arrey.Length; i++)
            {
                Difrent += Math.Abs(arrey[i] - toComper[i]);
            }
            return Difrent;
        }
        private long DifrentAvx(ImageToCompare image)
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
            }
            return Difrent;
        }

        public static long GetDifrent<T>(LogicAndImage<ImageToCompare, T> Left, LogicAndImage<ImageToCompare, T> Right)
            => Left.Logic.GetDifrent(Right.Logic);

        internal static long GetDifrent<T>((LogicAndImage<ImageToCompare, T>, LogicAndImage<ImageToCompare, T>) curent)
            => GetDifrent(curent.Item1, curent.Item2);
    }
}
