using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlajdyZdziec.BaseLogic
{
    public static class GraphicProcesing
    {

        public class Parameters
        {
            internal int tint;

            public float Exposition { get; set; } = 1.0f;
            public float Saturation { get; set; } = 1.0f;
            public float Contrast { get; set; } = 0;
            public int Temperature { get; set; } = 0;
            public float CostOfEditing { get; internal set; }
        }
        public static unsafe void BasicEditing4Parameter(Bitmap Obraz, float exposytion, float saturaion, float contrast, int temperature, int tint)
        {
            long avg = 0;
            BitmapData bp = Obraz.LockBits(new Rectangle(0, 0, Obraz.Width, Obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            ComputeAvg(Obraz, bp, ref avg);
            for (int y = 0; y < Obraz.Height; y++)
            {

                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                for (int x = 0; x < Obraz.Width; x++, kr++)
                {
                    int avgPixel = 0;
                    avgPixel = (*kr).r;
                    avgPixel += (*kr).g;
                    avgPixel += (*kr).b;
                    avgPixel /= 3;
                    float sumFromContrast = (avgPixel - avg) * contrast;
                    float r = (*kr).r;
                    float g = (*kr).g;
                    float b = (*kr).b;
                    r += -((*kr).r - avgPixel) + ((*kr).r - avgPixel) * saturaion;
                    g += -((*kr).g - avgPixel) + ((*kr).g - avgPixel) * saturaion;
                    b += -((*kr).b - avgPixel) + ((*kr).b - avgPixel) * saturaion;
                    r += sumFromContrast - temperature - tint;
                    g += sumFromContrast + (tint << 1);
                    b += sumFromContrast + temperature - tint;
                    r *= exposytion;
                    g *= exposytion;
                    b *= exposytion;
                    if (r < 0) { r = 0; }
                    if (g < 0) { g = 0; }
                    if (b < 0) { b = 0; }
                    if (r > 255) { r = 255; }
                    if (g > 255) { g = 255; }
                    if (b > 255) { b = 255; }
                    (*kr).r = Convert.ToByte(r);
                    (*kr).g = Convert.ToByte(g);
                    (*kr).b = Convert.ToByte(b);

                }
            }


            Obraz.UnlockBits(bp);

        }

        private static unsafe void ComputeAvg(Bitmap Obraz, BitmapData bp, ref long j)
        {
            for (int y = 0; y < Obraz.Height; y++)
            {

                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                for (int x = 0; x < Obraz.Width; x++, kr++)
                {
                    j = (*kr).r;
                    j += (*kr).g;
                    j += (*kr).b;
                }
            }
            j /= (Obraz.Width * Obraz.Height * 3);
        }

        struct rgb
        {
            public byte r, g, b;
        }
    }
}
