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
        byte[] vectors;
        byte[][] vectorsFromParameters;
        byte[] arrey;
        int[] tableWitColorSum;
        int[][] ParametertableWitColorSum;

        public enum TypeConvert { Bright, RGB };
        bool UseTables;
        TypeConvert typeConvert;
        public volatile float LimitMultitudesForThis = 1;
        public ImageToCompare()
        {

        }
        public ImageToCompare(Bitmap bitmap, Size size, TypeConvert typeConvert, List<GraphicProcesing.Parameters> parameters, bool UseAvx = true)
        {
            this.UseTables = UseAvx;
            this.typeConvert = typeConvert;

            if (parameters != null)
            {
                vectorsFromParameters = new byte[parameters.Count][];
                ParametertableWitColorSum = new int[parameters.Count][];
            }
            GraphicParameters = parameters;
            Load(bitmap, size, UseAvx);
        }

        private void Load(Bitmap bitmap, Size size, bool UseAvx)
        {
            Bitmap toDispose;
            if (UseAvx)
            {
                vectors = LoadVector(toDispose = new Bitmap(bitmap, size), size, out tableWitColorSum);
                if (GraphicParameters != null)
                {
                    Bitmap bitmap1 = new Bitmap(toDispose);
                    for (int i = 0; i < GraphicParameters.Count; i++)
                    {
                        var curPr = GraphicParameters[i];
                        GraphicProcesing.BasicEditing4Parameter(bitmap1, curPr.Exposition, curPr.Saturation, curPr.Contrast, curPr.Temperature, curPr.tint);
                        vectorsFromParameters[i] = LoadVector(bitmap1, size, out ParametertableWitColorSum[i]);
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
        private byte[] LoadVector(Bitmap bitmap, Size size, out int[] outTableWithSumes)
        {
            switch (typeConvert)
            {
                case TypeConvert.Bright:
                    outTableWithSumes = new int[1];
                    return ImageOperation.LoadMono(bitmap);
                case TypeConvert.RGB:
                    return ImageOperation.LoadRGBWitTTable(bitmap, out outTableWithSumes);
            }
            throw new NotImplementedException("Unknown type convert: " + typeConvert);

        }
        public long GetDifrent(ImageToCompare image, out GraphicProcesing.Parameters parameters)
        {
            float factorFromParameter = 1;
            parameters = null;
            if (UseTables)
            {
                long minDistance = DifrentRGB(vectors, image.vectors) + DifrentRGB(tableWitColorSum, image.tableWitColorSum);
                if (GraphicParameters != null)
                {
                    for (int i = 0; i < GraphicParameters.Count; i++)
                    {
                        long currentDistance = DifrentRGB(vectorsFromParameters[i], image.vectors) + DifrentRGB(ParametertableWitColorSum[i], image.tableWitColorSum);
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
        private static long DifrentRGB(byte[] vectors, byte[] image)
        {
            long Difrent = 0;
            for (int i = 0; i < vectors.Length; i++)
            {
                Difrent += Math.Abs(vectors[i] - image[i]);

            }
            return Difrent;
        }
        private static long DifrentRGB(int[] vectors, int[] image)
        {
            long Difrent = 0;
            for (int i = 0; i < vectors.Length; i++)
            {
                Difrent += Math.Abs(vectors[i] - image[i]);

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
