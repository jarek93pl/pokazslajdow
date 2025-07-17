using SlajdyZdziec.BaseLogic;
using SlajdyZdziec.BaseLogic.ThreadHelper;
using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SlajdyZdziec.ImagesInImage
{
    public static class Dispatcher
    {
        [Conditional("DEBUG")]
        public static void WriteTimeForDebug(string text)
        {
            System.Diagnostics.Debug.WriteLine($"{text} {stopwatch.ElapsedMilliseconds}");
        }
        static long AdjustedImage = 0, AllImageForAdjust, AllImageToPrint, PrintedImage;
        public static void NexAdjustedImage()
        {
            long diver = AllImageForAdjust / 100;
            diver = diver == 0 ? 1 : diver;
            Interlocked.Increment(ref AdjustedImage);
            if (AdjustedImage % diver == 0)
            {
                Console.WriteLine($"Adjusted {AdjustedImage} of {AllImageForAdjust} images {stopwatch.Elapsed}");
            }
        }
        public static void NexPrintedImage()
        {
            long diver = AllImageToPrint / 100;
            diver = diver == 0 ? 1 : diver;
            Interlocked.Increment(ref PrintedImage);
            if (PrintedImage % diver == 0)
            {
                Console.WriteLine($"PrintedImage {PrintedImage} of {AllImageToPrint} images {stopwatch.Elapsed}");
            }
        }

        static System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        private unsafe static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImageInOut, Size SizeToCompare, Func<LogicAndImage<ImageToCompare, PartImage>, Func<Bitmap>> PartImageFunc)
        {
            Size partSizeInImage;
            IntPtr sourcePoirter = (IntPtr)ImageOperation.LoadRGB(image, out int sizeOfImage);
            PartImage[] partImage = PartImage.GetPartImageDim(image, sourcePoirter, partsDim, out partSizeInImage);
            Dictionary<PartImage, Func<Bitmap>> BitmapDictionary = new Dictionary<PartImage, Func<Bitmap>>();
            WriteTimeForDebug("heder");
            var part1 = new LogicAndImage<ImageToCompare, PartImage>[partImage.Length];
            AllImageForAdjust = partImage.Length;
            WriteTimeForDebug("Get part procesed");

            partImage.AsParallel().WithDegreeOfParallelism(16).ForAll(X =>
            {//[pobieranie fragmentów
                part1[X.Pos] = new LogicAndImage<ImageToCompare, PartImage>()
                {
                    Bitmap = X,
                    Logic = new ImageToCompare((Bitmap)X, SizeToCompare, null, 1)
                };
            }
               );
            WriteTimeForDebug("Get part procesed");
            List<(LogicAndImage<ImageToCompare, PartImage> part, Func<Bitmap> bitmapFunc)> x = part1
#if ! DEBUG
                .AsParallel()
#endif
                .Select(X => (X, PartImageFunc(X))).ToList();//obliczenie najbarddziej podobnych obrazów


            WriteTimeForDebug("comparing");
            x.ForEach(X => BitmapDictionary.Add(X.part.Bitmap, X.bitmapFunc));
            SaveAsXml.Save(x.Select(X => X.part).ToList());
            Bitmap zw = MargeImage(partsDim, SizePartImageInOut, partSizeInImage, partImage, BitmapDictionary);

            WriteTimeForDebug("merge");
            Marshal.FreeHGlobal(sourcePoirter);
            return zw;
        }
        private static Bitmap MargeImage(Size partsDim, Size SizePartImageInOut, Size partSizeInImage, PartImage[] partImage, Dictionary<PartImage, Func<Bitmap>> BitmapDictionary)
        {
            try
            {
                AllImageToPrint = partImage.Length;
                Bitmap zw = new Bitmap(SizePartImageInOut.Width * partsDim.Width, SizePartImageInOut.Height * partsDim.Height);
                using (Graphics graphic = Graphics.FromImage(zw))
                {
                    foreach (var item in partImage)
                    {
                        graphic.DrawImage(BitmapDictionary[item](), new Rectangle(new Point(item.PointInImage.X * SizePartImageInOut.Width, item.PointInImage.Y * SizePartImageInOut.Height), SizePartImageInOut));
                        NexPrintedImage();
                    }
                }

                return zw;
            }
            catch (System.ArgumentException)
            {

                throw new Exception("picture is to large");
            }
        }

        public static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImageInOut, Size SizeToCompare, List<ImageUrl> imageUrls)
        {
            List<LogicAndImage<ImageToCompare, ImageUrl>> list = new List<LogicAndImage<ImageToCompare, ImageUrl>>();

            Func<Bitmap> Geter(LogicAndImage<ImageToCompare, PartImage> arg)
            {
                GraphicProcesing.Parameters parametersToEdit = null;
                long MinDifrent = long.MaxValue;
                LogicAndImage<ImageToCompare, ImageUrl> Best = null;
                foreach (var item in list)
                {
                    GraphicProcesing.Parameters currentparametersToEdit = null;
                    long CurentDistance;
                    if (MinDifrent > (CurentDistance = item.Logic.GetDifrent(arg.Logic, out currentparametersToEdit)))
                    {
                        if (item.Bitmap.CanApplyForThis(arg.Bitmap))
                        {
                            Best = item;
                            MinDifrent = CurentDistance;
                            parametersToEdit = currentparametersToEdit;
                        }
                    }

                }
                NexAdjustedImage();
                arg.BestResult = Best;
                arg.Parameters = parametersToEdit;
                arg.Difrence = MinDifrent;
                if (MinDifrent < 0)
                {
                    throw new Exception("images or parameter are to larger, ");
                }
                if (parametersToEdit == null)
                {
                    return () => Best.Bitmap.Bitmap(SizePartImageInOut);
                }
                else
                {
                    return () =>
                    {
                        var btimap = Best.Bitmap.Bitmap(SizePartImageInOut);
                        GraphicProcesing.BasicEditing4Parameter(btimap, parametersToEdit.Exposition, parametersToEdit.Saturation, parametersToEdit.Contrast);
                        return btimap;
                    };
                }
            }

            list.AddRange(imageUrls.AsParallel().Select(X => new LogicAndImage<ImageToCompare, ImageUrl>()
            {
                Bitmap = X,
                Logic = new ImageToCompare(X.Bitmap(SizeToCompare),
                SizeToCompare, X.GraphicParameters, 1, true)
                {
                    CompareFactor = X.factorTocompare

                }
            }));
            WriteTimeForDebug("LoadImages option from folder");
            return GetMultiImage(image, partsDim, SizePartImageInOut, SizeToCompare, Geter);
        }


    }
}
