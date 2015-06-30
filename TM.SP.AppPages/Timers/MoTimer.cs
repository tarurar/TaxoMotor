using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;
using TM.Utils.TimerJobs;

namespace TM.SP.AppPages.Timers
{
    public class MoTimer: JobDefinition
    {
        #region [methods]
        public MoTimer()
        {}

        public MoTimer(string jobName, string jobTitle, SPService service) : base(jobName, jobTitle, service) { }

        public MoTimer(string jobName, string jobTitle, SPWebApplication webapp) : base(jobName, jobTitle, webapp) { }
        
        protected override void Work(SPWeb web)
        {
            var cfg = Config.GetConfigValueOrDefault<string>(web, "AutoSendMo");
            if (!String.IsNullOrEmpty(cfg))
            {
                if (cfg.Equals("1") || cfg.ToUpper().Equals("ДА"))
                {
                    CommonService.SendMo(web);
                }
            }
        }

        #endregion
    }
}
