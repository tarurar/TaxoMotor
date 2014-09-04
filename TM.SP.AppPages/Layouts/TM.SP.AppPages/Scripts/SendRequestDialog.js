var TM;

(function (TM) {

    TM.SP = (function (TMSP) {

        TMSP.getProcessingMarkup = function (message) {
            return "<span style='float:left;'><img src='/_layouts/15/images/loadingcirclests16.gif'></span><span style='width:100%; padding: 2px;'>" + message + "</span></p>";
        };

        return TMSP;
    })(TM.SP || (TM.SP = {}));

})(TM || (TM = {}));