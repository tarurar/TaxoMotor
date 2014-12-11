using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.SP.BCSModels;
using TM.SP.BCSModels.CoordinateV5;
using TM.SP.BCSModels.Taxi;
using TM.Utils;

namespace TM.SP.AppPages
{
    public static class LicenseHelper
    {

        public static string GetChangeReasonText(SPListItem request)
        {
            var fields = new Dictionary<string, string>
            {
                {"Tm_RenewalReason_StateNumber"   , "Изменение гос. рег-го знака"},
                {"Tm_RenewalReason_AddressCompany", "Изменение адреса ЮЛ"},
                {"Tm_RenewalReason_IdentityCard"  , "Изменение данных документа удост. личность ИП"},
                {"Tm_RenewalReason_AddressPerson" , "Изменение адреса регистрации по месту жит. ИП"},
                {"Tm_RenewalReason_NameCompany"   , "Изменение наименования ЮЛ"},
                {"Tm_RenewalReason_ReorgCompany"  , "Реорганизация ЮЛ"},
                {"Tm_RenewalReason_NamePerson"    , "Изменение ФИО ИП"}
            };

            return
                fields.Where(fn => request[fn.Key] != null && Convert.ToBoolean(request[fn.Key]))
                    .Aggregate(String.Empty,
                        (current, fn) => current + (String.IsNullOrEmpty(current) ? fn.Value : "; " + fn.Value));
        }

        public static int PromoteDraftFor(SPWeb web, int incomeRequestId, int taxiId)
        {
            var rList    = web.GetListOrBreak("Lists/IncomeRequestList");
            var rItem    = rList.GetItemOrBreak(incomeRequestId);
            var taxiList = web.GetListOrBreak("Lists/TaxiList");
            var taxiItem = taxiList.GetItemById(taxiId);
            var ctId     = new SPContentTypeId(rItem["ContentTypeId"].ToString());

            var draft = BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "GetLicenseDraftForSPTaxiId",
                methodType  = MethodInstanceType.SpecificFinder
            }, taxiItem.ID);

            var declarantId = rItem["Tm_RequestAccountBCSLookup"] != null ? BCS.GetBCSFieldLookupId(rItem, "Tm_RequestAccountBCSLookup") : null;
            var declarant = declarantId != null ? SendRequestEGRULPage.GetRequestAccount((int)declarantId) : null;

            var orgHeadId = declarant != null ? declarant.RequestContact : null;
            var orgHead = orgHeadId != null
                ? BCS.ExecuteBcsMethod<RequestContact>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "RequestContact",
                    methodName  = "ReadRequestContactItem",
                    methodType  = MethodInstanceType.SpecificFinder
                }, orgHeadId)
                : null;

            var postalAddressId = declarant != null ? declarant.PostalAddress : null;
            var postalAddress = postalAddressId != null
                ? BCS.ExecuteBcsMethod<Address>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "Address",
                    methodName  = "ReadAddressItem",
                    methodType  = MethodInstanceType.SpecificFinder
                }, postalAddressId)
                : null;

            var parentId = draft.Parent;
            var parent = parentId != null
                ? BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBTaxiSystemName,
                    ns          = BCS.LOBTaxiSystemNamespace,
                    contentType = "License",
                    methodName  = "ReadLicenseItem",
                    methodType  = MethodInstanceType.SpecificFinder
                }, parentId) : null;

            draft.AccountAbbr        = "";
            draft.AddContactData     = "";
            draft.BlankNo            = taxiItem["Tm_BlankNo"] != null ? taxiItem["Tm_BlankNo"].ToString() : String.Empty;
            draft.BlankSeries        = taxiItem["Tm_BlankSeries"] != null ? taxiItem["Tm_BlankSeries"].ToString() : String.Empty;
            draft.Building           = postalAddress != null ? postalAddress.Building : "";
            draft.CancellationReason = "";
            draft.ChangeReason       = GetChangeReasonText(rItem);
            draft.City               = postalAddress != null ? postalAddress.City : "";
            draft.Country            = postalAddress != null ? postalAddress.Country : "";
            draft.Date_OD            = null;
            draft.Document           = "";
            draft.EMail              = declarant != null ?  declarant.EMail : "";
            draft.Facility           = postalAddress != null ? postalAddress.Facility : "";
            draft.Fax                = declarant != null ? declarant.Fax : "" ;
            draft.FirstName          = orgHead != null ? orgHead.FirstName : "";
            draft.Flat               = postalAddress != null ? postalAddress.Flat : "";
            draft.FromPortal         = rItem["Tm_RegNumber"] != null;
            draft.Guid_OD            = ctId == rList.ContentTypes["Новое"].Id ? "" : (parent != null ? parent.Guid_OD : "");
            draft.House              = postalAddress != null ? postalAddress.House : "";
            draft.Inn                = declarant != null ? declarant.Inn : "";
            draft.InvalidReason      = "";
            draft.JuridicalAddress   = declarant != null ? declarant.SingleStrPostalAddress : "";
            draft.LastName           = orgHead != null ? orgHead.LastName : "";
            draft.Lfb                = declarant != null ? declarant.OrgFormCode.Trim() : "";
            draft.Locality           = postalAddress != null ? postalAddress.Locality : "";
            draft.Ogrn               = declarant != null ? declarant.Ogrn : "";
            draft.OgrnDate           = declarant != null ? declarant.OgrnDate : null;
            draft.OrgName            = declarant != null ? (draft.Lfb == "91" ? declarant.Name : declarant.FullName) : "";
            draft.Ownership          = postalAddress != null ? postalAddress.Ownership : "";
            draft.PhoneNumber        = declarant != null ? declarant.Phone : "";
            draft.PostalCode         = postalAddress != null ? postalAddress.PostalCode : "";
            draft.Region             = postalAddress != null ? postalAddress.Region : "";
            draft.SecondName         = orgHead != null ? orgHead.MiddleName : "";
            draft.ShortName          = declarant != null ? declarant.Name : "";
            draft.Signature          = "";

            if (ctId == rList.ContentTypes["Новое"].Id || ctId == rList.ContentTypes["Переоформление"].Id )
            {
                draft.Status = (int)LicenseStatus.Origin;
            } else if (ctId == rList.ContentTypes["Выдача дубликата"].Id)
            {
                draft.Status = (int)LicenseStatus.Duplicate;
            } else if (ctId == rList.ContentTypes["Аннулирование"].Id)
            {
                draft.Status = (int)LicenseStatus.Cancelled;
            }

            draft.Street             = postalAddress != null ? postalAddress.Street : "";
            draft.Structure          = postalAddress != null ? postalAddress.Structure : "";
            draft.SuspensionReason   = "";
            draft.TaxiBrand          = taxiItem["Tm_TaxiBrand"] != null ?  taxiItem["Tm_TaxiBrand"].ToString() : "";
            draft.TaxiColor          = taxiItem["Tm_TaxiBodyColor"] != null ?  taxiItem["Tm_TaxiBodyColor"].ToString() : "";
            draft.TaxiModel          = taxiItem["Tm_TaxiModel"] != null ?  taxiItem["Tm_TaxiModel"].ToString() : "";
            draft.TaxiNumberColor    = taxiItem["Tm_TaxiStateNumberYellow"] != null ?  taxiItem["Tm_TaxiStateNumberYellow"].ToString() : "";
            draft.TaxiStateNumber    = taxiItem["Tm_TaxiStateNumber"] != null ?  taxiItem["Tm_TaxiStateNumber"].ToString() : "";
            draft.TaxiYear           = (int?) taxiItem["Tm_TaxiYear"];
            draft.TillSuspensionDate = null;
            draft.Town               = postalAddress != null ? postalAddress.Town : "";

            var newLicenseDate = DateTime.Now.AddYears(5).AddDays(-1).Date;
            if (ctId == rList.ContentTypes["Новое"].Id)
            {
                draft.TillDate = newLicenseDate;
            } else if (ctId == rList.ContentTypes["Аннулирование"].Id)
            {
                draft.TillDate = DateTime.Now.Date;
            } else if (ctId == rList.ContentTypes["Выдача дубликата"].Id ||
                        ctId == rList.ContentTypes["Переоформление"].Id)
            {
                draft.TillDate = parent != null ? parent.TillDate : newLicenseDate;
            }

            BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "UpdateLicense",
                methodType  = MethodInstanceType.Updater
            }, draft);

            return draft.Id;
        }
    }
}
