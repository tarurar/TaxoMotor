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
            })(RequestParams = IncomeRequest.RequestParams || (IncomeRequest.RequestParams = {}));
            var IncomeRequestEntityHelper = (function (_super) {
                __extends(IncomeRequestEntityHelper, _super);
                function IncomeRequestEntityHelper() {
                    _super.apply(this, arguments);
                }
                IncomeRequestEntityHelper.prototype.ServiceUrl = function () {
                    var rootUrl = _super.prototype.getServiceUrl.call(this);
                    return SP.ScriptHelpers.urlCombine(rootUrl, "IncomeRequestService.aspx");
                };
                return IncomeRequestEntityHelper;
            })(SP_.EntityHelper);
            IncomeRequest.IncomeRequestEntityHelper = IncomeRequestEntityHelper;
        })(IncomeRequest = SP_.IncomeRequest || (SP_.IncomeRequest = {}));
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
