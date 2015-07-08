// <copyright file="SendStatus.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-10-07 19:32:30Z</date>

// ReSharper disable once CheckNamespace
namespace TM.SP.AppPages
{
    using System;
    using System.Security.Permissions;
    using System.Net;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using System.Collections.Generic;
    using System.Linq;

    using ApplicationPages;
    using CamlexNET;
    using Utils;
    using CV5 = Services.CoordinateV5;
    using CV52 = Services.CoordinateV52;
    using MessageQueueService = ServiceClients.MessageQueue;
    using Communication;

    [Serializable]
    public class SendStatusRequestItem : RequestItem
    {
        public int StatusLookupId { get; set; }
        public List<int> AttachDocumentList { get; set; }
    }

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendStatus : SendRequestDialogBase
    {
        #region [resourceStrings]
        protected static readonly string ResNoDocumentsError = "$Resources:SendStatusRequest_DlgNoDocumentsError";
        protected static readonly string ResNoStatusErrorFmt = "$Resources:SendStatusRequest_DlgNoStatusError";
        #endregion

        #region [fields]
        protected static readonly string ServiceGuidConfigName = "AsGufServiceGuid";
        #endregion

        #region [methods]
        public SendStatus()
        {
            RightsCheckMode = RightsCheckModes.OnPreInit;
        }
        protected string AttachIdListParam
        {
            get
            {
                return String.IsNullOrEmpty(Request.Params["AttachDocuments"]) ? String.Empty : Request.Params["AttachDocuments"];
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            var documentList = LoadDocuments<SendStatusRequestItem>();
            var errorList = ValidateDocuments(documentList);
            BindDocuments(documentList);
            if (errorList.Count > 0)
                BindErrors(errorList);
            HandleDocumentsLoad(documentList, errorList);

            var success = false;
            try
            {
                success = SendRequests(documentList, false);
            }
// ReSharper disable once EmptyGeneralCatchClause
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

            SPListItemCollection docItems = docList.GetItems(new SPQuery
            {
                Query = Camlex.Query().Where(x => idList.Contains((int)x["ID"])).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            var retVal = new List<SendStatusRequestItem>();
            foreach (SPListItem item in docItems)
            {
                var state = item["Tm_IncomeRequestStateLookup"];
                retVal.Add(new SendStatusRequestItem {
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
                var doc = document as SendStatusRequestItem;
                #region [Rule#1 - Status cannot be null]
                if (doc != null && doc.StatusLookupId == 0)
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(ResNoStatusErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
            }

            if (documentList.All(i => i.HasError))
                retVal.Add(new ValidationErrorInfo
                {
                    Message = GetLocalizedString(ResNoDocumentsError),
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
            var doc = document as SendStatusRequestItem;
            if (doc == null)
                throw new Exception("Must be of type SendStatusRequestItem");

            var spItem       = GetList().GetItemOrBreak(doc.Id);
            var svcGuid      = new Guid(Config.GetConfigValueOrDefault<string>(Web, ServiceGuidConfigName));
            var buildOptions = new QueueMessageBuildOptions {Date = DateTime.Now, Method = 3, ServiceGuid = svcGuid};

            if (svcGuid.Equals(Services.MessageQueueServices.V5Guid))
            {
                var intBuilder = new CoordinateV5StatusMessageBuilder(spItem, doc.AttachDocumentList);
                return QueueMessageBuilder.Build(intBuilder, QueueClient, buildOptions);
            }

            if (svcGuid.Equals(Services.MessageQueueServices.V52Guid))
            {
                var intBuilder = new CoordinateV52StatusMessageBuilder(spItem, doc.AttachDocumentList);
                return QueueMessageBuilder.Build(intBuilder, QueueClient, buildOptions);
            }

            return null;
        }
        #endregion
    }
}

