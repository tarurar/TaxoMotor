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

            ir.HasRequestActingLicenses = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/HasRequestActingLicenses',
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

            ir.CreateIncomeRequestRefuseNotifyDocument = function(incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/CreateIncomeRequestRefuseNotifyDocument',
                    data: '{ refusedIncomeRequestId: ' + incomeRequestId + ' }',
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

            ir.ApplyForChangeForJuridicalPerson = ir.ApplyForNewForJuridicalPerson;

            ir.ApplyForChangeForPrivateEntrepreneur = ir.ApplyForNewForPrivateEntrepreneur;

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
                                    } else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' уже существует');
                                }).fail(onfail);
                            } else onfail('В обращении нет ТС');
                        }).fail(onfail);
                    } else onfail('Не всем ТС проставлен признак');
                }).fail(onfail);
            };

            ir.ApplyForDuplicate = function (incomeRequestId, onsuccess, onfail) {
                // Проверить всем ли ТС присвоен статус
                ir.IsAllTaxiInStatus(incomeRequestId, "В работе;Отказано").success(function (data) {
                    if (data && data.d) {
                        // Проверить, остались ли в обращении ТС со статусом
                        ir.IsAnyTaxiInStatus(incomeRequestId, "В работе").success(function (data) {
                            if (data && data.d) {
                                // Провести проверку на дубли разрешений. Наличие дублей обязательно.
                                ir.HasRequestActingLicenses(incomeRequestId).success(function (data) {
                                    if (data && data.d.CanRelease) {
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
                                    } else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' не существует');
                                }).fail(onfail);
                            } else onfail('В обращении нет ТС');
                        }).fail(onfail);
                    } else onfail('Не всем ТС проставлен признак');
                }).fail(onfail);
            };

            ir.ApplyForChange = function (incomeRequestId, onsuccess, onfail) {
                // Проверить всем ли ТС присвоен статус
                ir.IsAllTaxiInStatus(incomeRequestId, "В работе;Отказано").success(function (data) {
                    if (data && data.d) {
                        // Проверить, остались ли в обращении ТС со статусом
                        ir.IsAnyTaxiInStatus(incomeRequestId, "В работе").success(function (data) {
                            if (data && data.d) {
                                // Провести проверку на дубли разрешений. Наличие дублей обязательно.
                                ir.HasRequestActingLicenses(incomeRequestId).success(function (data) {
                                    if (data && data.d.CanRelease) {
                                        // Заявитель - индивидуальный предприниматель?
                                        ir.IsRequestDeclarantPrivateEntrepreneur(incomeRequestId).success(function (data) {
                                            if (data && data.d) {
                                                ir.ApplyForChangeForPrivateEntrepreneur(incomeRequestId, onsuccess, onfail);
                                            } else {
                                                ir.ApplyForChangeForJuridicalPerson(incomeRequestId, onsuccess, onfail);
                                            }
                                        }).fail(function (err) { onfail("При проверке заявителя возникла ошибка"); });
                                    } else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' не существует');
                                }).fail(onfail);
                            } else onfail('В обращении нет ТС');
                        }).fail(onfail);
                    } else onfail('Не всем ТС проставлен признак');
                }).fail(onfail);
            }

            ir.ApplyForCancellation = ir.ApplyForChange;

            ir.SetRefuseReasonAndComment = function(incomeRequestId, refuseReasonCode, refuseComment) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/SetRefuseReasonAndComment',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' , refuseReasonCode: ' + refuseReasonCode + ' , refuseComment: "' + refuseComment + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.Refuse = function(incomeRequestId, onsuccess, onfail) {
                // Получим причину и комментарий
                var options = {
                    url: _spPageContextInfo.webAbsoluteUrl + '/ProjectSitePages/DenyIncomeRequest.aspx',
                    title: 'Отказ по обращению',
                    allowMaximize: false,
                    width: 600,
                    showClose: true,
                    dialogReturnValueCallback: Function.createDelegate(null, function (result, returnValue) {
                        if (result == SP.UI.DialogResult.OK) {

                            var reasonCode = returnValue.SelectedReason.Code;
                            var reasonText = returnValue.ActionComment;

                            // Установка причины отказа и комментария
                            ir.SetRefuseReasonAndComment(incomeRequestId, reasonCode, reasonText).success(function() {
                                // Установка статуса обращения
                                ir.CalculateDatesAndSetStatus(incomeRequestId, 1080).success(function () {
                                    // Генерация документов отказа
                                    ir.CreateIncomeRequestRefuseNotifyDocument(incomeRequestId).success(function (refuseDocData) {
                                        if (refuseDocData && refuseDocData.d && refuseDocData.d.ID != 0) {
                                            // Подписание документа отказа
                                            ir.SignDocumentContent(refuseDocData.d.ServerRelativeUrl, function (signedRefuseDoc) {
                                                // Сохранение подписи документа отказа
                                                ir.SaveDocumentDetachedSignature(refuseDocData.d.ID, signedRefuseDoc).success(function () {
                                                    // Удаление черновиков всех разрешений по всем ТС обращения
                                                    ir.DeleteLicenseDraftsOnRefusing(incomeRequestId).success(function() {
                                                        // Получение xml для измененного состояния обращения
                                                        ir.GetIncomeRequestCoordinateV5StatusMessage(incomeRequestId).success(function (data) {
                                                            //Подписание xml
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
                                                }).fail(onfail);
                                            }, onfail);
                                        } else onfail("Не удалось создать документ-уведомление");
                                    }).fail(onfail);
                                }).fail(onfail);
                            }).fail(onfail);
                        }
                    })
                };

                SP.UI.ModalDialog.showModalDialog(options);
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

            ir.SaveDocumentDetachedSignature = function(documentId, signature) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/SaveDocumentDetachedSignature',
                    data: '{ documentId: ' + documentId + ' , signature: "' + encodeURIComponent(signature) + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

            ir.SignXml = function(xml, onsuccess, onfail) {

                var oCertificate = ir.SelectedCertificate || (cryptoPro && cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_LOCAL_MACHINE_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED));

                ir.SelectedCertificate = ir.SelectedCertificate || oCertificate;

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
                        if (onfail) onfail(errorMsg);
                    } else {
                        onsuccess(signedData);
                    }

                } else {
                    if (onfail) onfail("При формировании ЭЦП не удалось обнаружить сертификат");
                }
            };

            ir.SignPkcs7 = function (dataToSign, onsuccess, onfail) {

                var oCertificate = ir.SelectedCertificate || (cryptoPro && cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_LOCAL_MACHINE_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED));

                ir.SelectedCertificate = ir.SelectedCertificate || oCertificate;

                if (oCertificate) {
                    var signedData;
                    var errorMsg;
                    try {
                        signedData = cryptoPro.signPkcs7Create(oCertificate, dataToSign);
                    } catch (e) {
                        errorMsg = "Ошибка при формировании подписи pkcs7: " + e.message;
                    }

                    if (errorMsg) {
                        if (onfail) onfail(errorMsg);
                    } else {
                        onsuccess(signedData);
                    }

                } else {
                    if (onfail) onfail("При формировании ЭЦП pkcs7 не удалось обнаружить сертификат");
                }
            };

            ir.SignDocumentContent = function(documentUrl, onsuccess, onfail) {
                $.get(documentUrl, function(data) {
                    if (data) {
                        ir.SignPkcs7(data, onsuccess, onfail);
                    } else onfail("Невозможно загрузить документ по адресу " + documentUrl);
                });
            };

            ir.DeleteLicenseDraftsOnRefusing = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/DeleteLicenseDraftsOnRefusing',
                    data: '{ refusedIncomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.SelectedCertificate = null;

            return ir;
        })(tmsp.IncomeRequest || (tmsp.IncomeRequest = {}));

        return tmsp;
    })(tm.SP || (tm.SP = {}));

})(TM || (TM = {}));