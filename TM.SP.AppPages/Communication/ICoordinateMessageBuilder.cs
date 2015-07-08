using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.Communication
{
    public interface ICoordinateMessageBuilder<T>
    {
        T Build();
    }
}
