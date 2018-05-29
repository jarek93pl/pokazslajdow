using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SlajdyZdziec.BaseLogic.Order
{
    public interface IOrderDispatcher<T>
    {
        IEnumerable<T> GetItemWitchOrder(Size dimImage, IList<T> list);
    }
}
