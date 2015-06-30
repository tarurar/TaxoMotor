using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;
using TM.Utils.TimerJobs;
using ViewUpdater;

namespace TM.SP.AppPages.Timers
{
    class UpdateViewsTimer : JobDefinition
    {
        #region [methods]
        public UpdateViewsTimer()
        { }

        public UpdateViewsTimer(string jobName, string jobTitle, SPService service)
            : base(jobName, jobTitle, service) { }

        public UpdateViewsTimer(string jobName, string jobTitle, SPWebApplication webapp)
            : base(jobName, jobTitle, webapp) { }

        protected override void Work(SPWeb web)
        {
            CommonService.UpdateSQLViews(web);
        }
        #endregion
    }
}
