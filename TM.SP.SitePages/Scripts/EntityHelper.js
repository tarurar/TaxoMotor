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
            function EntityHelper(listGuid, itemId) {
                var ctx = SP.ClientContext.get_current();
                var web = ctx.get_web();
                var list = web.get_lists().getById(listGuid);
                this.CurrentItem = list.getItemById(itemId);
                ctx.load(this.CurrentItem);
                /*var def = $.Deferred<void>();
                this._loaded.push(def);
                ctx.executeQueryAsync(def.resolve, def.reject);
                */
                ctx.executeQueryAsync();
            }
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
