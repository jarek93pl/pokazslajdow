using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
namespace SlajdyZdziec.BaseLogic.ThreadHelper
{
    public static class CopyToLocal
    {
        [ThreadStatic]
        static Bitmap bitmap;
        [ThreadStatic]
        static Guid Guid;
        public static Bitmap GetLocalCopy(Bitmap b, Guid guid)
        {
            if (guid != Guid || bitmap == null)
            {
                lock (b)
                {
                    bitmap = (Bitmap)b.Clone();
                    Guid = guid;
                }
            }
            return bitmap;
        }
    }
}
