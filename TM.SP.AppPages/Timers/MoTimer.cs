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

namespace TM.SP.AppPages.Timers
{
    public class MoTimer: SPJobDefinition
    {
         #region [resource strings]

        public static readonly string TaxiListsFeatureId = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        #endregion

        #region [methods]
        public MoTimer()
        {}

        public MoTimer(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            Title = "ТаксоМотор: Отправка в МО";
        }

        public MoTimer(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            Title = "ТаксоМотор: Отправка в МО";
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                if (webApp != null)
                {
                    foreach (SPSite siteCollection in webApp.Sites)
                    {
                        try
                        {
                            SPWeb web = siteCollection.RootWeb;

                            if (web.Features[new Guid(TaxiListsFeatureId)] != null &&
                                web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
                            {
                                Work(web);
                            }
                        }
                        finally
                        {
                            siteCollection.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
	        {
                throw new Exception(String.Format("Во время отправки данных в МО произошла ошибка. Детали: {0}", ex.Message));
	        }
        }

        private void Work(SPWeb web)
        {
            var cfg = Config.GetConfigValueOrDefault<string>(web, "AutoSendMo");
            if (!String.IsNullOrEmpty(cfg))
            {
                if (cfg.Equals("1") || cfg.ToUpper().Equals("ДА"))
                {
                    var url = SPUtility.ConcatUrls(SPUtility.GetWebLayoutsFolder(web), "TaxoMotor/CommonService.aspx/SendMo");
                    var request = WebRequest.Create(url);

                    request.Method = "POST";
                    request.ContentLength = 0;
                    request.UseDefaultCredentials = true;
                    var response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Error sending web request (url = {0})", url));
                }
            }
        }

        #endregion
    }
}
