using System;
using System.Globalization;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.SP.BCSModels.TaxiV2;
using License = TM.SP.BCSModels.Taxi.License;
using TM.Utils;
using CamlexNET;

namespace TP.SP.DataMigration
{
    public static class LicenseMigrator
    {
        private static SPFieldLookupValue GetLicenseParentLookupValue(License license, int parentId, SPList parentList)
        {
            SPListItemCollection parentLicenses = parentList.GetItems(new SPQuery
            {
                Query = Camlex.Query().Where(x => (int)x["Tm_LicenseExternalId"] == parentId).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            });

            if (parentLicenses.Count != 1)
                throw new Exception(String.Format("Expected single parent item in a sharepoint list {0}. Parent external id = {1}. Item external id = {2}. Currently, there are {3} parent items in sharepoint list.", parentList.Title, parentId, license.Id, parentLicenses.Count));

            return new SPFieldLookupValue(parentLicenses[0].ID, parentLicenses[0].Title);
        }
        public static SPListItem Execute(SPWeb web, License license)
        {
            SPList list = web.GetListOrBreak("Lists/LicenseList");
            SPList taxiList = web.GetListOrBreak("Lists/TaxiList");

            string yearStr = license.CreationDate.HasValue ? license.CreationDate.Value.Year.ToString(CultureInfo.CurrentCulture) : "noDate";
            string monthstr = license.CreationDate.HasValue ? license.CreationDate.Value.ToString("MMM", CultureInfo.CurrentCulture) : "noDate";
            string num = license.RegNumber;
            SPFolder parentFolder = list.RootFolder.CreateSubFolders(new[] { yearStr, monthstr, num });

            SPListItem newItem = list.AddItem(parentFolder.ServerRelativeUrl, SPFileSystemObjectType.File);

            newItem["Title"]                          = license.RegNumber;
            newItem["Tm_BlankSeries"]                 = license.BlankSeries;
            newItem["Tm_BlankNo"]                     = license.BlankNo;
            newItem["Tm_OrganizationName"]            = license.OrgName;
            newItem["Tm_OrgOgrn"]                     = license.Ogrn;
            newItem["Tm_OrgInn"]                      = license.Inn;
            newItem["Tm_OrgLfb"]                      = license.Lfb;
            newItem["Tm_JuridicalAddress"]            = license.JuridicalAddress;
            newItem["Tm_PhoneNumber"]                 = license.PhoneNumber;
            newItem["Tm_AddContactData"]              = license.AddContactData;
            newItem["Tm_JuridicalPersonAbbreviation"] = license.AccountAbbr;
            newItem["Tm_LicenseOutputDate"]           = license.OutputDate;
            newItem["Tm_LicenseTillDate"]             = license.TillDate;
            newItem["Tm_LicenseTillSuspensionDate"]   = license.TillSuspensionDate;
            newItem["Tm_LicenseCancellationReason"]   = license.CancellationReason;
            newItem["Tm_LicenseSuspensionReason"]     = license.SuspensionReason;
            newItem["Tm_LicenseChangeReason"]         = license.ChangeReason;
            newItem["TmLicenseInvalidReason"]         = license.InvalidReason;
            newItem["Tm_TaxiYear"]                    = license.TaxiYear;
            newItem["Tm_TaxiStateNumber"]             = license.TaxiStateNumber;
            newItem["Tm_TaxiBrand"]                   = license.TaxiBrand;
            newItem["Tm_TaxiModel"]                   = license.TaxiModel;
            newItem["Tm_RegNumber"]                   = license.RegNumber;
            newItem["Tm_LicenseExternalId"]           = license.Id;

            // license status
            string status;
            switch (license.Status)
            {
                case 0:
                    status = "Оригинал";
                    break;
                case 1:
                    status = "Дубль";
                    break;
                case 2:
                    status = "Приостановлено";
                    break;
                case 3:
                    status = "Аннулировано";
                    break;
                default:
                    status = "Оригинал";
                    break;
            }
            newItem["Tm_LicenseStatus"] = status;
            // taxi lookup
            if (license.TaxiId != null)
            {
                SPListItem taxiItem = taxiList.GetItemOrBreak((int)license.TaxiId);
                newItem["Tm_TaxiLookup"] = new SPFieldLookupValue(taxiItem.ID, taxiItem.Title);
            }
            // external link to LicenseAllView
            var licenseAllViewLookup = BCS.ExecuteBcsMethod<LicenseAllView>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiV2SystemName,
                ns          = BCS.LOBTaxiV2SystemNamespace,
                contentType = "LicenseAllView",
                methodName  = "ReadLicenseAllViewItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, license.Id);
            BCS.SetBCSFieldValue(newItem, "Tm_LicenseAllViewBcsLookup", licenseAllViewLookup);
            // parent lookup
            if (license.Parent.HasValue)
                newItem["Tm_LicenseParentLicenseLookup"] = GetLicenseParentLookupValue(license, license.Parent.Value, list);
            // root parent lookup
            if (license.RootParent.HasValue)
                newItem["Tm_LicenseRtParentLicenseLookup"] = GetLicenseParentLookupValue(license, license.RootParent.Value, list);
            newItem["ContentTypeId"] = list.ContentTypes["Tm_License"].Id;

            newItem.Update();
            return newItem;
        }
    }
}
