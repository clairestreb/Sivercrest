document.addEventListener("DOMContentLoaded", function (event) {
    history.replaceState(null, "", location.href.split('?')[0]);
    document.getElementById("VerifyTwoFactorToken").focus();
});