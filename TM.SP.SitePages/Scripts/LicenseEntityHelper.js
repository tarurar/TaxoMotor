/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />
/// <reference path="CryptoProTs.ts" />
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
                LicenseEntityHelper.prototype.MakeObsoleteGetXml = function (obsolete) {
                    var param = new RequestParams.MakeObsoleteParam(this);
                    param.obsolete = obsolete;
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("MakeObsoleteGetXml"));
                };
                LicenseEntityHelper.prototype.MakeObsoleteSaveSigned = function (obsolete, signature) {
                    var param = new RequestParams.MakeObsoleteSignedParam(this);
                    param.obsolete = obsolete;
                    param.signature = encodeURIComponent(signature);
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("SaveSignedMakeObsolete"));
                };
                LicenseEntityHelper.prototype.DisableGibddGetXml = function (disabled) {
                    var param = new RequestParams.DisableGibddParam(this);
                    param.disabled = disabled;
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("DisableGibddGetXml"));
                };
                LicenseEntityHelper.prototype.DisabledGibddSaveSigned = function (disabled, signature) {
                    var param = new RequestParams.DisableGibddSignedParam(this);
                    param.disabled = disabled;
                    param.signature = encodeURIComponent(signature);
                    return SP_.RequestMethods.MakePostRequest(param, this.BuildMethodUrl("SaveSignedDisableGibdd"));
                };
                LicenseEntityHelper.prototype.ValidateLicense = function () {
                    return this.PostWebMethod(RequestParams.LicenseCommonParam, null, "ValidateLicense");
                };
                LicenseEntityHelper.prototype.ChangeObsoleteAttribute = function (obsolete, success, fail) {
                    var _this = this;
                    this.EnsureCertificate(function (data) {
                        _this.MakeObsoleteGetXml(obsolete).done(function (xml) {
                            var dataToSign = xml.d;
                            var oCertificate = cryptoPro.SelectCertificate(2 /* CAPICOM_CURRENT_USER_STORE */, cryptoPro.StoreNames.CAPICOM_MY_STORE, 2 /* CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED */);
                            if (oCertificate) {
                                dataToSign = "<?xml version=\"1.0\"?>\n" + "<Envelope xmlns=\"urn:envelope\">\n" + dataToSign + " \n" + "</Envelope>";
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
                                _this.MakeObsoleteSaveSigned(obsolete, signedData).done(success).fail(fail);
                            }
                        }).fail(function () {
                            fail("Ошибка при получении xml для разрешения");
                        });
                    }, function (error) {
                        fail('Не удалось выбрать сертификат для подписания. Действие прервано.');
                    });
                };
                LicenseEntityHelper.prototype.ChangeDisableGibddAttribute = function (disabled, success, fail) {
                    var _this = this;
                    this.EnsureCertificate(function (data) {
                        _this.DisableGibddGetXml(disabled).done(function (xml) {
                            var dataToSign = xml.d;
                            var oCertificate = cryptoPro.SelectCertificate(2 /* CAPICOM_CURRENT_USER_STORE */, cryptoPro.StoreNames.CAPICOM_MY_STORE, 2 /* CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED */);
                            if (oCertificate) {
                                dataToSign = "<?xml version=\"1.0\"?>\n" + "<Envelope xmlns=\"urn:envelope\">\n" + dataToSign + " \n" + "</Envelope>";
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
                                _this.DisabledGibddSaveSigned(disabled, signedData).done(success).fail(fail);
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
        })(License = SP_.License || (SP_.License = {}));
    })(SP_ = TM.SP_ || (TM.SP_ = {}));
})(TM || (TM = {}));
//# sourceMappingURL=LicenseEntityHelper.js.map