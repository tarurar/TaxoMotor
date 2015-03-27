// <copyright file="erIncomeRequestItem.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-10-28 15:18:13Z</date>
// ReSharper disable CheckNamespace
namespace TM.SP.Customizations
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Utils;

    /// <summary>
    /// TODO: Add comment for erIncomeRequestItem
    /// </summary>
// ReSharper disable InconsistentNaming
    public class erIncomeRequestItem : SPItemEventReceiver
// ReSharper restore InconsistentNaming
    {
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdding(SPItemEventProperties properties)
        {
            var currentCtId = new SPContentTypeId(properties.AfterProperties["ContentTypeId"].ToString());
            var folderCtId = properties.List.ContentTypes.BestMatch(new SPContentTypeId("0x0120"));
            if (currentCtId != folderCtId)
            {
                EventFiringEnabled = false;
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        SPSite siteCollection = properties.Site;
                        SPServiceContext context = SPServiceContext.GetContext(siteCollection);
                        using (new SPServiceContextScope(context))
                        {
                            DoItemAdding(properties);
                        }
                    });
                }
                catch (Exception ex)
                {
                    properties.Status = SPEventReceiverStatus.CancelWithError;
                    properties.ErrorMessage = ex.Message;
                    return;
                }
                finally
                {
                    EventFiringEnabled = true;
                }
            }

            base.ItemAdding(properties);
        }

        private static void DoItemAdding(SPItemEventProperties properties)
        {
            var afterProps = properties.AfterProperties;

            if (afterProps["Title"] == null || String.Empty == (string) afterProps["Title"])
                afterProps["Title"] = String.Format("Обращение от {0}",
                    afterProps["Tm_RequestAccountBCSLookup"] ?? afterProps["Tm_RequestContactBCSLookup"]);

            if (afterProps["Tm_RegistrationDate"] == null || String.Empty == (string) afterProps["Tm_RegistrationDate"])
                afterProps["Tm_RegistrationDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date);

            if (afterProps["Tm_IncomeRequestStateLookup"] == null || (string) afterProps["Tm_IncomeRequestStateLookup"] == String.Empty)
            {
                var stateList = properties.Web.GetListOrBreak("Lists/IncomeRequestStateBookList");
                var stateItem = stateList.GetSingleListItemByFieldValue("Tm_ServiceCode", "1040");
                if (stateItem != null)
                    afterProps["Tm_IncomeRequestStateLookup"] = new SPFieldLookupValue(stateItem.ID, stateItem.Title);
            }

            var serviceCode = String.Empty;
            var ctId = new SPContentTypeId(afterProps["ContentTypeId"].ToString());
            if (ctId == properties.List.ContentTypes["Новое"].Id)
            {
                serviceCode = "77200101";
            }
            else if (ctId == properties.List.ContentTypes["Переоформление"].Id)
            {
                serviceCode = "020202";
            }
            else if (ctId == properties.List.ContentTypes["Выдача дубликата"].Id)
            {
                serviceCode = "020203";
            }
            else if (ctId == properties.List.ContentTypes["Аннулирование"].Id)
            {
                serviceCode = "020204";
            }
            if (serviceCode == String.Empty)
                throw new Exception("New income request service code cann't be qualified");

            if (afterProps["Tm_SingleNumber"] == null || (string)afterProps["Tm_SingleNumber"] == String.Empty)
                afterProps["Tm_SingleNumber"] = Utility.GetIncomeRequestNewSingleNumber(serviceCode);

            if (afterProps["Tm_RequestedDocument"] == null || (string) afterProps["Tm_RequestedDocument"] == String.Empty)
            {
                var govServiceList = properties.Web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
                var govServiceItem = govServiceList.GetSingleListItemByFieldValue("Tm_ServiceCode", serviceCode);
                if (govServiceItem != null)
                    afterProps["Tm_RequestedDocument"] = new SPFieldLookupValue(govServiceItem.ID, govServiceItem.Title);
            }

            if (afterProps["Tm_PlannedWorkInDate"] == null || (string) afterProps["Tm_PlannedWorkInDate"] == String.Empty)
            {
                var plannedDate = Calendar.CalcFinishDate(properties.Web, DateTime.Now.AddDays(1), 3);
                afterProps["Tm_PlannedWorkInDate"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(plannedDate);
            }
        }
    }
}

