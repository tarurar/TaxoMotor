/// <reference path="jquery-2.1.1.js" />

var TM;

(function (TM) {

    TM.SP = (function (TMSP) {

        TMSP.showIconNotification = function (message, iconUrl, autoClose) {
            var messageHtml = '<div><span style="float: left;"><img src="' +
                iconUrl + '" /></span><span style="padding: 5px;">' + message + '</span></div>';
            return SP.UI.Notify.addNotification(messageHtml, !autoClose);
        };

        TMSP.setTargetBlank = function (toFindInLink) {
            // Get the collection of <a> tags
            var aAllLinks = document.getElementsByTagName('a');

            // For each <a> tags, 
            for (var i = 0; i < aAllLinks.length; i++) {
                var oA = aAllLinks[i];
                var sHref = oA.attributes["href"] ? oA.attributes["href"].value.toLowerCase() : null;

                // If href value contains paramter
                if (sHref && sHref.indexOf(toFindInLink.toLowerCase()) > 0)
                    oA.attributes["target"].value = "_blank";
            }
        };

        TMSP.GetBcsFieldIdentityFieldName = function (listId, bcsFieldName, success, fail) {

            var auto = bcsFieldName.startsWith('bdil_');
            if (auto) {
                var value = bcsFieldName.replace('bdil_', 'bdilid_');
                success(value);
            } else {
                SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                    var ctx = SP.ClientContext.get_current();
                    var list = ctx.get_web().get_lists().getById(listId);
                    var field = list.get_fields().getByInternalNameOrTitle(bcsFieldName);
                    ctx.load(field);
                    ctx.executeQueryAsync(function () {
                        var xml = field.get_schemaXml();
                        var xmlDoc = $.parseXML(xml);
                        var fn = $(xmlDoc).find('Field').attr('RelatedFieldWssStaticName');

                        success(fn);
                    }, fail);
                }, 'sp.js');
            }
        };

        TMSP.GetItemFieldValues = function(listId, itemId, fieldNames, success, fail) {
            SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getById(listId);
                var item = list.getItemById(itemId);
                ctx.load(item, fieldNames);
                ctx.executeQueryAsync(function () {
                    var values = [];
                    for (var i = 0; i < fieldNames.length; i++) {
                        values.push(item.get_item(fieldNames[i]));
                    }

                    success(values);
                }, fail);
            }, 'sp.js');
        };

        TMSP.GetListDefaultDisplayFormUrl = function(spListTitle, success, fail) {
            SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getByTitle(spListTitle);
                ctx.load(list, 'DefaultDisplayFormUrl');
                ctx.executeQueryAsync(function () {
                    var value = list.get_defaultDisplayFormUrl();
                    success(value);
                }, fail);
            }, 'sp.js');
        };

        TMSP.GetListDefaultNewFormUrl = function (spListTitle, success, fail) {
            SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getByTitle(spListTitle);
                ctx.load(list, 'DefaultNewFormUrl');
                ctx.executeQueryAsync(function () {
                    var value = list.get_defaultNewFormUrl();
                    success(value);
                }, fail);
            }, 'sp.js');
        };

        TMSP.GetListDefaultEditFormUrl = function (spListTitle, success, fail) {
            SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getByTitle(spListTitle);
                ctx.load(list, 'DefaultEditFormUrl');
                ctx.executeQueryAsync(function () {
                    var value = list.get_defaultEditFormUrl();
                    success(value);
                }, fail);
            }, 'sp.js');
        };

        TMSP.MakeBcsFieldControlLinked = function (listId, itemId, field, linkedListName) {
            var div = $('.fd_field[fd_name=' + field + '] > div[fd_type=BusinessData]');
            if (!div) return;
            var text = $(div).text().trim();
            if (!text) return;

            TMSP.GetBcsFieldIdentityFieldName(listId, field, function(fn) {
                TMSP.GetItemFieldValues(listId, itemId, [fn], function(fieldValues) {

                    var extItemId = fieldValues[0];
                    if (!extItemId) return;
                    TMSP.GetListDefaultDisplayFormUrl(linkedListName, function (url) {

                        var href = url + '?ID=' + extItemId;
                        var atag = '<a href="' + href + '" target="_blank">' + text + '</a>';
                        $(div).html(atag);
                    }, function(sender, args) {
                        console.error('Не удалось получить адрес формы списка. ' + args.get_message() + '\n' + args.get_stackTrace());
                    });
                }, function(sender, args) {
                    console.error('Не удалось получить значение полей. ' + args.get_message() + '\n' + args.get_stackTrace());
                });
            }, function(sender, args) {
                console.error('Не удалось получить описание поля. ' + args.get_message() + '\n' + args.get_stackTrace());
            });
        };

        TMSP.GetContentTypeName = function(ctId, listId, success, fail) {
            SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                var ctx = SP.ClientContext.get_current();
                var list = ctx.get_web().get_lists().getById(listId);
                var ctList = list.get_contentTypes();
                ctx.load(ctList);
                ctx.executeQueryAsync(function() {

                    var value = '';
                    var contentTypeEnumerator = ctList.getEnumerator();
                    while (contentTypeEnumerator.moveNext()) {
                        var content = contentTypeEnumerator.get_current();
                        var contentId = content.get_id().get_stringValue();
                        
                        if (contentId == ctId) {
                            value = content.get_name();
                            break;
                        }
                    };

                    if (value) {
                        success(value);
                    } else fail('Тип содержимого с id = ' + ctId + ' не найден');
                }, fail);
            }, 'sp.js');
        };

        TM.SP.GetListItemsByFieldValueEq = function (listName, fieldName, fieldValue, success, fail) {

            var filterUrlPart = "$filter=" + fieldName + " eq '" + fieldValue + "'";
            var url = _spPageContextInfo.webAbsoluteUrl + "/_api/web/lists/getByTitle('" + listName + "')/items?" + filterUrlPart;

            $.ajax({
                url: encodeURI(url),
                method: "GET",
                headers: { "Accept": "application/json; odata=verbose" },
                success: success,
                error: fail
            });
        };

        return TMSP;
    })(TM.SP || (TM.SP = {}));

})(TM || (TM = {}));