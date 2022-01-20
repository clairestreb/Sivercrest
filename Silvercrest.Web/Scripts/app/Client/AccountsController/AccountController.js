var accounts = [];
var groups = [];
var accountsInGroup = [];

var accountsPage = 0;
var groupsPage = 0;
var accountsInGroupPage = 0;
$(document).ready(function () {
    var isGroup = getUrlParam("isGroupQuery");
    var isClientGroup = getUrlParam("isClientGroupQuery");
    var entityId = getUrlParam("entityIdQuery");
    var contactId = getUrlParam("contactIdQuery");
    var pathname = window.location.pathname;


    var numOfItems = 6;
    var numOfCols = 4;
    var groupsPage = 0;
    accounts = model.AccountsData.Data;
    accounts.sort(compare);

    for (var g = 0; g < accounts.length && g < numOfCols * numOfItems; g += numOfItems) {

        $("<div>", { "class": "col-xs-3", id: "oneCol" + g / numOfItems }).appendTo("#collapseOne");
        for (var i = g + groupsPage * numOfItems; i < accounts.length && i < g + numOfItems; i++) {
            var a = accounts[i].ContactIdEnc;
            $('<a>', {
                text: accounts[i].Name,
//                href: pathname + "?isGroupQuery=" + Encrypt(accounts[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accounts[i].IsClientGroup) +
//                    "&contactIdQuery=" + a + "&entityIdQuery=" + Encrypt(accounts[i].EntityId)
                href: pathname + "?isGroupQuery=" + accounts[i].IsGroupEnc  + "&isClientGroupQuery=" + accounts[i].IsClientGroupEnc +
                    "&contactIdQuery=" + a + "&entityIdQuery=" + accounts[i].EntityIdEnc
        }).appendTo("#oneCol" + g / numOfItems);
        }
    }

    //arrows left
    $("#collapseOne .moreArrows").children().first().click(function () {

        if (accountsPage == 0) return;
        accountsPage -= 4;
        for (var g = 0; g < accounts.length && g < 24; g += numOfItems) {
            $("#oneCol" + g / numOfItems).empty();
        }

        for (var g = 0; g < accounts.length && g < 24; g += numOfItems) {

            for (var i = g + accountsPage * numOfItems; i < accounts.length && i < g + numOfItems + accountsPage * numOfItems; i++) {
                $('<a>', {
                    text: accounts[i].Name,
                    href: pathname + "?isGroupQuery=" + accounts[i].IsGroupEnc + "&isClientGroupQuery=" + accounts[i].IsClientGroupEnc +
                        "&contactIdQuery=" + accounts[i].ContactIdEnc + "&entityIdQuery=" + accounts[i].EntityIdEnc
                }).appendTo("#oneCol" + g / numOfItems);
            }
        }
    });

    //arrow right
    $("#collapseOne .moreArrows").children().last().click(function () {
        if (accountsPage * numOfItems + 24 >= accounts.length) return;
        accountsPage += 4;
        for (var g = 0; g < 24; g += numOfItems) {
            $("#oneCol" + g / numOfItems).empty();
        }

        for (var g = 0; g < accounts.length && g < 24; g += numOfItems) {

            for (var i = g + accountsPage * numOfItems; i < accounts.length && i < g + numOfItems + accountsPage * numOfItems; i++) {
                $('<a>', {
                    text: accounts[i].Name,
                    href: pathname + "?isGroupQuery=" + accounts[i].IsGroupEnc + "&isClientGroupQuery=" + accounts[i].IsClientGroupEnc +
                        "&contactIdQuery=" + accounts[i].ContactIdEnc + "&entityIdQuery=" + accounts[i].EntityIdEnc
                }).appendTo("#oneCol" + g / numOfItems);
            }
        }
    });

    //$.ajax({
    //    url: "/Client/Accounts/ViewAccounts?isGroupQuery=" + isGroup + "&isClientGroupQuery=" + isClientGroup
    //                + "&contactIdQuery=" + contactId + "&entityIdQuery=" + entityId,
    //    type: "get",


    //    success: function (response) {
    //        var numOfItems = 6;
    //        var numOfCols = 4;
    //        accounts = response;
    //        for (var g = 0; g < accounts.length && g < numOfCols * numOfItems; g += numOfItems) {


    //            $("<div>", { "class": "col-xs-3", id: "oneCol" + g / numOfItems}).appendTo("#collapseOne");

    //            for (var i = g + groupsPage * numOfItems; i < accounts.length && i < g + numOfItems; i++) {
    //                $('<a>', {
    //                    text:  accounts[i].Name,
    //                    href: pathname + "?isGroupQuery=" + Encrypt(accounts[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accounts[i].IsClientGroup) +
    //                        "&contactIdQuery=" + Encrypt(accounts[i].ContactId) + "&entityIdQuery=" + Encrypt(accounts[i].EntityId)
    //                }).appendTo("#oneCol" + g / numOfItems);
    //            }
    //        }

    //        //arrows left
    //        $("#collapseOne .moreArrows").children().first().click(function () {

    //            if (accountsPage == 0) return;
    //            accountsPage -= 4;
    //            for (var g = 0; g < accounts.length && g < 24; g += numOfItems) {
    //                $("#oneCol" + g / numOfItems).empty();
    //            }

    //            for (var g = 0; g < accounts.length && g < 24; g += numOfItems) {

    //                for (var i = g + accountsPage * numOfItems; i < accounts.length && i < g + numOfItems + accountsPage * numOfItems; i++) {
    //                    $('<a>', {
    //                        text: accounts[i].Name,
    //                        href: pathname + "?isGroupQuery=" + Encrypt(accounts[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accounts[i].IsClientGroup) +
    //                            "&contactIdQuery=" + Encrypt(accounts[i].ContactId) + "&entityIdQuery=" + Encrypt(accounts[i].EntityId)
    //                    }).appendTo("#oneCol" + g / numOfItems);
    //                }
    //            }
    //        });

    //        //arrow right
    //        $("#collapseOne .moreArrows").children().last().click(function () {
    //            if (accountsPage * numOfItems + 24 >= accounts.length) return;
    //            accountsPage += 4;
    //            for (var g = 0; g < 24; g += numOfItems) {
    //                $("#oneCol" + g / numOfItems).empty();
    //            }

    //            for (var g = 0; g < accounts.length && g < 24; g += numOfItems) {

    //                for (var i = g + accountsPage * numOfItems; i < accounts.length && i < g + numOfItems + accountsPage * numOfItems; i++) {
    //                    $('<a>', {
    //                        text:  accounts[i].Name,
    //                        href: pathname + "?isGroupQuery=" + Encrypt(accounts[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accounts[i].IsClientGroup) +
    //                            "&contactIdQuery=" + Encrypt(accounts[i].ContactId) + "&entityIdQuery=" + Encrypt(accounts[i].EntityId)
    //                    }).appendTo("#oneCol" + g / numOfItems);
    //                }
    //            }
    //        });
    //    }
    //});


    var groups = model.GroupsData.Data;
    groups.sort(compare);
    var groupsPage = 0;
    for (var g = 0; g < groups.length && g < 24; g += 6) {

        $("<div>", { "class": "col-xs-3", id: "twoCol" + g / 6 }).appendTo("#collapseTwo");

        for (var i = g + groupsPage * 6; i < groups.length && i < g + 6; i++) {
            $('<a>', {
                text: groups[i].Name,
                href: pathname + "?isGroupQuery=" + groups[i].IsGroupEnc + "&isClientGroupQuery=" + groups[i].IsClientGroupEnc +
                    "&contactIdQuery=" + groups[i].ContactIdEnc + "&entityIdQuery=" + groups[i].EntityIdEnc
            }).appendTo("#twoCol" + g / 6);
        }
    }

    //arrows left
    $("#collapseTwo .moreArrows").children().first().click(function () {

        if (groupsPage == 0) return;
        groupsPage -= 4;
        for (var g = 0; g < groups.length && g < 24; g += 6) {
            $("#twoCol" + g / 6).empty();
        }

        for (var g = 0; g < groups.length && g < 24; g += 6) {

            for (var i = g + groupsPage * 6; i < groups.length && i < g + 6 + groupsPage * 6; i++) {
                $('<a>', {
                    text: groups[i].Name,
                    href: pathname + "?isGroupQuery=" + groups[i].IsGroupEnc + "&isClientGroupQuery=" + groups[i].IsClientGroupEnc +
                        "&contactIdQuery=" + groups[i].ContactIdEnc + "&entityIdQuery=" + groups[i].EntityIdEnc
                }).appendTo("#twoCol" + g / 6);
            }
        }
    });

    //arrow right
    $("#collapseTwo .moreArrows").children().last().click(function () {
        if (groupsPage * 6 + 24 >= groups.length) return;
        groupsPage += 4;
        //empty
        for (var g = 0; g < 24; g += 6) {
            $("#twoCol" + g / 6).empty();
        }

        for (var g = 0; g < groups.length && g < 24; g += 6) {

            for (var i = g + groupsPage * 6; i < groups.length && i < g + 6 + groupsPage * 6; i++) {
                $('<a>', {
                    text: groups[i].Name,
                    href: pathname + "?isGroupQuery=" + groups[i].IsGroupEnc + "&isClientGroupQuery=" + groups[i].IsClientGroupEnc +
                        "&contactIdQuery=" + groups[i].ContactIdEnc + "&entityIdQuery=" + groups[i].EntityIdEnc
                }).appendTo("#twoCol" + g / 6);
            }
        }
    });

    //$.ajax({
    //    url: "/Client/Accounts/ViewGroups?isGroupQuery=" + isGroup + "&isClientGroupQuery=" + isClientGroup
    //                + "&contactIdQuery=" + contactId + "&entityIdQuery=" + entityId,
    //    type: "get",
    //    data: {},

    //    success: function (response) {
    //        var groups = response;
    //        var groupsPage = 0;
    //        for (var g = 0; g < groups.length && g < 24; g += 6) {

    //            $("<div>", { "class": "col-xs-3", id: "twoCol" + g / 6 }).appendTo("#collapseTwo");

    //            for (var i = g + groupsPage * 6; i < groups.length && i < g + 6; i++) {
    //                $('<a>', {
    //                    text: groups[i].Name,
    //                    href: pathname + "?isGroupQuery=" + Encrypt(groups[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(groups[i].IsClientGroup) +
    //                        "&contactIdQuery=" + Encrypt(groups[i].ContactId) + "&entityIdQuery=" + Encrypt(groups[i].EntityId)
    //                }).appendTo("#twoCol" + g / 6);
    //            }
    //        }

    //        //arrows left
    //        $("#collapseTwo .moreArrows").children().first().click(function () {

    //            if (groupsPage == 0) return;
    //            groupsPage-=4;
    //            for (var g = 0; g < groups.length && g < 24; g += 6) {
    //                $("#twoCol" + g / 6).empty();
    //            }

    //            for (var g = 0; g < groups.length && g < 24; g += 6) {

    //                for (var i = g + groupsPage * 6; i < groups.length && i < g + 6 + groupsPage * 6; i++) {
    //                    $('<a>', {
    //                        text: groups[i].Name,
    //                        href: pathname + "?isGroupQuery=" + Encrypt(groups[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(groups[i].IsClientGroup) +
    //                            "&contactIdQuery=" + Encrypt(groups[i].ContactId) + "&entityIdQuery=" + Encrypt(groups[i].EntityId)
    //                    }).appendTo("#twoCol" + g / 6);
    //                }
    //            }
    //        });

    //        //arrow right
    //        $("#collapseTwo .moreArrows").children().last().click(function () {
    //            if (groupsPage * 6 + 24 >= groups.length) return;
    //            groupsPage+=4;
    //            //empty
    //            for (var g = 0; g < 24; g += 6) {
    //                $("#twoCol" + g / 6).empty();
    //            }

    //            for (var g = 0; g < groups.length && g < 24; g += 6) {

    //                for (var i = g + groupsPage * 6; i < groups.length && i < g + 6 + groupsPage * 6; i++) {
    //                    $('<a>', {
    //                        text:groups[i].Name,
    //                        href: pathname + "?isGroupQuery=" + Encrypt(groups[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(groups[i].IsClientGroup) +
    //                            "&contactIdQuery=" + Encrypt(groups[i].ContactId) + "&entityIdQuery=" + Encrypt(groups[i].EntityId)
    //                    }).appendTo("#twoCol" + g / 6);
    //                }
    //            }
    //        });
    //    }
    //});


    accountsInGroup = model.AccountsWithinGroupsData.Data;
    accountsInGroup.sort(compare);
    for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {

        $("<div>", { "class": "col-xs-3", id: "threeCol" + g / 6 }).appendTo("#collapseThree");

        for (var i = g + accountsInGroupPage * 6; i < accountsInGroup.length && i < g + 6; i++) {
            $('<a>', {
                text: accountsInGroup[i].Name,
                href: pathname + "?isGroupQuery=" + accountsInGroup[i].IsGroupEnc + "&isClientGroupQuery=" + accountsInGroup[i].IsClientGroupEnc +
                   "&contactIdQuery=" + accountsInGroup[i].ContactIdEnc + "&entityIdQuery=" + accountsInGroup[i].EntityIdEnc
            }).appendTo("#threeCol" + g / 6);
        }
    }
    //arrows left
    $("#collapseThree .moreArrows").children().first().click(function () {

        if (accountsInGroupPage == 0) return;
        accountsInGroupPage -= 4;
        for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {
            $("#threeCol" + g / 6).empty();
        }

        for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {

            for (var i = g + accountsInGroupPage * 6; i < accountsInGroup.length && i < g + 6 + accountsInGroupPage * 6; i++) {
                $('<a>', {
                    text: accountsInGroup[i].Name,
                    href: pathname + "?isGroupQuery=" + accountsInGroup[i].IsGroupEnc + "&isClientGroupQuery=" + accountsInGroup[i].IsClientGroupEnc +
                        "&contactIdQuery=" + accountsInGroup[i].ContactIdEnc + "&entityIdQuery=" + accountsInGroup[i].EntityIdEnc
                }).appendTo("#threeCol" + g / 6);
            }
        }
    });

    //arrow right
    $("#collapseThree .moreArrows").children().last().click(function () {
        if (accountsInGroupPage * 6 + 24 >= accountsInGroup.length) return;
        accountsInGroupPage += 4;
        for (var g = 0; g < 24; g += 6) {
            $("#threeCol" + g / 6).empty();
        }

        for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {

            for (var i = g + accountsInGroupPage * 6; i < accountsInGroup.length && i < g + 6 + accountsInGroupPage * 6; i++) {
                $('<a>', {
                    text: accountsInGroup[i].Name,
                    href: pathname + "?isGroupQuery=" + accountsInGroup[i].IsGroupEnc + "&isClientGroupQuery=" + accountsInGroup[i].IsClientGroupEnc +
                        "&contactIdQuery=" + accountsInGroup[i].ContactIdEnc + "&entityIdQuery=" + accountsInGroup[i].EntityIdEnc
                }).appendTo("#threeCol" + g / 6);
            }
        }
    });
});

//    $.ajax({
//        url: "/Client/Accounts/ViewAccountsInGroups?isGroupQuery=" + isGroup + "&isClientGroupQuery=" + isClientGroup
//                    + "&contactIdQuery=" + contactId + "&entityIdQuery=" + entityId,
//        type: "get",
//        data: {},
//        success: function (response) {
//            accountsInGroup = response;
//            for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {

//                $("<div>", { "class": "col-xs-3", id: "threeCol" + g / 6 }).appendTo("#collapseThree");

//                for (var i = g + accountsInGroupPage * 6; i < accountsInGroup.length && i < g + 6; i++) {
//                    $('<a>', {
//                        text: accountsInGroup[i].Name,
//                        href: pathname + "?isGroupQuery=" + Encrypt(accountsInGroup[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accountsInGroup[i].IsClientGroup) +
//                           "&contactIdQuery=" + Encrypt(accountsInGroup[i].ContactId) + "&entityIdQuery=" + Encrypt(accountsInGroup[i].EntityId)
//                    }).appendTo("#threeCol" + g / 6);
//                }
//            }
//            //arrows left
//            $("#collapseThree .moreArrows").children().first().click(function () {

//                if (accountsInGroupPage == 0) return;
//                accountsInGroupPage -= 4;
//                for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {
//                    $("#threeCol" + g / 6).empty();
//                }

//                for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {

//                    for (var i = g + accountsInGroupPage * 6; i < accountsInGroup.length && i < g + 6 + accountsInGroupPage * 6; i++) {
//                        $('<a>', {
//                            text:  accountsInGroup[i].Name,
//                            href: pathname + "?isGroupQuery=" + Encrypt(accountsInGroup[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accountsInGroup[i].IsClientGroup) +
//                                "&contactIdQuery=" + Encrypt(accountsInGroup[i].ContactId) + "&entityIdQuery=" + Encrypt(accountsInGroup[i].EntityId)
//                        }).appendTo("#threeCol" + g / 6);
//                    }
//                }
//            });

//            //arrow right
//            $("#collapseThree .moreArrows").children().last().click(function () {
//                if (accountsInGroupPage * 6 + 24 >= accountsInGroup.length) return;
//                accountsInGroupPage += 4;
//                for (var g = 0; g < 24; g += 6) {
//                    $("#threeCol" + g / 6).empty();
//                }

//                for (var g = 0; g < accountsInGroup.length && g < 24; g += 6) {

//                    for (var i = g + accountsInGroupPage * 6; i < accountsInGroup.length && i < g + 6 + accountsInGroupPage * 6; i++) {
//                        $('<a>', {
//                            text: accountsInGroup[i].Name,
//                            href: pathname + "?isGroupQuery=" + Encrypt(accountsInGroup[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(accountsInGroup[i].IsClientGroup) +
//                                "&contactIdQuery=" + Encrypt(accountsInGroup[i].ContactId) + "&entityIdQuery=" + Encrypt(accountsInGroup[i].EntityId)
//                        }).appendTo("#threeCol" + g / 6);
//                    }
//                }
//            });
//        }
//    });
//});

function initList(array, divId, colId, wrap) {
    var numOfCols = 4;
    var numOfItems = 6;
    array.sort(compare);
    for (var g = 0; g < array.length && g < 24; g += numOfItems) {

        $("<div>", { "class": "col-xs-3", id: colId + g / numOfItems }).appendTo("#" + divId);
        for (var i = g + wrap.page * numOfItems; i < array.length && i < g + numOfItems; i++) {
            $('<a>', {
                text: array[i].Name,
                href: "/Client/Holdings/ViewAccount?isGroupQuery=" + Encrypt(array[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(array[i].IsClientGroup) +
                    "&contactIdQuery=" + Encrypt(array[i].ContactId) + "&entityIdQuery=" + Encrypt(array[i].EntityId)
            }).appendTo("#" + colId + g / numOfItems);
        }
    }
}
function nextListFucInit(array, divId, colId, wrap) {
    var numOfCols = 4;
    var numOfItems = 6;
    array.sort(compare);
    $("#" + divId + " .moreArrows").children().last().click(function () {

        if (colId.includes("one"))
            obj = { page: accountsPage };
        if (colId.includes("two"))
            obj = { page: groupsPage };
        if (colId.includes("three"))
            obj = { page: accountsInGroupPage };
        page = obj.page;
        if (page * numOfItems + 24 >= array.length) return;
        obj.page += 4;
        for (var g = 0; g < 24; g += numOfItems) {
            $("#" + colId + g / numOfItems).empty();
        }

        for (var g = 0; g < array.length && g < 24; g += numOfItems) {

            for (var i = g + page * numOfItems; i < array.length && i < g + numOfItems + page * numOfItems; i++) {
                $('<a>', {
                    text: array[i].Name,
                    href: "/Client/Holdings/ViewAccount?isGroupQuery=" + Encrypt(array[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(array[i].IsClientGroup) +
                        "&contactIdQuery=" + Encrypt(array[i].ContactId) + "&entityIdQuery=" + Encrypt(array[i].EntityId)
                }).appendTo("#" + colId + g / numOfItems);
            }
        }
        return false;
    });
}
function prevListFucInit(array, divId, colId, wrap) {
    var numOfCols = 4;
    var numOfItems = 6;
    array.sort(compare);
    $("#" + divId + " .moreArrows").children().first().click(function () {
        if (colId.includes("one"))
            obj = { page: accountsPage };
        if (colId.includes("two"))
            obj = { page: groupsPage };
        if (colId.includes("three"))
            obj = { page: accountsInGroupPage };
        page = obj.page;
        if (page == 0) return;
        obj.page -= 4;
        for (var g = 0; g < array.length && g < 24; g += numOfItems) {
            $("#" + colId + g / numOfItems).empty();
        }

        for (var g = 0; g < array.length && g < 24; g += numOfItems) {

            for (var i = g + page * numOfItems; i < array.length && i < g + numOfItems + page * numOfItems; i++) {
                $('<a>', {
                    text: i + ")" + array[i].Name,
                    href: "/Client/Holdings/ViewAccount?isGroupQuery=" + Encrypt(array[i].IsGroup) + "&isClientGroupQuery=" + Encrypt(array[i].IsClientGroup) +
                        "&contactIdQuery=" + Encrypt(array[i].ContactId) + "&entityIdQuery=" + Encrypt(array[i].EntityId)
                }).appendTo("#" + colId + g / numOfItems);
            }
        }

        return false;
    });
}

function compare(a, b) {
    if (a.Name > b.Name)
        return 1;
    if (a.Name < b.Name)
        return -1;
    if (a.Name === b.Name) {
        return 0;
    }
}
