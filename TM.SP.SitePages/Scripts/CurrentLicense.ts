/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="LicenseEntityHelper.ts" />

module TM.SP_.License {

    var _current: SP.ListItem = null;

    export function getCurrent(): SP.ListItem {
        return _current;
    }

    SP.SOD.loadMultiple(["sp.js", "sp.init.js"], () => {
        var def = $.Deferred();
        JSRequest.EnsureSetup();
        var listId = decodeURIComponent(JSRequest.QueryString["List"]);
        var itemId = parseInt(decodeURIComponent(JSRequest.QueryString["ID"]));

        if (listId && itemId) {
            var listGuid = new SP.Guid(listId);
            var helper = LicenseEntityHelper.Create<LicenseEntityHelper>(LicenseEntityHelper, listGuid, itemId,(license) => {
                _current = license.currentItem;
                def.resolve();
            }, def.reject);
        } else {
            def.reject();
        }

        def.always(() => {
            SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs("CurrentLicense.js");
        });
    });
}