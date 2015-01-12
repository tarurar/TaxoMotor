using System;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint;
using TM.Utils;
using TM.Services.CoordinateV5;

namespace TM.SP.AppPages
{
    public class IncomeRequestMessageBuilder
    {
        #region [fields]
        private readonly SPWeb _web;
        private readonly SPListItem _request;
        #endregion

        #region [methods]
        public IncomeRequestMessageBuilder(SPWeb web, int incomeRequestId)
        {
            _web = web;
            _request = _web.GetListOrBreak("Lists/IncomeRequestList").GetItemById(incomeRequestId);
        }

        public BaseDeclarant GetDeclarant()
        {
            var raLookupValue = _request["Tm_RequestAccountBCSLookup"];
            var raLookupId = raLookupValue != null
                ? BCS.GetBCSFieldLookupId(_request, "Tm_RequestAccountBCSLookup")
                : null;
            if (raLookupId == null) return null;

            var declarant = SendRequestEGRULPage.GetRequestAccount((int)raLookupId);
            if (declarant == null) return null;

            var orgHeadId = declarant.RequestContact;
            var orgHead = orgHeadId != null
                ? BCS.ExecuteBcsMethod<BCSModels.CoordinateV5.RequestContact>(new BcsMethodExecutionInfo
                {
                    lob         = BCS.LOBRequestSystemName,
                    ns          = BCS.LOBRequestSystemNamespace,
                    contentType = "RequestContact",
                    methodName  = "ReadRequestContactItem",
                    methodType  = MethodInstanceType.SpecificFinder
                }, orgHeadId)
                : null;

            return new RequestAccount
            {
                BankBik       = declarant.BankBik,
                BankName      = declarant.BankName,
                BrandName     = declarant.BrandName,
                CorrAccount   = declarant.CorrAccount,
                EMail         = declarant.EMail,
                FactAddress   = null,
                Fax           = declarant.Fax,
                FullName      = declarant.FullName,
                Inn           = declarant.Inn,
                InnAuthority  = declarant.InnAuthority,
                InnDate       = declarant.InnDate,
                InnNum        = declarant.InnNum,
                Kpp           = declarant.Kpp,
                Name          = declarant.Name,
                Ogrn          = declarant.Ogrn,
                OgrnAuthority = declarant.OgrnAuthority,
                OgrnDate      = declarant.OgrnDate,
                OgrnNum       = declarant.OgrnNum,
                Okfs          = declarant.Okfs,
                Okpo          = declarant.Okpo,
                Okved         = declarant.Okved,
                OrgFormCode   = declarant.OrgFormCode,
                OrgHead = orgHead != null ? new RequestContact
                {
                    BirthAddress      = null,
                    BirthDate         = orgHead.BirthDate,
                    Citizenship       = orgHead.Citizenship,
                    CitizenshipType   = null,
                    EMail             = orgHead.EMail,
                    FactAddress       = null,
                    FirstName         = orgHead.FirstName,
                    Gender            = null,
                    HomePhone         = orgHead.HomePhone,
                    Id                = orgHead.Id,
                    Inn               = orgHead.Inn,
                    IsiId             = orgHead.IsiId,
                    JobTitle          = orgHead.JobTitle,
                    LastName          = orgHead.LastName,
                    MiddleName        = orgHead.MiddleName,
                    MobilePhone       = orgHead.MobilePhone,
                    Nation            = orgHead.Nation,
                    OMSCompany        = orgHead.OMSCompany,
                    OMSDate           = orgHead.OMSDate,
                    OMSNum            = orgHead.OMSNum,
                    OMSValidityPeriod = null,
                    RegAddress        = null,
                    Snils             = orgHead.Snils,
                    WorkPhone         = orgHead.WorkPhone
                } : null,
                Phone         = declarant.Phone,
                PostalAddress = null,
                SetAccount    = declarant.SetAccount,
                WebSite       = declarant.WebSite
            };
        }

        public Department GetDepartment()
        {
            return new Department
            {
                Code    = Consts.TransportDepCode,
                Inn     = "",
                Name    = Consts.TaxoMotorDepName,
                Ogrn    = "",
                RegDate = null
            };
        }

        public RequestService GetService()
        {
            var rDocument = _request["Tm_RequestedDocument"] == null
                ? 0
                : new SPFieldLookupValue(_request["Tm_RequestedDocument"].ToString()).LookupId;

            var stList = _web.GetListOrBreak("Lists/GovServiceSubTypeBookList");
            var stItem = stList.GetItemOrNull(rDocument);
            var sCode = stItem == null
                ? String.Empty
                : (stItem["Tm_ServiceCode"] == null ? String.Empty : stItem["Tm_ServiceCode"].ToString());

            return new RequestService
            {
                Copies              = null,
                CreatedByDepartment = GetDepartment(),
                DeclineReasonCodes  = null,
                Department          = GetDepartment(),
                Documents           = null,
                OutputFactDate      = null,
                OutputTargetDate    = null,
                PrepareFactDate     = null,
                PrepareTargetDate   = null,
                RegDate             = (DateTime?)_request["Tm_ApplyDate"],
                RegNum              = (string)_request["Tm_SingleNumber"],
                Responsible = new Person
                {
                    Email      = _web.CurrentUser.Email,
                    FirstName  = _web.CurrentUser.Name,
                    IsiId      = "",
                    JobTitle   = _web.CurrentUser.LoginName,
                    LastName   = "",
                    MiddleName = "",
                    Phone      = ""
                },
                ServiceNumber   = (string)_request["Tm_SingleNumber"],
                ServicePrice    = null,
                ServiceTypeCode = sCode
            };
        }

        public RequestContact GetTrustee()
        {
            var rcLookupValue = _request["Tm_RequestTrusteeBcsLookup"];
            var rcLookupId = rcLookupValue != null
                ? BCS.GetBCSFieldLookupId(_request, "Tm_RequestTrusteeBcsLookup")
                : null;
            if (rcLookupId == null) return null;

            var trustee = BCS.ExecuteBcsMethod<BCSModels.CoordinateV5.RequestContact>(new BcsMethodExecutionInfo
            {
                lob         = BCS.LOBRequestSystemName,
                ns          = BCS.LOBRequestSystemNamespace,
                contentType = "RequestContact",
                methodName  = "ReadRequestContactItem",
                methodType  = MethodInstanceType.SpecificFinder
            }, rcLookupId);
            if (trustee == null) return null;

            return new RequestContact
            {
                BirthAddress      = null,
                BirthDate         = trustee.BirthDate,
                Citizenship       = trustee.Citizenship,
                CitizenshipType   = null,
                EMail             = trustee.EMail,
                FactAddress       = null,
                FirstName         = trustee.FirstName,
                Gender            = null,
                HomePhone         = trustee.HomePhone,
                Id                = trustee.Id,
                Inn               = trustee.Inn,
                IsiId             = trustee.IsiId,
                JobTitle          = trustee.JobTitle,
                LastName          = trustee.LastName,
                MiddleName        = trustee.MiddleName,
                MobilePhone       = trustee.MobilePhone,
                Nation            = trustee.Nation,
                OMSCompany        = trustee.OMSCompany,
                OMSDate           = trustee.OMSDate,
                OMSNum            = trustee.OMSNum,
                OMSValidityPeriod = null,
                RegAddress        = null,
                Snils             = trustee.Snils,
                WorkPhone         = trustee.WorkPhone
            };
        }

        public CoordinateMessage SynthesizeV5()
        {
            return new CoordinateMessage
            {
                Message = new CoordinateData
                {
                    Contacts         = null,
                    CustomAttributes = null,
                    Declarant        = GetDeclarant(),
                    Service          = GetService(),
                    Signature        = null,
                    Trustee          = GetTrustee(),
                },
                ServiceHeader = new Headers
                {
                    FromOrgCode     = Consts.TransportDepCode,
                    MessageId       = Guid.NewGuid().ToString("D"),
                    RelatesTo       = "",
                    RequestDateTime = DateTime.Now,
                    ServiceNumber   = (string)_request["Tm_SingleNumber"],
                    ToOrgCode       = Consts.AsgufSysCode
                }
            };
        }

        #endregion


    }
}
