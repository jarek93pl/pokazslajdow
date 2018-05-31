using SlajdyZdziec.BaseLogic;
using SlajdyZdziec.BaseLogic.Order;
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

        public static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImageInOut, Func<PartImage, Func<Bitmap>> PartImageFunc, IOrderDispatcher<PartImage> parts)
        {
            Size partSizeInImage;
            PartImage[] partImage = PartImage.GetPartImageDim(image, partsDim, out partSizeInImage);
            Dictionary<PartImage, Func<Bitmap>> BitmapDictionary = new Dictionary<PartImage, Func<Bitmap>>();
            List<(PartImage part, Func<Bitmap> bitmapFunc)> x = 
                (parts.GetItemWitchOrder(partsDim, partImage).ToList().AsParallel().Select(X => (X, PartImageFunc(X))).ToList());
            x.ForEach(X => BitmapDictionary.Add(X.part, X.bitmapFunc));
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
        public static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImageInOut, List<ImageUrl> imageUrls, IOrderDispatcher<PartImage> parts)
        {
            Size sizePart = PartImage.GetPartSize(image, partsDim);
            List<LogicAndImage<ImageToCompare, ImageUrl>> list = new List<LogicAndImage<ImageToCompare, ImageUrl>>();

            Func<Bitmap> Geter(PartImage arg)
            {
                long MinDifrent = long.MaxValue;
                LogicAndImage<ImageToCompare, ImageUrl> Best = null;
                ImageToCompare img = new ImageToCompare((Bitmap)arg, sizePart);
                foreach (var item in list)
                {
                    long CurentDistance;
                    if (MinDifrent > (CurentDistance = item.Logic.GetDifrent(img)))
                    {
                        Best = item;
                        MinDifrent = CurentDistance;
                    }

                }
                return () => Best.Bitmap.Bitmap(SizePartImageInOut);
            }

            list.AddRange(imageUrls.AsParallel().Select(X => new LogicAndImage<ImageToCompare, ImageUrl>()
            { Bitmap = X, Logic = new ImageToCompare(X.Bitmap(sizePart), sizePart, true) }));
            return GetMultiImage(image, partsDim, SizePartImageInOut, Geter, new BasicOrder<PartImage>());
        }


    }
}
