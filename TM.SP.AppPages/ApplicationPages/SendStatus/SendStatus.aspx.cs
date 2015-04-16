// <copyright file="SendStatus.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-07 19:32:30Z</date>
namespace TM.SP.AppPages
{
    using System;
    using System.Security.Permissions;
    using System.Net;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using System.Collections.Generic;
    using System.Linq;

    using TM.SP.AppPages.ApplicationPages;
    using CamlexNET;
    using TM.Utils;
    using CV5 = TM.Services.CoordinateV5;
    using CV52 = TM.Services.CoordinateV52;
    using MessageQueueService = TM.ServiceClients.MessageQueue;

    [Serializable]
    public class SendStatusRequestItem : RequestItem
    {
        public int StatusLookupId { get; set; }
        public List<int> AttachDocumentList { get; set; }
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
        protected static readonly string ServiceGuidConfigName = "AsGufServiceGuid";
        #endregion
        /// <summary>
        /// Initializes a new instance of the SendStatus class
        /// </summary>
        public SendStatus()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        protected string AttachIdListParam
        {
            get
            {
                return String.IsNullOrEmpty(Request.Params["AttachDocuments"]) ? String.Empty : Request.Params["AttachDocuments"];
            }
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
            var idList = String.IsNullOrEmpty(ItemIdListParam)
                ? null
                : ItemIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();
            var attachIdList = String.IsNullOrEmpty(AttachIdListParam)
                ? null
                : AttachIdListParam.Split(',').Select(v => Convert.ToInt32(v)).ToList();

            if (idList == null)
                throw new Exception("Для отправки статуса должен быть указан идентификатор обращения");

            SPListItemCollection docItems = docList.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
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

            if ((retVal.Count == 1) && (attachIdList != null))
                retVal[0].AttachDocumentList = attachIdList;

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
            var svcGuid = Config.GetConfigValueOrDefault<string>(this.Web, ServiceGuidConfigName);
            var svc = GetServiceClientInstance().GetService(new Guid(svcGuid.ToString()));

            if (svcGuid == TM.Services.MessageQueueServices.V5Guid)
            {
                CV5.CoordinateStatusMessage internalMessage = GetCoordinateV5StatusMessage(doc);

                return new MessageQueueService.Message()
                {
                    Service       = svc,
                    MessageId     = new Guid(internalMessage.ServiceHeader.MessageId),
                    MessageType   = 2,
                    MessageMethod = 3,
                    MessageDate   = DateTime.Now,
                    MessageText   = internalMessage.ToXElement<CV5.CoordinateStatusMessage>().ToString()
                };
            }
            else if (svcGuid == TM.Services.MessageQueueServices.V52Guid)
            {
                CV52.CoordinateStatusMessage internalMessage = GetCoordinateV52StatusMessage(doc);

                return new MessageQueueService.Message()
                {
                    Service       = svc,
                    MessageId     = new Guid(internalMessage.ServiceHeader.MessageId),
                    MessageType   = 2,
                    MessageMethod = 3,
                    MessageDate   = DateTime.Now,
                    MessageText   = internalMessage.ToXElement<CV52.CoordinateStatusMessage>().ToString()
                };
            }

            return null;
        }

        /// <summary>
        ///  Building CoordinateStatusMessage V5
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual CV5.CoordinateStatusMessage GetCoordinateV5StatusMessage<T>(T item) where T : SendStatusRequestItem
        {
            // request item
            SPListItem rItem = GetList().GetItemOrBreak(item.Id);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // status lookup item
            var stList = this.Web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var stItem = stList.GetItemOrNull(item.StatusLookupId);
            var stCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());
            // attachs
            CV5.ServiceDocument attachs = null;
            if (item.AttachDocumentList != null && item.AttachDocumentList.Count > 0)
            {
                var attachLib = Web.GetListOrBreak("AttachLib");

                attachs = new CV5.ServiceDocument
                {
                    DocCode   = "10004",
                    DocDate   = DateTime.Now,
                    DocNumber = "БН",
                    DocFiles  = (from attachId in item.AttachDocumentList
                                select attachLib.GetItemById(attachId)
                                into spItem
                                let content = spItem.File.OpenBinary()
                                select new CV5.File
                                {
                                    FileContent = content,
                                    FileName    = spItem.File.Name,
                                }).ToArray()
                };
            }

            var stCodeInt = Convert.ToInt32(stCode);
            var message = new CV5.CoordinateStatusMessage()
            {
                ServiceHeader = new CV5.Headers()
                {
                    FromOrgCode     = Consts.TaxoMotorDepCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CV5.CoordinateStatusData()
                {
                    ServiceNumber = sNumber,
                    StatusCode    = stCodeInt,
                    Documents = attachs != null ? new CV5.ServiceDocument[] { attachs } : null,
                    Result        = IncomeRequestHelper.GetResultObjectForCoordinateV5StatusMessage(stCodeInt)
                }
            };
           
            return message;
        }

        /// <summary>
        ///  Building CoordinateStatusMessage V5.2
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual CV52.CoordinateStatusMessage GetCoordinateV52StatusMessage<T>(T item) where T : SendStatusRequestItem
        {
            // request item
            SPListItem rItem = GetList().GetItemOrBreak(item.Id);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // status lookup item
            var stList = this.Web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var stItem = stList.GetItemOrNull(item.StatusLookupId);
            var stCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());
            // attachs
            CV52.ServiceDocument attachs = null;
            if (item.AttachDocumentList != null && item.AttachDocumentList.Count > 0)
            {
                var attachLib = Web.GetListOrBreak("AttachLib");

                attachs = new CV52.ServiceDocument
                {
                    DocCode = "10004",
                    DocDate = DateTime.Now,
                    DocNumber = "БН",
                    DocFiles = (from attachId in item.AttachDocumentList
                                select attachLib.GetItemById(attachId)
                                    into spItem
                                    let content = spItem.File.OpenBinary()
                                    select new CV52.File
                                    {
                                        FileContent = content,
                                        FileName = spItem.File.Name,
                                    }).ToArray()
                };
            }

            var stCodeInt = Convert.ToInt32(stCode);
            var message = new CV52.CoordinateStatusMessage()
            {
                ServiceHeader = new CV52.Headers()
                {
                    FromOrgCode     = Consts.TaxoMotorDepCode,
                    ToOrgCode       = Consts.AsgufSysCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = sNumber
                },
                StatusMessage = new CV52.CoordinateStatusData()
                {
                    ServiceNumber = sNumber,
                    StatusCode    = stCodeInt,
                    Documents     = attachs != null ? new CV52.ServiceDocument[] { attachs } : null,
                    Result        = IncomeRequestHelper.GetResultObjectForCoordinateV52StatusMessage(stCodeInt),
                    StatusDate    = DateTime.Now
                }
            };

            return message;
        }
    }
}

