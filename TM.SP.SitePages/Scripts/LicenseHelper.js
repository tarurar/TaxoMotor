var TM;

(function (tm) {

    var deferreds = [];

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

            var currentItemDef = $.Deferred();
            deferreds.push(currentItemDef);
            lic.GetCurrentItem(function (item) {
                lic.CurrentItem = item;
                currentItemDef.resolve();
            }, function () {
                console.error('Не удалось получить текущий объект разрешения');
                currentItemDef.reject();
            });

            lic.MakeObsoleteGetXml = function (licenseId, obsolete) {
                return $.ajax({
                    type: 'POST',
                    url: lic.ServiceUrl + '/MakeObsoleteGetXml',
                    data: '{ licenseId: ' + licenseId + ', obsolete: ' + obsolete + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            lic.MakeObsoleteSaveSigned = function (licenseId, obsolete, signature) {
                return $.ajax({
                    type: 'POST',
                    url: lic.ServiceUrl + '/SaveSignedMakeObsolete',
                    data: '{ licenseId: ' + licenseId + ', obsolete: ' + obsolete + ', signature: "' + encodeURIComponent(signature) + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            lic.DisableGibddGetXml = function (licenseId, disabled) {
                return $.ajax({
                    type: 'POST',
                    url: lic.ServiceUrl + '/DisableGibddGetXml',
                    data: '{ licenseId: ' + licenseId + ', disabled: ' + disabled + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            lic.DisabledGibddSaveSigned = function (licenseId, disabled, signature) {
                return $.ajax({
                    type: 'POST',
                    url: lic.ServiceUrl + '/SaveSignedDisableGibdd',
                    data: '{ licenseId: ' + licenseId + ', disabled: ' + disabled + ', signature: "' + encodeURIComponent(signature) + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            lic.ChangeObsoleteAttribute = function (licenseId, obsolete, success, fail) {
                lic.MakeObsoleteGetXml(licenseId, obsolete).success(function (data) {
                    var dataToSign = data.d;
                    debugger;
                    var oCertificate = cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_CURRENT_USER_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                    if (oCertificate) {
                        dataToSign =
                            "<?xml version=\"1.0\"?>\n" +
                                "<Envelope xmlns=\"urn:envelope\">\n" +
                                dataToSign +
                                " \n" +
                                "</Envelope>";

                        var signedData;
                        try {
                            signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                        } catch (e) {
                            fail("Ошибка при формировании подписи: " + e.message);
                        }

                        if (typeof signedData === 'undefined' || !signedData)
                            fail("Ошибка при формировании подписи");

                        lic.MakeObsoleteSaveSigned(licenseId, obsolete, signedData)
                            .success(success)
                            .fail(fail);
                    }
                }).fail(function () {
                    fail("Ошибка при получении xml для разрешения");
                });
            };

            lic.ChangeDisableGibddAttribute = function (licenseId, disabled, success, fail) {
                lic.DisableGibddGetXml(licenseId, disabled).success(function (data) {
                    var dataToSign = data.d;

                    var oCertificate = cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_CURRENT_USER_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                    if (oCertificate) {
                        dataToSign =
                            "<?xml version=\"1.0\"?>\n" +
                                "<Envelope xmlns=\"urn:envelope\">\n" +
                                dataToSign +
                                " \n" +
                                "</Envelope>";

                        var signedData;
                        try {
                            signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                        } catch (e) {
                            fail("Ошибка при формировании подписи: " + e.message);
                        }

                        if (typeof signedData === 'undefined' || !signedData)
                            fail("Ошибка при формировании подписи");

                        lic.DisabledGibddSaveSigned(licenseId, disabled, signedData)
                            .success(success)
                            .fail(fail);
                    }
                }).fail(function () {
                    fail("Ошибка при получении xml для разрешения");
                });
            };

            $.when.apply($, deferreds).always(function () {
                if (SP && SP.SOD) {
                    SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs('LicenseHelper.js');
                }
            });

            return lic;
        })(tmsp.License || (tmsp.License = {}));

        return tmsp;
    })(tm.SP || (tm.SP = {}));

})(TM || (TM = {}));