using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TM.SP.Ratings.Reports
{
    public class SPReport : BaseReport
    {
        private SPWeb _web;
        public SPReport(SPWeb web)
        {
            _web = web;
        }
    }
}
