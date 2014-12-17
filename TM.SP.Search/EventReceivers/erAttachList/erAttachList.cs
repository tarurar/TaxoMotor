// <copyright file="erAttachList.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDOMAIN\dev1</author>
// <date>2014-12-08 17:54:00Z</date>

using CamlexNET;
using TM.Utils;

namespace TM.SP.Search
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;

    /// <summary>
    /// TODO: Add comment for erAttachList
    /// </summary>
    public class erAttachList : SPItemEventReceiver
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

