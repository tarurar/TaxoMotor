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
        #endregion

        #region [methods]
        
        public TaxiDuplicateValidator(SPWeb web, int requestItemId, int taxiItemId) : base(web)
        {
            requestList = _web.GetListOrBreak("Lists/IncomeRequestList");
            taxiList = _web.GetListOrBreak("Lists/TaxiList");

            requestItem = requestList.GetItemOrBreak(requestItemId);
            taxiItem = taxiList.GetItemOrBreak(taxiItemId);
        }
        private bool ExecuteForNew()
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
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420").Cast<SPListItem>()
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
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420;1075;1030;1080;1085").Cast<SPListItem>()
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
        private bool ExecuteForRenew()
        {
            #region [getting data]
            var licenseList = _web.GetListOrBreak("Lists/LicenseList");

            var prevLicNumberFmt = taxiItem["Tm_TaxiPrevLicenseNumber"].ToString();
            var prevLicNumber = Convert.ToInt32(prevLicNumberFmt).ToString();
            if (String.IsNullOrEmpty(prevLicNumber))
                throw new Exception(StringsRes.noPrevLicNumberErr);

            var ogrn = String.Empty;
            try
            {
                var declarantId = BCS.GetBCSFieldLookupId(requestItem, "Tm_RequestAccountBCSLookup");
                var declarant = SendRequestEGRULPage.GetRequestAccount((int)declarantId);
                ogrn = declarant.Ogrn;
            }
            catch (Exception)
            {
                throw new Exception(StringsRes.noDeclarantOgrnErr);
            }
            #endregion

            #region [first condition check]
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Integer) "1",
                x => (string)x["Tm_RegNumber"] == prevLicNumber
            };

            var actingLicenses = licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            }).Cast<SPListItem>();

            var hasActingLicenses = actingLicenses.Any();
            var ogrnMatches = actingLicenses.All(l => (string)l["Tm_OrgOgrn"] == ogrn);
            var noCancellation = !actingLicenses.Any(l => (string)l["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);

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
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition = 
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, taxiItem["Tm_TaxiStateNumber"].ToString()) 
                    && !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, prevLicNumberFmt);

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
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition = 
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, taxiItem["Tm_TaxiStateNumber"].ToString())
                    && !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, prevLicNumberFmt);

                if (!thirdCondition) throw new Exception(StringsRes.requestExistsForCancellation);
            }
            #endregion

            return true;
        }
        private bool ExecuteForDuplicate() 
        {
            #region [getting data]
            var licenseList = _web.GetListOrBreak("Lists/LicenseList");

            var prevLicNumberFmt = taxiItem["Tm_TaxiPrevLicenseNumber"].ToString();
            var prevLicNumber = Convert.ToInt32(prevLicNumberFmt).ToString();
            if (String.IsNullOrEmpty(prevLicNumber))
                throw new Exception(StringsRes.noPrevLicNumberErr);

            var ogrn = String.Empty;
            try
            {
                var declarantId = BCS.GetBCSFieldLookupId(requestItem, "Tm_RequestAccountBCSLookup");
                var declarant = SendRequestEGRULPage.GetRequestAccount((int)declarantId);
                ogrn = declarant.Ogrn;
            }
            catch (Exception)
            {
                throw new Exception(StringsRes.noDeclarantOgrnErr);
            }
            #endregion

            #region [first condition check]
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Integer) "1",
                x => (string)x["Tm_RegNumber"] == prevLicNumber
            };

            var actingLicenses = licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            }).Cast<SPListItem>();

            var hasActingLicenses = actingLicenses.Any();
            var ogrnMatches = actingLicenses.All(l => (string)l["Tm_OrgOgrn"] == ogrn);
            var stateNumMatches = actingLicenses.All(l => (string)l["Tm_TaxiStateNumber"] == taxiItem["Tm_TaxiStateNumber"].ToString());
            var noCancellation = !actingLicenses.Any(l => (string)l["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);

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
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, taxiItem["Tm_TaxiStateNumber"].ToString())
                    && !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, prevLicNumberFmt);

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
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, taxiItem["Tm_TaxiStateNumber"].ToString())
                    && !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, prevLicNumberFmt);

                if (!thirdCondition) throw new Exception(StringsRes.requestExistsForCancellation);
            }
            #endregion

            return true;
        }
        private bool ExecuteForCancellation() 
        {
            #region [getting data]
            var licenseList = _web.GetListOrBreak("Lists/LicenseList");

            var prevLicNumberFmt = taxiItem["Tm_TaxiPrevLicenseNumber"].ToString();
            var prevLicNumber = Convert.ToInt32(prevLicNumberFmt).ToString();
            if (String.IsNullOrEmpty(prevLicNumber))
                throw new Exception(StringsRes.noPrevLicNumberErr);

            var ogrn = String.Empty;
            try
            {
                var declarantId = BCS.GetBCSFieldLookupId(requestItem, "Tm_RequestAccountBCSLookup");
                var declarant = SendRequestEGRULPage.GetRequestAccount((int)declarantId);
                ogrn = declarant.Ogrn;
            }
            catch (Exception)
            {
                throw new Exception(StringsRes.noDeclarantOgrnErr);
            }
            #endregion

            #region [first condition check]
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                // IsLast field - checking if license is acting
                x => x["_x0421__x0441__x044b__x043b__x04"] == (DataTypes.Integer) "1",
                x => (string)x["Tm_RegNumber"] == prevLicNumber
            };

            var actingLicenses = licenseList.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='RecursiveAll'"
            }).Cast<SPListItem>();

            var hasActingLicenses = actingLicenses.Any();
            var ogrnMatches = actingLicenses.All(l => (string)l["Tm_OrgOgrn"] == ogrn);
            var stateNumMatches = actingLicenses.All(l => (string)l["Tm_TaxiStateNumber"] == taxiItem["Tm_TaxiStateNumber"].ToString());
            var noCancellation = !actingLicenses.Any(l => (string)l["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);

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
            var requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctNew
                    || i.ContentType.Name == StringsRes.ctDuplicate
                    || i.ContentType.Name == StringsRes.ctRenew);

            foreach (SPListItem request in requestList)
            {
                secondCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, taxiItem["Tm_TaxiStateNumber"].ToString())
                    && !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, prevLicNumberFmt);

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
            requestList = IncomeRequestService.GetAllIncomeRequestInStatus("1050;1020;6420;1075;1030;1080;1085").Cast<SPListItem>()
                .Where(i => i.ContentType.Name == StringsRes.ctCancellation && i["Tm_OutputFactDate"] == null);
            foreach (SPListItem request in requestList)
            {
                thirdCondition =
                    !IncomeRequestService.HasRequestTaxiStateNumber(request.ID, taxiItem["Tm_TaxiStateNumber"].ToString())
                    && !IncomeRequestService.HasRequestTaxiLicenseNumber(request.ID, prevLicNumberFmt);

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
