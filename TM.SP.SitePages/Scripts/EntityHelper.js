/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
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
        "use strict";
        var RequestParams;
        (function (RequestParams) {
            var CommonParam = (function () {
                function CommonParam() {
                }
                CommonParam.prototype.Stringify = function () {
                    return JSON.stringify(this);
                };
                return CommonParam;
            })();
            RequestParams.CommonParam = CommonParam;
            var EntityCommonParam = (function (_super) {
                __extends(EntityCommonParam, _super);
                function EntityCommonParam(entity) {
                    _super.call(this);
                    this.EntityId = entity.currentItem.get_id();
                }
                return EntityCommonParam;
            })(CommonParam);
            RequestParams.EntityCommonParam = EntityCommonParam;
        })(RequestParams = SP_.RequestParams || (SP_.RequestParams = {}));
        var RequestMethods;
        (function (RequestMethods) {
            function MakePostRequest(param, methodUrl) {
                return $.ajax({
                    type: "POST",
                    url: methodUrl,
                    data: param.Stringify(),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                });
            }
            RequestMethods.MakePostRequest = MakePostRequest;
        })(RequestMethods = SP_.RequestMethods || (SP_.RequestMethods = {}));
        var EntityHelper = (function () {
            function EntityHelper() {
            }
            Object.defineProperty(EntityHelper.prototype, "currentItem", {
                get: function () {
                    return this._currentItem;
                },
                enumerable: true,
                configurable: true
            });
            EntityHelper.Create = function (t, listGuid, itemId, succeededCallback, failedCallback) {
                var ctx = SP.ClientContext.get_current();
                var web = ctx.get_web();
                var list = web.get_lists().getById(listGuid);
                var item = list.getItemById(itemId);
                ctx.load(item);
                ctx.executeQueryAsync(function (sender, args) {
                    var newEntity = new t();
                    newEntity._currentItem = item;
                    succeededCallback(newEntity);
                }, failedCallback);
            };
            EntityHelper.prototype.ServiceUrl = function () {
                if (!this._serviceUrl) {
                    var layoutsUrl = SP.ScriptHelpers.urlCombine(_spPageContextInfo.webAbsoluteUrl, _spPageContextInfo.layoutsUrl);
                    var tmUrl = SP.ScriptHelpers.urlCombine(layoutsUrl, "TaxoMotor");
                    this._serviceUrl = tmUrl;
                }
                return this._serviceUrl;
            };
            EntityHelper.prototype.BuildMethodUrl = function (methodName) {
                var rootUrl = this.ServiceUrl();
                return SP.ScriptHelpers.urlCombine(rootUrl, methodName);
            };
            EntityHelper.prototype.PostWebMethod = function (t, updateParam, methodName) {
                var param = new t(this);
                if (updateParam) {
                    updateParam(param);
                }
                return RequestMethods.MakePostRequest(param, this.BuildMethodUrl(methodName));
            };
            EntityHelper.prototype.PostNonEntityWebMethod = function (t, updateParam, methodName) {
                var param = new t();
                if (updateParam) {
                    updateParam(param);
                }
                return RequestMethods.MakePostRequest(param, this.BuildMethodUrl(methodName));
            };
            return EntityHelper;
        })();
        SP_.EntityHelper = EntityHelper;
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
//# sourceMappingURL=EntityHelper.js.map