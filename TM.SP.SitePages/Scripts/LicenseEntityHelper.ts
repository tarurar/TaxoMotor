/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />
/// <reference path="CryptoProTs.ts" />

module TM.SP_.License {
    "use strict";

    export module RequestParams {

        export class LicenseCommonParam extends TM.SP_.RequestParams.EntityCommonParam {

            public Stringify(): string {
                var str = super.Stringify();
                return str.replace("EntityId", "licenseId");
            }
        }

        export class MakeObsoleteParam extends LicenseCommonParam {
            public obsolete: boolean;
            public reason: string;
        }

        export class MakeObsoleteSignedParam extends MakeObsoleteParam {
            public signature: string;
        }

        export class DisableGibddParam extends LicenseCommonParam {
            public disabled: boolean;
            public reason: string;
        }

        export class DisableGibddSignedParam extends DisableGibddParam {
            public signature: string;
        }
    }

    export class LicenseEntityHelper extends EntityHelper {

        public ServiceUrl(): string {
            var rootUrl = super.ServiceUrl();
            return SP.ScriptHelpers.urlCombine(rootUrl, "LicenseService.aspx");
        }

        public MakeObsoleteGetXml(obsolete: boolean, reason: string): JQueryXHR {

            var param = new RequestParams.MakeObsoleteParam(this);
            param.obsolete = obsolete;
            param.reason = reason;

            return RequestMethods.MakePostRequest(param, this.BuildMethodUrl("MakeObsoleteGetXml"));
        }

        public MakeObsoleteSaveSigned(obsolete: boolean, reason: string, signature: string): JQueryXHR {

            var param       = new RequestParams.MakeObsoleteSignedParam(this);
            param.obsolete  = obsolete;
            param.reason    = reason;
            param.signature = encodeURIComponent(signature);

            return RequestMethods.MakePostRequest(param, this.BuildMethodUrl("SaveSignedMakeObsolete"));
        }

        public DisableGibddGetXml(disabled: boolean, reason: string): JQueryXHR {
            var param = new RequestParams.DisableGibddParam(this);
            param.disabled = disabled;
            param.reason = reason;

            return RequestMethods.MakePostRequest(param, this.BuildMethodUrl("DisableGibddGetXml"));
        }

        public DisabledGibddSaveSigned(disabled: boolean, reason: string, signature: string): JQueryXHR {
            var param       = new RequestParams.DisableGibddSignedParam(this);
            param.disabled  = disabled;
            param.reason    = reason;
            param.signature = encodeURIComponent(signature);

            return RequestMethods.MakePostRequest(param, this.BuildMethodUrl("SaveSignedDisableGibdd"));
        }

        public ValidateLicense(): JQueryXHR {
            return this.PostWebMethod<RequestParams.LicenseCommonParam>(
                RequestParams.LicenseCommonParam, null, "ValidateLicense");
        }

        public ChangeObsoleteAttribute(obsolete: boolean, reason: string, success: () => void, fail: (msg: string) => void): void
        {
            this.EnsureCertificate((data) => {
                this.MakeObsoleteGetXml(obsolete, reason).done((xml: any) => {

                    var dataToSign: string = xml.d;
                    var oCertificate = cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_CURRENT_USER_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                    if (oCertificate) {
                        dataToSign =
                        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                        "<Envelope xmlns=\"urn:envelope\">\n" +
                        dataToSign +
                        " \n" +
                        "</Envelope>";

                        var signedData: string;
                        try {
                            signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                        } catch (e) {
                            fail("Ошибка при формировании подписи: " + e.message);
                        }

                        if (typeof signedData === "undefined" || !signedData) {
                            fail("Ошибка при формировании подписи");
                        }

                        this.MakeObsoleteSaveSigned(obsolete, reason, signedData).done(success).fail(fail);
                    }
                }).fail(() => {
                    fail("Ошибка при получении xml для разрешения");
                });
            },(error) => {
                fail('Не удалось выбрать сертификат для подписания. Действие прервано.');
            });
        }

        public ChangeDisableGibddAttribute(disabled: boolean, reason: string, success: () => void, fail: (msg: string) => void): void
        {
            this.EnsureCertificate((data) => {
                this.DisableGibddGetXml(disabled, reason).done((xml: any) => {
                    var dataToSign: string = xml.d;

                    var oCertificate = cryptoPro.SelectCertificate(
                        cryptoPro.StoreLocation.CAPICOM_CURRENT_USER_STORE,
                        cryptoPro.StoreNames.CAPICOM_MY_STORE,
                        cryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                    if (oCertificate) {
                        dataToSign =
                        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                        "<Envelope xmlns=\"urn:envelope\">\n" +
                        dataToSign +
                        " \n" +
                        "</Envelope>";

                        var signedData: string;
                        try {
                            signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                        } catch (e) {
                            fail("Ошибка при формировании подписи: " + e.message);
                        }

                        if (typeof signedData === "undefined" || !signedData) {
                            fail("Ошибка при формировании подписи");
                        }

                        this.DisabledGibddSaveSigned(disabled, reason, signedData).done(success).fail(fail);
                    }
                }).fail(() => {
                    fail("Ошибка при получении xml для разрешения");
                });
            }, (error) => {
                fail('Не удалось выбрать сертификат для подписания. Действие прервано.');
            });
        }
    }
}