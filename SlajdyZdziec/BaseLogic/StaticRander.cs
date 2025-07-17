using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlajdyZdziec.BaseLogic
{
    public static class StaticRander
    {
        public static float GetNumber(int x, int y)
        {
            ulong number = (ulong)x * 56453 + (ulong)y;
            number *= 76463;
            float z = number % 10000;
            z /= 10000f;
            return z;
        }
    }
}
