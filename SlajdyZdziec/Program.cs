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
            if (args.Length == 0)
            {
                args = new string[2];
                Console.WriteLine("podaj lokalizacje obrazu ");
                args[0] = Console.ReadLine().Trim('"');
                Console.WriteLine("podaj lokalizacje config ");
                args[1] = Console.ReadLine().Trim('"');
            }
            Console.WriteLine("use parameter to get config");
            Console.WriteLine("number of retangle x");
            Console.WriteLine("number of retangle y");
            Console.WriteLine("size retangle out x");
            Console.WriteLine("size retangle out Y");
            Console.WriteLine("size to compare x");
            Console.WriteLine("size to compare y");

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
                    float factorLimiting;
                    Size numbers = new Size(Convert.ToInt32(textsConf[0]), Convert.ToInt32(textsConf[1]));
                    Size sizes = new Size(Convert.ToInt32(textsConf[2]), Convert.ToInt32(textsConf[3]));
                    Size compreSizes = new Size(Convert.ToInt32(textsConf[4]), Convert.ToInt32(textsConf[5]));
                    factorLimiting = FileHelper.floatReader(textsConf[6]);
                    List<ImageUrl> imageUrls = new List<ImageUrl>();
                    List<GraphicProcesing.Parameters> graphicParameters = new List<GraphicProcesing.Parameters>();
                    for (int i = 7; i < textsConf.Length; i++)
                    {
                        if (textsConf[i] == staticSpliter)
                        {
                            graphicParameters = new List<GraphicProcesing.Parameters>();
                            i++;
                            factorToCompare = FileHelper.floatReader(textsConf[i]);
                            i++;
                            for (; i < textsConf.Length; i++)
                            {
                                if (textsConf[i] == staticSpliter)
                                {
                                    goto EndLabe;
                                }

                                string[] list = textsConf[i].Split(';');
                                if (list.Length != 5)
                                {
                                    throw new Exception("config file is not correct");
                                }
                                GraphicProcesing.Parameters parameters = new GraphicProcesing.Parameters()
                                {
                                    Exposition = FileHelper.floatReader(list[0]),
                                    Saturation = FileHelper.floatReader(list[1]),
                                    Contrast = FileHelper.floatReader(list[2]),
                                    Temperature = Convert.ToInt32(list[3]),
                                    CostOfEditing = FileHelper.floatReader(list[4])
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
                                ChengeValue = FileHelper.floatReader(splited[1])
                            });
                        }

                    EndLabe:;
                    }
                    duringLoadData = false;
                    Bitmap outPut = Dispatcher.GetMultiImage(input, numbers, sizes, compreSizes, imageUrls, factorLimiting);
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
