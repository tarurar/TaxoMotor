﻿using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Security;
using CamlexNET;
using TM.Utils;

namespace TM.SP.Search.AttachListReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class AttachListReceiver : SPItemEventReceiver
    {
        private static SPListItem GetIdentityDocTypeListItem(string serviceCode, SPWeb web)
        {
            var list = web.GetListOrBreak("Lists/IdentityDocumentTypeBookList");
            SPListItemCollection items = list.GetItems(new SPQuery()
            {
                Query = Camlex.Query().Where(x => (string)x["Tm_ServiceCode"] == serviceCode).ToString(),
            });

            return items.Count > 0 ? items[0] : null;
        }

        private static string CalcDocString(SPItemEventDataCollection newFieldValues, SPWeb web)
        {
            var attachType = newFieldValues["Tm_AttachType"];
            var attachDocSerie = newFieldValues["Tm_AttachDocSerie"];
            var attachDocNumber = newFieldValues["Tm_AttachDocNumber"];
            var attachDocDate = newFieldValues["Tm_AttachDocDate"];
            var attachTypeNameToGet = default(string);

            if (attachType != null)
            {
                SPListItem item = GetIdentityDocTypeListItem(attachType.ToString(), web);
                if (item != null)
                    attachTypeNameToGet = item["Title"] != null ? item["Title"].ToString() : null;
            }

            string a =
                ((attachType != null ? attachType.ToString() + " " : String.Empty) +
                (attachTypeNameToGet != null ? attachTypeNameToGet + " " : String.Empty) +
                (attachDocSerie != null ? attachDocSerie.ToString() + " " : String.Empty) +
                (attachDocNumber != null ? attachDocNumber.ToString() + " " : String.Empty) +
                (attachDocDate != null ? DateTime.Parse(attachDocDate.ToString()).ToString("dd.MM.yyyy") + " " : String.Empty));

            return a.Trim();
        }
        /// <summary>
        /// TODO: Add comment for event ItemAdding in IncomeRequestAttachList 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdding(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            try
            {
                properties.AfterProperties["Tm_AttachSingleStrDocName"] = CalcDocString(properties.AfterProperties, properties.Web);
            }
            finally
            {
                EventFiringEnabled = true;
            }
        }

        /// <summary>
        /// TODO: Add comment for event ItemUpdating in IncomeRequestAttachList 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            try
            {
                properties.AfterProperties["Tm_AttachSingleStrDocName"] = CalcDocString(properties.AfterProperties, properties.Web);
            }
            finally
            {
                EventFiringEnabled = true;
            }
        }

    }
}