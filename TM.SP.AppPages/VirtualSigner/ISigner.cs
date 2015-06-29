using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.VirtualSigner
{
    interface ISigner
    {
        string SignXml(string content);
    }
}
