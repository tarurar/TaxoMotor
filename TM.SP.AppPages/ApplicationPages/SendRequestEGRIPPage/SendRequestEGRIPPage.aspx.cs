// <copyright file="SendRequestEGRIPPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-09 16:35:29Z</date>

// ReSharper disable CheckNamespace
namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using ApplicationPages;
    using BcsCoordinateV5Model = BCSModels.CoordinateV5;
    using Utils;
    using Services.CoordinateV5;
    using MessageQueueService = ServiceClients.MessageQueue;


    /// <summary>
    /// TODO: Add comment for SendRequestEGRIPPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class SendRequestEGRIPPage : SendRequestEGRULPage
    {

        #region [resourceStrings]
        protected static readonly string resAccNotEntrprnrErrorFmt  = "$Resources:EGRIPRequest_DlgAccountIsNotEntrepreneurErrorFmt";
        #endregion

        #region [methods]

        protected override List<T> LoadDocuments<T>()
        {
            var documentList = base.LoadDocuments<T>();
            foreach (var doc in documentList.Select(document => document as EGRULRequestItem))
            {
                doc.RequestTypeCode = OutcomeRequestType.Egrip;
            }

            return documentList;
        }

        protected override List<ValidationErrorInfo> ValidateDocuments<T>(List<T> documentList)
        {
            var retVal = new List<ValidationErrorInfo>();

            foreach (T document in documentList)
            {
                EGRULRequestItem doc = document as EGRULRequestItem;
                #region [Rule#1 - RequestAccount cannot be null]
                if (String.IsNullOrEmpty(doc.RequestAccount))
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(resNoAccountErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
                #region [Rule#2 - RequestAccount must be private entrepreneur]
                if (doc.OrgFormCode != PrivateEntrepreneurCode)
                {
                    retVal.Add(new ValidationErrorInfo
                    {
                        Message = String.Format(GetLocalizedString(resAccNotEntrprnrErrorFmt), doc.Title),
                        Severity = ValidationErrorSeverity.Warning
                    });

                    doc.HasError = true;
                }
                #endregion
            }

            if (documentList.All(i => i.HasError))
                retVal.Add(new ValidationErrorInfo
                {
                    Message = GetLocalizedString(resNoDocumentsError),
                    Severity = ValidationErrorSeverity.Critical
                });

            return retVal;
        }

        protected override CoordinateTaskMessage GetRelevantCoordinateTaskMessage<T>(T item)
        {
            // request item
            SPListItem rItem = Web.GetListOrBreak(String.Format("Lists/{0}", item.ListName)).GetItemOrBreak(item.Id);
            var rDocument = rItem["Tm_RequestedDocument"] == null ? 0 : new SPFieldLookupValue(rItem["Tm_RequestedDocument"].ToString()).LookupId;
            var sNumber   = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // request contact
            BcsCoordinateV5Model.RequestAccount rAccount = GetRequestAccount(item.RequestAccountId);
            if (rAccount == null)
                throw new Exception(String.Format("Bcs entity with Id = {0} not found", item.RequestAccountId));
            // service code lookup item
            var stList = Web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
            var stItem = stList.GetItemOrNull(rDocument);
            var sCode  = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            var message = Helpers.GetEGRIPMessageTemplate(getTaskParam(rAccount));
            message.ServiceHeader.ServiceNumber            = sNumber;
            message.TaskMessage.Task.Responsible.FirstName = String.Empty;
            message.TaskMessage.Task.Responsible.LastName  = Web.CurrentUser.Name;
            message.TaskMessage.Task.ServiceNumber         = sNumber;
            message.TaskMessage.Task.ServiceTypeCode       = sCode;
            return message;
        }

        #endregion
    }
}

