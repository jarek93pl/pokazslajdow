using SlajdyZdziec.BaseLogic;
using SlajdyZdziec.BaseLogic.Order;
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

        public static Bitmap GetMultiImage(Bitmap image, Size partsDim, Size SizePartImage, Func<PartImage, Func<Bitmap>> PartImageFunc, IOrderDispatcher<PartImage> parts)
        {
            Size partSizeInImage;
            PartImage[] partImage = PartImage.GetPartImageDim(image, partsDim, out partSizeInImage);
            Dictionary<PartImage, Func<Bitmap>> BitmapDictionary = new Dictionary<PartImage, Func<Bitmap>>(partImage.Length);
            foreach (var item in parts.GetItemWitchOrder(partsDim, partImage))
            {
                BitmapDictionary.Add(item, PartImageFunc(item));
            }
            Bitmap zw = new Bitmap(partSizeInImage.Width * partsDim.Width, partSizeInImage.Height * partsDim.Height);
            using (Graphics graphic = Graphics.FromImage(zw))
            {
                foreach (var item in partImage)
                {
                    graphic.DrawImage(BitmapDictionary[item](), new Rectangle(new Point(item.PointInImage.X * SizePartImage.Width, item.PointInImage.Y * SizePartImage.Height), SizePartImage));
                }
            }
            return zw;
        }
    }
}
