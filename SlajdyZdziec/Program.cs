using SlajdyZdziec.ImagesInImage;
using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlajdyZdziec
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Console.WriteLine("use parameter to get config");
            Console.WriteLine("number of retangle x");
            Console.WriteLine("number of retangle y");
            Console.WriteLine("size retangle out x");
            Console.WriteLine("size retangle out Y");
            Console.WriteLine("size to compare x");
            Console.WriteLine("size to compare y");
            args = new string[] { @"E:\OneDrive\Pulpit\fb activity\2025.07.03  MozaicApp\Screenshot 2025-07-03 151825.png" };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new Form1());
            }
            else
            {
                bool duringLoadData = true;
                string[] textsConf = File.ReadAllLines("config.txt");
                Bitmap input = (Bitmap)Bitmap.FromFile(args[0]);
                try
                {

                    Size numbers = new Size(Convert.ToInt32(textsConf[0]), Convert.ToInt32(textsConf[1]));
                    Size sizes = new Size(Convert.ToInt32(textsConf[2]), Convert.ToInt32(textsConf[3]));
                    Size compreSizes = new Size(Convert.ToInt32(textsConf[4]), Convert.ToInt32(textsConf[5]));
                    List<ImageUrl> imageUrls = new List<ImageUrl>();
                    for (int i = 6; i < textsConf.Length; i++)
                    {
                        imageUrls.Add(new ImageUrl(new FileInfo(textsConf[i])));
                    }
                    duringLoadData = false;
                    Bitmap outPut = Dispatcher.GetMultiImage(input, numbers, sizes, compreSizes, imageUrls);
                    outPut.Save($"outMosaic{Guid.NewGuid()}.png");
                }
                catch (Exception ex)
                {
                    if (duringLoadData)
                    {
                        Console.WriteLine("error during loading config file");
                    }
                    throw ex;
                }
            }
        }
    }
}
