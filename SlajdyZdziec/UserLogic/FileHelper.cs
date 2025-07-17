using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SlajdyZdziec.UserLogic
{
    public static class FileHelper
    {
        public static IEnumerable<FileInfo> GetFile(DirectoryInfo dr, string searchPatern)
        {
            foreach (var item in dr.GetFiles(searchPatern))
            {
                yield return item;
            }
            foreach (var item in dr.GetDirectories())
            {
                foreach (var item2 in GetFile(item, searchPatern))
                {
                    yield return item2;
                }
            }
        }

        public static float floatReader(string text, float min = -1, float max = 5)
        {
            float returned = float.Parse(text);
            string[] split = text.Split(',');
            if (split.Length == 2)
            {
                returned = GetValue(split[0], split[1]);
            }
            else
            {
                split = text.Split('.');
                if (split.Length == 2)
                {
                    returned = GetValue(split[0], split[1]);
                }
            }
            if (returned < min || returned > max)
            {
                throw new Exception($"vrong value {text}");
            }
            return returned;

        }
        private static float GetValue(string f1, string f2)
        {
            float vMain = float.Parse(f1);
            double f2Value = float.Parse(f2);

            if (f1[0] == '-')
            {
                f2Value *= -1;
            }
            f2Value = f2Value / Math.Pow(10, f2.Length);
            return vMain + (float)f2Value;
           
        }

    }
}
