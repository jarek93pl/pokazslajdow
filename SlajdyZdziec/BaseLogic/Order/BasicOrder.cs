using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SlajdyZdziec.BaseLogic.Order
{
    class BasicOrder<T> : IOrderDispatcher<T>
    {
        public IEnumerable<T> GetItemWitchOrder(Size dimImage, IList<T> list)
        {
            return list;
        }
    }
}
