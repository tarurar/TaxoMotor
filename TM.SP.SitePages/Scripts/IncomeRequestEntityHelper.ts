/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />

module TM.SP_.IncomeRequest {
    "use strict";
    
    export class TaxiStatus {
        static InWork      = "В работе";
        static Success     = "Решено положительно";
        static Refused     = "Отказано";
        static Fail        = "Решено отрицательно";
        static NotReceived = "Не получено";
    }

    export class RequestStrings {
        static EgripDlgTitle = "Отправка запроса в Единый Государственный Реестр Индивидуальных Предпринимателей (ЕГРИП)";
        static EgrulDlgTitle = "";
    }

    export class RequestErrStrings {
        static EgripDlg = "При отправке запроса в ЕГРИП возникли ошибки";
    }

    export module RequestParams {

        export class IncomeRequestCommonParam extends TM.SP_.RequestParams.EntityCommonParam {

            public Stringify(): string {
                var str = super.Stringify();
                return str.replace("EntityId", "incomeRequestId");
            }
        }

        export class ClosingIncomeRequestParam extends IncomeRequestCommonParam {
            public Stringify(): string {
                var str = super.Stringify();
                return str.replace("incomeRequestId", "closingIncomeRequestId");
            }
        }

        export class StatusesParam extends IncomeRequestCommonParam {
            public statuses: string;
        }

        export class StatusParam extends IncomeRequestCommonParam {
            public status: string;
        }

        export class TaxiListParam extends IncomeRequestCommonParam {
            public taxiIdList: string;
        }

        export class RefuseTaxiParam extends TaxiListParam {
            public refuseReasonCode: number;
            public refuseComment: string;
            public needPersonVisit: boolean;
        }

        export class LicenseListParam extends TM.SP_.RequestParams.CommonParam {
            public licenseIdList: string;
        }

        export class LicenseParam extends TM.SP_.RequestParams.CommonParam {
            public licenseId: number;
        }

        export class LicenseSignatureParam extends LicenseParam {
            public signature: string;
        }
    }

    export class IncomeRequestEntityHelper extends EntityHelper {
        public ServiceUrl(): string {
            var rootUrl = super.ServiceUrl();
            return SP.ScriptHelpers.urlCombine(rootUrl, "IncomeRequestService.aspx");
        }

        public IsAllTaxiInStatus(statuses: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.StatusesParam>(RequestParams.StatusesParam,(param) => {
                param.statuses = statuses;
            }, "IsAllTaxiInStatus");
        }

        public IsAnyTaxiInStatus(statuses: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.StatusesParam>(RequestParams.StatusesParam,(param) => {
                param.statuses = statuses;
            }, "IsAnyTaxiInStatus");
        }

        public IsAllTaxiInStatusHasBlankNo(status: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.StatusParam>(RequestParams.StatusParam,(param) => {
                param.status = status;
            }, "IsAllTaxiInStatusHasBlankNo");
        }

        public IsAllTaxiInStatusHasLicenseNumber(status: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.StatusParam>(RequestParams.StatusParam,(param) => {
                param.status = status;
            }, "IsAllTaxiInStatusHasLicenseNumber");
        }

        public GetAllTaxiInRequestByStatus(status: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.StatusParam>(RequestParams.StatusParam,(param) => {
                param.status = status;
            }, "GetAllTaxiInRequestByStatus");
        }

        public GetAllWorkingTaxiInRequest(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "GetAllWorkingTaxiInRequest");
        }

        public AcceptTaxiRequest(taxiIdList: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.TaxiListParam>(RequestParams.TaxiListParam,(param) => {
                param.taxiIdList = taxiIdList;
            }, "AcceptTaxi");
        }

        public RefuseTaxiRequest(taxiIdList: string, refuseReasonCode: number, refuseComment: string, needPersonVisit: boolean): JQueryXHR {
            return this.PostWebMethod<RequestParams.RefuseTaxiParam>(RequestParams.RefuseTaxiParam,(param) => {
                param.taxiIdList       = taxiIdList;
                param.refuseReasonCode = refuseReasonCode;
                param.refuseComment    = refuseComment;
                param.needPersonVisit  = needPersonVisit;
            }, "RefuseTaxi");
        }

        public CanReleaseNewLicensesForRequest(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "CanReleaseNewLicensesForRequest");
        }

        public HasRequestActingLicenses(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "HasRequestActingLicenses");
        }

        public CreateDocumentsWhileClosing(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "CreateDocumentsWhileClosing");
        }

        public CreateDocumentsWhileRefusing(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "CreateDocumentsWhileRefusing");
        }

        public GetDocumentsForSendStatus(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "GetDocumentsForSendStatus");
        }

        public PromoteLicenseDrafts(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "PromoteLicenseDrafts");
        }

        public GetLicenseXmlById(licenseIdList: string): JQueryXHR {
            return this.PostNonEntityWebMethod<RequestParams.LicenseListParam>(RequestParams.LicenseListParam,(param) => {
                param.licenseIdList = licenseIdList;
            }, "GetLicenseXmlById");
        }
        
        public DeleteLicenseDraftsByTaxiStatus(status: string): JQueryXHR {
            return this.PostWebMethod<RequestParams.StatusParam>(RequestParams.StatusParam,(param) => {
                param.status = status;
            }, "DeleteLicenseDraftsByTaxiStatus");
        }

        public SetStatusOnClosing(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "SetStatusOnClosing");
        }

        public UpdateSignatureForLicense(licenseId: number, signature: string): JQueryXHR {
            return this.PostNonEntityWebMethod<RequestParams.LicenseSignatureParam>(RequestParams.LicenseSignatureParam,(param) => {
                param.licenseId = licenseId;
                param.signature = encodeURIComponent(signature);
            }, "UpdateSignatureForLicense");
        }

        public UpdateOutcomeRequestsOnClosing(): JQueryXHR {
            return this.PostWebMethod<RequestParams.ClosingIncomeRequestParam>(
                RequestParams.ClosingIncomeRequestParam, null, "UpdateOutcomeRequestsOnClosing");
        }

        public GetCurrentStatusCode(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "GetCurrentStatusCode");
        }

        public OutputRequest(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "MakeOutput");
        }

        public SendToAsguf(): JQueryXHR {
            return this.PostWebMethod<RequestParams.IncomeRequestCommonParam>(
                RequestParams.IncomeRequestCommonParam, null, "SendToAsguf");
        }

        public SendEgripRequest(incomeRequestId: number, onsuccess: () => void, onfail: (err: string) => void): void {
            var url = SP.ScriptHelpers.urlCombine(this.ServiceUrl(), "SendRequestEGRIPPage.aspx");
            var urlParams = "IsDlg=1";
            urlParams += ("&ListId=" + _spPageContextInfo.pageListId);
            urlParams += ("&Items=" + incomeRequestId);
            urlParams += ("&Source=" + location.href);
            url += ("?" + urlParams);
            
            var options = {
                url: encodeURI(url),
                title: RequestStrings.EgripDlgTitle,
                allowMaximize: false,
                showClose: true,
                width: 800,
                dialogReturnValueCallback: function (result, returnValue) {
                    if (result == SP.UI.DialogResult.OK) {
                        if (onsuccess) {
                            onsuccess();
                        }
                    } else if (result == -1) {
                        if (onfail) {
                            onfail(RequestErrStrings.EgripDlg);
                        }
                    }
                }
            };

            SP.UI.ModalDialog.showModalDialog(options);
        }
    }
}