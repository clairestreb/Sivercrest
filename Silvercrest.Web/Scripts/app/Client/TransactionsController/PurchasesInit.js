

$(document).ready(function () {
    var url = "/Client/Transactions/GetPurchases?isGroup=" + getUrlParam("isGroup") + "&isClientGroup=" + getUrlParam("isClientGroup")
                    + "&contactId=" + getUrlParam("contactId") + "&entityId=" + getUrlParam("entityId");
    allCommands(url);

    // To put arrow below text when page Loads
    var sortedColumn = $("#purchasesTable .k-grid-header tr th[data-field='TradeDate']");
    $('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);

});

function allCommands(url) {

    $("#navigatino-item-active ul li:nth-child(3)").addClass('active');
    $("#bs-example-navbar-collapse-1 ul li:nth-child(3)").addClass('active');

    var numColumns = -1;

    var now = new Date();
    var actualDate = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
    actualDate.setFullYear(actualDate.getFullYear() - 5);
    $("#datepickerStart").datepicker().inputmask("mm/dd/yyyy");
    $("#datepickerStart").datepicker("option", "minDate", actualDate);
    $("#datepickerEnd").datepicker().inputmask("mm/dd/yyyy");
    $("#datepickerEnd").datepicker("option", "minDate", actualDate);


    $("#purchasesTable").kendoGrid({
        pdf: {
            allPages: true,
            avoidLinks: true,
        },
        excel: {
            fileName: "Purchases.xlsx",
            filterable: true
        },
                excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            var count = 0;
            var firstIndex;
            var secondIndex;
            var check = 0;
            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                if (row.type == "group-header") {
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];
                        if (cell.value !== undefined && cell.value.indexOf("Sorting") !== -1) {
                            check = i;
                        }
                    }
                }
                if (row.type == "group-footer") {
                    if (sheet.rows[i + 1] != undefined && sheet.rows[i + 1].type == "group-footer" && firstIndex != 0) {
                        sheet.rows.splice(i, 1);
                        sheet.rows.splice(i + 1, 1);
                        firstIndex = 0;
                    }
                    else if (firstIndex != 0) {
                        sheet.rows.splice(i, 1);
                        i--;
                        firstIndex = 0;
                    }
                    else
                        firstIndex = i;
                }
                if (check !== 0) {
                    sheet.rows.splice(check, 1);
                    check = 0;
                    i--;
                }
            }
            secondIndex = 0;
            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                if (row.type == "header")
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];
                         if (cell.value == "TRADE DATE")
                            for (var ro = i + 3; ro < sheet.rows.length; ro++) {
                                if (j < sheet.rows[ro].cells.length && sheet.rows[ro].cells[j].value)
                                sheet.rows[ro].cells[j].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
                            }
                    }
                if (row.type == "group-footer" || row.type == "footer" || row.type == "header" || row.type == "group-header") {
                    if (row.type == "group-header") {
                        count += 1;
                        if (count == 1)
                            firstIndex = i;
                        else
                            secondIndex = i;
                    }
                    if (row.type == "group-footer") {
                        count -= 1;
                        if (count == 1)
                            row.cells[4].value = row.cells[4].value + " " + sheet.rows[secondIndex].cells[3].value;
                        else
                            row.cells[4].value = row.cells[4].value + " " + sheet.rows[firstIndex].cells[1].value;
                    }
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            if (cell.value)
                                var res = cell.value.substring(0, 5);
                            if ((res === 'Count') || (res === 'Total') || (res !== '<div ')) {
                                cell.value = cell.value;
                            } else {
                                cell.value = $(cell.value).text();
                                cell.hAlign = "right";
                            }
                            if (cell.value.indexOf("<") !== -1 && cell.value.indexOf(">") !== -1)
                                cell.value = cell.value.replace(/<(?:.|\n)*?>/gm, '');
                        }
                    }
                }
            }
        },
        width:  '100%',
        dataSource: {
            data: model.TableData.Data,
            schema: {
                model: {
                    fields: {
                        Quantity: {
                            type: "number"
                        },
                        UsdAmount: {
                            type: "number"
                        },
                        SecurityName: {
                            type: "string"
                        },
                        Symbol: {
                            type: "string"
                        },
                        TradeDate: {
                            type: "date"
                        },
                    }
                }
            },
            group: [
                {
                    field: "SortingOne",
                    aggregates:
                    [{ field: "UsdAmount", aggregate: "sum" },]
                },
                {
                    field: "GroupingOne",
                    aggregates:
                    [{ field: "UsdAmount", aggregate: "sum" },]
                },
                {
                    field: "SortingSecond",
                    aggregates:
                    [{ field: "UsdAmount", aggregate: "sum" },]
                },
                {
                    field: "GroupingSecond",
                    aggregates:
                    [{ field: "UsdAmount", aggregate: "sum" },]
                }],
            aggregate: [
               { field: "UsdAmount", aggregate: "sum" }, ],
            sort: {
                field: "TradeDate",
                dir: "desc"
            },
        },
        sortable: {
            mode: "single",
            allowUnsort: false
        },
        scrollable: true,
        dataBound: function onDataBound(e) {
            $("#purchasesTable").find(".k-group-col,.k-group-cell").remove();
            var spanCells = $("#purchasesTable").find(".k-grouping-row").children("td");
            if (numColumns <= 0) {
                numColumns = spanCells.attr("colspan") - 4;
            }
            spanCells.attr("colspan", numColumns);
            sort();
            var rows = e.sender.tbody.children();
            for (var j = 0; j < rows.length; j++) {
                var row = $(rows[j]);
                row.addClass("row black tab");
            }
        },
        columns: [
            {
                field: "SecurityName",
                title: "SECURITY NAME",
                width: 250,
                headerAttributes: {
                    "class": "col2 first",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col2 first"
                },
                footerTemplate: "TOTAL PURCHASES",
                groupFooterTemplate:"TOTAL"
            },
            {
                field: "Symbol",
                title: "SYMBOL",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                     "class": "col2"
                },
                width: 100,
            },
            {
                field: "TradeDate",
                title: "TRADE DATE",
                format: "{0:MM/dd/yyyy}",
//                template: "#= kendo.toString(kendo.parseDate(TradeDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                     "class": "col2"
                },
                width: 100,
            },
            {
                field: "Quantity",
                title: "QUANTITY",
                template: "#= (Quantity == null) ? ' ' :  kendo.toString(Quantity, 'n0') #",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col2"
                },
                width: 100,
            },
            {
                field: "UsdAmount",
                title: "AMOUNT",
                template: "#= kendo.toString(UsdAmount, 'n0') #",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                     "class": "col2"
                },
                width: 100,
                footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                groupFooterTemplate: "<div class='amount' align='center'>#= kendo.toString(sum, 'n0') #</div>",
            },
            {
                hidden: true, field: "GroupingOne", groupHeaderTemplate: "<span class=\"groupOne #=value.replace(/ /g,'').replace(/&/g,'')#\">#=value#</span>"
            },
            {
                hidden: true, field: "GroupingSecond", groupHeaderTemplate: "<p class=\"groupSecond #=value.replace(/ /g,'').replace(/&/g,'')#\">#=value#</p>"
            },
        ],
    });
    var grid = $("#purchasesTable").data("kendoGrid");
    grid.bind("dataBound", grid_dataBound);

    $("#purchasesNoGroupTable").kendoGrid({
        excel: {
            fileName: "Purchases.xlsx",
            filterable: true
        },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            var count = 0;
            var firstIndex;
            var secondIndex;
            var check = 0;
            secondIndex = 0;
            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                if (row.type == "header")
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];
                        if (cell.value == "TRADE DATE")
                            for (var ro = i + 1; ro < sheet.rows.length; ro++) {
                                if (j < sheet.rows[ro].cells.length && sheet.rows[ro].cells[j].value)
                                    sheet.rows[ro].cells[j].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
                            }
                        if (cell.value == "GroupingOne")
                            cell.value = "ASSET TYPE";
                        if (cell.value == "GroupingSecond")
                            cell.value = "SECTOR";
                    }
                if (row.type == "group-footer" || row.type == "footer" || row.type == "header" || row.type == "group-header") {
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            if (cell.value)
                                var res = cell.value.substring(0, 5);
                            if ((res === 'Count') || (res === 'Total') || (res !== '<div ')) {
                                cell.value = cell.value;
                            } else {
                                cell.value = $(cell.value).text();
                                cell.hAlign = "right";
                            }
                            if (cell.value.indexOf("<") !== -1 && cell.value.indexOf(">") !== -1)
                                cell.value = cell.value.replace(/<(?:.|\n)*?>/gm, '');
                        }
                    }
                }
            }
        },
        width: '100%',
        dataSource: {
            data: model.TableData.Data,
            schema: {
                model: {
                    fields: {
                        Quantity: {
                            type: "number"
                        },
                        UsdAmount: {
                            type: "number"
                        },
                        SecurityName: {
                            type: "string"
                        },
                        Symbol: {
                            type: "string"
                        },
                        TradeDate: {
                            type: "date"
                        },
                    }
                }
            },
            aggregate: [
                { field: "UsdAmount", aggregate: "sum" },],
            sort: {
                field: "TradeDate",
                dir: "desc"
            },
        },
        scrollable: true,
        columns: [
            {
                field: "SecurityName",
                title: "SECURITY NAME",
                width: 250,
                headerAttributes: {
                    "class": "col1",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col1"
                },
                footerTemplate: "TOTAL PURCHASES",
                groupFooterTemplate: "TOTAL"
            },
            {
                field: "Symbol",
                title: "SYMBOL",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col2"
                },
                width: 100,
            },
            {
                field: "TradeDate",
                title: "TRADE DATE",
                format: "{0:MM/dd/yyyy}",
                //                template: "#= kendo.toString(kendo.parseDate(TradeDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                headerAttributes: {
                    "class": "col3",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col3"
                },
                width: 100,
            },
            {
                field: "Quantity",
                title: "QUANTITY",
                template: "#= (Quantity == null) ? ' ' :  kendo.toString(Quantity, 'n0') #",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col2"
                },
                width: 100,
            },
            {
                field: "UsdAmount",
                title: "AMOUNT",
                template: "#= kendo.toString(UsdAmount, 'n0') #",
                headerAttributes: {
                    "class": "col2",
                    style: "text-align: center; font-size: 12px"
                },
                attributes: {
                    "class": "col2"
                },
                width: 100,
                footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
            },
            {
                field: "GroupingOne"
            },
            {
                field: "GroupingSecond"
            },
        ],
    });
}
//sort();

function grid_dataBound(e) {
}


function sort() {
    $("th:has(a.k-link)").css("text-align", "center");
    $("th:has(a.k-link)").css("vertical-align", "top");
    $("a.k-link").css("color", "#163574");

    $("th:has(a.k-link)").bind("click", function (e) {

    });

    $("th:has(a.k-link)").click(onClickSort);
    $(".k-link").click(onClickSort);


    $(".groupOne").closest("tr").addClass(" groupingOne");
    $(".groupSecond").closest("tr").addClass(" groupingSecond");
    var k = $(".groupOne");
    $.each(k, function (key, elem) {
        var c = $(elem).attr("class");
        c = c.split(" ");
        c = c[1];
        var elems = $("." + c);
        var count = elems.length;
        for (var i = 1; i < count; i++) {
            $(elems[i]).closest("tr").remove();
        }
    })
    //var t = $(".groupSecond");
    //$.each(t, function (key, elem) {
    //    var c = $(elem).attr("class");
    //    c = c.split(" ");
    //    c = c[1];
    //    var elems = $("." + c);
    //    var count = elems.length;
    //    for (var i = 1; i < count; i++) {
    //        $(elems[i]).closest("tr").remove();
    //    }
    //})

    var h = $("#purchasesTable .k-grouping-row");
    $.each(h, function (key, elem) {
        var s = $(elem).attr("class");
        if (s.indexOf("groupingOne") == -1 && s.indexOf("groupingSecond") == -1) {
            $(elem).remove();
        }
    }) 
    var t = $("#purchasesTable .k-group-footer");
    $.each(t, function (key, elem) {
        if ($(elem).next().hasClass("groupingOne") || $(elem).is($("#purchasesTable tr:last")) || $(elem).next().length == 0) {
            if ($(elem).prev().hasClass("footerSecond") && !($(elem).next().hasClass("k-group-footer"))) {
                var prevLabel = $(elem).prevAll(".groupingOne:first").find("td p.k-reset").text();
                $(elem).find("td:contains('TOTAL')").text("Total " + prevLabel);
                $(elem).addClass(" footerFirst");
            }
        }
        else if (!($(elem).prev().hasClass("k-group-footer"))) {
            var prevLabel = $(elem).prevAll(".groupingSecond:first").find("td p.groupSecond").text();
            $(elem).find("td:contains('TOTAL')").text("Total " + prevLabel);
            $(elem).addClass(" footerSecond");
        }
        else {
            $(elem).remove();
        }

    });

    var f = $("#purchasesTable .footerFirst");
    $.each(f, function (key, elem) {
        var amount = 0;
        $(elem).prevUntil(".groupingOne", ".footerSecond").each(function (ke, el) {
            amount = amount + parseInt($(el).find(".amount")[0].textContent.replace(/,/g, ''));
        })
        $(elem).find(".amount")[0].textContent = (amount).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");;
    })

    $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
    $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
    $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");

    $("a.k-link").removeClass("k-link");
}

function onClickSort(e) {
    var target = $(this);
    // in case if it's redirect from children
    if (!$(target).is("th")) {
        target = $(target).parent();
    }

    $(target.closest('tr')).children('th').children('span').css("display", "none");
    var set = $(target).children();
    var dir = $(target).attr("data-dir");

    set.each(function (index, elem) {

        if (index > 0) {
            $(this).css("display", "none");
        }
        if (dir == "asc") {
            set.eq(2).css("display", "block");

        }
        if (dir == "desc") {
            set.eq(3).css("display", "block");

        }
        if (dir != "asc" && dir != "desc") {
            set.eq(1).css("display", "block");

        }

    });

}

function getPDF() {
    var grid = $("#purchasesTable").data("kendoGrid");
    grid.saveAsPDF();
}
function getExcel() {
    $("#purchasesNoGroupTable").getKendoGrid().saveAsExcel();
}

function isValidDate(date) {

    var isValid = false;
    if (date.match(/\d{1,2}[^\d]\d{1,2}[^\d]\d{4,4}/gi) == null) {
        isValid = false;
    }
    else {
        var t = date.split(/[^\d]/);
        var m0 = parseInt(t[0], 10) - 1;
        var dd = parseInt(t[1], 10);
        var yyyy = parseInt(t[2], 10);
        var d = new Date(yyyy, m0, dd);
        if (d.getDate() != dd || d.getMonth() != m0 || d.getFullYear() != yyyy) {

            isValid = false;
        }
        else {
            isValid = true;
        }
    }

    return isValid;
}


function insertDate() {
    var searchFrom = $('#datepickerStart').val();
    var searchTo = $('#datepickerEnd').val();

    if(isValidDate(searchFrom) && isValidDate(searchTo)) {
        var url = "/Client/Transactions/GetPurchases?searchFrom=" + searchFrom + '&searchTo=' + searchTo + " & isGroupQuery=" + getUrlParam("isGroupQuery") + "& isClientGroupQuery=" + getUrlParam("isClientGroupQuery")
            + "&contactIdQuery=" + getUrlParam("contactIdQuery") + "&entityIdQuery=" + getUrlParam("entityIdQuery");

        location.href = url;
    }
    else {
        showPopup("Invalid date.", true, "Error", true);
    }
    //var url = '/Client/Transactions/GetPurchases?searchFrom=' + searchFrom + '&searchTo=' + searchTo +"&isGroup=" + $("#isGroup").val() + "&isClientGroup=" + $("#isClientGroup").val()
    //                + "&contactId=" + $("#contactId").val() + "&entityId=" + $("#entityId").val();

    //allCommands(url);
    /*
    $.ajax({
            url: '/Client/Transactions/GetPurchases?searchFrom=' + searchFrom + '&searchTo=' + searchTo + "&isGroup=" + $("#isGroup").val() + "&isClientGroup=" + $("#isClientGroup").val()
                        + "&contactId=" + $("#contactId").val() + "&entityId=" + $("#entityId").val(),

            type: 'GET',
            dataType: "json",
            success: function (result) {
                var grid = $('#purchasesTable').getKendoGrid();
                grid.dataSource.data(result);
                grid.dataSource.schema(
                 {
                     model: {
                         fields: {
                             Quantity: {
                                 type: "number"
                             },
                             UsdAmount: {
                                 type: "number"
                             },
                             SecurityName: {
                                 type: "string"
                             },
                             Symbol: {
                                 type: "string"
                             },
                             TradeDate: {
                                 type: "date"
                             },
                         }
                     }
                 });
                grid.refresh();
            }
        });
*/
}


function showPopup(message, topPosition, messageType, pageReload) {
    $("#errorMessage").text(message);
    $("#activateMessage").text(messageType);

    if (topPosition == true)
        $("#popupActivateOrDeactivate").css("margin-top", "270px");
    if (topPosition == false)
        $("#popupActivateOrDeactivate").css("margin-top", "610px");

    if (pageReload == false) {
        $("#popupActivateOrDeactivate button").on('click', closeResultMsgPopup);
    }
    else {
        $("#popupActivateOrDeactivate button").on('click', refresh);
    }
    $("#popupActivateOrDeactivate").show();
    $("#popupActivateOrDeactivate").css("visibility", "visible");
    $(".overlay").show();
}

function refresh() {
    location.reload();
}

function closeResultMsgPopup() {
    $("#popupActivateOrDeactivate").hide();
    $("#popupActivateOrDeactivate").css("visibility", "hidden");
    $(".overlay").hide();
}