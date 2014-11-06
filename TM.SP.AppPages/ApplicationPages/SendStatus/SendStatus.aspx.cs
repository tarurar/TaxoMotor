// <copyright file="SendStatus.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-07 19:32:30Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Net;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Data;
    using System.Linq;

    using TM.SP.AppPages.ApplicationPages;
    using CamlexNET;
    using TM.Utils;
    using TM.Services.CoordinateV5;
    using MessageQueueService = TM.ServiceClients.MessageQueue;

    [Serializable]
    public class SendStatusRequestItem : RequestItem
    {
        public int StatusLookupId { get; set; }
    }

    /// <summary>
    /// TODO: Add comment for SendStatus
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendStatus : SendRequestDialogBase
    {
        #region [resourceStrings]
        protected static readonly string resNoDocumentsError = "$Resources:SendStatusRequest_DlgNoDocumentsError";
        protected static readonly string resNoStatusErrorFmt = "$Resources:SendStatusRequest_DlgNoStatusError";
        #endregion

        #region [fields]
        protected static readonly string ServiceGuidConfigName = "BR2ServiceGuid";
        #endregion
        /// <summary>
        /// Initializes a new instance of the SendStatus class
        /// </summary>
        public SendStatus()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            var documentList = LoadDocuments<SendStatusRequestItem>();
            var errorList = ValidateDocuments<SendStatusRequestItem>(documentList);
            BindDocuments<SendStatusRequestItem>(documentList);
            if (errorList.Count > 0)
                BindErrors(errorList);
            HandleDocumentsLoad<SendStatusRequestItem>(documentList, errorList);

            var success = false;
            try
            {
                success = SendRequests<SendStatusRequestItem>(documentList, false);
            }
            catch (Exception)
            {
                
            }
            Page.Response.Clear();
            Page.Response.StatusCode = success ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError;
            Page.Response.End();
        }

        protected override List<T> LoadDocuments<T>()
        {
            SPList docList = GetList();
            var idList = ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();

            SPListItemCollection docItems = docList.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString()
            });

            List<SendStatusRequestItem> retVal = new List<SendStatusRequestItem>();
            foreach (SPListItem item in docItems)
            {
                var state = item["Tm_IncomeRequestStateLookup"];
                retVal.Add(new SendStatusRequestItem(){
                    Id = item.ID,
                    Title = item.Title,
                    StatusLookupId = state != null ? new SPFieldLookupValue(state.ToString()).LookupId : 0,
                    HasError = false
                });
            }

            return retVal.Cast<T>().ToList();
        }
        protected override List<ValidationErrorInfo> ValidateDocuments<T>(List<T> documentList)
        {
            var retVal = new List<ValidationErrorInfo>();

            foreach (T document in documentList)
            {
                SendStatusRequestItem doc = document as SendStatusRequestItem;
                #region [Rule#1 - Status cannot be null]
                if (doc.StatusLookupId == 0)
                {
                    retVal.Add(new ValidationErrorInfo()
                    {
                        Message = String.Format(GetLocalizedString(resNoStatusErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
            }

            if (documentList.All(i => i.HasError))
                retVal.Add(new ValidationErrorInfo()
                {
                    Message = GetLocalizedString(resNoDocumentsError),
                    Severity = ValidationErrorSeverity.Critical
                });

            return retVal;
        }
        protected override void BindDocuments<T>(List<T> documentList)
        {
            // todo
        }

        protected override void BindErrors(List<ValidationErrorInfo> errorList)
        {
            // todo
        }

        protected override void HandleDocumentsLoad<T>(List<T> documentList, List<ValidationErrorInfo> errorList)
        {
            // todo
        }

        protected override ServiceClients.MessageQueue.Message BuildMessage<T>(T document)
        {
            SendStatusRequestItem doc = document as SendStatusRequestItem;
            SPListItem configItem = Config.GetConfigItem(this.Web, ServiceGuidConfigName);
            var svcGuid = Config.GetConfigValue(configItem);
            var svc = GetServiceClientInstance().GetService(new Guid(svcGuid.ToString()));
            var internalMessage = GetRelevantCoordinateStatusMessage(doc);

            return new MessageQueueService.Message()
            {
                Service = svc,
                MessageId = new Guid(internalMessage.ServiceHeader.MessageId),
                MessageType = 2,
                MessageMethod = 14,
                MessageDate = DateTime.Now,
                MessageText = internalMessage.ToXElement<CoordinateStatusMessage>().ToString(),
            };
        }

        /// <summary>
        ///  Building CoordinateStatusMessage
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual CoordinateStatusMessage GetRelevantCoordinateStatusMessage<T>(T item) where T : SendStatusRequestItem
        {
            // request item
            SPListItem rItem = GetList().GetItemOrBreak(item.Id);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // status lookup item
            var stList = this.Web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var stItem = stList.GetItemOrNull(item.StatusLookupId);
            var stCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            var message = new CoordinateStatusMessage() 
            {
                ServiceHeader = new Headers()
                {
                    FromOrgCode     = Consts.TaxoMotorSysCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CoordinateStatusData()
                {
                    ServiceNumber = sNumber,
                    StatusCode    = Convert.ToInt32(stCode),
                }
            };
           
            return message;
        }
    }
}

