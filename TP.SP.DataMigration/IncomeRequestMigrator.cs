using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.SP.BCSModels.CoordinateV5;
using CoordinateV5File = TM.SP.BCSModels.CoordinateV5.File;
using TM.Utils;

namespace TP.SP.DataMigration
{

    public static class IncomeRequestMigrator
    {
        #region consts
        // Service type constants
        private const string ScNew          = "77200101";
        private const string ScRenew        = "020202";
        private const string ScDuplicate    = "020203";
        private const string ScCancellation = "020204";

        #endregion

        private static List<SPListItem> GetListItemsByFieldValue(SPList list, string fn, string match)
        {
            List<SPListItem> matchingItems =
                (from SPListItem listItem in list.Items
                 where
                     listItem.Fields.ContainsField(fn) &&
                     listItem[fn] != null &&
                     listItem[fn].ToString().Equals(match, StringComparison.InvariantCultureIgnoreCase)
                 select listItem).ToList<SPListItem>();

            return matchingItems;
        }
        private static SPListItem GetSingleListItemByFieldValue(SPList list, string fn, string match)
        {
            List<SPListItem> items = GetListItemsByFieldValue(list, fn, match);
            if (items.Count > 1)
                throw new Exception(String.Format("Expected single value in a list {0} for specified field name {1}", list.Title, fn));

            return items.Count > 0 ? items[0] : null;
        }
        private static SPListItem MigrateIncomingRequestRow(SPWeb web, Request request)
        {
            var svc = BCS.ExecuteBcsMethod<Service>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = "Service",
                methodName  = "ReadServiceItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, request.Service);

            SPList list = web.GetListOrBreak("Lists/IncomeRequestList");
            SPListItem newItem = list.AddItem();

            switch (svc.ServiceTypeCode)
            {
                case ScNew:
                    newItem = AssignNewIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Новое"].Id;
                    break;
                case ScRenew:
                    newItem = AssignRenewIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Переоформление"].Id;
                    break;
                case ScDuplicate:
                    newItem = AssignDuplicateIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Выдача дубликата"].Id;
                    break;
                case ScCancellation:
                    newItem = AssignCancelIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Аннулирование"].Id;
                    break;
                default:
                    throw new Exception(String.Format("Unknown income request ServiceTypeCode value {0}", svc.ServiceTypeCode));
            }

            newItem.Update();
            return newItem;
        }
        /// <summary>
        /// Assign base field values that present in any content type
        /// </summary>
        /// <returns></returns>
        private static SPListItem AssignIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            var svc = BCS.ExecuteBcsMethod<Service>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = "Service",
                methodName  = "ReadServiceItem",
                methodType  = MethodInstanceType.SpecificFinder,
                
            }, request.Service);
            var govSubTypeList       = web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
            var requestStateBookList = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
            var requestStateOk       = GetSingleListItemByFieldValue(requestStateBookList, "Tm_ServiceCode", "1020");

            newItem["Tm_RegNumber"]    = svc.RegNum;
            newItem["Tm_SingleNumber"] = svc.ServiceNumber;
            if (requestStateOk != null)
                newItem["Tm_IncomeRequestStateLookup"] = new SPFieldLookupValue(requestStateOk.ID, requestStateOk.Title);
            // todo: default values
            // newItem["Tm_IncomeRequestStateInternalLookup"] = внутренний статус
            newItem["Tm_RegistrationDate"] = svc.RegDate;
            newItem["Tm_IncomeRequestForm"] = "Портал госуслуг";
            if (request.DeclarantRequestAccount != null)
            {
                var account = BCS.ExecuteBcsMethod<RequestAccount>(new BcsMethodExecutionInfo
                {
                    lob = BCS.LOBRequestSystemName,
                    ns = BCS.LOBRequestSystemNamespace,
                    contentType = "RequestAccount",
                    methodName = "ReadRequestAccountItem",
                    methodType = MethodInstanceType.SpecificFinder
                }, request.DeclarantRequestAccount);

                BCS.SetBCSFieldValue(newItem, "Tm_RequestAccountBCSLookup", account);
            }
            if (request.DeclarantRequestContact != null)
            {
                var contact = BCS.ExecuteBcsMethod<RequestContact>(new BcsMethodExecutionInfo
                {
                    lob = BCS.LOBRequestSystemName,
                    ns = BCS.LOBRequestSystemNamespace,
                    contentType = "RequestContact",
                    methodName = "ReadRequestContactItem",
                    methodType = MethodInstanceType.SpecificFinder
                }, request.DeclarantRequestContact);

                BCS.SetBCSFieldValue(newItem, "Tm_RequestContactBCSLookup", contact, "Id_Auto");
            }
            if (request.TrusteeRequestContact != null)
            {
                var contact = BCS.ExecuteBcsMethod<RequestContact>(new BcsMethodExecutionInfo
                {
                    lob = BCS.LOBRequestSystemName,
                    ns = BCS.LOBRequestSystemNamespace,
                    contentType = "RequestContact",
                    methodName = "ReadRequestContactItem",
                    methodType = MethodInstanceType.SpecificFinder
                }, request.TrusteeRequestContact);

                BCS.SetBCSFieldValue(newItem, "Tm_RequestTrusteeBcsLookup", contact, "Id_Auto");
            }
            if (!String.IsNullOrEmpty(svc.ServiceTypeCode))
            {
                SPListItem serviceCode = GetSingleListItemByFieldValue(govSubTypeList, "Tm_ServiceCode", svc.ServiceTypeCode);
                if (serviceCode != null)
                    newItem["Tm_RequestedDocument"] = serviceCode.ID;
            }
            newItem["Tm_InstanceCounter"]        = svc.Copies;
            newItem["Tm_RequestedDocumentPrice"] = svc.ServicePrice;
            newItem["Tm_PrepareTargetDate"]      = svc.PrepareTargetDate;
            newItem["Tm_OutputTargetDate"]       = svc.OutputTargetDate;
            /*
            newItem["Tm_PrepareFactDate"]        = svc.PrepareFactDate;
            newItem["Tm_OutputFactDate"]         = svc.OutputFactDate;
             */
            newItem["Tm_MessageId"]              = request.MessageId;
            newItem["Title"] = String.Format("Обращение №{0} от {1}", svc.RegNum, newItem["Tm_RequestAccountBCSLookup"] ?? newItem["Tm_RequestContactBCSLookup"]);

            return newItem;
        }
        private static SPListItem AssignNewIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            return AssignIncomeRequestFieldValues(web, newItem, request);
        }
        private static SPListItem AssignDuplicateIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            return AssignIncomeRequestFieldValues(web, newItem, request);
        }
        private static SPListItem AssignRenewIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            var svcProps = BCS.ExecuteBcsMethod<ServiceProperties>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = "ServiceProperties",
                methodName  = "ReadServicePropertiesItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, request.ServiceProperties);

            newItem["Tm_RenewalReason_StateNumber"]    = svcProps.pr_pereoformlenie;
            newItem["Tm_RenewalReason_NameCompany"]    = svcProps.pr_pereoformlenie_2;
            newItem["Tm_RenewalReason_AddressCompany"] = svcProps.pr_pereoformlenie_3;
            newItem["Tm_RenewalReason_ReorgCompany"]   = svcProps.pr_pereoformlenie_4;
            newItem["Tm_RenewalReason_NamePerson"]     = svcProps.pr_pereoformlenie_5;
            newItem["Tm_RenewalReason_AddressPerson"]  = svcProps.pr_pereoformlenie_6;
            newItem["Tm_RenewalReason_IdentityCard"]   = svcProps.pr_pereoformlenie_7;

            return AssignIncomeRequestFieldValues(web, newItem, request);
        }
        private static SPListItem AssignCancelIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            var svcProps = BCS.ExecuteBcsMethod<ServiceProperties>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = "ServiceProperties",
                methodName  = "ReadServicePropertiesItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, request.ServiceProperties);

            SPList cancellationReasonList = web.GetListOrBreak("Lists/CancellationReasonBookList");

            if (svcProps.delete != null)
            {
                SPListItem delete = GetSingleListItemByFieldValue(cancellationReasonList, "Tm_ServiceCode", svcProps.delete.ToString());
                if (delete != null)
                    newItem["Tm_CancellationReasonLookup"] = new SPFieldLookupValue(delete.ID, delete.Title);
            }
            newItem["Tm_CancellationReasonOther"] = svcProps.other;
            return AssignIncomeRequestFieldValues(web, newItem, request);
        }
        private static void MigrateTaxiRow(SPWeb web, SPListItem parent, taxi_info taxiInfo)
        {
            SPList list = web.GetListOrBreak("Lists/TaxiList");
            SPList possessionReasonList = web.GetListOrBreak("Lists/PossessionReasonBookList");
            SPListItem newItem = list.AddItem();
            // assign values
            newItem["Tm_TaxiBrand"]              = taxiInfo.brand;
            newItem["Tm_TaxiModel"]              = taxiInfo.model;
            newItem["Tm_TaxiYear"]               = taxiInfo.year;
            newItem["Tm_TaxiLastToDate"]         = taxiInfo.todate;
            newItem["Tm_TaxiStateNumber"]        = taxiInfo.num;
            newItem["Tm_LeasingContractDetails"] = taxiInfo.lizdetails;
            if (taxiInfo.doc != null)
            {
                var possessionReasonLookup = GetSingleListItemByFieldValue(
                    possessionReasonList, "Tm_ServiceCode", taxiInfo.doc.ToString());
                if (possessionReasonLookup != null)
                    newItem["Tm_PossessionReasonLookup"] = new SPFieldLookupValue(possessionReasonLookup.ID, possessionReasonLookup.Title);
            }
            newItem["Tm_TaxiStsDetails"]        = taxiInfo.details;
            newItem["Tm_TaxiBodyYellow"]        = taxiInfo.color_yellow;
            newItem["Tm_TaxiBodyColor"]         = taxiInfo.color;
            newItem["Tm_TaxiBodyColor2"]        = taxiInfo.color_2;
            newItem["Tm_TaxiStateNumberYellow"] = taxiInfo.color_number;
            newItem["Tm_TaxiTaxometer"]         = taxiInfo.taxometr;
            newItem["Tm_TaxiGps"]               = taxiInfo.gps;
            newItem["Tm_TaxiPrevStateNumber"]   = taxiInfo.num2;
            newItem["Tm_BlankNo"]               = taxiInfo.blankno;
            newItem["Tm_TaxiInfoOld"]           = taxiInfo.taxi_info_old;
            newItem["Tm_TaxiPrevLicenseNumber"] = taxiInfo.number_ran;

            DateTime prevLicenseDate;
            if (DateTime.TryParse(taxiInfo.date_ran, out prevLicenseDate))
                newItem["Tm_TaxiPrevLicenseDate"] = prevLicenseDate;

            newItem["Tm_MessageId"]           = taxiInfo.MessageId;
            newItem["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
            newItem["Title"]                  = String.Format("{0} гос. номер {1}", taxiInfo.brand, taxiInfo.num);
            newItem.Update();
        }
        private static void MigrateDocumentRow(SPWeb web, SPListItem parent, ServiceDocument document)
        {
            SPList list = web.GetListOrBreak("Lists/IncomeRequestAttachList");
            SPList docTypeBooklist = web.GetListOrBreak("Lists/DocumentTypeBookList");

            SPListItem newAttach = list.AddItem();
            DateTime validityPeriod;
            // assign values
            newAttach["Title"]              = document.DocNumber;
            newAttach["Tm_AttachType"]      = document.DocCode;
            if (!String.IsNullOrEmpty(document.DocCode))
            {
                var docTypeItem = docTypeBooklist.GetSingleListItemByFieldValue("Tm_ServiceCode", document.DocCode);
                if (docTypeItem != null)
                    newAttach["Tm_DocumentTypeLookup"] = new SPFieldLookupValue(docTypeItem.ID, docTypeItem.Title);
            }
            newAttach["Tm_AttachDocNumber"] = document.DocNumber;
            newAttach["Tm_AttachDocDate"]   = document.DocDate;
            newAttach["Tm_AttachDocSerie"]  = document.DocSerie;
            newAttach["Tm_AttachWhoSigned"] = document.WhoSign;
            newAttach["Tm_AttachSubType"]   = document.DocSubType;
            /*
             * В процессе обсуждения было решено отказаться от переноса значений поля DocPerson
             * newAttach["Tm_AttachDocPersonBcsLookup"] = document.DocPerson; 
             */
            newAttach["Tm_AttachListCount"]     = document.ListCount;
            newAttach["Tm_AttachCopyCount"]     = document.CopyCount;
            newAttach["Tm_AttachDivisionCode"]  = document.DivisionCode;
            newAttach["Tm_MessageId"]           = document.MessageId;
            newAttach["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
            if (DateTime.TryParse(document.ValidityPeriod, out validityPeriod))
                newAttach["Tm_AttachValidityPeriod"] = validityPeriod;
            newAttach.Update();
            // add attachment files
            var attachLib = web.GetListOrBreak("AttachLib");
            var parentFolder = attachLib.RootFolder.CreateSubFolders(new[] { 
                DateTime.Now.Year.ToString(CultureInfo.InvariantCulture), 
                DateTime.Now.Month.ToString(CultureInfo.InvariantCulture), 
                parent.Title });

            var fileList = BCS.ExecuteBcsMethod<IList<CoordinateV5File>>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = "ServiceDocument",
                methodName  = "Id_AutoOfServiceDocumentToServiceDocumentOfFile",
                methodType  = MethodInstanceType.AssociationNavigator
            }, document.Id_Auto);

            foreach (CoordinateV5File file in fileList)
            {
                var content = BCS.ExecuteBcsMethod<MemoryStream>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "File",
                    methodName  = "ReadFileItemContentInstance",
                    methodType  = MethodInstanceType.StreamAccessor
                }, file.Id_Auto);

                if (content != null)
                {
                    var uplFolder  = parentFolder.CreateSubFolders(new[] { (parentFolder.ItemCount + 1).ToString(CultureInfo.InvariantCulture) });
                    var randomFn   = new Random().Next(1, 999999).ToString(CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
                    var attachFile = uplFolder.Files.Add(randomFn, content);
                    uplFolder.Update();

                    attachFile.Item["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
                    attachFile.Item["Tm_IncomeRequestAttachLookup"] = new SPFieldLookupValue(newAttach.ID, newAttach.Title);
                    attachFile.Item.Update();
                }
            }
        }
        public static SPListItem Execute(SPWeb web, Request request)
        {
            SPListItem spRequest = MigrateIncomingRequestRow(web, request);
            try
            {
                // process taxi list
                var taxiList = BCS.ExecuteBcsMethod<IEnumerable<taxi_info>>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "ServiceProperties",
                    methodName  = "IdOfServicePropertiesToServicePropertiesOftaxi_info",
                    methodType  = MethodInstanceType.AssociationNavigator
                }, request.ServiceProperties);
                foreach (taxi_info taxi in taxiList)
                {
                    MigrateTaxiRow(web, spRequest, taxi);
                }
                // process service document list
                var docList = BCS.ExecuteBcsMethod<IList<ServiceDocument>>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "Service",
                    methodName  = "IdOfServiceToServiceOfServiceDocument",
                    methodType  = MethodInstanceType.AssociationNavigator
                }, request.Service);
                foreach (ServiceDocument doc in docList)
                {
                    MigrateDocumentRow(web, spRequest, doc);
                }
            }
            catch (Exception)
            {
                // marking income request with error status
                var stateList = web.GetListOrBreak("Lists/IncomeRequestStateBookList");
                var stateError = GetSingleListItemByFieldValue(stateList, "Tm_ServiceCode", "2010");
                if (stateError != null)
                {
                    spRequest["Tm_IncomeRequestStateLookup"] = new SPFieldLookupValue(stateError.ID, stateError.Title);
                    spRequest.Update();
                }
                // reraising exception for calling code to be able to process it
                throw;
            }

            return spRequest;
        }
    }
}
