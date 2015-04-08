/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var TM;
(function (TM) {
    var SP_;
    (function (SP_) {
        var IncomeRequest;
        (function (IncomeRequest) {
            "use strict";
            var TaxiStatus = (function () {
                function TaxiStatus() {
                }
                TaxiStatus.InWork = "В работе";
                TaxiStatus.Success = "Решено положительно";
                TaxiStatus.Refused = "Отказано";
                TaxiStatus.Fail = "Решено отрицательно";
                TaxiStatus.NotReceived = "Не получено";
                return TaxiStatus;
            })();
            IncomeRequest.TaxiStatus = TaxiStatus;
            var RequestStrings = (function () {
                function RequestStrings() {
                }
                RequestStrings.EgripDlgTitle = "Отправка запроса в Единый Государственный Реестр Индивидуальных Предпринимателей (ЕГРИП)";
                RequestStrings.EgrulDlgTitle = "";
                return RequestStrings;
            })();
            IncomeRequest.RequestStrings = RequestStrings;
            var RequestErrStrings = (function () {
                function RequestErrStrings() {
                }
                RequestErrStrings.EgripDlg = "При отправке запроса в ЕГРИП возникли ошибки";
                return RequestErrStrings;
            })();
            IncomeRequest.RequestErrStrings = RequestErrStrings;
            var RequestParams;
            (function (RequestParams) {
                var IncomeRequestCommonParam = (function (_super) {
                    __extends(IncomeRequestCommonParam, _super);
                    function IncomeRequestCommonParam() {
                        _super.apply(this, arguments);
                    }
                    IncomeRequestCommonParam.prototype.Stringify = function () {
                        var str = _super.prototype.Stringify.call(this);
                        return str.replace("EntityId", "incomeRequestId");
                    };
                    return IncomeRequestCommonParam;
                })(TM.SP_.RequestParams.EntityCommonParam);
                RequestParams.IncomeRequestCommonParam = IncomeRequestCommonParam;
                var ClosingIncomeRequestParam = (function (_super) {
                    __extends(ClosingIncomeRequestParam, _super);
                    function ClosingIncomeRequestParam() {
                        _super.apply(this, arguments);
                    }
                    ClosingIncomeRequestParam.prototype.Stringify = function () {
                        var str = _super.prototype.Stringify.call(this);
                        return str.replace("incomeRequestId", "closingIncomeRequestId");
                    };
                    return ClosingIncomeRequestParam;
                })(IncomeRequestCommonParam);
                RequestParams.ClosingIncomeRequestParam = ClosingIncomeRequestParam;
                var StatusesParam = (function (_super) {
                    __extends(StatusesParam, _super);
                    function StatusesParam() {
                        _super.apply(this, arguments);
                    }
                    return StatusesParam;
                })(IncomeRequestCommonParam);
                RequestParams.StatusesParam = StatusesParam;
                var StatusParam = (function (_super) {
                    __extends(StatusParam, _super);
                    function StatusParam() {
                        _super.apply(this, arguments);
                    }
                    return StatusParam;
                })(IncomeRequestCommonParam);
                RequestParams.StatusParam = StatusParam;
                var TaxiListParam = (function (_super) {
                    __extends(TaxiListParam, _super);
                    function TaxiListParam() {
                        _super.apply(this, arguments);
                    }
                    return TaxiListParam;
                })(IncomeRequestCommonParam);
                RequestParams.TaxiListParam = TaxiListParam;
                var RefuseTaxiParam = (function (_super) {
                    __extends(RefuseTaxiParam, _super);
                    function RefuseTaxiParam() {
                        _super.apply(this, arguments);
                    }
                    return RefuseTaxiParam;
                })(TaxiListParam);
                RequestParams.RefuseTaxiParam = RefuseTaxiParam;
                var LicenseListParam = (function (_super) {
                    __extends(LicenseListParam, _super);
                    function LicenseListParam() {
                        _super.apply(this, arguments);
                    }
                    return LicenseListParam;
                })(TM.SP_.RequestParams.CommonParam);
                RequestParams.LicenseListParam = LicenseListParam;
                var LicenseParam = (function (_super) {
                    __extends(LicenseParam, _super);
                    function LicenseParam() {
                        _super.apply(this, arguments);
                    }
                    return LicenseParam;
                })(TM.SP_.RequestParams.CommonParam);
                RequestParams.LicenseParam = LicenseParam;
                var LicenseSignatureParam = (function (_super) {
                    __extends(LicenseSignatureParam, _super);
                    function LicenseSignatureParam() {
                        _super.apply(this, arguments);
                    }
                    return LicenseSignatureParam;
                })(LicenseParam);
                RequestParams.LicenseSignatureParam = LicenseSignatureParam;
            })(RequestParams = IncomeRequest.RequestParams || (IncomeRequest.RequestParams = {}));
            var IncomeRequestEntityHelper = (function (_super) {
                __extends(IncomeRequestEntityHelper, _super);
                function IncomeRequestEntityHelper() {
                    _super.apply(this, arguments);
                }
                IncomeRequestEntityHelper.prototype.ServiceUrl = function () {
                    var rootUrl = _super.prototype.ServiceUrl.call(this);
                    return SP.ScriptHelpers.urlCombine(rootUrl, "IncomeRequestService.aspx");
                };
                IncomeRequestEntityHelper.prototype.IsAllTaxiInStatus = function (statuses) {
                    return this.PostWebMethod(RequestParams.StatusesParam, function (param) {
                        param.statuses = statuses;
                    }, "IsAllTaxiInStatus");
                };
                IncomeRequestEntityHelper.prototype.IsAnyTaxiInStatus = function (statuses) {
                    return this.PostWebMethod(RequestParams.StatusesParam, function (param) {
                        param.statuses = statuses;
                    }, "IsAnyTaxiInStatus");
                };
                IncomeRequestEntityHelper.prototype.IsAllTaxiInStatusHasBlankNo = function (status) {
                    return this.PostWebMethod(RequestParams.StatusParam, function (param) {
                        param.status = status;
                    }, "IsAllTaxiInStatusHasBlankNo");
                };
                IncomeRequestEntityHelper.prototype.IsAllTaxiInStatusHasLicenseNumber = function (status) {
                    return this.PostWebMethod(RequestParams.StatusParam, function (param) {
                        param.status = status;
                    }, "IsAllTaxiInStatusHasLicenseNumber");
                };
                IncomeRequestEntityHelper.prototype.GetAllTaxiInRequestByStatus = function (status) {
                    return this.PostWebMethod(RequestParams.StatusParam, function (param) {
                        param.status = status;
                    }, "GetAllTaxiInRequestByStatus");
                };
                IncomeRequestEntityHelper.prototype.GetAllWorkingTaxiInRequest = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "GetAllWorkingTaxiInRequest");
                };
                IncomeRequestEntityHelper.prototype.AcceptTaxiRequest = function (taxiIdList) {
                    return this.PostWebMethod(RequestParams.TaxiListParam, function (param) {
                        param.taxiIdList = taxiIdList;
                    }, "AcceptTaxi");
                };
                IncomeRequestEntityHelper.prototype.RefuseTaxiRequest = function (taxiIdList, refuseReasonCode, refuseComment, needPersonVisit) {
                    return this.PostWebMethod(RequestParams.RefuseTaxiParam, function (param) {
                        param.taxiIdList = taxiIdList;
                        param.refuseReasonCode = refuseReasonCode;
                        param.refuseComment = refuseComment;
                        param.needPersonVisit = needPersonVisit;
                    }, "RefuseTaxi");
                };
                IncomeRequestEntityHelper.prototype.CanReleaseNewLicensesForRequest = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "CanReleaseNewLicensesForRequest");
                };
                IncomeRequestEntityHelper.prototype.HasRequestActingLicenses = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "HasRequestActingLicenses");
                };
                IncomeRequestEntityHelper.prototype.CreateDocumentsWhileClosing = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "CreateDocumentsWhileClosing");
                };
                IncomeRequestEntityHelper.prototype.CreateDocumentsWhileRefusing = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "CreateDocumentsWhileRefusing");
                };
                IncomeRequestEntityHelper.prototype.GetDocumentsForSendStatus = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "GetDocumentsForSendStatus");
                };
                IncomeRequestEntityHelper.prototype.PromoteLicenseDrafts = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "PromoteLicenseDrafts");
                };
                IncomeRequestEntityHelper.prototype.GetLicenseXmlById = function (licenseIdList) {
                    return this.PostNonEntityWebMethod(RequestParams.LicenseListParam, function (param) {
                        param.licenseIdList = licenseIdList;
                    }, "GetLicenseXmlById");
                };
                IncomeRequestEntityHelper.prototype.DeleteLicenseDraftsByTaxiStatus = function (status) {
                    return this.PostWebMethod(RequestParams.StatusParam, function (param) {
                        param.status = status;
                    }, "DeleteLicenseDraftsByTaxiStatus");
                };
                IncomeRequestEntityHelper.prototype.SetStatusOnClosing = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "SetStatusOnClosing");
                };
                IncomeRequestEntityHelper.prototype.UpdateSignatureForLicense = function (licenseId, signature) {
                    return this.PostNonEntityWebMethod(RequestParams.LicenseSignatureParam, function (param) {
                        param.licenseId = licenseId;
                        param.signature = encodeURIComponent(signature);
                    }, "UpdateSignatureForLicense");
                };
                IncomeRequestEntityHelper.prototype.UpdateOutcomeRequestsOnClosing = function () {
                    return this.PostWebMethod(RequestParams.ClosingIncomeRequestParam, null, "UpdateOutcomeRequestsOnClosing");
                };
                IncomeRequestEntityHelper.prototype.GetCurrentStatusCode = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "GetCurrentStatusCode");
                };
                IncomeRequestEntityHelper.prototype.OutputRequest = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "MakeOutput");
                };
                IncomeRequestEntityHelper.prototype.SendToAsguf = function () {
                    return this.PostWebMethod(RequestParams.IncomeRequestCommonParam, null, "SendToAsguf");
                };
                IncomeRequestEntityHelper.prototype.SendEgripRequest = function (incomeRequestId, onsuccess, onfail) {
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
                            }
                            else if (result == -1) {
                                if (onfail) {
                                    onfail(RequestErrStrings.EgripDlg);
                                }
                            }
                        }
                    };
                    SP.UI.ModalDialog.showModalDialog(options);
                };
                return IncomeRequestEntityHelper;
            })(SP_.EntityHelper);
            IncomeRequest.IncomeRequestEntityHelper = IncomeRequestEntityHelper;
        })(IncomeRequest = SP_.IncomeRequest || (SP_.IncomeRequest = {}));
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
//# sourceMappingURL=IncomeRequestEntityHelper.js.map