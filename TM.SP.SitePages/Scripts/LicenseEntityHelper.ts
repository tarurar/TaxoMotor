/// <reference path="typings/sharepoint/SharePoint.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="EntityHelper.ts" />
/// <reference path="CryptoProTs.ts" />

module TM.SP_.License {

    export module RequestParams {

        export class LicenseCommonParam extends TM.SP_.RequestParams.CommonParam {
            public licenseId: number;

            constructor(entity: LicenseEntityHelper) {
                super();

                this.licenseId = entity.currentItem.get_id();
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

            return RequestMethods.MakePostRequest(param, "MakeObsoleteGetXml");
        }

        public MakeObsoleteSaveSigned(obsolete: boolean, signature: string): JQueryXHR {

            var param       = new RequestParams.MakeObsoleteSignedParam(this);
            param.obsolete  = obsolete;
            param.signature = encodeURIComponent(signature);
                
            return RequestMethods.MakePostRequest(param, "SaveSignedMakeObsolete");
        }

        public DisableGibddGetXml(disabled: boolean): JQueryXHR {
            var param = new RequestParams.DisableGibddParam(this);
            param.disabled = disabled;

            return RequestMethods.MakePostRequest(param, "DisableGibddGetXml");
        }

        public DisabledGibddSaveSigned(disabled: boolean, signature: string): JQueryXHR {
            var param       = new RequestParams.DisableGibddSignedParam(this);
            param.disabled  = disabled;
            param.signature = signature;
                
            return RequestMethods.MakePostRequest(param, "SaveSignedDisableGibdd");
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