﻿using SlajdyZdziec.BaseLogic;
using SlajdyZdziec.BaseLogic.Order;
using SlajdyZdziec.BaseLogic.ThreadHelper;
using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlajdyZdziec.ImagesInImage
{
    public static class Dispatcher
    {

        public static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImageInOut, Size SizeToCompare, Func<LogicAndImage<ImageToCompare, PartImage>, Func<Bitmap>> PartImageFunc, IOrderDispatcher<PartImage> parts)
        {
            Size partSizeInImage;
            PartImage[] partImage = PartImage.GetPartImageDim(image, partsDim, out partSizeInImage);
            Dictionary<PartImage, Func<Bitmap>> BitmapDictionary = new Dictionary<PartImage, Func<Bitmap>>();

            List<(LogicAndImage<ImageToCompare, PartImage> part, Func<Bitmap> bitmapFunc)> x =
                (parts.GetItemWitchOrder(partsDim, partImage).AsParallel().Select(X =>//[pobieranie fragmentów
                new LogicAndImage<ImageToCompare, PartImage>()
                {
                    Bitmap = X,
                    Logic = new ImageToCompare((Bitmap)X, SizeToCompare)
                }
                ).ToList().AsParallel().// Dołączenie do fragmentów danych o wektorach
                Select(X => (X, PartImageFunc(X))).ToList());//obliczenie najbarddziej podobnych obrazów


            x.ForEach(X => BitmapDictionary.Add(X.part.Bitmap, X.bitmapFunc));
            Bitmap zw = MargeImage(partsDim, SizePartImageInOut, partSizeInImage, partImage, BitmapDictionary);
            return zw;
        }
        private static Bitmap MargeImage(Size partsDim, Size SizePartImageInOut, Size partSizeInImage, PartImage[] partImage, Dictionary<PartImage, Func<Bitmap>> BitmapDictionary)
        {
            Bitmap zw = new Bitmap(partSizeInImage.Width * partsDim.Width, partSizeInImage.Height * partsDim.Height);
            using (Graphics graphic = Graphics.FromImage(zw))
            {
                foreach (var item in partImage)
                {
                    graphic.DrawImage(BitmapDictionary[item](), new Rectangle(new Point(item.PointInImage.X * SizePartImageInOut.Width, item.PointInImage.Y * SizePartImageInOut.Height), SizePartImageInOut));
                }
            }

            return zw;
        }

        public static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImageInOut, Size SizeToCompare, List<ImageUrl> imageUrls, IOrderDispatcher<PartImage> parts)
        {
            List<LogicAndImage<ImageToCompare, ImageUrl>> list = new List<LogicAndImage<ImageToCompare, ImageUrl>>();

            Func<Bitmap> Geter(LogicAndImage<ImageToCompare, PartImage> arg)
            {
                long MinDifrent = long.MaxValue;
                LogicAndImage<ImageToCompare, ImageUrl> Best = null;
                foreach (var item in list)
                {
                    long CurentDistance;
                    if (MinDifrent > (CurentDistance = item.Logic.GetDifrent(arg.Logic)))
                    {
                        Best = item;
                        MinDifrent = CurentDistance;
                    }

                }
                return () => Best.Bitmap.Bitmap(SizePartImageInOut);
            }

            list.AddRange(imageUrls.AsParallel().Select(X => new LogicAndImage<ImageToCompare, ImageUrl>()
            { Bitmap = X, Logic = new ImageToCompare(X.Bitmap(SizeToCompare), SizeToCompare, true) }));
            return GetMultiImage(image, partsDim, SizePartImageInOut, SizeToCompare, Geter, new BasicOrder<PartImage>());
        }


    }
}
