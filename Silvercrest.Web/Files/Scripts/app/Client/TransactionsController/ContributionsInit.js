

$(document).ready(function () {
    var url = "/Client/Transactions/GetContributions?isGroup=" + getUrlParam("isGroup") + "&isClientGroup=" + getUrlParam("isClientGroup")
                    + "&contactId=" + getUrlParam("contactId") + "&entityId=" + getUrlParam("entityId");
    allCommands(url);

    // To put arrow below text when page Loads
    var sortedColumn = $("#ContributionsGrid .k-grid-header tr th[data-field='TradeDate']");
    $('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);

});

function allCommands(url){
    kendo.pdf.defineFont({
        "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
        "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
        "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
        "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
    });

    $("#navigatino-item-active ul li:nth-child(3)").addClass('active');
    $("#bs-example-navbar-collapse-1 ul li:nth-child(3)").addClass('active');

    var now = new Date();
    var actualDate = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
    actualDate.setFullYear(actualDate.getFullYear() - 5);
    $("#datepickerStart").datepicker().inputmask("mm/dd/yyyy");
    $("#datepickerStart").datepicker("option", "minDate", actualDate);
    $("#datepickerEnd").datepicker().inputmask("mm/dd/yyyy");
    $("#datepickerEnd").datepicker("option", "minDate", actualDate);

    $("#ContributionsGrid").kendoGrid({
        excel: {
            fileName: "Contributions-Withdrawals.xlsx",
            filterable: true
        },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            var count = 0;
            var firstIndex;
            var secondIndex;

            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                if (row.type === "header")
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];
                        if (cell.value === "TRADE DATE")
                            for (var ro = i + 1; ro < sheet.rows.length; ro++) {
                                if(j < sheet.rows[ro].cells.length && sheet.rows[ro].cells[j].value)
                                sheet.rows[ro].cells[j].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
                            }
                    }
                if (row.type === "group-footer" || row.type === "footer" || row.type === "header" || row.type === "group-header") {
                if (row.type === "group-header") {
                    count += 1;
                    if (count === 1)
                        firstIndex = i;
                    else
                        secondIndex = i;
                }
                if (row.type === "group-footer") {
                    count -= 1;
                    if (count === 1)
                        row.cells[1].value = row.cells[1].value + " " + sheet.rows[secondIndex].cells[0].value;
                    else
                        row.cells[1].value = row.cells[1].value + " " + sheet.rows[firstIndex].cells[0].value;
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
        dataSource: {
            data: model.TableData.Data,
            schema: {
                model: {
                    fields: {
                        UsdAmount: {
                            type: "number"
                        },
                        SecurityName: {
                            type: "string"
                        },
                        TradeDate: {
                             type: "date"
                        },
                    }
                }
            },
            group: {
                field: "TransactionType", aggregates: [
                    {
                        field: "UsdAmount", aggregate: "sum"
                    }]
            },
            aggregate: [
                   { field: "UsdAmount", aggregate: "sum" },
            ],
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
        dataBound: function onDataBound(arg) {
            $(".k-group-col,.k-group-cell").remove();
            var spanCells = $(".k-grouping-row").children("td");
            spanCells.attr("colspan", spanCells.attr("colspan") - 1);
            sort();
        },
        columns: [
            {
                field: "SecurityName",
                title: "SECURITY NAME",
                attributes: {
                    "class": "col1"
                },
                width: 400,
                groupFooterTemplate: "TOTAL ",
            },
            {
                field: "TradeDate",
                title: "TRADE DATE",
                format: "{0:MM/dd/yyyy}",
//                template: "#= kendo.toString(kendo.parseDate(TradeDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                attributes: {
                    "class": "col2"
                },
                width: 120,
            },
             {
                 field: "UsdAmount",
                 title: "AMOUNT",
                 template: "#= kendo.toString(UsdAmount, 'n0') #",
                 attributes: {
                     "class": "col3"
                 },
                 width: 160,
                 groupFooterTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>"
             },
            {
                field: "Comment",
                title: "COMMENTS",
                template: "# if (Comment === null || Comment === '') {##} else {# <a href=\"\\#\" onclick=\"commentBound(event)\" class=\"red underline\" data-toggle=\"modal\" data-target=\"\\#myModal\">View Comment</a> #} #",
                attributes: {
                    "class": "col4"
                },
                width: 120,
            },
            {
                hidden: true, field: "TransactionType", groupHeaderTemplate: "#=value#"
            }

        ]
    });

    var grid = $("#ContributionsGrid").data("kendoGrid");
    grid.bind("dataBound", grid_dataBound);

    function grid_dataBound(e) {
        var rows = e.sender.tbody.children();
        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            row.addClass("row black tab");
        }
    }

    function sort() {

        $("th:has(a.k-link)").css("text-align", "center");
        $("th:has(a.k-link)").css("vertical-align", "top");
        $("a.k-link").css("color", "#163574");
        $("tr.k-footer-template").remove();

        $("th:has(a.k-link)").click(onClickSort);
        $(".k-link").click(onClickSort);

        var t = $("#ContributionsGrid .k-group-footer");
        $.each(t, function (key, elem) {
            var prevLabel = $(elem).prevAll(".k-grouping-row:first").find("td p.k-reset").text();
//            alert(prevLabel);
            $(elem).find("td:contains('TOTAL')").text("Total " + prevLabel);
        });


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
            if (dir === "asc") {
                set.eq(2).css("display", "block");

            }
            if (dir === "desc") {
                set.eq(3).css("display", "block");

            }
            if (dir !== "asc" && dir !== "desc") {
                set.eq(1).css("display", "block");

            }

        });

    }

}


function getPDF() {
    $("td.col4").children().toggleClass("underline")
    var grid = $("#ContributionsGrid").data("kendoGrid");
    grid.saveAsPDF()
    .done(function (data) {
        $("td.col4").children().toggleClass("underline")
    })
}
function getExcel() {
    $("#ContributionsGrid").getKendoGrid().saveAsExcel();
}
function commentBound(e) {
    var grid = $("#ContributionsGrid").data("kendoGrid");
    var parameters = grid.dataItem($(e.currentTarget).closest("tr"));
    $("#modalHeader").text("TRANSACTION COMMENT");
    $("#modalBody").text(parameters.Comment);
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

    if (isValidDate(searchFrom) && isValidDate(searchTo)) {
        var url = '/Client/Transactions/GetContributions?searchFrom=' + searchFrom + '&searchTo=' + searchTo + "&isGroupQuery=" + getUrlParam("isGroupQuery") + "&isClientGroupQuery=" + getUrlParam("isClientGroupQuery")
            + "&contactIdQuery=" + getUrlParam("contactIdQuery") + "&entityIdQuery=" + getUrlParam("entityIdQuery");

        location.href = url;
    }
    else {
        showPopup("Invalid date.", true, "Error", true);
    }
    //allCommands(url);
    /*
    $.ajax({
            url: '/Client/Transactions/GetContributions?searchFrom=' + searchFrom + '&searchTo=' + searchTo + "&isGroup=" + getUrlParam("isGroup") + "&isClientGroup=" + getUrlParam("isClientGroup")
            + "&contactId=" + getUrlParam("contactId") + "&entityId=" + getUrlParam("entityId"),
            type: 'GET',
            dataType: "json",
            success: function (result) {
                var grid = $('#ContributionsGrid').getKendoGrid();
                grid.dataSource.data(result);
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