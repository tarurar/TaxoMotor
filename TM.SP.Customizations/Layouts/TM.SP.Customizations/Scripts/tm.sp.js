var TM;

(function (TM) {

    TM.SP = (function (TMSP) {

        TMSP.showIconNotification = function (message, iconUrl, autoClose) {
            var messageHtml = '<div><span style="float: left;"><img src="' +
                iconUrl + '" /></span><span style="padding: 5px;">' + message + '</span></div>';
            return SP.UI.Notify.addNotification(messageHtml, !autoClose);
        };

        return TMSP;
    })(TM.SP || (TM.SP = {}));

})(TM || (TM = {}));