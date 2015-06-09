using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;
using TM.SP.BCSModels;
using System.Linq.Expressions;
using CamlexNET;

namespace TM.SP.AppPages.Validators
{
    /// <summary>
    /// Проверки на дубли при принятии в работу транспортного средства
    /// </summary>
    public class TaxiDuplicateValidator : Validator
    {
        #region [fields]
        protected SPListItem requestItem;
        protected SPListItem taxiItem;
        protected SPList requestList;
        protected SPList taxiList;
        protected SPList licenseList;
        private string _declarantOgrn = null;
        #endregion

        #region [properties]
        protected string DeclarantOgrn
        {
            get
            {
                if (_declarantOgrn != null) return _declarantOgrn;

                try
                {
                    var declarantId = BCS.GetBCSFieldLookupId(requestItem, "Tm_RequestAccountBCSLookup");
                    var declarant = SendRequestEGRULPage.GetRequestAccount((int)declarantId);
                    _declarantOgrn = declarant.Ogrn;
                }
                catch (Exception)
                {
                    throw new Exception(StringsRes.noDeclarantOgrnErr);
                }
                return _declarantOgrn;
            }
        }
        protected string PrevLicNumberFmt
        {
            get
            {
                return taxiItem.TryGetValue<string>("Tm_TaxiPrevLicenseNumber");
            }
        }
        protected string PrevLicNumber
        {
            get
            {
                var prevLicNumber = Convert.ToInt32(PrevLicNumberFmt).ToString();
                if (String.IsNullOrEmpty(prevLicNumber))
                    throw new Exception(StringsRes.noPrevLicNumberErr);

                return prevLicNumber;
            }
        }
        protected string TaxiStateNumber
        {
            get
            {
                return taxiItem.TryGetValue<string>("Tm_TaxiStateNumber");
            }
        }
        #endregion

        #region [methods]

        protected IEnumerable<SPListItem> GetActingLicensesByNumber(string regNumber)
        {
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Number) "1",
                x => (string)x["Tm_RegNumber"] == regNumber
            };

            return licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='Recursive'"
            }).Cast<SPListItem>();
        }

        public TaxiDuplicateValidator(SPWeb web, int requestItemId, int taxiItemId) : base(web)
        {
            requestList = _web.GetListOrBreak("Lists/IncomeRequestList");
            taxiList = _web.GetListOrBreak("Lists/TaxiList");
            licenseList = _web.GetListOrBreak("Lists/LicenseList");

            requestItem = requestList.GetItemOrBreak(requestItemId);
            taxiItem = taxiList.GetItemOrBreak(taxiItemId);
        }
        protected virtual bool ExecuteForNew()
        {
            #region [first condition check]
            // выбираем все действующие разрешения
            var actingLicenses = IncomeRequestService.GetTaxiNumberActingLicenses(taxiItem.ID).Cast<SPListItem>();

            // нет действующих разрешений - ок
            var noActingLicenses = !actingLicenses.Any();
            // есть, но аннулированные - ок
            var onlyCancelledLicenses = actingLicenses.All(i => (string)i["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);
            // есть, но такие, которые вскоре прекращают свое действие - ок
            var onlySoonExpired = actingLicenses.All(i => (DateTime)i["Tm_LicenseTillDate"] <= DateTime.Now.AddDays(40));
            // есть, но с установленным признаком "Устаревшие данные" - ок
            var onlyObsolete = actingLicenses.All(i => (bool?)i["Tm_LicenseObsolete"] == true);

            var firstCondition = noActingLicenses || onlyCancelledLicenses || onlySoonExpired || onlyObsolete;
            if (!firstCondition) throw new Exception(StringsRes.actingLicensesError);
            #endregion

            #region [second condition check]
            var secondCondition = false;
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew 
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition = !IncomeRequestService.HasRequestTaxiStateNumber(
                    request.ID, taxiItem["Tm_TaxiStateNumber"].ToString());

                if (!secondCondition) {
                    var errMsg = String.Empty;

                    if (request.ContentType.Name == StringsRes.ctNew)
                    {
                        errMsg = StringsRes.requestExistsForNew;
                    }
                    else if (request.ContentType.Name == StringsRes.ctDuplicate)
                    {
                        errMsg = StringsRes.requestExistsForDuplicate;
                    }
                    else if (request.ContentType.Name == StringsRes.ctRenew)
                    {
                        errMsg = StringsRes.requestExistsForRenew;
                    }

                    throw new Exception(errMsg); 
                };
            }
            #endregion

            #region [third condition check]
            var thirdCondition = false;
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition = !IncomeRequestService.HasRequestTaxiStateNumber(
                    request.ID, taxiItem["Tm_TaxiStateNumber"].ToString());

                if (!thirdCondition) throw new Exception(StringsRes.requestExistsForCancellation);
            }
            #endregion

            return true;
        }
        protected virtual bool ExecuteForRenew()
        {
            #region [first condition check]
            var actingLicenses = GetActingLicensesByNumber(PrevLicNumber);

            var hasActingLicenses = actingLicenses.Any();
            var ogrnMatches       = actingLicenses.All(l => (string)l["Tm_OrgOgrn"] == DeclarantOgrn);
            var noCancellation    = !actingLicenses.Any(l => (string)l["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);

            var firstCondition = hasActingLicenses && ogrnMatches && noCancellation;
            if (!firstCondition)
            {
                var errMsg = String.Empty;
                if (!hasActingLicenses)
                {
                    errMsg = StringsRes.noActingLicenseForRenewErr;
                } 
                else if (!ogrnMatches) 
                {
                    errMsg = StringsRes.ogrnMismatchErr;
                }
                else if (!noCancellation)
                {
                    errMsg = StringsRes.licenseCancelledErr;
                }

                throw new Exception(errMsg);
            }
            #endregion

            #region [second condition check]
            var secondCondition = false;
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition = 
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, TaxiStateNumber) && 
                    !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, PrevLicNumberFmt);

                if (!secondCondition)
                {
                    var errMsg = String.Empty;

                    if (request.ContentType.Name == StringsRes.ctNew)
                    {
                        errMsg = StringsRes.requestExistsForNew;
                    }
                    else if (request.ContentType.Name == StringsRes.ctDuplicate)
                    {
                        errMsg = StringsRes.requestExistsForDuplicate;
                    }
                    else if (request.ContentType.Name == StringsRes.ctRenew)
                    {
                        errMsg = StringsRes.requestExistsForRenew;
                    }

                    throw new Exception(errMsg);
                };
            }
            #endregion

            #region [third condition check]
            var thirdCondition = false;
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition = 
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, TaxiStateNumber) && 
                    !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, PrevLicNumberFmt);

                if (!thirdCondition) throw new Exception(StringsRes.requestExistsForCancellation);
            }
            #endregion

            return true;
        }
        protected virtual bool ExecuteForDuplicate() 
        {
            #region [first condition check]
            var actingLicenses = GetActingLicensesByNumber(PrevLicNumber);

            var hasActingLicenses = actingLicenses.Any();
            var ogrnMatches       = actingLicenses.All(l => (string)l["Tm_OrgOgrn"] == DeclarantOgrn);
            var stateNumMatches   = actingLicenses.All(l => (string)l["Tm_TaxiStateNumber"] == TaxiStateNumber);
            var noCancellation    = !actingLicenses.Any(l => (string)l["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);

            var firstCondition = hasActingLicenses && ogrnMatches && stateNumMatches && noCancellation;
            if (!firstCondition)
            {
                var errMsg = String.Empty;
                if (!hasActingLicenses)
                {
                    errMsg = StringsRes.noActingLicenseForDuplicateErr;
                }
                else if (!ogrnMatches)
                {
                    errMsg = StringsRes.ogrnMismatchErr;
                }
                else if (!stateNumMatches)
                {
                    errMsg = StringsRes.stateNumMismatchErr;
                }
                else if (!noCancellation)
                {
                    errMsg = StringsRes.licenseCancelledErr;
                }

                throw new Exception(errMsg);
            }
            #endregion

            #region [second condition check]
            var secondCondition = false;
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, TaxiStateNumber) && 
                    !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, PrevLicNumberFmt);

                if (!secondCondition)
                {
                    var errMsg = String.Empty;

                    if (request.ContentType.Name == StringsRes.ctNew)
                    {
                        errMsg = StringsRes.requestExistsForNew;
                    }
                    else if (request.ContentType.Name == StringsRes.ctDuplicate)
                    {
                        errMsg = StringsRes.requestExistsForDuplicate;
                    }
                    else if (request.ContentType.Name == StringsRes.ctRenew)
                    {
                        errMsg = StringsRes.requestExistsForRenew;
                    }

                    throw new Exception(errMsg);
                };
            }
            #endregion

            #region [third condition check]
            var thirdCondition = false;
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, TaxiStateNumber) && 
                    !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, PrevLicNumberFmt);

                if (!thirdCondition) throw new Exception(StringsRes.requestExistsForCancellation);
            }
            #endregion

            return true;
        }
        protected virtual bool ExecuteForCancellation() 
        {
            #region [first condition check]
            var actingLicenses = GetActingLicensesByNumber(PrevLicNumber);

            var hasActingLicenses = actingLicenses.Any();
            var ogrnMatches       = actingLicenses.All(l => (string)l["Tm_OrgOgrn"] == DeclarantOgrn);
            var stateNumMatches   = actingLicenses.All(l => (string)l["Tm_TaxiStateNumber"] == TaxiStateNumber);
            var noCancellation    = !actingLicenses.Any(l => (string)l["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);

            var firstCondition = hasActingLicenses && ogrnMatches && stateNumMatches && noCancellation;
            if (!firstCondition)
            {
                var errMsg = String.Empty;
                if (!hasActingLicenses)
                {
                    errMsg = StringsRes.noActingLicenseForCancellationErr;
                }
                else if (!ogrnMatches)
                {
                    errMsg = StringsRes.ogrnMismatchErr;
                }
                else if (!stateNumMatches)
                {
                    errMsg = StringsRes.stateNumMismatchErr;
                }
                else if (!noCancellation)
                {
                    errMsg = StringsRes.licenseCancelledErr;
                }

                throw new Exception(errMsg);
            }
            #endregion

            #region [second condition check]
            var secondCondition = false;
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, TaxiStateNumber) && 
                    !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, PrevLicNumberFmt);

                if (!secondCondition)
                {
                    var errMsg = String.Empty;

                    if (request.ContentType.Name == StringsRes.ctNew)
                    {
                        errMsg = StringsRes.requestExistsForNew;
                    }
                    else if (request.ContentType.Name == StringsRes.ctDuplicate)
                    {
                        errMsg = StringsRes.requestExistsForDuplicate;
                    }
                    else if (request.ContentType.Name == StringsRes.ctRenew)
                    {
                        errMsg = StringsRes.requestExistsForRenew;
                    }

                    throw new Exception(errMsg);
                };
            }
            #endregion

            #region [third condition check]
            var thirdCondition = false;
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;7704;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, TaxiStateNumber) && 
                    !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, PrevLicNumberFmt);

                if (!thirdCondition) throw new Exception(StringsRes.requestExistsForCancellation);
            }
            #endregion

            return true;
        }
        public override bool Execute(params object[] paramsList)
        {
            var requestCtId = new SPContentTypeId(requestItem["ContentTypeId"].ToString());

            if (requestCtId == requestList.ContentTypes[StringsRes.ctNew].Id)
            {
                return ExecuteForNew();
            }
            else if (requestCtId == requestList.ContentTypes[StringsRes.ctRenew].Id)
            {
                return ExecuteForRenew();
            }
            else if (requestCtId == requestList.ContentTypes[StringsRes.ctDuplicate].Id)
            {
                return ExecuteForDuplicate();
            }
            else if (requestCtId == requestList.ContentTypes[StringsRes.ctCancellation].Id)
            {
                return ExecuteForCancellation();
            }

            return base.Execute(paramsList);
        }
        #endregion
    }
}
