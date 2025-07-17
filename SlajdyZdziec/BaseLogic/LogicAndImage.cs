using SlajdyZdziec.UserLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SlajdyZdziec.BaseLogic
{
    public class LogicAndImage<LogicType, BitmapType>
    {
        public LogicType Logic { get; set; }
        public BitmapType Bitmap { get; set; }
        public LogicAndImage<LogicType, ImageUrl> BestResult { get; set; }
        public GraphicProcesing.Parameters Parameters { get; set; }
        public long Difrence { get; internal set; }
    }
}
