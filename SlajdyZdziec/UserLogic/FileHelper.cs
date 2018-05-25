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
    }
}
