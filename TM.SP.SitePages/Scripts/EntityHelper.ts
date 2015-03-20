 /// <reference path="typings/sharepoint/SharePoint.d.ts" />
 /// <reference path="typings/jquery/jquery.d.ts" />

module TM.SP_ {

    export module RequestParams {
        export class CommonParam {

        }
    }

    export module RequestMethods {
        export function MakePostRequest(param: RequestParams.CommonParam, method: string): JQueryXHR {
            return $.ajax({
                type       : "POST",
                url        : SP.ScriptHelpers.urlCombine(this.getServiceUrl(), method),
                data       : JSON.stringify(param),
                contentType: "application/json; charset=utf-8",
                dataType   : "json"
            });
        }
    }

    export class EntityHelper {
            
        //private _loaded: JQueryDeferred<void>[];    
        private _serviceUrl: string;
        public CurrentItem: SP.ListItem;

        constructor(listGuid: SP.Guid, itemId: number) {
            
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

        public getServiceUrl(): string {
            if (!this._serviceUrl) {
                var layoutsUrl = SP.ScriptHelpers.urlCombine(
                    _spPageContextInfo.webAbsoluteUrl,
                    _spPageContextInfo.layoutsUrl);
                var tmUrl = SP.ScriptHelpers.urlCombine(layoutsUrl, "TaxoMotor");
                this._serviceUrl = tmUrl;
            }
            return this._serviceUrl;
        }

        /*public loaded(): JQueryPromise<JQueryDeferred<void>[]> {
            return $.when(this._loaded).always();
        }*/
    }

}
