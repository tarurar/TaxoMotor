var TM;

(function (tm) {

    tm.SP = (function (tmsp) {

        tmsp.IncomeRequest = (function(ir) {

            ir.ServiceUrl = _spPageContextInfo.webAbsoluteUrl + '/' + _spPageContextInfo.layoutsUrl + '/TaxoMotor/IncomeRequestService.aspx';

            ir.IsAllTaxiInStatus = function (incomeRequestId, statuses) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/IsAllTaxiInStatus',
                    data: '{ incomeRequestId: ' + incomeRequestId + ', statuses: "' + statuses + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.IsAnyTaxiInStatus = function (incomeRequestId, statuses) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/IsAnyTaxiInStatus',
                    data: '{ incomeRequestId: ' + incomeRequestId + ', statuses: "' + statuses + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.GetAllWorkingTaxiInRequest = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/GetAllWorkingTaxiInRequest',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });

            };

            ir.CanReleaseNewLicensesForRequest = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/CanReleaseNewLicensesForRequest',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.SendEgripRequest = function (incomeRequestId, onsuccess, onfail) {
                var url = SP.Utilities.Utility.getLayoutsPageUrl('TaxoMotor/SendRequestEGRIPPage.aspx') + '?IsDlg=1&ListId=' +
                    _spPageContextInfo.pageListId + '&Items=' + incomeRequestId + '&Source=' + location.href;

                var options = {
                    url: encodeURI(url),
                    title: 'Отправка запроса в Единый Государственный Реестр Индивидуальных Предпринимателей (ЕГРИП)',
                    allowMaximize: false,
                    showClose: true,
                    width: 800,
                    dialogReturnValueCallback: Function.createDelegate(null, function(result, returnValue) {
                        if (result == SP.UI.DialogResult.OK) {
                            if (onsuccess) {
                                onsuccess();
                            }
                        } else if (result == -1) {
                            if (onfail) {
                                onfail('При отправке запроса в ЕГРИП возникли ошибки');
                            }
                        }
                    })
                };

                SP.UI.ModalDialog.showModalDialog(options);
            };

            ir.SendEgrulRequest = function (incomeRequestId, onsuccess, onfail) {
                var url = SP.Utilities.Utility.getLayoutsPageUrl('TaxoMotor/SendRequestEGRULPage.aspx') + '?IsDlg=1&ListId=' +
                    _spPageContextInfo.pageListId + '&Items=' + incomeRequestId + '&Source=' + location.href;
                var options = {
                    url: encodeURI(url),
                    title: 'Отправка запроса в Единый Государственный Реестр Юридических Лиц (ЕГРЮЛ)',
                    allowMaximize: false,
                    showClose: true,
                    width: 800,
                    dialogReturnValueCallback: Function.createDelegate(null, function (result, returnValue) {
                        if (result == SP.UI.DialogResult.OK) {
                            if (onsuccess) {
                                onsuccess();
                            }
                        }
                        else if (result == -1) {
                            if (onfail) {
                                onfail('При отправке запроса в ЕГРЮЛ возникли ошибки');
                            }
                        }
                    })
                };

                SP.UI.ModalDialog.showModalDialog(options);
            };

            ir.SendPTSRequest = function (incomeRequestId, onsuccess, onfail) {

                ir.GetAllWorkingTaxiInRequest(incomeRequestId).success(function(data) {

                    if (data && data.d) {

                        var ctx = SP.ClientContext.get_current();
                        var taxiList = ctx.get_web().get_lists().getByTitle('Транспортные средства');
                        ctx.load(taxiList);
                        ctx.executeQueryAsync(function() {
                            
                            var taxiItems = data.d.replace(/\;/g, ',');

                            var url = SP.Utilities.Utility.getLayoutsPageUrl('TaxoMotor/SendRequestPTSPage.aspx') + '?IsDlg=1&ListId=' +
                                taxiList.get_id() + '&Items=' + taxiItems + '&Source=' + location.href;

                            var options = {
                                url: encodeURI(url),
                                title: 'Запрос сведений о транспортных средствах и владельцах',
                                allowMaximize: false,
                                showClose: true,
                                width: 800,
                                dialogReturnValueCallback: Function.createDelegate(null, function (result, returnValue) {
                                    if (result == SP.UI.DialogResult.OK) {
                                        if (onsuccess) {
                                            onsuccess();
                                        }
                                    }
                                    else if (result == -1) {
                                        if (onfail) {
                                            onfail('При отправке запросов по транспортным средствам возникли ошибки');
                                        }
                                    }
                                })
                            };

                            SP.UI.ModalDialog.showModalDialog(options);

                        }, onfail);

                    } else onfail();

                }).fail(onfail);
            };

            ir.SendStatus = function(incomeRequestId) {
                var url = _spPageContextInfo.webAbsoluteUrl + '/' + _spPageContextInfo.layoutsUrl + '/' + 'TaxoMotor/SendStatus.aspx?ListId=' +
                    _spPageContextInfo.pageListId + '&Items=' + incomeRequestId;

                return $.ajax({
                    url: encodeURI(url),
                    method: 'POST'
                });
            };

            ir.CalculateDatesAndSetStatus = function(incomeRequestId, statusCode) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/CalculateDatesAndSetStatus',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' , statusCode: ' + statusCode + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

            ir.GetIncomeRequestCoordinateV5StatusMessage = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/GetIncomeRequestCoordinateV5StatusMessage',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

            ir.SaveIncomeRequestStatusLog = function(incomeRequestId, signature) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/SaveIncomeRequestStatusLog',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' , signature: "' + encodeURIComponent(signature) + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

            ir.EndingApplyForNew = function (incomeRequestId, onsuccess, onfail) {
                // Запросы ПТС по транспортным средствам обращения
                ir.SendPTSRequest(incomeRequestId, function () {
                    // Расчет сроков оказания услуги и установка статуса обращения
                    ir.CalculateDatesAndSetStatus(incomeRequestId, 1110).success(function () {
                        // Получение xml для измененного состояния обращения
                        ir.GetIncomeRequestCoordinateV5StatusMessage(incomeRequestId).success(function (data) {
                            //Подписывание xml
                            if (data && data.d) {
                                ir.SignXml(data.d, function (signedData) {
                                    // Сохранение факта изменения статуса обращения в историю изменения статусов
                                    ir.SaveIncomeRequestStatusLog(incomeRequestId, signedData).success(function () {
                                        // Отправка статуса обращения по межведомственному взаимодействию
                                        ir.SendStatus(incomeRequestId).success(onsuccess).fail(function (err) { onfail("При отправке статуса возникла ошибка"); });
                                    }).fail(onfail);
                                }, onfail);
                            } else onfail("Не удалось получить статус обращения в виде xml");
                        }).fail(onfail);
                    }).fail(onfail);
                }, onfail);
            };

            ir.ApplyForNewForJuridicalPerson = function (incomeRequestId, onsuccess, onfail) {
                // Запрос в ЕГРЮЛ
                ir.SendEgrulRequest(incomeRequestId, function () {
                    ir.EndingApplyForNew(incomeRequestId, onsuccess, onfail);
                }, onfail);
            };

            ir.ApplyForNewForPrivateEntrepreneur = function (incomeRequestId, onsuccess, onfail) {
                // Запрос в ЕГРИП
                ir.SendEgripRequest(incomeRequestId, function () {
                    ir.EndingApplyForNew(incomeRequestId, onsuccess, onfail);
                }, onfail);
            };

            ir.ApplyForNew = function(incomeRequestId, onsuccess, onfail) {
                // Проверить всем ли ТС присвоен статус
                ir.IsAllTaxiInStatus(incomeRequestId, "В работе;Отказано").success(function (data) {
                    if (data && data.d) {
                        // Проверить, остались ли в обращении ТС со статусом
                        ir.IsAnyTaxiInStatus(incomeRequestId, "В работе").success(function (data) {
                            if (data && data.d) {
                                // Провести проверку на дубли разрешений
                                ir.CanReleaseNewLicensesForRequest(incomeRequestId).success(function (data) {
                                    if (data && data.d.CanRelease) {
                                        // Заявитель - индивидуальный предприниматель?
                                        ir.IsRequestDeclarantPrivateEntrepreneur(incomeRequestId).success(function (data) {
                                            if (data && data.d) {
                                                ir.ApplyForNewForPrivateEntrepreneur(incomeRequestId, onsuccess, onfail);
                                            } else {
                                                ir.ApplyForNewForJuridicalPerson(incomeRequestId, onsuccess, onfail);
                                            }
                                        }).fail(function (err) { onfail("При проверке заявителя возникла ошибка"); });
                                    } else onfail('Разрешение на ТС с номером ' + data3.d.TaxiNumber + ' уже существует');
                                }).fail(onfail);
                            } else onfail('В обращении нет ТС');
                        }).fail(onfail);
                    } else onfail('Не всем ТС проставлен признак');
                }).fail(onfail);
            };

            ir.IsRequestDeclarantPrivateEntrepreneur = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/IsRequestDeclarantPrivateEntrepreneur',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

            ir.SignXml = function(xml, onsuccess, onfail) {

                var oCertificate = cryptoPro && cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_LOCAL_MACHINE_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                if (oCertificate) {
                    xml =
                        "<?xml version=\"1.0\"?>\n" +
                        "<Envelope xmlns=\"urn:envelope\">\n" +
                        xml +
                        " \n" +
                        "</Envelope>";

                    var signedData;
                    var errorMsg;
                    try {
                        signedData = cryptoPro.SignXMLCreate(oCertificate, xml);
                    } catch (e) {
                        errorMsg = "Ошибка при формировании подписи: " + e.message;
                    }

                    if (errorMsg) {
                        onfail(errorMsg);
                    } else {
                        onsuccess(signedData);
                    }

                } else {
                    onfail("При формировании ЭЦП не удалось обнаружить сертификат");
                }
            };

            return ir;
        })(tmsp.IncomeRequest || (tmsp.IncomeRequest = {}));

        return tmsp;
    })(tm.SP || (tm.SP = {}));

})(TM || (TM = {}));