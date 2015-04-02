﻿/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />
/// <reference path="CryptoProTs.ts" />

module TM.SP_.License {

    export module RequestParams {

        export class LicenseCommonParam extends TM.SP_.RequestParams.EntityCommonParam {

            public Stringify(): string {
                var str = super.Stringify();
                return str.replace("EntityId", "licenseId");
            }
        }

        export class MakeObsoleteParam extends LicenseCommonParam {
            public obsolete: boolean;
        }

        export class MakeObsoleteSignedParam extends MakeObsoleteParam {
            public signature: string;
        }

        export class DisableGibddParam extends LicenseCommonParam {
            public disabled: boolean;
        }

        export class DisableGibddSignedParam extends DisableGibddParam {
            public signature: string;
        }
    }

    export class LicenseEntityHelper extends EntityHelper {

        public ServiceUrl(): string {
            var rootUrl = super.getServiceUrl();
            return SP.ScriptHelpers.urlCombine(rootUrl, "LicenseService.aspx");
        }

        public MakeObsoleteGetXml(obsolete: boolean): JQueryXHR {

            var param = new RequestParams.MakeObsoleteParam(this);
            param.obsolete = obsolete;
            var url = SP.ScriptHelpers.urlCombine(this.ServiceUrl(), "MakeObsoleteGetXml");

            return RequestMethods.MakePostRequest(param, url);
        }

        public MakeObsoleteSaveSigned(obsolete: boolean, signature: string): JQueryXHR {

            var param       = new RequestParams.MakeObsoleteSignedParam(this);
            param.obsolete  = obsolete;
            param.signature = encodeURIComponent(signature);
            var url = SP.ScriptHelpers.urlCombine(this.ServiceUrl(), "SaveSignedMakeObsolete");
                
            return RequestMethods.MakePostRequest(param, url);
        }

        public DisableGibddGetXml(disabled: boolean): JQueryXHR {
            var param = new RequestParams.DisableGibddParam(this);
            param.disabled = disabled;
            var url = SP.ScriptHelpers.urlCombine(this.ServiceUrl(), "DisableGibddGetXml");

            return RequestMethods.MakePostRequest(param, url);
        }

        public DisabledGibddSaveSigned(disabled: boolean, signature: string): JQueryXHR {
            var param       = new RequestParams.DisableGibddSignedParam(this);
            param.disabled  = disabled;
            param.signature = encodeURIComponent(signature);
            var url = SP.ScriptHelpers.urlCombine(this.ServiceUrl(), "SaveSignedDisableGibdd");
                
            return RequestMethods.MakePostRequest(param, url);
        }

        public ChangeObsoleteAttribute(obsolete: boolean, success: () => void, fail: (msg: string) => void): void
        {
            this.MakeObsoleteGetXml(obsolete).done((xml : any) => {

                var dataToSign: string = xml.d;
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

                    var signedData: string;
                    try {
                        signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                    } catch (e) {
                        fail("Ошибка при формировании подписи: " + e.message);
                    }

                    if (typeof signedData === 'undefined' || !signedData) {
                        fail("Ошибка при формировании подписи");
                    }

                    this.MakeObsoleteSaveSigned(obsolete, signedData).done(success).fail(fail);
                }
            }).fail(() => {
                fail("Ошибка при получении xml для разрешения");
            });
        }

        public ChangeDisableGibddAttribute(disabled: boolean, success: () => void, fail: (msg: string) => void): void
        {
            this.DisableGibddGetXml(disabled).done((xml) => {
                var dataToSign: string = xml.d;

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

                    var signedData: string;
                    try {
                        signedData = cryptoPro.SignXMLCreate(oCertificate, dataToSign);
                    } catch (e) {
                        fail("Ошибка при формировании подписи: " + e.message);
                    }

                    if (typeof signedData === 'undefined' || !signedData)
                        fail("Ошибка при формировании подписи");

                    this.DisabledGibddSaveSigned(disabled, signedData).done(success).fail(fail);
                }
            }).fail(function () {
                fail("Ошибка при получении xml для разрешения");
            });
        }
    }
}