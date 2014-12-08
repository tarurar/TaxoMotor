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

            ir.IsAllTaxiInStatusHasBlankNo = function(incomeRequestId, status) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/IsAllTaxiInStatusHasBlankNo',
                    data: '{ incomeRequestId: ' + incomeRequestId + ', status: "' + status + '" }',
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

            ir.AcceptTaxiRequest = function (incomeRequestId, taxiIdList) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/AcceptTaxi',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' , taxiIdList: "' + taxiIdList + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.RefuseTaxiRequest = function(incomeRequestId, taxiIdList, refuseReasonCode, refuseComment,
                needPersonVisit) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/RefuseTaxi',
                    data: '{ incomeRequestId: ' + incomeRequestId +
                          ' , taxiIdList: "' + taxiIdList + '"' +
                          ' , refuseReasonCode: ' + refuseReasonCode +
                          ' , refuseComment: "' + refuseComment + '"' +
                          ' , needPersonVisit: ' + needPersonVisit + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

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

            ir.CreateDocumentsWhileClosing = function(incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/CreateDocumentsWhileClosing',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.CreateDocumentsWhileRefusing = function(incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/CreateDocumentsWhileRefusing',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.PromoteLicenseDrafts = function(incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/PromoteLicenseDrafts',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.GetLicenseXmlById = function (licenseIdList) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/GetLicenseXmlById',
                    data: '{ licenseIdList: "' + licenseIdList + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.DeleteLicenseDraftsByTaxiStatus = function (incomeRequestId, status) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/DeleteLicenseDraftsByTaxiStatus',
                    data: '{ incomeRequestId: ' + incomeRequestId + ', status: "' + status + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.SetStatusOnClosing = function(incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/SetStatusOnClosing',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.UpdateSignatureForLicense = function (licenseId, signature) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/UpdateSignatureForLicense',
                    data: '{ licenseId: ' + licenseId + ', signature: "' + encodeURIComponent(signature) + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.UpdateOutcomeRequestsOnClosing = function(closingIncomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/UpdateOutcomeRequestsOnClosing',
                    data: '{ closingIncomeRequestId: ' + closingIncomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.GetCurrentStatusCode = function(incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/GetCurrentStatusCode',
                    data: '{ incomeRequestId: ' + incomeRequestId + ' }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            };

            ir.OutputRequest = function (incomeRequestId) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/Output',
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

            ir.SendStatus = function(incomeRequestId, attachsStr) {
                var url = _spPageContextInfo.webAbsoluteUrl + '/' + _spPageContextInfo.layoutsUrl + '/' + 'TaxoMotor/SendStatus.aspx?ListId=' +
                    _spPageContextInfo.pageListId +
                    '&Items=' + incomeRequestId +
                    (attachsStr ? '&AttachDocuments=' + attachsStr : '');

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

            ir.EndingApplyForCancellation = function (incomeRequestId, onsuccess, onfail) {
                // Запросы ПТС по транспортным средствам обращения
                //ir.SendPTSRequest(incomeRequestId, function () {
                    // Расчет сроков оказания услуги и установка статуса обращения
                    ir.CalculateDatesAndSetStatus(incomeRequestId, 1050).success(function () {
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
                //}, onfail);
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

            ir.ApplyForCancellationForPrivateEntrepreneur = function (incomeRequestId, onsuccess, onfail) {
                // Запрос в ЕГРИП
                //ir.SendEgripRequest(incomeRequestId, function () {
                    ir.EndingApplyForCancellation(incomeRequestId, onsuccess, onfail);
                //}, onfail);
            };

            ir.ApplyForCancellationForJuridicalPerson = function (incomeRequestId, onsuccess, onfail) {
                // Запрос в ЕГРЮЛ
                //ir.SendEgrulRequest(incomeRequestId, function () {
                    ir.EndingApplyForCancellation(incomeRequestId, onsuccess, onfail);
                //}, onfail);
            };

            ir.ApplyForNew = function(incomeRequestId, onsuccess, onfail) {
                // Проверить всем ли ТС присвоен статус
                ir.IsAllTaxiInStatus(incomeRequestId, "В работе;Отказано").success(function (data) {
                    if (data && data.d) {
                        // Проверить, остались ли в обращении ТС со статусом
                        ir.IsAnyTaxiInStatus(incomeRequestId, "В работе").success(function (data) {
                            if (data && data.d) {
                                // Провести проверку на дубли разрешений
                                //ir.CanReleaseNewLicensesForRequest(incomeRequestId).success(function (data) {
                                    //if (data && data.d.CanRelease) {
                                        // Заявитель - индивидуальный предприниматель?
                                        ir.IsRequestDeclarantPrivateEntrepreneur(incomeRequestId).success(function (data) {
                                            if (data && data.d) {
                                                ir.ApplyForNewForPrivateEntrepreneur(incomeRequestId, onsuccess, onfail);
                                            } else {
                                                ir.ApplyForNewForJuridicalPerson(incomeRequestId, onsuccess, onfail);
                                            }
                                        }).fail(function (err) { onfail("При проверке заявителя возникла ошибка"); });
                                    //} else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' уже существует');
                                //}).fail(onfail);
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
                                //ir.HasRequestActingLicenses(incomeRequestId).success(function (data) {
                                    //if (data && data.d.CanRelease) {
                                        // Расчет сроков оказания услуги и установка статуса обращения
                                        ir.CalculateDatesAndSetStatus(incomeRequestId, 1050).success(function () {
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
                                    //} else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' не существует');
                                //}).fail(onfail);
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
                                //ir.HasRequestActingLicenses(incomeRequestId).success(function (data) {
                                    //if (data && data.d.CanRelease) {
                                        // Заявитель - индивидуальный предприниматель?
                                        ir.IsRequestDeclarantPrivateEntrepreneur(incomeRequestId).success(function (data) {
                                            if (data && data.d) {
                                                ir.ApplyForChangeForPrivateEntrepreneur(incomeRequestId, onsuccess, onfail);
                                            } else {
                                                ir.ApplyForChangeForJuridicalPerson(incomeRequestId, onsuccess, onfail);
                                            }
                                        }).fail(function (err) { onfail("При проверке заявителя возникла ошибка"); });
                                    //} else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' не существует');
                                //}).fail(onfail);
                            } else onfail('В обращении нет ТС');
                        }).fail(onfail);
                    } else onfail('Не всем ТС проставлен признак');
                }).fail(onfail);
            }

            ir.ApplyForCancellation = function (incomeRequestId, onsuccess, onfail) {
                // Проверить всем ли ТС присвоен статус
                ir.IsAllTaxiInStatus(incomeRequestId, "В работе;Отказано").success(function (data) {
                    if (data && data.d) {
                        // Проверить, остались ли в обращении ТС со статусом
                        ir.IsAnyTaxiInStatus(incomeRequestId, "В работе").success(function (data) {
                            if (data && data.d) {
                                // Провести проверку на дубли разрешений. Наличие дублей обязательно.
                                //ir.HasRequestActingLicenses(incomeRequestId).success(function (data) {
                                    //if (data && data.d.CanRelease) {
                                        // Заявитель - индивидуальный предприниматель?
                                        ir.IsRequestDeclarantPrivateEntrepreneur(incomeRequestId).success(function (data) {
                                            if (data && data.d) {
                                                ir.ApplyForCancellationForPrivateEntrepreneur(incomeRequestId, onsuccess, onfail);
                                            } else {
                                                ir.ApplyForCancellationForJuridicalPerson(incomeRequestId, onsuccess, onfail);
                                            }
                                        }).fail(function (err) { onfail("При проверке заявителя возникла ошибка"); });
                                    //} else onfail('Разрешение на ТС с номером ' + data.d.TaxiNumber + ' не существует');
                                //}).fail(onfail);
                            } else onfail('В обращении нет ТС');
                        }).fail(onfail);
                    } else onfail('Не всем ТС проставлен признак');
                }).fail(onfail);
            };

            ir.SetRefuseReasonAndComment = function (incomeRequestId, refuseReasonCode, refuseComment, needPersonVisit, refuseDocuments) {
                return $.ajax({
                    type: 'POST',
                    url: ir.ServiceUrl + '/SetRefuseReasonAndComment',
                    data: '{ incomeRequestId: ' + incomeRequestId +
                            ' , refuseReasonCode: ' + refuseReasonCode +
                            ' , refuseComment: "' + refuseComment +
                            '", needPersonVisit: ' + needPersonVisit +
                            ', refuseDocuments: ' +refuseDocuments + ' }',
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
                            var needPersonVisit = returnValue.NeedPersonVisit;
                            var refuseDocuments = returnValue.RefuseDocuments;

                            // Установка причины отказа и комментария
                            ir.SetRefuseReasonAndComment(incomeRequestId, reasonCode, reasonText, needPersonVisit, refuseDocuments).success(function () {
                                    // Генерация документов отказа
                                    ir.CreateDocumentsWhileRefusing(incomeRequestId).success(function (refuseDocData) {
                                        if (refuseDocData && refuseDocData.d && refuseDocData.d[0]) {
                                            var refuseFileId = refuseDocData.d[0].DocumentId;
                                            // Подписание документа отказа
                                            ir.SignDocumentContent(refuseDocData.d[0].DocumentUrl, function (signedRefuseDoc) {
                                                // Сохранение подписи документа отказа
                                                ir.SaveDocumentDetachedSignature(refuseFileId, signedRefuseDoc).success(function (sigFileData) {
                                                    if (sigFileData && sigFileData.d) {
                                                        var sigFileId = sigFileData.d;
                                                        // Удаление черновиков всех разрешений по всем ТС обращения
                                                        ir.DeleteLicenseDraftsOnRefusing(incomeRequestId).success(function () {
                                                            // В зависимости от текущего статуса устанавливаем новый
                                                            ir.GetCurrentStatusCode(incomeRequestId).success(function (data) {
                                                                var currStatus = data.d.Data;
                                                                var newStatus = currStatus == 1010 ? 1030 : 1080;
                                                                // Установка статуса обращения
                                                                ir.CalculateDatesAndSetStatus(incomeRequestId, newStatus).success(function () {
                                                                    // Получение xml для измененного состояния обращения
                                                                    ir.GetIncomeRequestCoordinateV5StatusMessage(incomeRequestId).success(function (data) {
                                                                        //Подписание xml
                                                                        if (data && data.d) {
                                                                            ir.SignXml(data.d, function (signedData) {
                                                                                // Сохранение факта изменения статуса обращения в историю изменения статусов
                                                                                ir.SaveIncomeRequestStatusLog(incomeRequestId, signedData).success(function () {
                                                                                    var attachs = refuseFileId + ', ' + sigFileId;
                                                                                    // Отправка статуса обращения по межведомственному взаимодействию
                                                                                    ir.SendStatus(incomeRequestId, attachs).success(onsuccess).fail(function (err) { onfail("При отправке статуса возникла ошибка"); });
                                                                                }).fail(onfail);
                                                                            }, onfail);
                                                                        } else onfail("Не удалось получить статус обращения в виде xml");
                                                                    }).fail(onfail);
                                                                }).fail(onfail);
                                                            }).fail(function(jqXhr, status, error) {
                                                                var response = $.parseJSON(jqXhr.responseText).d;
                                                                console.error("Exception Message: " + response.Error.SystemMessage);
                                                                console.error("Exception StackTrace: " + response.Error.StackTrace);
                                                            });
                                                        }).fail(onfail);
                                                    } else onfail("Не удалось создать документ открепленной подписи");
                                                }).fail(onfail);
                                            }, onfail);
                                        } else onfail("Не удалось создать документ-уведомление");
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
                    data: '{ documentId: ' + documentId + ' , signature: "' + signature + '" }',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                });
            }

            ir.SignXml = function(xml, onsuccess, onfail) {

                var oCertificate = ir.SelectedCertificate || (cryptoPro && cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_CURRENT_USER_STORE,
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
                        cryptoPro.StoreLocation.CAPICOM_CURRENT_USER_STORE,
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

            ir.AcceptTaxi = function (incomeRequestId, taxiIdList, onsuccess, onfail) {
                ir.AcceptTaxiRequest(incomeRequestId, taxiIdList)
                    .success(onsuccess)
                    .fail(onfail);
            };

            ir.RefuseTaxi = function (incomeRequestId, taxiIdList, onsuccess, onfail) {
                // Получим причину и комментарий
                var options = {
                    url: _spPageContextInfo.webAbsoluteUrl + '/ProjectSitePages/DenyTaxi.aspx',
                    title: 'Отказ по транспортному средству',
                    allowMaximize: false,
                    width: 600,
                    showClose: true,
                    dialogReturnValueCallback: Function.createDelegate(null, function (result, returnValue) {
                        if (result == SP.UI.DialogResult.OK) {
                            
                            var reasonCode = returnValue.SelectedReason.Code;
                            var reasonText = returnValue.ActionComment;
                            var needPersonVisit = returnValue.NeedPersonVisit;
                            // Выполним запрос на отмену транспортных средств в обращении
                            ir.RefuseTaxiRequest(incomeRequestId, taxiIdList, reasonCode, reasonText, needPersonVisit)
                                .success(onsuccess)
                                .fail(onfail);
                        }
                    })
                };

                SP.UI.ModalDialog.showModalDialog(options);
            }

            ir.SignDocumentSaveSignatureMultiple = function(docsMetadata, onsucces, onfail) {
                var deferreds = [];

                for (var i = 0; i < docsMetadata.length; i++) {
                    var currentDeferred = $.Deferred();
                    var currentDocData = docsMetadata[i];

                    ir.SignDocumentContent(currentDocData.DocumentUrl, function(document, def) {
                        return function(signedDoc) {
                            ir.SaveDocumentDetachedSignature(document.DocumentId, signedDoc).success(function(sigFileData) {
                                if (sigFileData && sigFileData.d) {
                                    def.resolveWith(null, [sigFileData.d]);
                                } else def.rejectWith(document, ["Не удалось создать документ открепленной подписи"]);
                            }).fail(function(jqXhr, status, error) {
                                def.rejectWith(document, [error]);
                            });
                        }
                    }(currentDocData, currentDeferred), function(document, def) {
                        return function(err) {
                            def.rejectWith(document, [err]);
                        }
                    }(currentDocData, currentDeferred));

                    deferreds.push(currentDeferred);
                }

                $.when.apply($, deferreds).done(onsucces).fail(onfail);
            };

            ir.SignLicenseSaveSignatureMultiple = function (docsMetadata, onsucces, onfail) {
                var deferreds = [];

                for (var i = 0; i < docsMetadata.length; i++) {
                    var currentDeferred = $.Deferred();
                    var currentDocData = docsMetadata[i];

                    ir.SignXml(currentDocData.Xml, function(document, def) {
                        return function(signedData) {
                            ir.UpdateSignatureForLicense(document.ExternalId, signedData).success(function() {
                                def.resolve();
                            }).fail(function (jqXhr, status, error) {
                                var response = $.parseJSON(jqXhr.responseText).d;
                                console.error("Exception Message: " + response.Error.SystemMessage);
                                console.error("Exception StackTrace: " + response.Error.StackTrace);
                                def.rejectWith(document, [response.Error.UserMessage]);
                            });
                        }
                    }(currentDocData, currentDeferred), function(document, def) {
                        return function(err)
                        {
                            def.rejectWith(document, [err]);
                        }
                    }(currentDocData, currentDeferred));

                    deferreds.push(currentDeferred);
                }

                $.when.apply($, deferreds).done(onsucces).fail(onfail);
            };

            // Выполняется для всех услуг кроме аннулирования
            ir.PromoteLicenseDraftsAndSign = function (incomeRequestId) {

                var def = $.Deferred();

                ir.GetContentTypeName(incomeRequestId, function (ctName) {
                    if (ctName == 'Аннулирование') {
                        def.resolve();
                    }
                    else {
                        ir.PromoteLicenseDrafts(incomeRequestId).success(function (data) {
                            ir.GetLicenseXmlById(data.d.Data).success(function (data) {
                                ir.SignLicenseSaveSignatureMultiple(data.d.Data, def.resolve, def.reject);
                            }).fail(function (jqXhr, status, error) {
                                var response = $.parseJSON(jqXhr.responseText).d;
                                console.error("Exception Message: " + response.Error.SystemMessage);
                                console.error("Exception StackTrace: " + response.Error.StackTrace);
                                def.reject();
                            });
                        }).fail(function (jqXhr, status, error) {
                            var response = $.parseJSON(jqXhr.responseText).d;
                            console.error("Exception Message: " + response.Error.SystemMessage);
                            console.error("Exception StackTrace: " + response.Error.StackTrace);
                            def.reject();
                        });
                    }
                });

                return def;
            };

            ir.GetContentTypeName = function (incomeRequestId, callback) {

                SP.SOD.executeOrDelayUntilScriptLoaded(function() {
                    var ctx  = SP.ClientContext.get_current();
                    var list = ctx.get_web().get_lists().getById(_spPageContextInfo.pageListId);
                    var item = list.getItemById(incomeRequestId);
                    var ct   = item.get_contentType();

                    ctx.load(ct);
                    ctx.executeQueryAsync(function () {
                        if (callback)
                            callback(ct.get_name());
                    });
                }, "sp.js");
            }

            ir.Output = function (incomeRequestId, onsuccess, onfail) {
                ir.OutputRequest(incomeRequestId)
                    .success(onsuccess)
                    .fail(onfail);
            };

            ir.Close = function(incomeRequestId, onsuccess, onfail) {

                var options = {
                    url: _spPageContextInfo.webAbsoluteUrl + '/ProjectSitePages/ProgressDlg.aspx',
                    title: 'Завершение работы с обращением',
                    allowMaximize: false,
                    showClose: false,
                    dialogReturnValueCallback: Function.createDelegate(null, function (result, returnValue) {
                        SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);
                    })
                };
                SP.UI.ModalDialog.showModalDialog(options);

                TM.SP.registeredProgressDlgConsumer = function(progress) {
                    
                    var action = progress.addAction('Проверка на наличие ТС со статусом "Решено положительно"');
                    ir.IsAnyTaxiInStatus(incomeRequestId, "Решено положительно").success(function(data) {
                        if (data && data.d) {
                            progress.finishAction(action, 10);

                            action = progress.addAction('Проверяем все ли ТС находятся в статусе "Решено положительно" или "Отказано" или "Решено отрицательно"');
                            ir.IsAllTaxiInStatus(incomeRequestId, "Решено положительно;Отказано;Решено отрицательно").success(function (data) {
                                if (data && data.d) {
                                    progress.finishAction(action, 20);

                                    action = progress.addAction('Проверяем у всех ли ТС со статусом "Решено положительно" заполнены номер и серия бланка разрешения');
                                    ir.IsAllTaxiInStatusHasBlankNo(incomeRequestId, "Решено положительно").success(function (data) {
                                        if (data && data.d) {
                                            progress.finishAction(action, 30);

                                            action = progress.addAction('Формирование и подписание уведомлений');
                                            ir.CreateDocumentsWhileClosing(incomeRequestId).success(function (docs) {
                                                if (docs && docs.d) {

                                                    var fileIdList = $.map(docs.d, function (el) { return el.DocumentId; }).join(',');
                                                    ir.SignDocumentSaveSignatureMultiple(docs.d, function () {

                                                        var sigFileIdList = Array.prototype.slice.call(arguments).join(',');
                                                        progress.finishAction(action, 50);
                                                        // Для всех услуг кроме аннулирование подписываем разрешения
                                                        action = progress.addAction('Подписание разрешений');
                                                        ir.PromoteLicenseDraftsAndSign(incomeRequestId).done(function () {
                                                            progress.finishAction(action, 70);
                                                            
                                                            action = progress.addAction('Удаление черновиков разрешений');
                                                            ir.DeleteLicenseDraftsByTaxiStatus(incomeRequestId, "Отказано").success(function() {
                                                                progress.finishAction(action, 80);

                                                                action = progress.addAction('Установка и отправка статуса обращения в АС ГУФ');
                                                                ir.SetStatusOnClosing(incomeRequestId).success(function () {
                                                                    // Получение xml для измененного состояния обращения
                                                                    ir.GetIncomeRequestCoordinateV5StatusMessage(incomeRequestId).success(function (data) {
                                                                        //Подписание xml
                                                                        if (data && data.d) {
                                                                            ir.SignXml(data.d, function(signedData) {
                                                                                // Сохранение факта изменения статуса обращения в историю изменения статусов
                                                                                ir.SaveIncomeRequestStatusLog(incomeRequestId, signedData).success(function () {
                                                                                    // Отправка статуса обращения по межведомственному взаимодействию
                                                                                    var attachs = (fileIdList ? fileIdList + ',' : '') + sigFileIdList;
                                                                                    ir.SendStatus(incomeRequestId, attachs).success(function() {
                                                                                        progress.finishAction(action, 90);

                                                                                        action = progress.addAction('Обновление межведомственных запросов');
                                                                                        ir.UpdateOutcomeRequestsOnClosing(incomeRequestId).success(function() {
                                                                                            progress.finishAction(action, 100);
                                                                                        }).fail(function (jqXhr, status, error) {
                                                                                            progress.errorAction(action, error);
                                                                                        });

                                                                                    }).fail(function (jqXhr, status, error) {
                                                                                        progress.errorAction(action, error);
                                                                                    });
                                                                                }).fail(function (jqXhr, status, error) {
                                                                                    progress.errorAction(action, error);
                                                                                });
                                                                            }, function (jqXhr, status, error) {
                                                                                progress.errorAction(action, error);
                                                                            });
                                                                        } else {
                                                                            progress.errorAction(action, "Не удалось получить статус обращения в виде xml");
                                                                        };
                                                                    }).fail(function (jqXhr, status, error) {
                                                                        progress.errorAction(action, error);
                                                                    });
                                                                }).fail(function(jqXhr, status, error) {
                                                                    var response = $.parseJSON(jqXhr.responseText).d;
                                                                    console.error("Exception Message: " + response.Error.SystemMessage);
                                                                    console.error("Exception StackTrace: " + response.Error.StackTrace);
                                                                    progress.errorAction(action, response.Error.UserMessage);
                                                                });
                                                            }).fail(function(jqXhr, status, error) {
                                                                var response = $.parseJSON(jqXhr.responseText).d;
                                                                console.error("Exception Message: " + response.Error.SystemMessage);
                                                                console.error("Exception StackTrace: " + response.Error.StackTrace);
                                                                progress.errorAction(action, response.Error.UserMessage);
                                                            });
                                                        }).fail(function(err) {
                                                            progress.errorAction(action, err);
                                                        });
                                                    }, function(err) {
                                                        progress.errorAction(action, err);
                                                    });
                                                } else progress.errorAction(action);
                                            }).fail(function(jqXhr, status, error) {
                                                progress.errorAction(action, error.message);
                                            });
                                        } else progress.errorAction(action);
                                    }).fail(function (jqXhr, status, error) {
                                        progress.errorAction(action, error.message);
                                    });
                                } else progress.errorAction(action);
                            }).fail(function (jqXhr, status, error) {
                                progress.errorAction(action, error.message);
                            });
                        } else progress.errorAction(action);
                    }).fail(function (jqXHR, status, error) {
                        progress.errorAction(action, error.message);
                    });
                }
            };

            ir.SelectedCertificate = null;

            ir.FindWebPartsByListUrl = function (listUrl) {
                if (!fd_relateditems_webparts) return null;

                var retVal = [];

                for (var i = 0; i < fd_relateditems_webparts.length; i++) {
                    var ctxNum = g_ViewIdToViewCounterMap['{' + fd_relateditems_webparts[i].toUpperCase() + '}'];
                    var ctxT = window["ctx" + ctxNum];

                    if (ctxT.listUrlDir == listUrl)
                        retVal.push(ctxT);
                }

                return retVal;
            };

            ir.GetWebPartSelectedItemsDict = function(webPartContext) {
                var dictSelRet = [];
                var i = 0;

                for (var key in GetSelectedItemsDict(webPartContext)) {
                    dictSelRet[i] = {
                        id: webPartContext.dictSel[key].id,
                        fsObjType: webPartContext.dictSel[key].fsObjType
                    };
                    i++;
                }

                return dictSelRet;
            };

            return ir;
        })(tmsp.IncomeRequest || (tmsp.IncomeRequest = {}));

        return tmsp;
    })(tm.SP || (tm.SP = {}));

})(TM || (TM = {}));