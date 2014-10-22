'use strict';

function ErrorMessage(message) {
    var self = this;

    self.MessageBody = ko.observable(message);
}

function Params() {
    var self = this;

    self.DateFrom     = ko.observable(new Date());
    self.DateTill     = ko.observable();
    self.ActionReason = ko.observable("");
}

function LicenseModel() {
    var self = this;
    // model properties
    self.Params = ko.observable(new Params());
    self.Errors = ko.observableArray([]);
    // model methods
    self.Validate = function() {
        self.Errors.removeAll();

        var dateFrom = self.Params().DateFrom();
        var dateTill = self.Params().DateTill();

        if (!dateFrom)
            self.Errors.push(new ErrorMessage('Необходимо указать дату начала действия приостановки'));
        if (dateFrom && dateTill && (dateFrom >= dateTill))
            self.Errors.push(new ErrorMessage('Дата завершения действия приостановки должна быть больше даты начала действия приостановки'));
        if (!CryptoPro.isPluginInstalled())
            self.Errors.push(new ErrorMessage('Необходимо <a href="http://www.cryptopro.ru/products/cades/plugin/get">установить плагин</a> для работы с ЭЦП'));

        var dlg = SP.UI.ModalDialog.get_childDialog();
        if (dlg) dlg.autoSize();
    };

    self.BuildGetXmlJson = function () {
        var itemId   = JSRequest.QueryString["ItemId"];
        var dateFrom = self.Params().DateFrom().getTime();
        var dateTill = self.Params().DateTill() ? self.Params().DateTill().getTime() : 0;
        var reason   = self.Params().ActionReason();

        return '{' +
                    'licenseId: ' + itemId + ', ' +
                    'dateFrom: "\\\/Date(' + dateFrom + ')\\\/", ' +
                    'dateTo: "\\\/Date(' + dateTill + ')\\\/", ' +
                    'reason: "' + reason + '"' +
               '}';
    };

    self.BuildSaveSignedJson = function (signatureValue) {
        var itemId   = JSRequest.QueryString["ItemId"];
        var dateFrom = self.Params().DateFrom().getTime();
        var dateTill = self.Params().DateTill() ? self.Params().DateTill().getTime() : 0;
        var reason   = self.Params().ActionReason();

        return '{' +
                    'licenseId: ' + itemId + ', ' +
                    'dateFrom: "\\\/Date(' + dateFrom + ')\\\/", ' +
                    'dateTo: "\\\/Date(' + dateTill + ')\\\/", ' +
                    'reason: "' + reason + '", ' +
                    'signature: "' + encodeURIComponent(signatureValue) + '"' +
               '}';
    };

    self.DoAction = function() {
        self.Validate();

        if (self.Errors().length > 0) return;

        $.ajax({
            type: 'POST',
            url: self.RequestUrl + '/SuspensionGetXml',
            data: self.BuildGetXmlJson(),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    var dataToSign = data.d;

                    var oCertificate = CryptoPro.SelectCertificate(
                            CryptoPro.StoreLocation.CAPICOM_LOCAL_MACHINE_STORE,
                            CryptoPro.StoreNames.CAPICOM_MY_STORE,
                            CryptoPro.StoreOpenMode.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                    if (oCertificate) {
                        dataToSign = 
                                    "<?xml version=\"1.0\"?>\n" +
                                    "<Envelope xmlns=\"urn:envelope\">\n" +
                                    dataToSign +
                                    " \n" +
                                    "</Envelope>";

                        var signedData = CryptoPro.SignXMLCreate(oCertificate, dataToSign);

                        $.ajax({
                            type: 'POST',
                            url: self.RequestUrl + '/SaveSignedSuspension',
                            data: self.BuildSaveSignedJson(signedData),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                window.frameElement.commonModalDialogClose(1, null);
                            }
                        });
                    }
                }
            }
        });
    };

    self.CancelAction = function() {
        window.frameElement.commonModalDialogClose(0, null);
    };

    // initialization
    JSRequest.EnsureSetup();
    self.RequestUrl = _spPageContextInfo.webAbsoluteUrl + '/' + _spPageContextInfo.layoutsUrl + '/TaxoMotor/LicenseService.aspx';
}

function sharepointReady() {
    ko.applyBindings(new LicenseModel());
}

ko.bindingHandlers.href = {
    update: function (element, valueAccessor) {
        ko.bindingHandlers.attr.update(element, function () {
            return { href: valueAccessor() }
        });
    }
};

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {},
            $el = $(element);

        $el.datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($el.datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $el.datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            $el = $(element);

        //handle date data coming via json from Microsoft
        if (String(value).indexOf('/Date(') == 0) {
            value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
        }

        var current = $el.datepicker("getDate");

        if (value - current !== 0) {
            $el.datepicker("setDate", value);
        }
    }
};

$(document).ready(function () {
    SP.SOD.executeOrDelayUntilScriptLoaded(sharepointReady, "SP.js");

    jQuery(function ($) {
        $.datepicker.regional['ru'] = {
            closeText: 'Закрыть',
            prevText: '&#x3c;Пред',
            nextText: 'След&#x3e;',
            currentText: 'Сегодня',
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
            'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
            'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
            dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
            dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
            dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            weekHeader: 'Не',
            dateFormat: 'dd.mm.yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['ru']);
    });
});
