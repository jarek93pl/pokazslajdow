using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlajdyZdziec.BaseLogic
{
    public class PartImage
    {
        public PartImage(Bitmap bitmap, Point point, Size sizeRectangle)
        {
            Source = bitmap;
            PointInImage = point;
            Rectangle = new Rectangle(new Point(sizeRectangle.Width * point.X, sizeRectangle.Height * point.Y), sizeRectangle);

        }
        public Rectangle Rectangle;
        public Point PointInImage;
        Bitmap Source;
        public static explicit operator Bitmap(PartImage part)
        {
            lock (part.Source)
            {
                return part.Source.Clone(part.Rectangle, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            }
        }
        public static PartImage[] GetPartImageDim(Bitmap source, Size parts, out Size partSize)
        {
            partSize = GetPartSize(source, in parts);
            PartImage[] returned = new PartImage[parts.Width * parts.Height];
            int l = 0;
            for (int i = 0; i < parts.Height; i++)
            {
                for (int j = 0; j < parts.Width; j++)
                {
                    returned[l++] = new PartImage(source, new Point(j, i), partSize);
                }
            }
            return returned;
        }

        public static Size GetPartSize(Bitmap source, in Size parts)
        {
            return new Size(source.Width / parts.Width, source.Height / parts.Height);
        }
    }
}
