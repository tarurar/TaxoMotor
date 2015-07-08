using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.Communication
{
    public class CoordinateBaseMessageBuilder<T>: ICoordinateMessageBuilder<T>
    {
        public virtual T Build()
        {
            throw new NotImplementedException();
        }
    }
}
