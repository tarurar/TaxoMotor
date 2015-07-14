using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TM.SP.AppPages.Tracker
{
    /// <summary>
    /// Контекст обращения
    /// </summary>
    public class IncomeRequestTrackingContext: BaseTrackingContext
    {
        public IncomeRequestTrackingContext(SPListItem item) : base(item)
        {
        }

        protected override SPListItem GetIncomeRequest()
        {
            return Item;
        }
    }
}
