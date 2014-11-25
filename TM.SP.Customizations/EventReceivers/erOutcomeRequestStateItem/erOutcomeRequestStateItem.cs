// <copyright file="erOutcomeRequestStateItem.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-10-29 14:29:21Z</date>

using System.Globalization;
using CamlexNET;

// ReSharper disable CheckNamespace
namespace TM.SP.Customizations
// ReSharper restore CheckNamespace
{
    using System;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Utils;

// ReSharper disable InconsistentNaming
    public class erOutcomeRequestStateItem : SPItemEventReceiver
// ReSharper restore InconsistentNaming
    {
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(properties.Site.ID))
                    using (var web = site.OpenWeb(properties.RelativeWebUrl))
                    {
                        var context = SPServiceContext.GetContext(site);
                        using (new SPServiceContextScope(context))
                        {
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                DoItemUpdating(properties, web);
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                            
                        }
                    }
                });
            }
            catch(Exception ex)
            {
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.ErrorMessage = "erOutcomeRequestStateItem.ItemUpdating receiver error. Details: " + ex.Message;
                return;
            }
            finally
            {
                EventFiringEnabled = true;    
            }

            base.ItemUpdating(properties);
        }

        private static void DoItemUpdating(SPItemEventProperties properties, SPWeb web)
        {
            var afterPropAnswered = properties.AfterProperties["Tm_AnswerReceived"];
            var answered = afterPropAnswered != null &&
                           (afterPropAnswered.ToString() == "1" || afterPropAnswered.ToString() == "-1" ||
                            Convert.ToBoolean(afterPropAnswered));

            if (!properties.IsFieldChanged("Tm_AnswerReceived") || !answered) return;

            var rList              = web.GetListOrBreak("Lists/IncomeRequestList");
            var rStatusList        = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var rLookupField       = properties.List.Fields.GetFieldByInternalName("Tm_IncomeRequestLookup") as SPFieldLookup;
            var rStatusLookupField = rList.Fields.GetFieldByInternalName("Tm_IncomeRequestStateLookup") as SPFieldLookup;

            var rLookup = properties.AfterProperties["Tm_IncomeRequestLookup"] ??
                          properties.ListItem["Tm_IncomeRequestLookup"];

            SPListItem rItem;
            SPListItem rStatusItem;
            if (Utility.TryGetListItemFromLookupValue(rLookup, rLookupField, out rItem) && 
                Utility.TryGetListItemFromLookupValue(rItem["Tm_IncomeRequestStateLookup"], rStatusLookupField, out rStatusItem))
            {
                var rStatusCode = rStatusItem["Tm_ServiceCode"];
                if (rStatusCode != null && (string)rStatusCode == "1110")
                {
                    var orList = web.GetListOrBreak("Lists/OutcomeRequestStateList");

                    // check all outcome requests for uncompleteness
                    SPListItemCollection uncompleteRequests = orList.GetItems(new SPQuery
                    {
                        Query =
                            Camlex.Query()
                                .Where(
                                    x =>
                                        x["Tm_IncomeRequestLookup"] == (DataTypes.LookupId)rItem.ID.ToString(CultureInfo.InvariantCulture) &&
                                        (bool)x["Tm_AnswerReceived"] == false)
                                .ToString()
                    });
                    // the current item hasn't been updated yet so we have to concern about it
                    if (uncompleteRequests.Count <= 1)
                    {
                        var rCompletedStatusItem =
                            rStatusList.GetSingleListItemByFieldValue("Tm_ServiceCode", "6420");
                        if (rCompletedStatusItem != null)
                        {
                            rItem["Tm_IncomeRequestStateLookup"] =
                                new SPFieldLookupValue(rCompletedStatusItem.ID, rCompletedStatusItem.Title);
                            rItem.SystemUpdate();
                        }
                    }
                }
            }
        }
    }
}

