var TM;

(function (tm) {

    tm.SP = (function (tmsp) {

        tmsp.License = (function (lic) {

            lic.ServiceUrl = _spPageContextInfo.webAbsoluteUrl + '/' + _spPageContextInfo.layoutsUrl + '/TaxoMotor/LicenseService.aspx';

            lic.GetItem = function (licenseId, success, fail) {

                SP.SOD.executeOrDelayUntilScriptLoaded(function () {

                    var ctx  = SP.ClientContext.get_current();
                    var web  = ctx.get_web();
                    var list = web.get_lists().getByTitle('Разрешения');
                    var item = list.getItemById(licenseId);

                    ctx.load(item);
                    ctx.executeQueryAsync(function() {
                        success(item);
                    }, fail);

                }, 'sp.js');
            };

            lic.GetCurrentItem = function (success, fail) {
                JSRequest.EnsureSetup();
                var currentItemId = JSRequest.QueryString.ID;

                if (currentItemId) {
                    lic.GetItem(currentItemId, success, fail);
                }
            };

            lic.GetCurrentItem(function (item) {
                lic.CurrentItem = item;
            }, function () {
                console.error('Не удалось получить текущий объект разрешения');
            });

            return lic;
        })(tmsp.License || (tmsp.License = {}));

        return tmsp;
    })(tm.SP || (tm.SP = {}));

})(TM || (TM = {}));