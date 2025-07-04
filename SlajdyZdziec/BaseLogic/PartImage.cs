using SlajdyZdziec.BaseLogic.ThreadHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlajdyZdziec.BaseLogic
{
    public class PartImage
    {
        public int Pos;
        public IntPtr Source;
        int WidthSource;
        public PartImage(IntPtr bitmap, int widthSource, Point point, Size sizeRectangle)
        {
            Source = bitmap;
            WidthSource = widthSource;
            PointInImage = point;
            Rectangle = new Rectangle(new Point(sizeRectangle.Width * point.X, sizeRectangle.Height * point.Y), sizeRectangle);

        }
        public Rectangle Rectangle;
        public Point PointInImage;
        public static Bitmap Staticsource = null;
        public static unsafe explicit operator Bitmap(PartImage part)
        {
            return ImageOperation.LoadBitmap((byte*)part.Source, part.WidthSource, part.Rectangle);

        }
        public static PartImage[] GetPartImageDim(Bitmap source, IntPtr sourcePtr, Size parts, out Size partSize)
        {
            partSize = GetPartSize(source, in parts);
            PartImage[] returned = new PartImage[parts.Width * parts.Height];
            int l = 0;
            for (int i = 0; i < parts.Height; i++)
            {
                for (int j = 0; j < parts.Width; j++)
                {
                    returned[l++] = new PartImage(sourcePtr, source.Width, new Point(j, i), partSize) { Pos = l - 1 };
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
