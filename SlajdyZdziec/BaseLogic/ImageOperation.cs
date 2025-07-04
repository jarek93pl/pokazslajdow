using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace SlajdyZdziec.BaseLogic
{
    public unsafe static class ImageOperation
    {
        public static byte* LoadMono(Bitmap Obraz, out int Size)
        {
            IntPtr mr = Marshal.AllocHGlobal(Size = (Obraz.Width * Obraz.Height));
            byte* obsugiwana = (byte*)mr;

            int j = 0;
            BitmapData bp = Obraz.LockBits(new Rectangle(0, 0, Obraz.Width, Obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            for (int y = 0; y < Obraz.Height; y++)
            {

                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                for (int x = 0; x < Obraz.Width; x++, kr++, obsugiwana++)
                {
                    j = (*kr).r;
                    j += (*kr).g;
                    j += (*kr).b;
                    byte zw = ((byte)(j / 3));
                    *obsugiwana = zw;
                }
            }
            Obraz.UnlockBits(bp);
            return (byte*)mr;
        }
        public static byte* LoadRGB(Bitmap Obraz, out int Size)
        {
            IntPtr mr = Marshal.AllocHGlobal(Size = (Obraz.Width * Obraz.Height * 3));
            byte* obsugiwana = (byte*)mr;

            BitmapData bp = Obraz.LockBits(new Rectangle(0, 0, Obraz.Width, Obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            for (int y = 0; y < Obraz.Height; y++)
            {

                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                for (int x = 0; x < Obraz.Width; x++, kr++)
                {
                    *(obsugiwana++) = (*kr).r;
                    *(obsugiwana++) = (*kr).g;
                    *(obsugiwana++) = (*kr).b;
                }
            }
            Obraz.UnlockBits(bp);
            return (byte*)mr;
        }

        public static Bitmap LoadBitmap(byte* Obraz, int widthSource, Rectangle rectangle)
        {
            Bitmap newImage = new Bitmap(rectangle.Size.Width, rectangle.Size.Height);
            BitmapData bp = newImage.LockBits(new Rectangle(0, 0, newImage.Width, newImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            for (int y = 0; y < newImage.Height; y++)
            {
                byte* place = (Obraz + ((widthSource * (y + rectangle.Y)) + rectangle.X) * 3);
                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                rgb* inTo = (rgb*)place;
                for (int x = 0; x < newImage.Width; x++, kr++, inTo++)
                {
                    *kr = *inTo;
                }
            }
            newImage.UnlockBits(bp);
            return newImage;
        }

        public static byte[] LoadMono(Bitmap Obraz)
        {
            byte[] table = new byte[(Obraz.Width * Obraz.Height)];
            int obsugiwana = 0;

            int j = 0;
            BitmapData bp = Obraz.LockBits(new Rectangle(0, 0, Obraz.Width, Obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            for (int y = 0; y < Obraz.Height; y++)
            {

                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                for (int x = 0; x < Obraz.Width; x++, kr++, obsugiwana++)
                {
                    j = (*kr).r;
                    j += (*kr).g;
                    j += (*kr).b;
                    byte zw = ((byte)(j / 3));
                    table[obsugiwana] = zw;
                }
            }
            Obraz.UnlockBits(bp);
            return table;
        }
        public static byte[] LoadRGB(Bitmap Obraz)
        {
            byte[] table = new byte[(Obraz.Width * Obraz.Height) * 3];
            int obsugiwana = 0;

            int j = 0;
            BitmapData bp = Obraz.LockBits(new Rectangle(0, 0, Obraz.Width, Obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            for (int y = 0; y < Obraz.Height; y++)
            {

                rgb* kr = (rgb*)((byte*)(bp.Scan0 + y * bp.Stride));
                for (int x = 0; x < Obraz.Width; x++, kr++)
                {
                    table[obsugiwana++] = (*kr).r;
                    table[obsugiwana++] = (*kr).g;
                    table[obsugiwana++] = (*kr).b;
                }
            }
            Obraz.UnlockBits(bp);
            return table;
        }
        public static IEnumerable<Vector<short>> GetVector(byte[] imageInByte)
        {
            List<short> vs = new List<short>();
            vs.AddRange(imageInByte.Select(X => (short)X));
            int Numer = imageInByte.Length / Vector<short>.Count;
            Numer += imageInByte.Length % Vector<short>.Count == 0 ? 0 : 1;
            int Size = Numer * Vector<short>.Count;
            for (int j = imageInByte.Length; j < Size; j++)
            {
                vs.Add(0);
            }
            short[] arrey = vs.ToArray();
            for (int i = 0; i < Numer; i++)
            {
                yield return new Vector<short>(arrey, i * Vector<short>.Count);
            }
        }
    }
}
