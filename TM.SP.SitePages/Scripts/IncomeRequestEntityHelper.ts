/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />

module TM.SP_.IncomeRequest {
    "use strict";

    export module RequestParams {

        export class IncomeRequestCommonParam extends TM.SP_.RequestParams.EntityCommonParam {

            public Stringify(): string {
                var str = super.Stringify();
                return str.replace("EntityId", "incomeRequestId");
            }
        }
    }

    export class IncomeRequestEntityHelper extends EntityHelper {
        public ServiceUrl(): string {
            var rootUrl = super.getServiceUrl();
            return SP.ScriptHelpers.urlCombine(rootUrl, "IncomeRequestService.aspx");
        }
    }
}