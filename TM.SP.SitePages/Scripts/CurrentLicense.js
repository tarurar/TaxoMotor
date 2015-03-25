/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="LicenseEntityHelper.ts" />
var TM;
(function (TM) {
    var SP_;
    (function (SP_) {
        var License;
        (function (License) {
            var _current = null;
            function Current() {
            }
            License.Current = Current;
            var def = $.Deferred();
            JSRequest.EnsureSetup();
            var listId = JSRequest.QueryString["List"];
            var itemId = parseInt(JSRequest.QueryString["ID"]);
            if (listId && itemId) {
                var listGuid = new SP.Guid(listId);
                var helper = License.LicenseEntityHelper.Create(listGuid, itemId, function (license) {
                    def.resolve();
                }, function (sender, args) {
                    def.reject();
                });
            }
            else {
                def.reject();
            }
        })(License = SP_.License || (SP_.License = {}));
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
