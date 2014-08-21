﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData;
using Microsoft.SharePoint.Utilities;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.Infrastructure;

using TM.Utils;
using TM.SP.BCSModels.CoordinateV5;
using CoordinateV5File = TM.SP.BCSModels.CoordinateV5.File;

namespace TM.SP.DataMigrationTimerJob
{
    public class CoordinateV5DataMigrationTimerJob : SPJobDefinition
    {
        #region resource strings

        private static readonly string FeatureId = "{785b2032-a102-44b8-a747-08121f2a9d0b}";

        private static readonly string RequestCT                = GetFeatureLocalizedResource("RequestEntityName");
        private static readonly string ServiceCT                = GetFeatureLocalizedResource("ServiceEntityName");
        private static readonly string ServiceHeaderCT          = GetFeatureLocalizedResource("ServiceHeaderEntityName");
        private static readonly string RequestAccountCT         = GetFeatureLocalizedResource("RequestAccountEntityName");
        private static readonly string RequestContactCT         = GetFeatureLocalizedResource("RequestContactEntityName");
        private static readonly string ServiceDocCT             = GetFeatureLocalizedResource("ServiceDocumentEntityName");
        private static readonly string TaxiInfoCT               = GetFeatureLocalizedResource("TaxiInfoEntityName");
        private static readonly string ServicePropsCT           = GetFeatureLocalizedResource("ServicePropertiesEntityName");
        private static readonly string FileCT                   = GetFeatureLocalizedResource("FileEntityName");

        private static readonly string TakeItemMethod           = GetFeatureLocalizedResource("TakeItemMethodName");
        private static readonly string UpdateMigrationStatus    = GetFeatureLocalizedResource("UpdateMigrationStatusMethodName");
        private static readonly string FinishMigration          = GetFeatureLocalizedResource("FinishMigrationMethodName");
        private static readonly string ReadRequestItem          = GetFeatureLocalizedResource("ReadRequestItemMethodName");
        private static readonly string ReadServicePropsItem     = GetFeatureLocalizedResource("ReadServicePropertiesItemMethodName");
        private static readonly string ListMissingErrorFmt      = GetFeatureLocalizedResource("ListMissingErrorFmt");
        private static readonly string SingleListValueErrorFmt  = GetFeatureLocalizedResource("SingleListValueErrorFmt");
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

        private static string GetFeatureLocalizedResource(string resourceName)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, 1033);
        }
        public CoordinateV5DataMigrationTimerJob() : base() {}

        public CoordinateV5DataMigrationTimerJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        public CoordinateV5DataMigrationTimerJob(string jobName, SPWebApplication webapp): base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = GetFeatureLocalizedResource("JobTitle");
        }

        private SPList GetListByUrlOrBreak(SPWeb web, string url)
        {
            SPList list = null;

            try
            {
                list = web.GetList("Lists/" + url);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(ListMissingErrorFmt, url));
            }

            return list;
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

        private SPListItem MigrateIncomingRequestRow(SPWeb web, Request request)
        {
            SPList list = GetListByUrlOrBreak(web, "IncomeRequestList");
            SPList govSubTypeList = GetListByUrlOrBreak(web, "GovServiceSubTypeBookList");
            Service svc = ExecBCSMethod<Service>(ServiceCT, ReadServiceItem, MethodInstanceType.SpecificFinder, request.Service);

            SPListItem newItem = list.AddItem();
            // assign values
            newItem["Tm_RegNumber"] = svc.RegNum;
            newItem["Tm_SingleNumber"] = svc.ServiceNumber;
            // todo: default values
            // newItem["Tm_IncomeRequestStateLookup"] = состояние обращения
            // newItem["Tm_IncomeRequestStateInternalLookup"] = внутренний статус
            newItem["Tm_RegistrationDate"] = svc.RegDate;
            newItem["Tm_IncomeRequestForm"] = GetFeatureLocalizedResource("IncomeRequestFormDefValue");
            if (request.DeclarantRequestAccount != null)
            {
                RequestAccount account = ExecBCSMethod<RequestAccount>(RequestAccountCT, ReadRequestAccountItem, 
                    MethodInstanceType.SpecificFinder, request.DeclarantRequestAccount);
                BCS.SetBCSFieldValue(newItem, "Tm_RequestAccountBCSLookup", account);
            }
            if (request.DeclarantRequestContact != null)
            {
                RequestContact contact = ExecBCSMethod<RequestContact>(RequestContactCT, ReadRequestContactItem,
                    MethodInstanceType.SpecificFinder, request.DeclarantRequestContact);
                BCS.SetBCSFieldValue(newItem, "Tm_RequestContactBCSLookup", contact, "Id_Auto");
            }
            if (request.TrusteeRequestContact != null)
            {
                RequestContact contact = ExecBCSMethod<RequestContact>(RequestContactCT, ReadRequestContactItem,
                    MethodInstanceType.SpecificFinder, request.TrusteeRequestContact);
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
            newItem["Title"] = String.Format(RequestTitleFmt, svc.RegNum, 
                newItem["Tm_RequestAccountBCSLookup"] ?? newItem["Tm_RequestContactBCSLookup"]);
            newItem.Update();

            return newItem;
        }

        private SPListItem MigrateTaxiRow(SPWeb web, SPListItem parent, taxi_info taxiInfo)
        {
            SPList list = GetListByUrlOrBreak(web, "TaxiList");
            SPList possessionReasonList = GetListByUrlOrBreak(web, "PossessionReasonBookList");
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
            SPList list = GetListByUrlOrBreak(web, "IncomeRequestAttachList");

            SPListItem newItem = list.AddItem();
            // assign values
            newItem["Title"] = document.DocNumber;
            newItem["Tm_AttachType"] = document.DocCode;
            newItem["Tm_AttachDocNumber"] = document.DocNumber;
            newItem["Tm_AttachDocDate"] = document.DocDate;
            newItem["Tm_AttachDocSerie"] = document.DocSerie;
            newItem["Tm_AttachWhoSigned"] = document.WhoSign;
            newItem["Tm_MessageId"] = document.MessageId;
            newItem["Tm_IncomeRequestLookup"] = new SPFieldLookupValue(parent.ID, parent.Title);
            // assign attachments
            IList<CoordinateV5File> fileList = ExecBCSMethod<IList<CoordinateV5File>>(ServiceDocCT, ServiceDocumentFileList,
                MethodInstanceType.AssociationNavigator, document.Id_Auto);
            foreach (CoordinateV5File file in fileList)
            {
                MemoryStream content = ExecBCSMethod<MemoryStream>(FileCT, ReadFileItemContent, MethodInstanceType.StreamAccessor, file.Id_Auto);
                if (content != null)
                {
                    newItem.Attachments.Add(file.FileName, content.ToArray());
                }
            }
            newItem.Update();

            return newItem;
        }

        private object ExecBCSMethod(IEntity contentType, string methodName, MethodInstanceType methodType, object inParam)
        {
            List<object> args = new List<object>();
            if (inParam != null)
                args.Add(inParam);

            var parameters = args.ToArray();
            return BCS.GetDataFromMethod(BCS.LOBRequestSystemName, contentType, methodName, methodType, ref parameters);
        }

        private EntityType ExecBCSMethod<EntityType>(string contentTypeName, string methodName, MethodInstanceType methodType, object inParam)
        {
            IEntity contentType = BCS.GetEntity(SPServiceContext.Current, String.Empty, BCS.LOBRequestSystemNamespace,
                contentTypeName);
            return (EntityType)ExecBCSMethod(contentType, methodName, methodType, inParam);
        }
        private void MigrateIncomingRequest(SPWeb web)
        {
            // trying to get next entity which hasn't been migrated yet
            MigratingRequest mRequest = ExecBCSMethod<MigratingRequest>(RequestCT, TakeItemMethod, MethodInstanceType.Scalar, null);
            if (mRequest == null) return;

            try
            {
                // set entity status to designate acting
                mRequest.Status = (Int32)MigratingStatus.Processing;
                ExecBCSMethod<MigratingRequest>(RequestCT, UpdateMigrationStatus, MethodInstanceType.Updater, mRequest);

                // process request itself
                Request request = ExecBCSMethod<Request>(RequestCT, ReadRequestItem, MethodInstanceType.SpecificFinder, mRequest.RequestId);
                SPListItem spRequest = MigrateIncomingRequestRow(web, request);
                // process taxi list
                IEnumerable<taxi_info> taxiList = ExecBCSMethod<IEnumerable<taxi_info>>(ServicePropsCT, ServicePropsTaxiList, 
                    MethodInstanceType.AssociationNavigator, request.ServiceProperties);
                foreach (taxi_info taxi in taxiList)
                {
                    MigrateTaxiRow(web, spRequest, taxi);
                }
                // process service document list
                IList<ServiceDocument> docList = ExecBCSMethod<IList<ServiceDocument>>(ServiceCT, ServiceDocumentList,
                    MethodInstanceType.AssociationNavigator, request.Service);
                foreach (ServiceDocument doc in docList)
                {
                    MigrateDocumentRow(web, spRequest, doc);
                }

                mRequest.Status = (Int32)MigratingStatus.Processed;
                ExecBCSMethod<MigratingRequest>(RequestCT, FinishMigration, MethodInstanceType.Updater, mRequest);
            }
            catch (Exception ex)
            {
                mRequest.Status = (Int32)MigratingStatus.Error;
                mRequest.ErrorInfo = ex.Message;
                mRequest.StackInfo = ex.StackTrace;
                ExecBCSMethod<MigratingRequest>(RequestCT, FinishMigration, MethodInstanceType.Updater, mRequest);

                throw;
            }
        }

        private void ProcessMigration(SPWeb web)
        {
            MigrateIncomingRequest(web);
            //todo: migrate other entities
        }

        private string GetTargetWebUrlOrBreak()
        {
            string propKeyName = DataMigrationTimerJobEventReceiver.webUrlPropertyKeyName;

            string retVal = this.Properties[propKeyName].ToString();
            if (retVal == String.Empty)
                throw new Exception(String.Format(GetFeatureLocalizedResource("WebUrlMissedErrorFmt"), this.Title, propKeyName));

            return retVal;
        }

        public override void Execute(Guid targetInstanceId)
        {
            try 
	        {
                string webUrl = GetTargetWebUrlOrBreak();

                using (SPSite site = new SPSite(webUrl))
                using (SPWeb web = site.OpenWeb())
                {
                    var context = SPServiceContext.GetContext(site);
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