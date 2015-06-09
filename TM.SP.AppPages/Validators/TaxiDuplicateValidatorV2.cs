using CamlexNET;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TM.Utils;

namespace TM.SP.AppPages.Validators
{

    public class TaxiDuplicateValidatorV2: TaxiDuplicateValidator
    {
        #region [fields]
        private bool? _useVinParamAllowed = null;
        #endregion

        #region [properties]
        protected bool UseVinAllowed
        {
            get
            {
                if (_useVinParamAllowed.HasValue) return _useVinParamAllowed.Value;

                var useVinParamStr = Config.GetConfigValueOrDefault<string>(_web, "UseVIN");
                _useVinParamAllowed = String.IsNullOrEmpty(useVinParamStr) ? false : (useVinParamStr.Equals("1") || useVinParamStr.ToUpper().Equals("ДА"));
                return _useVinParamAllowed.Value;
            }
        }

        protected string TaxiVin
        {
            get
            {
                return taxiItem.TryGetValue<string>("Tm_TaxiVin");
            }
        }
        #endregion

        #region [methods]

        public TaxiDuplicateValidatorV2(SPWeb web, int requestItemId, int taxiItemId) : base(web, requestItemId, taxiItemId) { }

        protected override bool ExecuteForNew()
        {
            #region [first condition check]
            // выбираем все действующие разрешения по номеру ТС
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

            // проверка по VIN
            var taxiVin = taxiItem.TryGetValue<string>("Tm_TaxiVin");
            var useVinParamStr = Config.GetConfigValueOrDefault<string>(_web, "UseVIN");
            if (!String.IsNullOrEmpty(useVinParamStr) && !String.IsNullOrEmpty(taxiVin))
            {
                if (useVinParamStr.Equals("1") || useVinParamStr.ToUpper().Equals("ДА"))
                {
                    // выбираем все действующие разрешения по коду VIN
                    var actingLicensesVin = IncomeRequestService.GetTaxiVinActingLicenses(taxiItem.ID).Cast<SPListItem>();

                    // нет действующих разрешений - ок
                    var noActingLicensesVin = !actingLicensesVin.Any();
                    // есть, но аннулированные - ок
                    var onlyCancelledLicensesVin = actingLicensesVin.All(i => (string)i["Tm_LicenseStatus"] == StringsRes.LicenseStatusCancellation);
                    // есть, но такие, которые вскоре прекращают свое действие - ок
                    var onlySoonExpiredVin = actingLicensesVin.All(i => (DateTime)i["Tm_LicenseTillDate"] <= DateTime.Now.AddDays(40));
                    // есть, но с установленным признаком "Устаревшие данные" - ок
                    var onlyObsoleteVin = actingLicensesVin.All(i => (bool?)i["Tm_LicenseObsolete"] == true);

                    var firstConditionVin = noActingLicensesVin || onlyCancelledLicensesVin || onlySoonExpiredVin || onlyObsoleteVin;
                    firstCondition = firstCondition && firstConditionVin;
                }
            }

            if (!firstCondition) throw new Exception(StringsRes.actingLicensesError);
            #endregion

            #region [second condition check]
            var itemsCount1 = FilterRequests("1050;6420",
                Predicates.V2.IncomeRequestNew.SelectRequestsExceptCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно"));

            var itemsCount2 = FilterRequests("1050;6420;1085;1075",
                Predicates.V2.IncomeRequestNew.SelectRequestsCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно"));

            if (itemsCount1 != 0 || itemsCount2 != 0)
            {
                throw new Exception(StringsRes.requestExistsForAny);
            };
            #endregion

            return true;
        }

        protected override bool ExecuteForRenew()
        {
            #region [first condition check]
            var actingLicenses = GetActingLicensesByNumber(PrevLicNumber);
            var firstCondition = actingLicenses.All(al =>
            {
                var lVin = al.TryGetValue<string>("Tm_TaxiVin");
                var validateVin = UseVinAllowed && !String.IsNullOrEmpty(TaxiVin) && !String.IsNullOrEmpty(lVin);

                var ogrnMatches    = al.TryGetValue<string>("Tm_OrgOgrn") == DeclarantOgrn;
                var noCancellation = al.TryGetValue<string>("Tm_LicenseStatus") != StringsRes.LicenseStatusCancellation;
                var vinMatches     = validateVin ? lVin == TaxiVin : true;

                return ogrnMatches && noCancellation && vinMatches;
            });

            if (!firstCondition)
            {
                throw new Exception(StringsRes.noActingLicenseForRenewErr);
            }
            #endregion

            #region [second condition check]
            var itemsCount1 = FilterRequests("1050;6420",
                Predicates.V2.IncomeRequestNew.SelectRequestsExceptCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно", PrevLicNumberFmt));

            var itemsCount2 = FilterRequests("1050;6420;1085;1075",
                Predicates.V2.IncomeRequestNew.SelectRequestsCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно", PrevLicNumberFmt));

            if (itemsCount1 != 0 || itemsCount2 != 0)
            {
                throw new Exception(StringsRes.requestExistsForAny);
            }
            #endregion

            return true;
        }

        protected override bool ExecuteForDuplicate()
        {
            #region [first condition check]
            var actingLicenses = GetActingLicensesByNumber(PrevLicNumber);
            var firstCondition = actingLicenses.All(al =>
            {
                var lVin = al.TryGetValue<string>("Tm_TaxiVin");
                var validateVin = UseVinAllowed && !String.IsNullOrEmpty(TaxiVin) && !String.IsNullOrEmpty(lVin);

                var ogrnMatches     = al.TryGetValue<string>("Tm_OrgOgrn") == DeclarantOgrn;
                var stateNumMatches = al.TryGetValue<string>("Tm_TaxiStateNumber") == TaxiStateNumber;
                var noCancellation  = al.TryGetValue<string>("Tm_LicenseStatus") != StringsRes.LicenseStatusCancellation;
                var vinMatches      = validateVin ? lVin == TaxiVin : true;

                return ogrnMatches && stateNumMatches && noCancellation && vinMatches;
            });

            if (!firstCondition)
            {
                throw new Exception(StringsRes.noActingLicenseForDuplicateErr);
            }
            #endregion

            #region [second condition check]
            var itemsCount1 = FilterRequests("1050;6420",
                Predicates.V2.IncomeRequestNew.SelectRequestsExceptCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно"));

            var itemsCount2 = FilterRequests("1050;6420;1085;1075",
                Predicates.V2.IncomeRequestNew.SelectRequestsCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно"));

            if (itemsCount1 != 0 || itemsCount2 != 0)
            {
                throw new Exception(StringsRes.requestExistsForAny);
            }
            #endregion

            return true;
        }

        protected override bool ExecuteForCancellation()
        {
            #region [first condition check]
            var actingLicenses = GetActingLicensesByNumber(PrevLicNumber);
            var firstCondition = actingLicenses.All(al =>
            {
                var lVin = al.TryGetValue<string>("Tm_TaxiVin");
                var validateVin = UseVinAllowed && !String.IsNullOrEmpty(TaxiVin) && !String.IsNullOrEmpty(lVin);

                var ogrnMatches     = al.TryGetValue<string>("Tm_OrgOgrn") == DeclarantOgrn;
                var stateNumMatches = al.TryGetValue<string>("Tm_TaxiStateNumber") == TaxiStateNumber;
                var noCancellation  = al.TryGetValue<string>("Tm_LicenseStatus") != StringsRes.LicenseStatusCancellation;
                var vinMatches      = validateVin ? lVin == TaxiVin : true;

                return ogrnMatches && stateNumMatches && noCancellation && vinMatches;
            });

            if (!firstCondition)
            {
                throw new Exception(StringsRes.noActingLicenseForCancellationErr);
            }
            #endregion

            #region [second condition check]
            var itemsCount1 = FilterRequests("1050;6420", 
                Predicates.V2.IncomeRequestNew.SelectRequestsExceptCancellation, 
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно"));

            var itemsCount2 = FilterRequests("1050;6420;1085;1075",
                Predicates.V2.IncomeRequestNew.SelectRequestsCancellation,
                Predicates.V2.GetFor.TaxiInRequest(TaxiStateNumber, "Решено положительно"));

            if (itemsCount1 != 0 || itemsCount2 != 0)
            {
                throw new Exception(StringsRes.requestExistsForAny);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// Фильтрация списка обращений по указанным предикатам
        /// </summary>
        /// <param name="irStatuses">Статусы обращений, которые нужно фильтровать. Коды статусов через точку с запятой</param>
        /// <param name="irPredicate">Предикат для отбора обращений по атрибутам обращения</param>
        /// <param name="taxiPredicate">Предикат для отбора обращений по атрибутам ТС обращения</param>
        /// <returns>Количество обращений</returns>
        private static int FilterRequests(string irStatuses, Func<SPListItem, bool> irPredicate, Func<SPListItem, bool> taxiPredicate)
        {
            var requestListByStatus = IncomeRequestService.GetAllIncomeRequestInStatus(irStatuses).Cast<SPListItem>();

            var requestFilteredList = requestListByStatus
                .Where(irPredicate)
                .Where(taxiPredicate);

            return requestFilteredList.Count();
        }

        #endregion
    }
}
