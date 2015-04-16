using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CV5 = TM.Services.CoordinateV5;
using CV52 = TM.Services.CoordinateV52;
using TM.Utils;

namespace TM.SP.AppPages
{
    // helper class for working with income request entity
    public class IncomeRequestHelper
    {
        /// <summary>
        ///  Получение xml сообщения статуса обращения по V5
        /// </summary>
        /// <param name="incomeRequestId">Идентифифкатор обращения</param>
        /// <param name="web">Объект SPWeb</param>
        /// <returns>Сообщение в виде xml</returns>
        public static string GetIncomeRequestCoordinateV5StatusMessage(int incomeRequestId, SPWeb web)
        {
            var rList = web.GetListOrBreak("Lists/IncomeRequestList");
            // request item
            SPListItem rItem = rList.GetItemOrBreak(incomeRequestId);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // status lookup item
            var stList = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var stItemId = rItem["Tm_IncomeRequestStateLookup"] != null ? new SPFieldLookupValue(rItem["Tm_IncomeRequestStateLookup"].ToString()).LookupId : 0;
            var stItem = stList.GetItemOrNull(stItemId);
            var stCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            var stCodeInt = Convert.ToInt32(stCode);
            var message = new CV5.CoordinateStatusMessage
            {
                ServiceHeader = new CV5.Headers
                {
                    FromOrgCode     = Consts.TaxoMotorDepCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CV5.CoordinateStatusData
                {
                    ServiceNumber = sNumber,
                    StatusCode    = stCodeInt,
                    Result        = GetResultObjectForCoordinateV5StatusMessage(stCodeInt)
                }
            };

            return message.ToXElement<CV5.CoordinateStatusMessage>().ToString();
        }

        /// <summary>
        ///  Получение xml сообщения статуса обращения по V5.2
        /// </summary>
        /// <param name="incomeRequestId">Идентифифкатор обращения</param>
        /// <param name="web">Объект SPWeb</param>
        /// <returns>Сообщение в виде xml</returns>
        public static string GetIncomeRequestCoordinateV52StatusMessage(int incomeRequestId, SPWeb web)
        {
            var rList = web.GetListOrBreak("Lists/IncomeRequestList");
            // request item
            SPListItem rItem = rList.GetItemOrBreak(incomeRequestId);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // status lookup item
            var stList = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var stItemId = rItem["Tm_IncomeRequestStateLookup"] != null ? new SPFieldLookupValue(rItem["Tm_IncomeRequestStateLookup"].ToString()).LookupId : 0;
            var stItem = stList.GetItemOrNull(stItemId);
            var stCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            var stCodeInt = Convert.ToInt32(stCode);
            var message = new CV52.CoordinateStatusMessage
            {
                ServiceHeader = new CV52.Headers
                {
                    FromOrgCode     = Consts.TaxoMotorDepCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CV52.CoordinateStatusData
                {
                    ServiceNumber = sNumber,
                    StatusCode    = stCodeInt,
                    Result        = GetResultObjectForCoordinateV52StatusMessage(stCodeInt),
                    StatusDate    = DateTime.Now
                }
            };

            return message.ToXElement<CV52.CoordinateStatusMessage>().ToString();
        }

        /// <summary>
        ///  Сохранение подписанного состояния обращения в список IncomeRequestStatusLogList
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="signature">Xml подписи</param>
        /// <param name="web">Объект SPWeb</param>
        public static void SaveIncomeRequestStatusLog(int incomeRequestId, string signature, SPWeb web)
        {
            SPList spList = web.GetListOrBreak("Lists/IncomeRequestList");
            SPList logList = web.GetListOrBreak("Lists/IncomeRequestStatusLogList");
            SPListItem spItem = spList.GetItemOrBreak(incomeRequestId);

            if (spItem["Tm_IncomeRequestStateLookup"] == null)
                throw new Exception("SaveIncomeRequestStatusLog: Income request state not defined");

            web.AllowUnsafeUpdates = true;
            try
            {
                string yearStr = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                string monthstr = DateTime.Now.ToString("MMM", CultureInfo.CurrentCulture);
                string num = spItem.Title.ToString(CultureInfo.InvariantCulture);

                SPFolder parentFolder = logList.RootFolder.CreateSubFolders(new[] { yearStr, monthstr, num });
                SPListItem newLogItem = logList.AddItem(parentFolder.ServerRelativeUrl, SPFileSystemObjectType.File);

                newLogItem["Title"] = new SPFieldLookupValue(spItem["Tm_IncomeRequestStateLookup"].ToString()).LookupValue;
                newLogItem["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(spItem.ID, spItem.Title);
                newLogItem["Tm_IncomeRequestStateLookup"] = spItem["Tm_IncomeRequestStateLookup"];
                newLogItem["Tm_XmlValue"] = Uri.UnescapeDataString(signature);
                newLogItem.Update();
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }

        }

        /// <summary>
        /// Отправка статуса обращения
        /// </summary>
        /// <param name="incomeRequestId">Идентификатор обращения</param>
        /// <param name="web">Объект SPWeb</param>
        public static void NotifyAboutItemStatus(int incomeRequestId, SPWeb web)
        {
            SPList spList = web.GetListOrBreak("Lists/IncomeRequestList");
            SPListItem spItem = spList.GetItemOrBreak(incomeRequestId);

            var url = SPUtility.ConcatUrls(SPUtility.GetWebLayoutsFolder(web), "TaxoMotor/SendStatus.aspx");
            var uriBuilder = new UriBuilder(url) { Port = -1 };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["ListId"] = spList.ID.ToString("B");
            query["Items"] = spItem.ID.ToString();
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();

            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = 0;
            request.UseDefaultCredentials = true;
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(String.Format("Error sending web request (url = {0})", url));
        }

        public static CV5.RequestResult GetResultObjectForCoordinateV5StatusMessage(int incomeRequestStatusCode)
        {
            int resultCode;
            switch(incomeRequestStatusCode)
            {
                case 1075: 
                    resultCode = 1;
                    break;
                case 1085:
                    resultCode = 1;
                    break;
                case 1080:
                    resultCode = 3;
                    break;
                case 1030:
                    resultCode = 3;
                    break;
                default:
                    resultCode = 0;
                    break;
            }

            if (resultCode == 0) return null;

            return new CV5.RequestResult() { ResultCode = resultCode.ToString() };
        }

        public static CV52.RequestResult GetResultObjectForCoordinateV52StatusMessage(int incomeRequestStatusCode)
        {
            int resultCode;
            switch (incomeRequestStatusCode)
            {
                case 1075:
                    resultCode = 1;
                    break;
                case 1085:
                    resultCode = 1;
                    break;
                case 1080:
                    resultCode = 3;
                    break;
                case 1030:
                    resultCode = 3;
                    break;
                default:
                    resultCode = 0;
                    break;
            }

            if (resultCode == 0) return null;

            return new CV52.RequestResult() { ResultCode = resultCode.ToString() };
        }
    }
}
