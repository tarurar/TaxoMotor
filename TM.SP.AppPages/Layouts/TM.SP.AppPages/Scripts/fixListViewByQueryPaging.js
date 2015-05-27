(function () {

    function fromUnicode(source) {
        var rxUnicode = /\\u([\d\w]{4})/gi;

        var res = source.replace(rxUnicode, function (match, grp) {
            return String.fromCharCode(parseInt(grp, 16));
        });

        return res;
    }

    function removeURLParameter(url, parameter) {
        //prefer to use l.search if you have a location/link object
        var urlparts = url.split('?');
        if (urlparts.length >= 2) {

            var prefix = encodeURIComponent(parameter) + '=';
            var pars = urlparts[1].split(/[&;]/g);

            //reverse iteration as may be destructive
            for (var i = pars.length; i-- > 0;) {
                //idiom for string.startsWith
                if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                    pars.splice(i, 1);
                }
            }

            url = urlparts[0] + '?' + pars.join('&');
            return url;
        } else {
            return url;
        }
    }

    function makeFix() {

        var elToFix = $(".ms-bottompaging td.ms-paging").siblings("td:has(a)").find("a");
        if (elToFix && elToFix.length > 0) {

            var rx = /\"(http:.+)\"/g;

            $.each(elToFix, function (i, el) {
                var onclickText = el.getAttributeNode("onclick").value;
                
                var matches = rx.exec(onclickText);
                if (matches) {
                    var url = matches[1];
                    url = fromUnicode(url);
                    url = decodeURIComponent(url);
                    var newUrl = removeURLParameter(url, "View");

                    var newOnClickText = onclickText.replace(rx, '"' + newUrl + '"');
                    el.setAttribute("onclick", newOnClickText);
                }
            });
        }
    }

    if (!_spBodyOnLoadCalled) {
        _spBodyOnLoadFunctions.push(makeFix);
    } else {
        makeFix();
    }
})();