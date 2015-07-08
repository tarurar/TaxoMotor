using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.Communication
{
    public struct RequestAccountData: IRequestAccountData
    {
        public string Ogrn { get; set; }
        public string Inn { get; set; }
    }
}
