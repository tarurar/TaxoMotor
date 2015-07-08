using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.SP.BCSModels;
using TM.SP.BCSModels.CoordinateV5;
using TM.SP.BCSModels.Taxi;
using TM.Utils;
using CamlexNET;
using System.Linq.Expressions;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;
using TM.Utils.Attributes;

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
            var rList                = web.GetListOrBreak("Lists/IncomeRequestList");
            var possessionReasonList = web.GetListOrBreak("Lists/PossessionReasonBookList");
            var rItem                = rList.GetItemOrBreak(incomeRequestId);
            var taxiList             = web.GetListOrBreak("Lists/TaxiList");
            var attachList           = web.GetListOrBreak("Lists/IncomeRequestAttachList");
            var taxiItem             = taxiList.GetItemById(taxiId);
            var ctId                 = new SPContentTypeId(rItem["ContentTypeId"].ToString());

            var draft = LicenseHelper.GetLicenseDraftForSPTaxiId(taxiItem.ID);
            #region [getting declarant and orgHead objects]
            var declarantId = rItem["Tm_RequestAccountBCSLookup"] != null ? BCS.GetBCSFieldLookupId(rItem, "Tm_RequestAccountBCSLookup") : null;
            var declarant = declarantId != null ? IncomeRequestHelper.ReadRequestAccountItem((int)declarantId) : null;

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

            #endregion
            #region [getting addresses]
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

            var factAddressId = declarant != null ? declarant.FactAddress : null;
            var factAddress = factAddressId != null
                ? BCS.ExecuteBcsMethod<Address>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "Address",
                    methodName  = "ReadAddressItem",
                    methodType  = MethodInstanceType.SpecificFinder
                }, factAddressId)
                : null;
            #endregion
            #region [getting parent license]
            var parentId = draft.Parent;
            var parent = parentId != null && parentId.HasValue ? LicenseHelper.ReadLicenseItem(parentId.Value) : null;
            #endregion
            #region [getting taxi posession item]
            string posessionItemServiceCode = null;
            var taxiOwnTypeValue = taxiItem.TryGetValue<string>("Tm_PossessionReasonLookup");
            if (!String.IsNullOrEmpty(taxiOwnTypeValue))
            {
                var taxiOwnTypeLookup = new SPFieldLookupValue(taxiOwnTypeValue);
                var posessionItem = possessionReasonList.GetItemOrNull(taxiOwnTypeLookup.LookupId);
                if (posessionItem != null)
                {
                    posessionItemServiceCode = posessionItem.TryGetValue<string>("Tm_ServiceCode");
                }
            }
            #endregion
            #region [getting taxi own document]
            var ownNumber = String.Empty;
            DateTime? ownDate = null;

            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // checking for exactly this taxi
                x => x["Tm_TaxiLookup"] == (DataTypes.LookupId) taxiId.ToString()
            };
            SPListItemCollection docItems = attachList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='Recursive'"
            });

            SPListItem docItem = null;
            if (docItems.Count == 1)
            {
                docItem = docItems[0];
            } else if (docItems.Count > 1)
            {
                var filteredDocs = docItems.Cast<SPListItem>().Where(x => (int)x["Tm_AttachType"] != 347);
                docItem = filteredDocs.Count() > 0 ? filteredDocs.First() : docItems[0];
            }
            if (docItem != null)
            {
                var docSerie = docItem.TryGetValue<string>("Tm_AttachDocSerie");
                var docNumber = docItem.TryGetValue<string>("Tm_AttachDocNumber");
                ownNumber = String.Format("{0} {1}", docSerie, docNumber).Trim();
                ownDate = docItem.TryGetValueOrNull<DateTime>("Tm_AttachDocDate");
            }
            #endregion

            #region [setting common values]
            draft.AccountAbbr        = "";
            draft.AddContactData     = "";
            draft.CancellationReason = "";
            draft.ChangeReason       = GetChangeReasonText(rItem);
            draft.Date_OD            = null;
            draft.Document           = "";
            draft.FromPortal         = rItem["Tm_RegNumber"] != null;
            draft.Guid_OD            = ctId == rList.ContentTypes["Новое"].Id ? "" : (parent != null ? parent.Guid_OD : "");
            draft.InvalidReason      = "";
            draft.Signature          = "";
            draft.SuspensionReason   = "";
            draft.TillSuspensionDate = null;
            draft.OwnType            = String.IsNullOrEmpty(posessionItemServiceCode) ? null : (int?)Convert.ToInt32(posessionItemServiceCode);
            draft.OwnNumber          = ownNumber;
            draft.OwnDate            = ownDate;

            if (ctId == rList.ContentTypes["Новое"].Id || ctId == rList.ContentTypes["Переоформление"].Id)
            {
                draft.Status = (int)LicenseStatus.Origin;
            }
            else if (ctId == rList.ContentTypes["Выдача дубликата"].Id)
            {
                draft.Status = (int)LicenseStatus.Duplicate;
            }
            else if (ctId == rList.ContentTypes["Аннулирование"].Id)
            {
                draft.Status = (int)LicenseStatus.Cancelled;
            }
            #endregion
            #region [setting fact address]
            draft.Address_Fact       = declarant.SingleStrFactAddress;
            draft.Country_Fact       = factAddress != null ? factAddress.Country : "";
            draft.PostalCode_Fact    = factAddress != null ? factAddress.PostalCode : "";
            draft.Locality_Fact      = factAddress != null ? factAddress.Locality : "";
            draft.Region_Fact        = factAddress != null ? factAddress.Region : "";
            draft.City_Fact          = factAddress != null ? factAddress.City : "";
            draft.Town_Fact          = factAddress != null ? factAddress.Town : "";
            draft.Street_Fact        = factAddress != null ? factAddress.Street : "";
            draft.House_Fact         = factAddress != null ? factAddress.House : "";
            draft.Building_Fact      = factAddress != null ? factAddress.Building : "";
            draft.Structure_Fact     = factAddress != null ? factAddress.Structure : "";
            draft.Facility_Fact      = factAddress != null ? factAddress.Facility : "";
            draft.Ownership_Fact     = factAddress != null ? factAddress.Ownership : "";
            draft.Flat_Fact          = factAddress != null ? factAddress.Flat : "";
            #endregion
            #region [setting values from taxi item]
            draft.Gps                = taxiItem.TryGetValue<bool>("Tm_TaxiGps");
            draft.Taxometr           = taxiItem.TryGetValue<bool>("Tm_TaxiTaxometer");
            draft.TODate             = taxiItem.TryGetValueOrNull<DateTime>("Tm_TaxiLastToDate");
            draft.STSNumber          = taxiItem.TryGetValue<string>("Tm_TaxiStsDetails");
            draft.STSDate            = null;

            if (ctId == rList.ContentTypes["Аннулирование"].Id)
            {
                draft.BlankNo = parent != null ? parent.BlankNo : String.Empty;
                draft.BlankSeries = parent != null ? parent.BlankSeries : String.Empty;
            }
            else
            {
                draft.BlankNo = taxiItem.TryGetValue<string>("Tm_BlankNo");
                draft.BlankSeries = taxiItem.TryGetValue<string>("Tm_BlankSeries");
            }

            draft.TaxiBrand          = taxiItem.TryGetValue<string>("Tm_TaxiBrand");
            draft.TaxiColor          = taxiItem.TryGetValue<string>("Tm_TaxiBodyColor");
            draft.TaxiModel          = taxiItem.TryGetValue<string>("Tm_TaxiModel");
            draft.TaxiNumberColor    = taxiItem.TryGetValue<bool>("Tm_TaxiStateNumberYellow") ? "1" : "0";
            draft.TaxiStateNumber    = taxiItem.TryGetValue<string>("Tm_TaxiStateNumber");
            draft.TaxiYear           = taxiItem.TryGetValue<int>("Tm_TaxiYear");
            draft.TaxiVin            = taxiItem.TryGetValue<string>("Tm_TaxiVin");
            #endregion
            #region [setting postal address] 
            draft.Building           = postalAddress != null ? postalAddress.Building : "";
            draft.City               = postalAddress != null ? postalAddress.City : "";
            draft.Country            = postalAddress != null ? postalAddress.Country : "";
            draft.Facility           = postalAddress != null ? postalAddress.Facility : "";
            draft.Flat               = postalAddress != null ? postalAddress.Flat : "";
            draft.House              = postalAddress != null ? postalAddress.House : "";
            draft.Locality           = postalAddress != null ? postalAddress.Locality : "";
            draft.Ownership          = postalAddress != null ? postalAddress.Ownership : "";
            draft.PostalCode         = postalAddress != null ? postalAddress.PostalCode : "";
            draft.Region             = postalAddress != null ? postalAddress.Region : "";
            draft.Street             = postalAddress != null ? postalAddress.Street : "";
            draft.Structure          = postalAddress != null ? postalAddress.Structure : "";
            draft.Town               = postalAddress != null ? postalAddress.Town : "";
            #endregion
            #region [setting values from declarant]
            draft.EMail            = declarant != null ? declarant.EMail : "";
            draft.Fax              = declarant != null ? declarant.Fax : "";
            draft.Inn              = declarant != null ? declarant.Inn : "";
            draft.InnDate          = declarant != null ? declarant.InnDate : null;
            draft.InnName          = declarant != null ? declarant.InnAuthority : "";
            draft.InnNum           = declarant != null ? declarant.InnNum : "";
            draft.JuridicalAddress = declarant != null ? declarant.SingleStrPostalAddress : "";
            draft.FirstName        = orgHead != null ? orgHead.FirstName : "";
            draft.LastName         = orgHead != null ? orgHead.LastName : "";
            draft.Lfb              = declarant != null ? declarant.OrgFormCode.Trim() : "";
            draft.Ogrn             = declarant != null ? declarant.Ogrn : "";
            draft.OgrnDate         = declarant != null ? declarant.OgrnDate : null;
            draft.OgrnNum          = declarant != null ? declarant.OgrnNum : "";
            draft.OgrnName         = declarant != null ? declarant.OgrnAuthority : "";
            draft.OrgName          = declarant != null ? (draft.Lfb == "91" ? declarant.Name : declarant.FullName) : "";
            draft.PhoneNumber      = declarant != null ? declarant.Phone : "";
            draft.SecondName       = orgHead != null ? orgHead.MiddleName : "";
            draft.ShortName        = declarant != null ? declarant.Name : "";
            #endregion

            LicenseHelper.UpdateLicense(draft);

            return draft.Id;
        }


        public static string GetLicenseXml(int licenseId, SPWeb web)
        {
            SPList spList     = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);
            var license = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

            //serialization
            var intWriter    = new StringWriter(new StringBuilder());
            XmlWriter writer = new XmlTextWriter(intWriter);
            var serializer   = new XmlSerializer(typeof(License));
            serializer.Serialize(writer, license);

            return intWriter.ToString();
        }

        /// <summary>
        /// Получение xml разрешения из подписи
        /// </summary>
        /// <param name="licenseId">Идентификатора разрешения в SharePoint</param>
        /// <param name="web">Объект SPWeb</param>
        /// <returns>Строка с xml</returns>
        public static string GetLicenseSavedXml(int licenseId, SPWeb web)
        {
            SPList spList = web.GetListOrBreak("Lists/LicenseList");
            SPListItem spItem = spList.GetItemOrBreak(licenseId);
            var license = LicenseHelper.GetLicense(Convert.ToInt32(spItem["Tm_LicenseExternalId"]));

            return GetLicenseSavedXml(license);
        }

        /// <summary>
        /// Получение xml разрешения из подписи
        /// </summary>
        /// <param name="license">Объект разрешения (BCS)</param>
        /// <returns></returns>
        public static string GetLicenseSavedXml(License license)
        {
            var savedXml = String.Empty;
            if (!String.IsNullOrEmpty(license.Signature))
            {
                XDocument xdoc = XDocument.Parse(license.Signature);
                var licenseNode = xdoc.Descendants().Where(n => n.Name.LocalName == "License").FirstOrDefault();
                if (licenseNode != null)
                {
                    savedXml = licenseNode.ToString();
                }
            }

            return savedXml;
        }

        public static License GetLicense(int? id)
        {
            if (id == null || id == 0)
                throw new Exception("Item id must be specified");

            return LicenseHelper.ReadLicenseItem(id.Value); 
        }

        /// <summary>
        /// Получение списка свойств объекта разрешения (BCS), которые необходимо сравнивать с клоном в SharePoint на равенство
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetLicenseFieldsToCompare()
        {
            Func<MemberInfo, bool> predicate = (mi) => 
            {
                var xmlAttrib = mi.GetCustomAttribute<XmlElementAttribute>();
                var spAttrib = mi.GetCustomAttribute<SharepointFieldAttribute>();

                return xmlAttrib != null && spAttrib != null;
            };

            Type licenseType = typeof(License);

            return 
                licenseType.GetFields().Where(predicate)
                .Concat(
                licenseType.GetProperties().Where(predicate));
        }

        /// <summary>
        /// Возвращает значение CN из поля SubjectName сертификата
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        public static string GetCNFromSubjectName(string subjectName)
        {
            var searchString = "CN=";
            var tokens = subjectName.Split(',');
            return tokens
                    .Select(t => t.Trim())
                    .Where(t => t.StartsWith(searchString))
                    .Select(t => t.Substring(searchString.Length))
                    .FirstOrDefault();
        }

        /// <summary>
        /// Сериализация разрешения в xml
        /// </summary>
        /// <param name="license"></param>
        /// <returns>Строка в виде xml</returns>
        public static string Serialize(License license)
        {
            var intWriter  = new StringWriter(new StringBuilder());
            var writer     = new XmlTextWriter(intWriter);
            var serializer = new XmlSerializer(typeof(License));

            writer.WriteStartElement("Data");
            serializer.Serialize(writer, license);
            writer.WriteEndElement();

            return intWriter.ToString();
        }

        public static License GetUnsignedLicense()
        {
            return BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "TakeAnyUnsignedLicense",
                methodType  = MethodInstanceType.Scalar
            }, null);
        }

        public static void UpdateLicense(License license)
        {
            BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "UpdateLicense",
                methodType  = MethodInstanceType.Updater
            }, license);
        }

        public static void DeleteLicenseDraftForSPTaxiId(int taxiId)
        {
            BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "DeleteLicenseDraftForSPTaxiIdInstance",
                methodType  = MethodInstanceType.Updater
            }, taxiId);
        }

        public static License GetLicenseDraftForSPTaxiId(int taxiId)
        {
            return BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "GetLicenseDraftForSPTaxiId",
                methodType  = MethodInstanceType.SpecificFinder
            }, taxiId);
        }

        public static License CreateLicense(License license)
        {
            return BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "CreateLicense",
                methodType  = MethodInstanceType.Creator
            }, license);
        }

        public static License ReadLicenseItem(int licenseId)
        {
            return BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "ReadLicenseItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, licenseId);
        }

        public static License GetAnyLicenseForSPTaxiId(int taxiId)
        {
            return BCS.ExecuteBcsMethod<License>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBTaxiSystemName,
                ns          = BCS.LOBTaxiSystemNamespace,
                contentType = "License",
                methodName  = "GetAnyLicenseForSPTaxiId",
                methodType  = MethodInstanceType.SpecificFinder
            }, taxiId);
        }
    }
}
