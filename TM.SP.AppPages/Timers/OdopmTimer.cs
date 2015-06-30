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
    public class OdopmTimer: JobDefinition
    {
        #region [methods]
        public OdopmTimer()
        {}

        public OdopmTimer(string jobName, string jobTitle, SPService service): base(jobName, jobTitle, service) { }

        public OdopmTimer(string jobName, string jobTitle, SPWebApplication webapp): base(jobName, jobTitle, webapp) { }

        protected override void Work(SPWeb web)
        {
            var cfg = Config.GetConfigValueOrDefault<string>(web, "AutoSendOdopm");
            if (!String.IsNullOrEmpty(cfg))
            {
                if (cfg.Equals("1") || cfg.ToUpper().Equals("ДА"))
                {
                    CommonService.SendOdopm(web);
                }
            }
        }

        #endregion
    }
}
