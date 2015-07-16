/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />
/// <reference path="CryptoProTs.ts" />
/// <reference path="CurrentLicense.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var TM;
(function (TM) {
    var SP_;
    (function (SP_) {
        var License;
        (function (License) {
            "use strict";
            var RequestParams;
            (function (RequestParams) {
                var LicenseCommonParam = (function (_super) {
                    __extends(LicenseCommonParam, _super);
                    function LicenseCommonParam() {
                        _super.apply(this, arguments);
                    }
                    LicenseCommonParam.prototype.Stringify = function () {
                        var str = _super.prototype.Stringify.call(this);
                        return str.replace("EntityId", "licenseId");
                    };
                    return LicenseCommonParam;
                })(TM.SP_.RequestParams.EntityCommonParam);
                RequestParams.LicenseCommonParam = LicenseCommonParam;
                var MakeObsoleteParam = (function (_super) {
                    __extends(MakeObsoleteParam, _super);
                    function MakeObsoleteParam() {
                        _super.apply(this, arguments);
                    }
                    return MakeObsoleteParam;
                })(LicenseCommonParam);
                RequestParams.MakeObsoleteParam = MakeObsoleteParam;
                var MakeObsoleteSignedParam = (function (_super) {
                    __extends(MakeObsoleteSignedParam, _super);
                    function MakeObsoleteSignedParam() {
                        _super.apply(this, arguments);
                    }
                    return MakeObsoleteSignedParam;
                })(MakeObsoleteParam);
                RequestParams.MakeObsoleteSignedParam = MakeObsoleteSignedParam;
                var DisableGibddParam = (function (_super) {
                    __extends(DisableGibddParam, _super);
                    function DisableGibddParam() {
                        _super.apply(this, arguments);
                    }
                    return DisableGibddParam;
                })(LicenseCommonParam);
                RequestParams.DisableGibddParam = DisableGibddParam;
                var DisableGibddSignedParam = (function (_super) {
                    __extends(DisableGibddSignedParam, _super);
                    function DisableGibddSignedParam() {
                        _super.apply(this, arguments);
                    }
                    return DisableGibddSignedParam;
                })(DisableGibddParam);
                RequestParams.DisableGibddSignedParam = DisableGibddSignedParam;
            })(RequestParams = License.RequestParams || (License.RequestParams = {}));
            var LicenseEntityHelper = (function (_super) {
                __extends(LicenseEntityHelper, _super);
                function LicenseEntityHelper() {
                    _super.apply(this, arguments);
                }
                LicenseEntityHelper.prototype.ServiceUrl = function () {
                    var rootUrl = _super.prototype.ServiceUrl.call(this);
                    return SP.ScriptHelpers.urlCombine(rootUrl, "LicenseService.aspx");
                };
                LicenseEntityHelper.prototype.MakeObsoleteGetXml = function (obsolete, reason) {
                    var param = new RequestParams.MakeObsoleteParam(this);
                    param.obsolete = obsolete;
                    param.reason = reason;
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("MakeObsoleteGetXml"));
                };
                LicenseEntityHelper.prototype.MakeObsoleteSaveSigned = function (obsolete, reason, signature) {
                    var param = new RequestParams.MakeObsoleteSignedParam(this);
                    param.obsolete = obsolete;
                    param.reason = reason;
                    param.signature = encodeURIComponent(signature);
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("SaveSignedMakeObsolete"));
                };
                LicenseEntityHelper.prototype.DisableGibddGetXml = function (disabled, reason) {
                    var param = new RequestParams.DisableGibddParam(this);
                    param.disabled = disabled;
                    param.reason = reason;
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("DisableGibddGetXml"));
                };
                LicenseEntityHelper.prototype.DisabledGibddSaveSigned = function (disabled, reason, signature) {
                    var param = new RequestParams.DisableGibddSignedParam(this);
                    param.disabled = disabled;
                    param.reason = reason;
                    param.signature = encodeURIComponent(signature);
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("SaveSignedDisableGibdd"));
                };
                LicenseEntityHelper.prototype.ValidateLicense = function () {
                    return this.PostWebMethod(RequestParams.LicenseCommonParam, null, "ValidateLicense");
                };
                LicenseEntityHelper.prototype.ChangeObsoleteAttribute = function (obsolete, reason, success, fail) {
                    var _this = this;
                    this.EnsureCertificate(function (data) {
                        _this.MakeObsoleteGetXml(obsolete, reason).done(function (xml) {
                            var dataToSign = xml.d;
                            var oCertificate = _this.selectedCertificate || cryptoPro.SelectCertificate(2 /* CAPICOM_CURRENT_USER_STORE */, cryptoPro.StoreNames.CAPICOM_MY_STORE, 2 /* CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED */);
                            if (oCertificate) {
                                dataToSign = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" + "<Envelope xmlns=\"urn:envelope\">\n" + dataToSign + " \n" + "</Envelope>";
                                var signedData;
                                try {
                                    signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                                }
                                catch (e) {
                                    fail("Ошибка при формировании подписи: " + e.message);
                                }
                                if (typeof signedData === "undefined" || !signedData) {
                                    fail("Ошибка при формировании подписи");
                                }
                                _this.MakeObsoleteSaveSigned(obsolete, reason, signedData).done(success).fail(fail);
                            }
                        }).fail(function () {
                            fail("Ошибка при получении xml для разрешения");
                        });
                    }, function (error) {
                        fail('Не удалось выбрать сертификат для подписания. Действие прервано.');
                    });
                };
                LicenseEntityHelper.prototype.ChangeDisableGibddAttribute = function (disabled, reason, success, fail) {
                    var _this = this;
                    this.EnsureCertificate(function (data) {
                        _this.DisableGibddGetXml(disabled, reason).done(function (xml) {
                            var dataToSign = xml.d;
                            var oCertificate = _this.selectedCertificate || cryptoPro.SelectCertificate(2 /* CAPICOM_CURRENT_USER_STORE */, cryptoPro.StoreNames.CAPICOM_MY_STORE, 2 /* CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED */);
                            if (oCertificate) {
                                dataToSign = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" + "<Envelope xmlns=\"urn:envelope\">\n" + dataToSign + " \n" + "</Envelope>";
                                var signedData;
                                try {
                                    signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                                }
                                catch (e) {
                                    fail("Ошибка при формировании подписи: " + e.message);
                                }
                                if (typeof signedData === "undefined" || !signedData) {
                                    fail("Ошибка при формировании подписи");
                                }
                                _this.DisabledGibddSaveSigned(disabled, reason, signedData).done(success).fail(fail);
                            }
                        }).fail(function () {
                            fail("Ошибка при получении xml для разрешения");
                        });
                    }, function (error) {
                        fail('Не удалось выбрать сертификат для подписания. Действие прервано.');
                    });
                };
                return LicenseEntityHelper;
            })(SP_.EntityHelper);
            License.LicenseEntityHelper = LicenseEntityHelper;
            var RibbonActions = (function () {
                function RibbonActions() {
                }
                RibbonActions.makeObsolete = function () {
                    JSRequest.EnsureSetup();
                    SP.UI.Status.removeAllStatus(true);
                    var newValue = !License.getCurrent().get_item('Tm_LicenseObsolete');
                    if (newValue) {
                        var options = new SP.UI.DialogOptions();
                        options.url = _spPageContextInfo.webAbsoluteUrl + '/ProjectSitePages/LicenseMakeObsolete.aspx?ItemId=' + License.getCurrent().get_id();
                        options.title = 'Установка признака "Устаревшие данные"';
                        options.allowMaximize = false;
                        options.showClose = true;
                        options.dialogReturnValueCallback = function (dialogResult, returnValue) {
                            if (dialogResult == SP.UI.DialogResult.OK) {
                                if (returnValue == null) {
                                    TM["SP"].showIconNotification('Признак "Устаревшие данные" изменен', '_layouts/15/images/kpinormal-0.gif', true);
                                    debugger;
                                    setTimeout(function () {
                                        var gobackBtn = $('input[type=button][name*="GoBack"]');
                                        if (gobackBtn) {
                                            gobackBtn.click();
                                        }
                                    }, 2000);
                                }
                                else if (returnValue == -1) {
                                    TM["SP"].showIconNotification('В процессе установки признака возникли ошибки', '_layouts/15/images/kpinormal-2.gif', true);
                                    setTimeout(function () {
                                        SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.cancel);
                                    }, 2000);
                                }
                            }
                        };
                        SP.UI.ModalDialog.showModalDialog(options);
                    }
                    else {
                        License.getHelper().ChangeObsoleteAttribute(false, '', function () {
                            var successMsgPart = newValue ? 'установлен' : 'снят';
                            var successStatus = SP.UI.Status.addStatus('Признак "Устаревшие данные" успешно ' + successMsgPart);
                            SP.UI.Status.setStatusPriColor(successStatus, 'green');
                            debugger;
                            setTimeout(function () {
                                var gobackBtn = $('input[type=button][name*="GoBack"]');
                                if (gobackBtn) {
                                    gobackBtn.click();
                                }
                            }, 2000);
                        }, function (failObj) {
                            var msg;
                            if (typeof failObj == 'string') {
                                msg = failObj;
                            }
                            else {
                                var jqXhr = failObj;
                                var response = $.parseJSON(jqXhr.responseText).d;
                                console.error('Exception Message: ' + response.Error.SystemMessage);
                                console.error('Exception StackTrace: ' + response.Error.StackTrace);
                                msg = response.Error.UserMessage + ': ' + response.Error.SystemMessage;
                            }
                            var failStatus = SP.UI.Status.addStatus(msg);
                            SP.UI.Status.setStatusPriColor(failStatus, 'red');
                        });
                    }
                };
                RibbonActions.makeObsoleteEnabled = function () {
                    var result = false;
                    if (License.getCurrent() != null) {
                        var isLast = License.getCurrent().get_item('_x0421__x0441__x044b__x043b__x04');
                        result = isLast;
                    }
                    else {
                        SP.SOD.executeOrDelayUntilScriptLoaded(RefreshCommandUI, 'CurrentLicense.js');
                    }
                    return result;
                };
                return RibbonActions;
            })();
            License.RibbonActions = RibbonActions;
        })(License = SP_.License || (SP_.License = {}));
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
//# sourceMappingURL=LicenseEntityHelper.js.map