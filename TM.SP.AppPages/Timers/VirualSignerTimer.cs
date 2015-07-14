using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using TM.Utils.TimerJobs;

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
