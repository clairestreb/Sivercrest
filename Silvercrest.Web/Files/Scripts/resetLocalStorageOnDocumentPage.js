//window.localStorage.removeItem("selected-filter-entity");
//window.localStorage.removeItem("selected-filter-docType");
//function deleteAllCookies() {
//    var cookies = document.cookie.split(";");
//    for (var i = 0; i < cookies.length; i++)
//        deleteCookie(cookies[i].split("=")[0]);
//}

function setCookie(name, value, expireDays) {
    var date = new Date();
    date.setTime(date.getTime() + (expireDays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + date.toUTCString();
    document.cookie = name + "=" + value + "; " + expires;
}

function deleteFromCookie() {
    var cookies = get_cookies_array();
    for (var name in cookies) {
        if (name.indexOf("searchFrom") >= 0)
            setCookie(name, "", -1);
    } 
    return 0;
}

function deleteToCookie() {
    var cookies = get_cookies_array();
    for (var name in cookies) {
        if (name.indexOf("searchTo") >= 0)
            setCookie(name, "", -1);
    }
    return 0;
}

function get_cookies_array() {

    var cookies = {};

    if (document.cookie && document.cookie != '') {
        var split = document.cookie.split(';');
        for (var i = 0; i < split.length; i++) {
            var name_value = split[i].split("=");
            name_value[0] = name_value[0].replace(/^ /, '');
            cookies[decodeURIComponent(name_value[0])] = decodeURIComponent(name_value[1]);
        }
    }
    return cookies;
}



window.sessionStorage.removeItem("selected-filter-entity");
window.sessionStorage.removeItem("selected-filter-docType");
deleteFromCookie();
deleteToCookie();
window.sessionStorage.removeItem("kendo-grid-options");
window.localStorage.removeItem("alreadyChecked");



