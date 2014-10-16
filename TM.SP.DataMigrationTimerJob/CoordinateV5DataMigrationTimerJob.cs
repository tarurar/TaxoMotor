using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData;
using Microsoft.SharePoint.Utilities;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.Infrastructure;

using TM.Utils;
using TM.SP.BCSModels;
using TM.SP.BCSModels.CoordinateV5;
using TM.SP.BCSModels.Taxi;
using TM.SP.BCSModels.TaxiV2;
using CoordinateV5File = TM.SP.BCSModels.CoordinateV5.File;
using CamlexNET;

namespace TM.SP.DataMigrationTimerJob
{

    public class CoordinateV5DataMigrationTimerJob : SPJobDefinition
    {
        #region resource strings

        private static readonly string FeatureId           = "{785b2032-a102-44b8-a747-08121f2a9d0b}";
        public static readonly string CV5ListsFeatureId    = "{88749623-db7e-4ffc-b1e4-b6c4cf9332b6}";
        public static readonly string TaxiListsFeatureId   = "{fd2daa37-e95d-4e98-b360-2f8390c3f2ba}";
        public static readonly string TaxiV2ListsFeatureId = "{38cd390b-fda5-434c-8f3b-2810dee6c8a1}";

        private static readonly string RequestCT                = GetFeatureLocalizedResource("RequestEntityName");
        private static readonly string ServiceCT                = GetFeatureLocalizedResource("ServiceEntityName");
        private static readonly string ServiceHeaderCT          = GetFeatureLocalizedResource("ServiceHeaderEntityName");
        private static readonly string RequestAccountCT         = GetFeatureLocalizedResource("RequestAccountEntityName");
        private static readonly string RequestContactCT         = GetFeatureLocalizedResource("RequestContactEntityName");
        private static readonly string ServiceDocCT             = GetFeatureLocalizedResource("ServiceDocumentEntityName");
        private static readonly string TaxiInfoCT               = GetFeatureLocalizedResource("TaxiInfoEntityName");
        private static readonly string ServicePropsCT           = GetFeatureLocalizedResource("ServicePropertiesEntityName");
        private static readonly string FileCT                   = GetFeatureLocalizedResource("FileEntityName");
        private static readonly string LicenseCT                = GetFeatureLocalizedResource("LicenseEntityName");
        private static readonly string LicenseV2AllViewCT       = GetFeatureLocalizedResource("LicenseV2AllViewEntityName");

        private static readonly string TakeItemMethod           = GetFeatureLocalizedResource("TakeItemMethodName");
        private static readonly string UpdateMigrationStatus    = GetFeatureLocalizedResource("UpdateMigrationStatusMethodName");
        private static readonly string FinishMigration          = GetFeatureLocalizedResource("FinishMigrationMethodName");

        private static readonly string ReadRequestItem          = GetFeatureLocalizedResource("ReadRequestItemMethodName");
        private static readonly string ReadLicenseAllViewItem   = GetFeatureLocalizedResource("ReadLicenseAllViewItemMethodName");
        private static readonly string ReadLicenseItem          = GetFeatureLocalizedResource("ReadLicenseItemMethodName");
        private static readonly string ReadServicePropsItem     = GetFeatureLocalizedResource("ReadServicePropertiesItemMethodName");
        private static readonly string ListMissingErrorFmt      = GetFeatureLocalizedResource("ListMissingErrorFmt");
        private static readonly string SingleListValueErrorFmt  = GetFeatureLocalizedResource("SingleListValueErrorFmt");
        private static readonly string SingleParentItemErrorFmt = GetFeatureLocalizedResource("SingleParentItemErrorFmt");
        private static readonly string ReadServiceItem          = GetFeatureLocalizedResource("ReadServiceItemMethodName");
        private static readonly string ReadServiceHeaderItem    = GetFeatureLocalizedResource("ReadServiceHeaderItemMethodName");
        private static readonly string ReadRequestAccountItem   = GetFeatureLocalizedResource("ReadRequestAccountItemMethodName");
        private static readonly string ReadRequestContactItem   = GetFeatureLocalizedResource("ReadRequestContactItemMethodName");
        private static readonly string RequestTitleFmt          = GetFeatureLocalizedResource("RequestTitleFmt");
        private static readonly string TaxiTitleFmt             = GetFeatureLocalizedResource("TaxiTitleFmt");
        private static readonly string ServicePropsTaxiList     = GetFeatureLocalizedResource("ServicePropertiesTaxiListMethodName");
        private static readonly string ServiceDocumentList      = GetFeatureLocalizedResource("ServiceDocumentListMethodName");
        private static readonly string ServiceDocumentFileList  = GetFeatureLocalizedResource("ServiceDocumentFileListMethodName");
        private static readonly string ReadFileItemContent      = GetFeatureLocalizedResource("ReadFileItemContentMethodName");

        #endregion

        #region consts
        // Service type constants
        private const string scNew = "77200101";
        private const string scRenew = "020202";
        private const string scDuplicate = "020203";
        private const string scCancellation = "020204";

        #endregion

        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }
        public CoordinateV5DataMigrationTimerJob() : base() { }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        private List<SPListItem> GetListItemsByFieldValue(SPList list, string fn, string match)
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
        private SPListItem GetSingleListItemByFieldValue(SPList list, string fn, string match)
        {
            List<SPListItem> items = GetListItemsByFieldValue(list, fn, match);
            if (items.Count > 1)
                throw new Exception(String.Format(SingleListValueErrorFmt, list.Title, fn));

            return items.Count > 0 ? items[0] : null;
        }

        /// <summary>
        /// Assign base field values that present in any content type
        /// </summary>
        /// <returns></returns>
        private SPListItem AssignIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            Service svc = ExecBCSMethod<Service>(new BcsMethodExecutionInfo() { 
                contentType = ServiceCT, 
                lob         = BCS.LOBRequestSystemName, 
                methodName  = ReadServiceItem, 
                methodType  = MethodInstanceType.SpecificFinder, 
                ns          = BCS.LOBRequestSystemNamespace }, request.Service);
            SPList govSubTypeList = web.GetListOrBreak("Lists/GovServiceSubTypeBookList");

            newItem["Tm_RegNumber"] = svc.RegNum;
            newItem["Tm_SingleNumber"] = svc.ServiceNumber;
            // todo: default values
            // newItem["Tm_IncomeRequestStateLookup"] = состояние обращения
            // newItem["Tm_IncomeRequestStateInternalLookup"] = внутренний статус
            newItem["Tm_RegistrationDate"] = svc.RegDate;
            newItem["Tm_IncomeRequestForm"] = GetFeatureLocalizedResource("IncomeRequestFormDefValue");
            if (request.DeclarantRequestAccount != null)
            {
                RequestAccount account = ExecBCSMethod<RequestAccount>(new BcsMethodExecutionInfo()
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestAccountCT,
                    methodName  = ReadRequestAccountItem,
                    methodType  = MethodInstanceType.SpecificFinder
                }, request.DeclarantRequestAccount);
                
                BCS.SetBCSFieldValue(newItem, "Tm_RequestAccountBCSLookup", account);
            }
            if (request.DeclarantRequestContact != null)
            {
                RequestContact contact = ExecBCSMethod<RequestContact>(new BcsMethodExecutionInfo()
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestContactCT,
                    methodName  = ReadRequestContactItem,
                    methodType  = MethodInstanceType.SpecificFinder
                }, request.DeclarantRequestContact);

                BCS.SetBCSFieldValue(newItem, "Tm_RequestContactBCSLookup", contact, "Id_Auto");
            }
            if (request.TrusteeRequestContact != null)
            {
                RequestContact contact = ExecBCSMethod<RequestContact>(new BcsMethodExecutionInfo()
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestContactCT,
                    methodName  = ReadRequestContactItem,
                    methodType  = MethodInstanceType.SpecificFinder
                }, request.TrusteeRequestContact);

                BCS.SetBCSFieldValue(newItem, "Tm_RequestTrusteeBcsLookup", contact, "Id_Auto");
            }
            if (!String.IsNullOrEmpty(svc.ServiceTypeCode))
            {
                SPListItem serviceCode = GetSingleListItemByFieldValue(govSubTypeList, "Tm_ServiceCode", svc.ServiceTypeCode);
                if (serviceCode != null)
                    newItem["Tm_RequestedDocument"] = serviceCode.ID;
            }
            newItem["Tm_InstanceCounter"] = svc.Copies;
            newItem["Tm_RequestedDocumentPrice"] = svc.ServicePrice;
            newItem["Tm_PrepareTargetDate"] = svc.PrepareTargetDate;
            newItem["Tm_OutputTargetDate"] = svc.OutputTargetDate;
            newItem["Tm_PrepareFactDate"] = svc.PrepareFactDate;
            newItem["Tm_OutputFactDate"] = svc.OutputFactDate;
            newItem["Tm_MessageId"] = request.MessageId;
            newItem["Title"] = String.Format(RequestTitleFmt, svc.RegNum, newItem["Tm_RequestAccountBCSLookup"] ?? newItem["Tm_RequestContactBCSLookup"]);

            return newItem;
        }

        private SPListItem AssignNewIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            return AssignIncomeRequestFieldValues(web, newItem, request);
        }

        private SPListItem AssignDuplicateIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            return AssignIncomeRequestFieldValues(web, newItem, request);
        }

        private SPListItem AssignRenewIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            ServiceProperties svcProps = ExecBCSMethod<ServiceProperties>(new BcsMethodExecutionInfo()
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = ServicePropsCT,
                methodName  = ReadServicePropsItem,
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

        private SPListItem AssignCancelIncomeRequestFieldValues(SPWeb web, SPListItem newItem, Request request)
        {
            ServiceProperties svcProps = ExecBCSMethod<ServiceProperties>(new BcsMethodExecutionInfo()
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = ServicePropsCT,
                methodName  = ReadServicePropsItem,
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

        private SPListItem MigrateIncomingRequestRow(SPWeb web, Request request)
        {
            Service svc = ExecBCSMethod<Service>(new BcsMethodExecutionInfo()
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = ServiceCT,
                methodName  = ReadServiceItem,
                methodType  = MethodInstanceType.SpecificFinder
            }, request.Service);
            
            SPList list = web.GetListOrBreak("Lists/IncomeRequestList");
            SPListItem newItem = list.AddItem();

            switch (svc.ServiceTypeCode)
            {
                case scNew:
                    newItem = AssignNewIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Tm_NewIncomeRequest"].Id;
                    break;
                case scRenew:
                    newItem = AssignRenewIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Tm_RenewIncomeRequest"].Id;
                    break;
                case scDuplicate:
                    newItem = AssignDuplicateIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Tm_DuplicateIncomeRequest"].Id;
                    break;
                case scCancellation:
                    newItem = AssignCancelIncomeRequestFieldValues(web, newItem, request);
                    newItem["ContentTypeId"] = list.ContentTypes["Tm_CancelIncomeRequest"].Id;
                    break;
                default:
                    throw new Exception(String.Format("Unknown income request ServiceTypeCode value {0}", svc.ServiceTypeCode));
            }

            newItem.Update();
            return newItem;
        }

        private SPListItem MigrateLicenseRow(SPWeb web, License license)
        {
            SPList list     = web.GetListOrBreak("Lists/LicenseList");
            SPList taxiList = web.GetListOrBreak("Lists/TaxiList");
            
            string yearStr        = license.CreationDate.HasValue ? license.CreationDate.Value.Year.ToString() : "noDate";
            string monthstr       = license.CreationDate.HasValue ? license.CreationDate.Value.ToString("MMM", CultureInfo.CurrentCulture) : "noDate";
            string num            = license.RegNumber;
            SPFolder parentFolder = list.RootFolder.CreateSubFolders(new string[] { yearStr, monthstr, num });

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
            string status = String.Empty;
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
            LicenseAllView licenseAllViewLookup = ExecBCSMethod<LicenseAllView>(new BcsMethodExecutionInfo()
            {
                lob         = BCS.LOBTaxiV2SystemName,
                ns          = BCS.LOBTaxiV2SystemNamespace,
                contentType = LicenseV2AllViewCT,
                methodName  = ReadLicenseAllViewItem,
                methodType  = MethodInstanceType.SpecificFinder
            }, license.Id);
            BCS.SetBCSFieldValue(newItem, "Tm_LicenseAllViewBcsLookup", licenseAllViewLookup);
            // parent lookup
            if (license.Parent.HasValue)
            {
                SPListItemCollection parentLicenses = list.GetItems(new SPQuery()
                {
                    Query = Camlex.Query().Where(x => (int)x["Tm_LicenseExternalId"] == license.Parent.Value).ToString(),
                    ViewAttributes = "Scope='RecursiveAll'"
                });

                if (parentLicenses.Count != 1)
                    throw new Exception(String.Format(SingleParentItemErrorFmt, list.Title, license.Parent.Value, license.Id, parentLicenses.Count));

                newItem["Tm_LicenseParentLicenseLookup"] = new SPFieldLookupValue(parentLicenses[0].ID, parentLicenses[0].Title);
            }
            newItem["ContentTypeId"] = list.ContentTypes["Tm_License"].Id;

            newItem.Update();
            return newItem;
        }

        private SPListItem MigrateTaxiRow(SPWeb web, SPListItem parent, taxi_info taxiInfo)
        {
            SPList list = web.GetListOrBreak("Lists/TaxiList");
            SPList possessionReasonList = web.GetListOrBreak("Lists/PossessionReasonBookList");
            SPListItem newItem = list.AddItem();
            // assign values
            newItem["Tm_TaxiBrand"] = taxiInfo.brand;
            newItem["Tm_TaxiModel"] = taxiInfo.model;
            newItem["Tm_TaxiYear"] = taxiInfo.year;
            newItem["Tm_TaxiLastToDate"] = taxiInfo.todate;
            newItem["Tm_TaxiStateNumber"] = taxiInfo.num;
            newItem["Tm_LeasingContractDetails"] = taxiInfo.lizdetails;
            if (taxiInfo.doc != null)
            {
                var possessionReasonLookup = GetSingleListItemByFieldValue(
                    possessionReasonList, "Tm_ServiceCode", taxiInfo.doc.ToString());
                if (possessionReasonLookup != null)
                    newItem["Tm_PossessionReasonLookup"] = new SPFieldLookupValue(possessionReasonLookup.ID, possessionReasonLookup.Title);
            }
            newItem["Tm_TaxiStsDetails"] = taxiInfo.details;
            newItem["Tm_TaxiBodyYellow"] = taxiInfo.color_yellow;
            newItem["Tm_TaxiBodyColor"] = taxiInfo.color;
            newItem["Tm_TaxiBodyColor2"] = taxiInfo.color_2;
            newItem["Tm_TaxiStateNumberYellow"] = taxiInfo.color_number;
            newItem["Tm_TaxiTaxometer"] = taxiInfo.taxometr;
            newItem["Tm_TaxiGps"] = taxiInfo.gps;
            newItem["Tm_TaxiPrevStateNumber"] = taxiInfo.num2;
            newItem["Tm_TaxiBlankNo"] = taxiInfo.blankno;
            newItem["Tm_TaxiInfoOld"] = taxiInfo.taxi_info_old;
            newItem["Tm_TaxiPrevLicenseNumber"] = taxiInfo.number_ran;

            DateTime prevLicenseDate;
            if (DateTime.TryParse(taxiInfo.date_ran, out prevLicenseDate))
                newItem["Tm_TaxiPrevLicenseDate"] = prevLicenseDate;

            newItem["Tm_MessageId"] = taxiInfo.MessageId;
            newItem["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
            newItem["Title"] = String.Format(TaxiTitleFmt, taxiInfo.brand, taxiInfo.num);
            newItem.Update();

            return newItem;
        }

        private SPListItem MigrateDocumentRow(SPWeb web, SPListItem parent, ServiceDocument document)
        {
            SPList list = web.GetListOrBreak("Lists/IncomeRequestAttachList");

            SPListItem newAttach = list.AddItem();
            DateTime validityPeriod;
            // assign values
            newAttach["Title"] = document.DocNumber;
            newAttach["Tm_AttachType"] = document.DocCode;
            newAttach["Tm_AttachDocNumber"] = document.DocNumber;
            newAttach["Tm_AttachDocDate"] = document.DocDate;
            newAttach["Tm_AttachDocSerie"] = document.DocSerie;
            newAttach["Tm_AttachWhoSigned"] = document.WhoSign;
            newAttach["Tm_AttachSubType"] = document.DocSubType;
            /*
             * В процессе обсуждения было решено отказаться от переноса значений поля DocPerson
             * newAttach["Tm_AttachDocPersonBcsLookup"] = document.DocPerson; 
             */
            newAttach["Tm_AttachListCount"] = document.ListCount;
            newAttach["Tm_AttachCopyCount"] = document.CopyCount;
            newAttach["Tm_AttachDivisionCode"] = document.DivisionCode;
            newAttach["Tm_MessageId"] = document.MessageId;
            newAttach["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
            if (DateTime.TryParse(document.ValidityPeriod, out validityPeriod))
                newAttach["Tm_AttachValidityPeriod"] = validityPeriod;
            newAttach.Update();
            // add attachment files
            var attachLib = web.GetListOrBreak("AttachLib");
            var parentFolder = attachLib.RootFolder.CreateSubFolders(new string[] { 
                DateTime.Now.Year.ToString(), 
                DateTime.Now.Month.ToString(), 
                parent.Title });

            IList<CoordinateV5File> fileList = ExecBCSMethod<IList<CoordinateV5File>>(new BcsMethodExecutionInfo() 
            { 
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = ServiceDocCT,
                methodName  = ServiceDocumentFileList,
                methodType  = MethodInstanceType.AssociationNavigator
            }, document.Id_Auto);
            
            foreach (CoordinateV5File file in fileList)
            {
                MemoryStream content = ExecBCSMethod<MemoryStream>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = FileCT,
                    methodName  = ReadFileItemContent,
                    methodType  = MethodInstanceType.StreamAccessor
                }, file.Id_Auto);

                if (content != null)
                {
                    var uplFolder = parentFolder.CreateSubFolders(new string[] { (parentFolder.ItemCount + 1).ToString() });
                    var attachFile = uplFolder.Files.Add(file.FileName, content);
                    uplFolder.Update();

                    attachFile.Item["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
                    attachFile.Item["Tm_IncomeRequestAttachLookup"] = new SPFieldLookupValue(newAttach.ID, newAttach.Title);
                    attachFile.Item.Update();
                }
            }

            return newAttach;
        }

        private object ExecBCSMethod(IEntity contentType, BcsMethodExecutionInfo methodInfo, object inParam)
        {
            List<object> args = new List<object>();
            if (inParam != null)
                args.Add(inParam);

            var parameters = args.ToArray();
            return BCS.GetDataFromMethod(methodInfo.lob, contentType, methodInfo.methodName, methodInfo.methodType, ref parameters);
        }

        private EntityType ExecBCSMethod<EntityType>(BcsMethodExecutionInfo methodInfo , object inParam)
        {
            IEntity contentType = BCS.GetEntity(SPServiceContext.Current, String.Empty, methodInfo.ns, methodInfo.contentType);
            return (EntityType)ExecBCSMethod(contentType, methodInfo, inParam);
        }
        private void MigrateIncomingRequest(SPWeb web)
        {
            // trying to get next entity which hasn't been migrated yet
            MigratingRequest mRequest = ExecBCSMethod<MigratingRequest>(new BcsMethodExecutionInfo() 
            { 
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = RequestCT,
                methodName  = TakeItemMethod,
                methodType  = MethodInstanceType.Scalar
            }, null);
            
            if (mRequest == null) return;

            try
            {
                // set entity status to designate acting
                mRequest.Status = (Int32)MigratingStatus.Processing;
                ExecBCSMethod<MigratingRequest>(new BcsMethodExecutionInfo() 
                { 
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestCT,
                    methodName  = UpdateMigrationStatus,
                    methodType  = MethodInstanceType.Updater
                }, mRequest);

                // process request itself
                Request request = ExecBCSMethod<Request>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestCT,
                    methodName  = ReadRequestItem,
                    methodType  = MethodInstanceType.SpecificFinder
                }, mRequest.RequestId);
                SPListItem spRequest = MigrateIncomingRequestRow(web, request);
                // process taxi list
                IEnumerable<taxi_info> taxiList = ExecBCSMethod<IEnumerable<taxi_info>>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = ServicePropsCT,
                    methodName  = ServicePropsTaxiList,
                    methodType  = MethodInstanceType.AssociationNavigator
                }, request.ServiceProperties);
                foreach (taxi_info taxi in taxiList)
                {
                    MigrateTaxiRow(web, spRequest, taxi);
                }
                // process service document list
                IList<ServiceDocument> docList = ExecBCSMethod<IList<ServiceDocument>>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = ServiceCT,
                    methodName  = ServiceDocumentList,
                    methodType  = MethodInstanceType.AssociationNavigator
                }, request.Service);
                foreach (ServiceDocument doc in docList)
                {
                    MigrateDocumentRow(web, spRequest, doc);
                }

                mRequest.Status = (Int32)MigratingStatus.Processed;
                ExecBCSMethod<MigratingRequest>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestCT,
                    methodName  = FinishMigration,
                    methodType  = MethodInstanceType.Updater
                }, mRequest);
            }
            catch (Exception ex)
            {
                mRequest.Status = (Int32)MigratingStatus.Error;
                mRequest.ErrorInfo = ex.Message;
                mRequest.StackInfo = ex.StackTrace;
                ExecBCSMethod<MigratingRequest>(new BcsMethodExecutionInfo()
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = RequestCT,
                    methodName  = FinishMigration,
                    methodType  = MethodInstanceType.Updater
                }, mRequest);

                throw;
            }
        }
        private void MigrateLicense(SPWeb web)
        {
            // trying to get next entity which hasn't been migrated yet
            MigratingLicense mLicense = ExecBCSMethod<MigratingLicense>(new BcsMethodExecutionInfo() 
            { 
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = LicenseCT,
                methodName  = TakeItemMethod,
                methodType  = MethodInstanceType.Scalar
            }, null);
            if (mLicense == null) return;

            try
            {
                // set entity status to designate acting
                mLicense.Status = (Int32)MigratingStatus.Processing;
                ExecBCSMethod<MigratingLicense>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBTaxiSystemName,
                    ns          = BCS.LOBTaxiSystemNamespace,
                    contentType = LicenseCT,
                    methodName  = UpdateMigrationStatus,
                    methodType  = MethodInstanceType.Updater
                }, mLicense);

                // process license itself
                License license = ExecBCSMethod<License>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBTaxiSystemName,
                    ns          = BCS.LOBTaxiSystemNamespace,
                    contentType = LicenseCT,
                    methodName  = ReadLicenseItem,
                    methodType  = MethodInstanceType.SpecificFinder
                }, mLicense.LicenseId);
                SPListItem spLicense = MigrateLicenseRow(web, license);

                mLicense.Status = (Int32)MigratingStatus.Processed;
                ExecBCSMethod<MigratingLicense>(new BcsMethodExecutionInfo() 
                {
                    lob         = BCS.LOBTaxiSystemName,
                    ns          = BCS.LOBTaxiSystemNamespace,
                    contentType = LicenseCT,
                    methodName  = FinishMigration,
                    methodType  = MethodInstanceType.Updater
                }, mLicense);
            }
            catch (Exception ex)
            {
                mLicense.Status = (Int32)MigratingStatus.Error;
                mLicense.ErrorInfo = ex.Message;
                mLicense.StackInfo = ex.StackTrace;
                ExecBCSMethod<MigratingLicense>(new BcsMethodExecutionInfo()
                {
                    lob         = BCS.LOBTaxiSystemName,
                    ns          = BCS.LOBTaxiSystemNamespace,
                    contentType = LicenseCT,
                    methodName  = FinishMigration,
                    methodType  = MethodInstanceType.Updater
                }, mLicense);

                throw;
            }
        }

        private void ProcessMigration(SPWeb web)
        {
            if (web.Features[new Guid(CV5ListsFeatureId)] != null)
            {
                MigrateIncomingRequest(web);
            }

            if (web.Features[new Guid(TaxiListsFeatureId)] != null && web.Features[new Guid(TaxiV2ListsFeatureId)] != null)
            {
                MigrateLicense(web);
            }
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                SPWebApplication webApp = this.Parent as SPWebApplication;
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    SPWeb web = siteCollection.RootWeb;
                    var context = SPServiceContext.GetContext(siteCollection);
                    using (var scope = new SPServiceContextScope(context))
                    {
                        ProcessMigration(web);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(GetFeatureLocalizedResource("MigrationGeneralErrorFmt"), ex.Message));
            }
        }


    }
}
