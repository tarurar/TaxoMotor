/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="LicenseEntityHelper.ts" />

module TM.SP_.License {

    var _current: SP.ListItem = null;

    export function Current() {



    }

    var def = $.Deferred();

    JSRequest.EnsureSetup();
    var listId = JSRequest.QueryString["List"];
    var itemId = parseInt(JSRequest.QueryString["ID"]);

    if (listId && itemId) {
        var listGuid = new SP.Guid(listId);
        var helper = LicenseEntityHelper.Create(listGuid, itemId, (license) => {
            def.resolve();
        },(sender, args) => {
            def.reject();
        });
    } else {
        def.reject();
    }
}