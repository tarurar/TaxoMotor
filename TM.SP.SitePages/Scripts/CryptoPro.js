﻿var CryptoPro;

(function(CryptoPro) {

    // CAPICOM_STORE_LOCATION enumeration
    CryptoPro.StoreLocation = {
        CAPICOM_MEMORY_STORE: 0,
        CAPICOM_LOCAL_MACHINE_STORE: 1,
        CAPICOM_CURRENT_USER_STORE: 2,
        CAPICOM_ACTIVE_DIRECTORY_USER_STORE: 3,
        CAPICOM_SMART_CARD_USER_STORE: 4
    };

    // CAPICOM_STORE_OPEN_MODE enumeration
    CryptoPro.StoreOpenMode = {
        CAPICOM_STORE_OPEN_READ_ONLY: 0,
        CAPICOM_STORE_OPEN_READ_WRITE: 1,
        CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED: 2,
        CAPICOM_STORE_OPEN_EXISTING_ONLY: 128,
        CAPICOM_STORE_OPEN_INCLUDE_ARCHIVED: 256
    };

    // CAPICOM_CERTIFICATE_FIND_TYPE enumeration
    CryptoPro.CertFindType = {
        CAPICOM_CERTIFICATE_FIND_SHA1_HASH: 0,
        CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME: 1,
        CAPICOM_CERTIFICATE_FIND_ISSUER_NAME: 2,
        CAPICOM_CERTIFICATE_FIND_ROOT_NAME: 3,
        CAPICOM_CERTIFICATE_FIND_TEMPLATE_NAME: 4,
        CAPICOM_CERTIFICATE_FIND_EXTENSION: 5,
        CAPICOM_CERTIFICATE_FIND_EXTENDED_PROPERTY: 6,
        CAPICOM_CERTIFICATE_FIND_APPLICATION_POLICY: 7,
        CAPICOM_CERTIFICATE_FIND_CERTIFICATE_POLICY: 8,
        CAPICOM_CERTIFICATE_FIND_TIME_VALID: 9,
        CAPICOM_CERTIFICATE_FIND_TIME_NOT_YET_VALID: 10,
        CAPICOM_CERTIFICATE_FIND_TIME_EXPIRED: 11,
        CAPICOM_CERTIFICATE_FIND_KEY_USAGE: 12
    };

    // CAPICOM_PROPID enumeration
    CryptoPro.PropId = {
        CAPICOM_PROPID_UNKNOWN: 0,
        CAPICOM_PROPID_KEY_PROV_HANDLE: 1,
        CAPICOM_PROPID_KEY_PROV_INFO: 2,
        CAPICOM_PROPID_SHA1_HASH: 3,
        CAPICOM_PROPID_HASH_PROP: 3,
        CAPICOM_PROPID_MD5_HASH: 4,
        CAPICOM_PROPID_KEY_CONTEXT: 5,
        CAPICOM_PROPID_KEY_SPEC: 6,
        CAPICOM_PROPID_IE30_RESERVED: 7,
        CAPICOM_PROPID_PUBKEY_HASH_RESERVED: 8,
        CAPICOM_PROPID_ENHKEY_USAGE: 9,
        CAPICOM_PROPID_CTL_USAGE: 9,
        CAPICOM_PROPID_NEXT_UPDATE_LOCATION: 10,
        CAPICOM_PROPID_FRIENDLY_NAME: 11,
        CAPICOM_PROPID_PVK_FILE: 12,
        CAPICOM_PROPID_DESCRIPTION: 13,
        CAPICOM_PROPID_ACCESS_STATE: 14,
        CAPICOM_PROPID_SIGNATURE_HASH: 15,
        CAPICOM_PROPID_SMART_CARD_DATA: 16,
        CAPICOM_PROPID_EFS: 17,
        CAPICOM_PROPID_FORTEZZA_DATA: 18,
        CAPICOM_PROPID_ARCHIVED: 19,
        CAPICOM_PROPID_KEY_IDENTIFIER: 20,
        CAPICOM_PROPID_AUTO_ENROLL: 21,
        CAPICOM_PROPID_PUBKEY_ALG_PARA: 22,
        CAPICOM_PROPID_CROSS_CERT_DIST_POINTS: 23,
        CAPICOM_PROPID_ISSUER_PUBLIC_KEY_MD5_HASH: 24,
        CAPICOM_PROPID_SUBJECT_PUBLIC_KEY_MD5_HASH: 25,
        CAPICOM_PROPID_ENROLLMENT: 26,
        CAPICOM_PROPID_DATE_STAMP: 27,
        CAPICOM_PROPID_ISSUER_SERIAL_NUMBER_MD5_HASH: 28,
        CAPICOM_PROPID_SUBJECT_NAME_MD5_HASH: 29,
        CAPICOM_PROPID_EXTENDED_ERROR_INFO: 30,
        CAPICOM_PROPID_RENEWAL: 64,
        CAPICOM_PROPID_ARCHIVED_KEY_HASH: 65,
        CAPICOM_PROPID_FIRST_RESERVED: 66,
        CAPICOM_PROPID_LAST_RESERVED: 0x00007FFF,
        CAPICOM_PROPID_FIRST_USER: 0x00008000,
        CAPICOM_PROPID_LAST_USER: 0x0000FFFF
    };

    // CADESCOM_XML_SIGNATURE_TYPE enumeration
    CryptoPro.SignatureType = {
        CADESCOM_XML_SIGNATURE_TYPE_ENVELOPED: 0,
        CADESCOM_XML_SIGNATURE_TYPE_ENVELOPING: 1,
        CADESCOM_XML_SIGNATURE_TYPE_TEMPLATE: 2
    };

    // CADESCOM_HASH_ALGORITHM enumeration
    CryptoPro.HashAlgorithm = {
        CADESCOM_HASH_ALGORITHM_CP_GOST_3411: 100,
        CADESCOM_HASH_ALGORITHM_MD2: 1,
        CADESCOM_HASH_ALGORITHM_MD4: 2,
        CADESCOM_HASH_ALGORITHM_MD5: 3,
        CADESCOM_HASH_ALGORITHM_SHA_256: 4,
        CADESCOM_HASH_ALGORITHM_SHA_384: 5,
        CADESCOM_HASH_ALGORITHM_SHA_512: 6,
        CADESCOM_HASH_ALGORITHM_SHA1: 0
    };

    CryptoPro.StoreNames = {
        CAPICOM_MY_STORE: "My"
    };

    CryptoPro.GostXmlDSigUrls = {
        XmlDsigGost3410Url : "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102001-gostr3411",
        XmlDsigGost3411Url : "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr3411"
    };

    function GetErrorMessage(e) {
        var err = e.message;
        if (!err) {
            err = e;
        } else if (e.number) {
            err += " (" + e.number + ")";
        }
        return err;
    }

    CryptoPro.isPluginInstalled = function () {
        try {
            var oAbout = this.CreateObject("CAdESCOM.About");
            // После получения объекта CAdESCOM.About можно дополнительно проверить версию
            // установленного КриптоПро ЭЦП Browser plug-in
            return true;
        }
        catch (err) { }
        return false;
    }

    CryptoPro.CreateObject = function (name) {
        switch (navigator.appName) {
            case "Microsoft Internet Explorer":
                return new ActiveXObject(name);
            default:
                var userAgent = navigator.userAgent;
                if (userAgent.match(/Trident\/./i)) { // IE11
                    return new ActiveXObject(name);
                }
                var cadesobject = document.getElementById("cadesplugin");
                return cadesobject.CreateObject(name);
        }
    }

    CryptoPro.SelectCertificate = function (storeLocation, storeName, storeOpenMode) {
        try {
            var oStore = this.CreateObject("CAPICOM.Store");
            oStore.Open(storeLocation, storeName, storeOpenMode);

            var oCertificates = oStore.Certificates;
            // Не рассматриваются сертификаты, в которых отсутствует закрытый ключ
            /*if (oCertificates.Count > 0)
                oCertificates = oCertificates.Find(CAPICOM_CERTIFICATE_FIND_EXTENDED_PROPERTY, CryptoPro.PropId.CAPICOM_PROPID_KEY_PROV_INFO);*/

            // Выбираются только сертификаты, действительные в настоящее время
            var today = new Date();
            var date = today.getFullYear() + "/" + (today.getMonth() + 1) + "/" + today.getDate();
            /*if (oCertificates.Count > 0)
                oCertificates = oCertificates.Find(CryptoPro.CertFindType.CAPICOM_CERTIFICATE_FIND_TIME_VALID, date);*/
                
            if (oCertificates.Count > 1) {
                oCertificates = oCertificates.Select("Выбор сертификата для подписи", "АИС ТаксоМотор", false);
            }
            var oCertificate = oCertificates.Item(1);
            oStore.Close();

            return oCertificate;

        } catch (e) {
            if (e.number != -2138568446) // отказ от выбора сертификата
                alert("Ошибка выбора сертификата: " + GetErrorMessage(e));
            return false;
        } 
    }

    CryptoPro.SignXMLCreate = function(oCertificate, dataToSign) {
        // Создаем объект CAdESCOM.CPSigner
        var oSigner = this.CreateObject("CAdESCOM.CPSigner");
        oSigner.Certificate = oCertificate;

        // Создаем объект CAdESCOM.SignedXML
        var oSignedXML = this.CreateObject("CAdESCOM.SignedXML");
        oSignedXML.Content = dataToSign;

        // Указываем тип подписи - в данном случае вложенная
        oSignedXML.SignatureType = this.SignatureType.CADESCOM_XML_SIGNATURE_TYPE_ENVELOPED;

        // Указываем алгоритм подписи
        oSignedXML.SignatureMethod = this.GostXmlDSigUrls.XmlDsigGost3410Url;

        // Указываем алгоритм хэширования
        oSignedXML.DigestMethod = this.GostXmlDSigUrls.XmlDsigGost3411Url;

        var sSignedMessage = "";
        try {
            sSignedMessage = oSignedXML.Sign(oSigner);
        } catch (err) {
            alert("Failed to create signature. Error: " + GetErrorMessage(err));
            return;
        }

        return sSignedMessage;
    }

    CryptoPro.XmlVerify = function(sSignedMessage) {
        // Создаем объект CAdESCOM.SignedXML
        var oSignedXML = this.CreateObject("CAdESCOM.SignedXML");

        try {
            oSignedXML.Verify(sSignedMessage);
        } catch (err) {
            alert("Failed to verify signature. Error: " + GetErrorMessage(err));
            return false;
        }

        return true;
    }

    CryptoPro.SignSimpleCreate = function(oCertificate, dataToSign) {
        var oSigner = this.CreateObject("CAdESCOM.CPSigner");
        oSigner.Certificate = oCertificate;

        var oSignedData = this.CreateObject("CAdESCOM.CadesSignedData");
        oSignedData.Content = dataToSign;
        oSigner.Options = 1; // simple signature

        try {
            var sSignedData = oSignedData.SignCades(oSigner, 1);
        } catch (e) {
            alert("Не удалось создать подпись из-за ошибки: " + GetErrorMessage(e));
            return;
        }

        return sSignedData;
    }

})(CryptoPro || (CryptoPro = {}));