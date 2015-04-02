 /// <reference path="typings/sharepoint/SharePoint.d.ts" />
 /// <reference path="typings/jquery/jquery.d.ts" />

module TM.SP_ {
    "use strict";

    export module RequestParams {
        export class CommonParam {
            public Stringify(): string {
                return JSON.stringify(this);
            }
        }

        export class EntityCommonParam extends CommonParam {
            public EntityId: number;

            constructor(entity: EntityHelper) {
                super();

                this.EntityId = entity.currentItem.get_id();
            }
        }
    }

    export module RequestMethods {
        export function MakePostRequest(param: RequestParams.CommonParam, methodUrl: string): JQueryXHR {
            return $.ajax({
                type       : "POST",
                url        : methodUrl,
                data       : param.Stringify(),
                contentType: "application/json; charset=utf-8",
                dataType   : "json"
            });
        }
    }

    export class EntityHelper {
        private _serviceUrl: string;
        private _currentItem: SP.ListItem;

        public get currentItem(): SP.ListItem {
            return this._currentItem;
        }

        public static Create<T extends EntityHelper>(t: { new (): T;}, listGuid: SP.Guid, itemId: number,
            succeededCallback: (entity: T) => void,
            failedCallback?: (sender: any, args: SP.ClientRequestFailedEventArgs) => void) {

            var ctx  = SP.ClientContext.get_current();
            var web  = ctx.get_web();
            var list = web.get_lists().getById(listGuid);
            var item = list.getItemById(itemId);

            ctx.load(item);
            ctx.executeQueryAsync((sender: any, args: SP.ClientRequestSucceededEventArgs) => {
                var newEntity = new t();
                newEntity._currentItem = item;
                succeededCallback(newEntity);
            }, failedCallback);

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
    }

}
