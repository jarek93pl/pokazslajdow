using SlajdyZdziec.BaseLogic;
using SlajdyZdziec.ImagesInImage;
using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlajdyZdziec
{

    static class Program
    {
        const string staticSpliter = "---";
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CultureInfo culutre = new CultureInfo("us-EN", false);
            Console.WriteLine("use parameter to get config");
            Console.WriteLine("number of retangle x");
            Console.WriteLine("number of retangle y");
            Console.WriteLine("size retangle out x");
            Console.WriteLine("size retangle out Y");
            Console.WriteLine("size to compare x");
            Console.WriteLine("size to compare y");
#if DEBUG
            args = new string[] { @"E:\OneDrive\Pulpit\Apki\PokazSlajdow\SlajdyZdziec\Screenshot 2025-07-09 100659+v2.jpg", "E:\\OneDrive\\Pulpit\\Apki\\PokazSlajdow\\SlajdyZdziec\\config.txt" };
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new Form1());
            }
            else
            {
                bool duringLoadData = true;
                string[] textsConf = File.ReadAllLines(args[1]);
                Bitmap input = (Bitmap)Bitmap.FromFile(args[0]);
                try
                {
                    float factorToCompare = 1;
                    Size numbers = new Size(Convert.ToInt32(textsConf[0]), Convert.ToInt32(textsConf[1]));
                    Size sizes = new Size(Convert.ToInt32(textsConf[2]), Convert.ToInt32(textsConf[3]));
                    Size compreSizes = new Size(Convert.ToInt32(textsConf[4]), Convert.ToInt32(textsConf[5]));
                    List<ImageUrl> imageUrls = new List<ImageUrl>();
                    List<GraphicProcesing.Parameters> graphicParameters = new List<GraphicProcesing.Parameters>();
                    for (int i = 6; i < textsConf.Length; i++)
                    {
                        if (textsConf[i] == staticSpliter)
                        {
                            graphicParameters = new List<GraphicProcesing.Parameters>();
                            i++;
                            factorToCompare = Convert.ToSingle(textsConf[i], culutre);
                            i++;
                            for (; i < textsConf.Length; i++)
                            {
                                if (textsConf[i] == staticSpliter)
                                {
                                    goto EndLabe;
                                }

                                string[] list = textsConf[i].Split(';');
                                if (list.Length != 3)
                                {
                                    throw new Exception("config file is not correct");
                                }
                                GraphicProcesing.Parameters parameters = new GraphicProcesing.Parameters()
                                {
                                    Exposition = Convert.ToSingle(list[0], culutre),
                                    Saturation = Convert.ToSingle(list[1], culutre),
                                    Contrast = Convert.ToSingle(list[2], culutre)
                                };
                                graphicParameters.Add(parameters);
                            }

                        }
                        string[] splited = textsConf[i].Split(';');
                        if (splited.Length < 2)
                        {
                            imageUrls.Add(new ImageUrl(new FileInfo(textsConf[i]))
                            {
                                GraphicParameters = graphicParameters,
                                factorTocompare = factorToCompare
                            });
                        }
                        else
                        {
                            imageUrls.Add(new ImageUrl(new FileInfo(splited[0]))
                            {
                                GraphicParameters = graphicParameters,
                                factorTocompare = factorToCompare,
                                PropabilityAccept = Convert.ToSingle(splited[1], culutre)
                            });
                        }

                    EndLabe:;
                    }
                    duringLoadData = false;
                    Bitmap outPut = Dispatcher.GetMultiImage(input, numbers, sizes, compreSizes, imageUrls);
                    string nameFile = Path.GetFileNameWithoutExtension(args[1]);
                    outPut.Save($"outMosaic{nameFile}.png");
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
