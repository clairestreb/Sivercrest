/// <reference path="../Client/TransactionsController/PurchasesInit.js" />


(function startUpdatingTime() {
    var origin = window.location.origin;
    var url = origin + "/analytics/updateTime";
    setInterval(function () { updateTime(url) }, 60 * 1000);
})();

