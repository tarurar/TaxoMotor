/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="LicenseEntityHelper.ts" />
var TM;
(function (TM) {
    var SP_;
    (function (SP_) {
        var License;
        (function (License) {
            "use strict";
            var _current = null;
            function getCurrent() {
                return _current;
            }
            License.getCurrent = getCurrent;
            SP.SOD.loadMultiple(["sp.js", "sp.init.js"], function () {
                var def = $.Deferred();
                JSRequest.EnsureSetup();
                var listId = decodeURIComponent(JSRequest.QueryString["List"]);
                var itemId = parseInt(decodeURIComponent(JSRequest.QueryString["ID"]), 10);
                if (listId && itemId) {
                    var listGuid = new SP.Guid(listId);
                    License.LicenseEntityHelper.Create(License.LicenseEntityHelper, listGuid, itemId, function (helper) {
                        _current = helper.currentItem;
                        def.resolve();
                    }, def.reject);
                }
                else {
                    def.reject();
                }
                def.always(function () {
                    SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs("CurrentLicense.js");
                });
            });
        })(License = SP_.License || (SP_.License = {}));
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
//# sourceMappingURL=CurrentLicense.js.map