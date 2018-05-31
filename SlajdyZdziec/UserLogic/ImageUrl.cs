using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace SlajdyZdziec.UserLogic
{
    public class ImageUrl
    {
        public FileInfo file;
        public ImageUrl(FileInfo file)
        {
            this.file = file;
        }
        public Bitmap Bitmap(Size s)
        {
            return new Bitmap(new Bitmap(file.FullName), s);

        }
        public override string ToString()
        {
            return file.Name;
        }
    }
}
