var TM;

(function (TM) {

    TM.SP = (function (TMSP) {

        TMSP.showIconNotification = function (message, iconUrl, autoClose) {
            var messageHtml = '<div><span style="float: left;"><img src="' +
                iconUrl + '" /></span><span style="padding: 5px;">' + message + '</span></div>';
            return SP.UI.Notify.addNotification(messageHtml, !autoClose);
        };

        TMSP.setTargetBlank = function (toFindInLink) {
            // Get the collection of <a> tags
            var aAllLinks = document.getElementsByTagName('a');

            // For each <a> tags, 
            for (var i = 0; i < aAllLinks.length; i++) {
                var oA = aAllLinks[i];
                var sHref = oA.attributes["href"] ? oA.attributes["href"].value.toLowerCase() : null;

                // If href value contains paramter
                if (sHref && sHref.indexOf(toFindInLink.toLowerCase()) > 0)
                    oA.attributes["target"].value = "_blank";
            }
        };

        return TMSP;
    })(TM.SP || (TM.SP = {}));

})(TM || (TM = {}));