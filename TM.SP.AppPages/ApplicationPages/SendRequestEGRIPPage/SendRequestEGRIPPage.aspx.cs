// <copyright file="SendRequestEGRIPPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-09 16:35:29Z</date>

using TM.SP.AppPages.Communication;
using TM.Utils;
// ReSharper disable CheckNamespace


namespace TM.SP.AppPages
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SharePoint.Security;
    using ApplicationPages;
    using BcsCoordinateV5Model = BCSModels.CoordinateV5;
    using MessageQueueService = ServiceClients.MessageQueue;


    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
// ReSharper disable InconsistentNaming
    public partial class SendRequestEGRIPPage : SendRequestEGRULPage
// ReSharper restore InconsistentNaming
    {

        #region [resourceStrings]
// ReSharper disable InconsistentNaming
        protected static readonly string resAccNotEntrprnrErrorFmt  = "$Resources:EGRIPRequest_DlgAccountIsNotEntrepreneurErrorFmt";
// ReSharper restore InconsistentNaming
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
                var doc = document as EGRULRequestItem;
                #region [Rule#1 - RequestAccount cannot be null]
                if (doc != null && String.IsNullOrEmpty(doc.RequestAccount))
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
                if (doc != null && doc.OrgFormCode != PrivateEntrepreneurCode)
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

        protected override ServiceClients.MessageQueue.Message BuildMessage<T>(T document)
        {
            var doc = document as EGRULRequestItem;
            if (doc == null)
                throw new Exception("Must be of type EGRULRequestItem");

            var svcGuid = new Guid(Config.GetConfigValueOrDefault<string>(Web, EgrulServiceGuidConfigName));
            var spItem = GetList().GetItemOrBreak(doc.Id);
            var buildOptions = new QueueMessageBuildOptions { Date = DateTime.Now, Method = 2, ServiceGuid = svcGuid };
            return QueueMessageBuilder.Build(new CoordinateV5EgripMessageBuilder(spItem, doc.RequestAccountId),
                QueueClient, buildOptions);
        }
        #endregion
    }
}

