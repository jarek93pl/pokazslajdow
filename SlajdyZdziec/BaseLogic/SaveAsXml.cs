using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SlajdyZdziec.BaseLogic
{

    public static class SaveAsXml
    {
        internal static void Save(List<LogicAndImage<ImageToCompare, PartImage>> list)
        {
            List<string> rows = new List<string>();
            rows.Add("X;Y;FullName;Saturation;Contrast;Exposition;tint;Temperature;CostOfEditing;Difrence;frendlyName");
            foreach (var X in list)
            {

                string row = "";
                row += X.Bitmap.PointInImage.X + ";";
                row += X.Bitmap.PointInImage.Y + ";";
                row += X.BestResult.Bitmap.file.FullName + ";";
                row += X?.Parameters?.Saturation + ";";
                row += X?.Parameters?.Contrast + ";";
                row += X?.Parameters?.Exposition + ";";
                row += X?.Parameters?.tint + ";";
                row += X?.Parameters?.Temperature + ";";
                row += X?.Parameters?.CostOfEditing + ";";
                row += X.Difrence + ";";
                row += $"{X.Bitmap.PointInImage.X}_{X.Bitmap.PointInImage.Y}";
                rows.Add(row);
            }
            File.WriteAllLines($"result{Program.nameFile}.csv", rows);
        }
    }
}
