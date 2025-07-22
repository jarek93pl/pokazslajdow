using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using SlajdyZdziec.UserLogic;
using static SlajdyZdziec.BaseLogic.GraphicProcesing;

namespace SlajdyZdziec.BaseLogic
{
    public class ImageToCompare
    {
        public float CompareFactorFromGroup;
        public List<GraphicProcesing.Parameters> GraphicParameters { get; set; }
        List<Vector<short>> vectors = new List<Vector<short>>();
        List<Vector<short>>[] vectorsFromParameters;
        byte[] arrey;
        public enum TypeConvert { Bright, RGB };
        bool UseAvx;
        TypeConvert typeConvert;
        public volatile float LimitMultitudesForThis = 1;
        public ImageToCompare()
        {

        }
        public ImageToCompare(Bitmap bitmap, Size size, TypeConvert typeConvert, List<GraphicProcesing.Parameters> parameters, bool UseAvx = true)
        {
            this.UseAvx = UseAvx;
            this.typeConvert = typeConvert;

            if (parameters != null)
            {
                vectorsFromParameters = new List<Vector<short>>[parameters.Count];
            }
            GraphicParameters = parameters;
            Load(bitmap, size, UseAvx);
        }

        private void Load(Bitmap bitmap, Size size, bool UseAvx)
        {
            Bitmap toDispose;
            if (UseAvx)
            {
                LoadVector(toDispose = new Bitmap(bitmap, size), size, ref vectors);
                if (GraphicParameters != null)
                {
                    Bitmap bitmap1 = new Bitmap(toDispose);
                    for (int i = 0; i < GraphicParameters.Count; i++)
                    {
                        vectorsFromParameters[i] = new List<Vector<short>>();
                        var curPr = GraphicParameters[i];
                        GraphicProcesing.BasicEditing4Parameter(bitmap1, curPr.Exposition, curPr.Saturation, curPr.Contrast);
                        LoadVector(bitmap1, size, ref vectorsFromParameters[i]);
                    }
                    bitmap1.Dispose();
                }
            }
            else
            {
                throw new NotImplementedException();
                // Load(toDispose = new Bitmap(bitmap, size), size);
            }
            toDispose.Dispose();
        }

        public ImageToCompare(Bitmap bitmap, Size size, List<GraphicProcesing.Parameters> parameters, float compareFactor, bool UseAvx = true) : this(bitmap, size, TypeConvert.RGB, parameters, UseAvx)
        {
            CompareFactorFromGroup = compareFactor;
        }
        private void LoadVector(Bitmap bitmap, Size size, ref List<Vector<short>> vectors)
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

            if (vectors.Count > 254)
            {
                throw new Exception("image to compare large");
            }
        }
        public long GetDifrent(ImageToCompare image, out GraphicProcesing.Parameters parameters)
        {
            float factorFromParameter = 1;
            parameters = null;
            if (UseAvx)
            {
                long minDistance = DifrentAvx(vectors, image.vectors);
                if (GraphicParameters != null)
                {
                    for (int i = 0; i < GraphicParameters.Count; i++)
                    {
                        long currentDistance = DifrentAvx(vectorsFromParameters[i], image.vectors);
                        if (currentDistance < minDistance)
                        {
                            parameters = GraphicParameters[i];
                            minDistance = currentDistance;
                            factorFromParameter = GraphicParameters[i].CostOfEditing;
                        }
                    }
                }
                return Convert.ToInt64(minDistance * CompareFactorFromGroup * LimitMultitudesForThis);

            }
            else
            {
                throw new NotImplementedException("Difrent for arrey not implemented");
                //return DifrentArrey(image);
            }
        }
        private static long DifrentAvx(List<Vector<short>> vectors, List<Vector<short>> image)
        {
            long Difrent = 0;
            int i = 0;
            while (i < vectors.Count)
            {
                Vector<short> AgregateSum = Vector<short>.Zero;
                for (int j = 0; j < 254 && i < vectors.Count; j++, i++)
                {
                    AgregateSum += Vector.Abs((vectors[i] - image[i]));
                }

                for (int j = 0; j < Vector<short>.Count; j++)
                {
                    Difrent += AgregateSum[j];
                }
            }
            return Difrent;
        }

        public static long GetDifrent<T>(LogicAndImage<ImageToCompare, T> Left, LogicAndImage<ImageToCompare, T> Right)
            => throw new NotImplementedException();

        internal static long GetDifrent<T>((LogicAndImage<ImageToCompare, T>, LogicAndImage<ImageToCompare, T>) curent)
            => GetDifrent(curent.Item1, curent.Item2);

        internal void UseFactorLimiting(float factorLimiting)
        {
            lock (this)
            {
                LimitMultitudesForThis *= factorLimiting;
            }
        }
    }
}
