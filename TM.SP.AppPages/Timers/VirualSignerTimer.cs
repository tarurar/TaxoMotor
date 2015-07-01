using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;
using TM.Utils.TimerJobs;
using TM.SP.BCSModels.Taxi;
using Microsoft.BusinessData.MetadataModel;
using TM.SP.AppPages.VirtualSigner;

namespace TM.SP.AppPages.Timers
{
    public class VirualSignerTimer : JobDefinition
    {
        #region [methods]
        public VirualSignerTimer()
        {}

        public VirualSignerTimer(string jobName, string jobTitle, SPService service): base(jobName, jobTitle, service) { }

        public VirualSignerTimer(string jobName, string jobTitle, SPWebApplication webapp): base(jobName, jobTitle, webapp) { }

        protected override void Work(SPWeb web)
        {
            CommonService.RunVirtualSigner(web);
        }

        #endregion
    }
}
