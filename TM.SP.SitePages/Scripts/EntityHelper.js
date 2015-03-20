/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
var TM;
(function (TM) {
    var SP_;
    (function (SP_) {
        var RequestParams;
        (function (RequestParams) {
            var CommonParam = (function () {
                function CommonParam() {
                }
                return CommonParam;
            })();
            RequestParams.CommonParam = CommonParam;
        })(RequestParams = SP_.RequestParams || (SP_.RequestParams = {}));
        var RequestMethods;
        (function (RequestMethods) {
            function MakePostRequest(param, method) {
                return $.ajax({
                    type: "POST",
                    url: SP.ScriptHelpers.urlCombine(this.getServiceUrl(), method),
                    data: JSON.stringify(param),
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
            EntityHelper.Create = function (listGuid, itemId, succeededCallback, failedCallback) {
                var ctx = SP.ClientContext.get_current();
                var web = ctx.get_web();
                var list = web.get_lists().getById(listGuid);
                var item = list.getItemById(itemId);
                ctx.load(item);
                ctx.executeQueryAsync(function (sender, args) {
                    var newEntity = new EntityHelper();
                    newEntity._currentItem = item;
                    succeededCallback(newEntity);
                }, failedCallback);
            };
            EntityHelper.prototype.getServiceUrl = function () {
                if (!this._serviceUrl) {
                    var layoutsUrl = SP.ScriptHelpers.urlCombine(_spPageContextInfo.webAbsoluteUrl, _spPageContextInfo.layoutsUrl);
                    var tmUrl = SP.ScriptHelpers.urlCombine(layoutsUrl, "TaxoMotor");
                    this._serviceUrl = tmUrl;
                }
                return this._serviceUrl;
            };
            return EntityHelper;
        })();
        SP_.EntityHelper = EntityHelper;
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
if (SP && SP.SOD) {
    SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs("EntityHelper.js");
}
//# sourceMappingURL=EntityHelper.js.map