// <copyright file="erAttachLibItem.cs" company="CompanyName">
// Copyright CompanyName. All rights reserved.
// </copyright>
// <author>TAXOMOTOR\developer</author>
// <date>2015-04-09 16:53:08Z</date>
namespace TM.SP.Customizations
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using TM.Utils;
    using System.IO;

    /// <summary>
    /// TODO: Add comment for erAttachLibItem
    /// </summary>
    public class erAttachLibItem : SPItemEventReceiver
    {
        private static readonly string NameFn = "Имя";
        /// <summary>
        /// TODO: Add comment for event ItemAdding in erAttachLibItem 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            SPListItem item = properties.ListItem;
            if (item[NameFn] == null) return;

            var currentCtId = new SPContentTypeId(item["ContentTypeId"].ToString());
            var folderCtId = properties.List.ContentTypes.BestMatch(new SPContentTypeId("0x0120"));
            var notFolder = currentCtId != folderCtId;
            var rootLocation = item.File.ParentFolder.Url == item.ParentList.RootFolder.Url;
            // автоматические изменения имен добавляемых файлов работают только для файлов, добавляемых в корневой каталог библиотеки
            if (notFolder && rootLocation)
            {
                EventFiringEnabled = false;
                try
                {
                    Utility.WithSafeUpdate(properties.Web, (safeWeb) =>
                    {
                        var oldFileName = item.TryGetValue<string>(NameFn);
                        var fnNoExt = Path.GetFileNameWithoutExtension(oldFileName);
                        var fileId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
                        var newFileName = String.Format("{0}_{1}{2}", fnNoExt, fileId, Path.GetExtension(oldFileName));
                        newFileName = Utility.MakeFileNameSharePointCompatible(newFileName);

                        item.TrySetValue<string>(NameFn, newFileName);
                        item.Update();
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
        }
    }
}

