using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.BCSModels
{
    public enum MigratingStatus
    {
        Undefined = 0,
        Reserved,
        Processing,
        Processed,
        Error
    }
}
