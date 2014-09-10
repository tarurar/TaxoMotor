// <copyright file="SendRequestEGRIPPage.aspx.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-09-09 16:35:29Z</date>
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
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Data;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.BusinessData.SharedService;
    using Microsoft.SharePoint.BusinessData.MetadataModel;
    using Microsoft.BusinessData.Runtime;
    using Microsoft.BusinessData.MetadataModel;
    using Microsoft.BusinessData.MetadataModel.Collections;
    using CamlexNET;

    using TM.SP.AppPages.ApplicationPages;
    using BcsCoordinateV5Model = TM.SP.BCSModels.CoordinateV5;
    using TM.Utils;
    using TM.Services.CoordinateV5;
    using MessageQueueService = TM.ServiceClients.MessageQueue;


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

        protected override List<ValidationErrorInfo> ValidateDocuments<T>(List<T> documentList)
        {
            var retVal = new List<ValidationErrorInfo>();

            foreach (T document in documentList)
            {
                EGRULRequestItem doc = document as EGRULRequestItem;
                #region [Rule#1 - RequestAccount cannot be null]
                if (String.IsNullOrEmpty(doc.RequestAccount))
                {
                    retVal.Add(new ValidationErrorInfo()
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
                    retVal.Add(new ValidationErrorInfo()
                    {
                        Message = String.Format(GetLocalizedString(resAccNotEntrprnrErrorFmt), doc.Title),
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

        protected override CoordinateTaskMessage GetRelevantCoordinateTaskMessage<T>(T item)
        {
            // request item
            SPListItem rItem = GetList().GetItemOrBreak(item.Id);
            var rDocument = rItem["Tm_RequestedDocument"] == null ? 0 : Convert.ToInt32(rItem["Tm_RequestedDocument"]);
            var sNumber = rItem["Tm_SingleNumber"] == null ? String.Empty : rItem["Tm_SingleNumber"].ToString();
            // request contact
            BcsCoordinateV5Model.RequestAccount rAccount = GetRequestAccount(item.RequestAccountId);
            if (rAccount == null)
                throw new Exception(String.Format("Bcs entity with Id = {0} not found", item.RequestAccountId));
            // service code lookup item
            SPList stList = this.Web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
            SPListItem stItem = stList.GetItemOrNull(rDocument);
            var sCode = stItem == null ? String.Empty :
                (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            return new CoordinateTaskMessage()
            {
                ServiceHeader = new Headers()
                {
                    FromOrgCode = String.Empty,                     // todo
                    ToOrgCode = "705",                              // todo
                    MessageId = Guid.NewGuid().ToString("D"),       // todo
                    ServiceNumber = sNumber,
                    RequestDateTime = DateTime.Now
                },
                TaskMessage = new CoordinateTaskData()
                {
                    Data = new DocumentsRequestData()
                    {
                        DocumentTypeCode = "7830",                  // todo
                        IncludeBinaryView = true,
                        IncludeXmlView = true,
                        Parameter = getTaskParam(rAccount),
                        ParameterTypeCode = String.Empty            // todo
                    },
                    Task = new RequestTask()
                    {
                        Code = "¡–2",                               // todo
                        Department = new Department()
                        {
                            Name = "ƒ“Ë–ƒ“»",                       // todo
                            Code = "2009",                          // todo
                            RegDate = null                          // todo
                        },
                        RequestId = new Guid().ToString("D"),       // todo
                        Responsible = new Person()
                        {
                            LastName = String.Empty,                // todo
                            FirstName = this.Web.CurrentUser.Name   // todo
                        },
                        ServiceNumber = sNumber,
                        ServiceTypeCode = sCode,
                        Subject = String.Empty,                     // todo
                        ValidityPeriod = null                       // todo
                    },
                    Signature = String.Empty                        // todo
                }
            };
        }

        #endregion
    }
}

