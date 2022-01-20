$(document).ready(function () {
    var url = "/Client/Transactions/GetSales?isGroup=" + getUrlParam("isGroup") + "&isClientGroup=" + getUrlParam("isClientGroup")
                    + "&contactId=" + getUrlParam("contactId") + "&entityId=" + getUrlParam("entityId");
    allCommands(url);

    // To put arrow below text when page Loads
    var sortedColumn = $("#salesTable .k-grid-header tr th[data-field='CloseDate']");
    $('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);

});

var numColumns = -1;

function allCommands(url) {

    $("#navigatino-item-active ul li:nth-child(3)").addClass('active');
    $("#bs-example-navbar-collapse-1 ul li:nth-child(3)").addClass('active');

    var now = new Date();
    var actualDate = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
    actualDate.setFullYear(actualDate.getFullYear() - 5);
    $("#datepickerStart").datepicker().inputmask("mm/dd/yyyy");
    $("#datepickerStart").datepicker("option", "minDate", actualDate);
    $("#datepickerEnd").datepicker().inputmask("mm/dd/yyyy");
    $("#datepickerEnd").datepicker("option", "minDate", actualDate);


    $("#salesTable").kendoGrid({
        pdf: {
            allPages: true,
        },
        excel: {
            fileName: "Sales.xlsx",
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
                        if (cell.value == "DATE")
                            for (var ro = i + 4; ro < sheet.rows.length; ro++) {
                                if (j < sheet.rows[ro].cells.length && sheet.rows[ro].cells[j].value)
                                sheet.rows[ro].cells[j+ 2].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j + 2].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
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
        dataSource: {
            //transport: {
            //    read: {
            //        type: "GET",
            //        url: url,
            //        dataType: "json",
            //        data: {
            //            startDate: $("#datepickerStart").val(),
            //            endDate: $("#datepickerEnd").val()
            //        },
            //        complete: function (data, status) {

            //            if (status === "success") {

            //                sort();
            //            }
            //        }

            //    },
            //    serverSorting: false,
            //    pageSize: 20,
            //    serverFiltering: false,
            //    serverPaging: false
            //},
            data: model.TableData.Data,
            schema: {
                model: {
                    fields: {
                        OpenDate: {
                            type: "date"
                        },
                        CloseDate: {
                            type: "date"
                        },
                    }
                }
            },
            group: [
                {
                    field: "SortingOne", aggregates: [
                        { field: "OriginalTotal", aggregate: "sum" },
                        { field: "AdjustedTotal", aggregate: "sum" },
                        { field: "ProceedsTotal", aggregate: "sum" },
                        { field: "ShortTerm", aggregate: "sum" },
                        { field: "LongTerm", aggregate: "sum" },
                    ], dir: "asc"
                },
                {
                    field: "GroupingOne", aggregates: [
                        { field: "OriginalTotal", aggregate: "sum" },
                        { field: "AdjustedTotal", aggregate: "sum" },
                        { field: "ProceedsTotal", aggregate: "sum" },
                        { field: "ShortTerm", aggregate: "sum" },
                        { field: "LongTerm", aggregate: "sum" },
                    ],
                },
                {
                    field: "SortingSecond", aggregates: [
                        { field: "OriginalTotal", aggregate: "sum" },
                        { field: "AdjustedTotal", aggregate: "sum" },
                        { field: "ProceedsTotal", aggregate: "sum" },
                        { field: "ShortTerm", aggregate: "sum" },
                        { field: "LongTerm", aggregate: "sum" },
                    ],
                },
                {
                    field: "GroupingSecond", aggregates: [
                        { field: "OriginalTotal", aggregate: "sum" },
                        { field: "AdjustedTotal", aggregate: "sum" },
                        { field: "ProceedsTotal", aggregate: "sum" },
                        { field: "ShortTerm", aggregate: "sum" },
                        { field: "LongTerm", aggregate: "sum" },
                    ],
                },
            ],
            aggregate: [
                   { field: "OriginalTotal", aggregate: "sum" },
                   { field: "AdjustedTotal", aggregate: "sum" },
                   { field: "ProceedsTotal", aggregate: "sum" },
                   { field: "ShortTerm", aggregate: "sum" },
                   { field: "LongTerm", aggregate: "sum" },
            ],
            sort: {
                field: "CloseDate",
                dir: "desc"
            },
        },
        sortable: {
            mode: "single",
            allowUnsort: false
        },
        scrollable: true,
        dataBound: grid_dataBound,
        columns: [

             {
                 field: "Security",
                 title: "SECURITY",
                 footerTemplate: "TOTAL REALIZED GAIN/LOSS",
                 groupFooterTemplate: "TOTAL ",
                 attributes: {
                     "class": "col2 first"
                 },
                 width: 280,
             },

            {
                field: "Quantity",
                title: "QUANTITY",
                template: "#= (Quantity == null) ? ' ' : kendo.toString(Quantity, 'n0') #",
                attributes: {
                    "class": "col2"
                },
                width: 60
            },


       {
           title: "ORIGINAL COST",
           columns: [
            {
                field: "OpenDate", title: "DATE",
                format: "{0:MM/dd/yyyy}",
//                template: "#= kendo.toString(kendo.parseDate(OpenDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                attributes: {
                    "class": "col2"
                },
                headerAttributes: {
                    style: "border-left-width: 2px !important;"
                },
                width: 90
            },
           {
               field: "OriginalTotal", title: "TOTAL",
               template: "#= kendo.toString(OriginalTotal, 'n0') #",
               groupFooterTemplate: "<div class='originalTotal' align='center'>#= kendo.toString(sum, 'n0') #</div>",
               footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
               attributes: {
                   "class": "col2"
               }, width: 90

           }]
           , headerAttributes: { style: "text-align: center; cursor:default;" }
       },
       {
           title: "ADJUSTED COST",
           columns: [
            {
                field: "AdjustedUnit", title: "UNIT",
                template: "#= (Quantity == null) ? ' ' : kendo.toString(AdjustedUnit, 'n2') #",
                attributes: {
                    "class": "col2"
                },
                width: 60
            },
           {
               field: "AdjustedTotal", title: "TOTAL",
               template: "#= kendo.toString(AdjustedTotal, 'n0') #",
               groupFooterTemplate: "<div class='adjustedTotal' align='center'>#= kendo.toString(sum, 'n0') #</div>",
               footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
               attributes: {
                   "class": "col2"
               },
               width: 90
           }]
           , headerAttributes: { style: "text-align: center; cursor:default;" }
       },
       {
           title: "PROCEEDS",
           columns: [
            {
                field: "CloseDate", title: "DATE",
                format: "{0:MM/dd/yyyy}",
//                template: "#= kendo.toString(kendo.parseDate(CloseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                attributes: {
                    "class": "col2"
                },
                width: 90
            },
            {
                field: "ProceedsUnit", title: "UNIT",
                template: "#= (Quantity == null) ? ' ' : kendo.toString(ProceedsUnit, 'n2') #",
                attributes: {
                    "class": "col2"
                },

                width: 60
            },
            {
                field: "ProceedsTotal", title: "TOTAL",
                template: "#= kendo.toString(ProceedsTotal, 'n0') #",
                groupFooterTemplate: "<div class='proceedsTotal' align='center'>#= kendo.toString(sum, 'n0') #</div>",
                footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                attributes: {
                    "class": "col2"
                },
                width: 90,
            }]
           , headerAttributes: { style: "text-align: center; cursor:default;" }
       },
        {
            title: "GAIN/LOSS",
            attributes: {
                "class": "col3"
            },
            columns: [
             {
                 field: "ShortTerm", title: "SHORT TERM",
                 groupFooterTemplate: "<div class='shortTerm' align='center'>#= kendo.toString(sum, 'n0') #</div>",
                 footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                 template: "#= kendo.toString(ShortTerm, 'n0') #",
                 attributes: {
                     "class": "col2"
                 },
                 width: 80
             },
             {
                 field: "LongTerm", title: "LONG TERM",
                 groupFooterTemplate: "<div class='longTerm' align='center'>#= kendo.toString(sum, 'n0') #</div>",
                 footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                 template: "#= kendo.toString(LongTerm, 'n0') #",
                 attributes: {
                     "class": "col2"
                 },
                 width: 80
             }]
            , headerAttributes: { style: "text-align: center; cursor:default;" }
        },
        {
            hidden: true, field: "GroupingOne", groupHeaderTemplate: "<span class=\"groupOne #=value.replace(/ /g,'').replace(/&/g,'')#\">#=value#</span>"
        },
        {
            hidden: true, field: "GroupingSecond", groupHeaderTemplate: "<p class=\"groupSecond #=value.replace(/ /g,'').replace(/&/g,'')#\">#=value#</p>"
        },
        ]
    });
    var grid = $("#salesTable").data("kendoGrid");
    grid.bind("dataBound", grid_dataBound);

    function grid_dataBound(e) {
        $("#salesTable").find(".k-group-col,.k-group-cell").remove();
        var spanCells = $("#salesTable").find(".k-grouping-row").children("td");
        if (numColumns <= 0) {
            numColumns = spanCells.attr("colspan") - 4;
        }
        spanCells.attr("colspan", numColumns);
        sort();
        var rows = e.sender.tbody.children();
        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            row.addClass("row black tab2");
        }
    }

    function sort() {

        $("th:has(a.k-link)").css("text-align", "center");
        $("th:has(a.k-link)").css("vertical-align", "top");
        $("a.k-link").css("color", "#163574");

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


        $("th:has(a.k-link)").click(onClickSort);
//        $(".k-link").click(onClickSort);
        var h = $("#salesTable .k-grouping-row");
        $.each(h, function (key, elem) {
            var s = $(elem).attr("class");
            if (s.indexOf("groupingOne") == -1 && s.indexOf("groupingSecond") == -1) {
                $(elem).remove();
            }
        }) 

        var t = $("#salesTable .k-group-footer");
        $.each(t, function (key, elem) {
            if ($(elem).next().hasClass("groupingOne") || $(elem).is($("#salesTable tr:last")) || $(elem).next().length == 0) {
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

        var f = $("#salesTable .footerFirst");
        $.each(f, function (key, elem) {
            var original = 0;
            var adjusted = 0;
            var proceeds = 0;
            var short = 0;
            var long = 0;
            $(elem).prevUntil(".groupingOne", ".footerSecond").each(function (ke, el) {
                original = original + parseInt($(el).find(".originalTotal")[0].textContent.replace(/,/g, ''));
                adjusted += parseInt($(el).find(".adjustedTotal")[0].textContent.replace(/,/g, ''));
                proceeds += parseInt($(el).find(".proceedsTotal")[0].textContent.replace(/,/g, ''));
                short += parseInt($(el).find(".shortTerm")[0].textContent.replace(/,/g, ''));
                long += parseInt($(el).find(".longTerm")[0].textContent.replace(/,/g, ''));
            })
            $(elem).find(".originalTotal")[0].textContent = (original).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");;
            $(elem).find(".adjustedTotal")[0].textContent = (adjusted).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");;
            $(elem).find(".proceedsTotal")[0].textContent = (proceeds).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");;
            $(elem).find(".shortTerm")[0].textContent = (short).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");;
            $(elem).find(".longTerm")[0].textContent = (long).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");;
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

//        sort();
    }

    $("#salesNoGroupTable").kendoGrid({
        pdf: {
            allPages: true,
        },
        excel: {
            fileName: "Sales.xlsx",
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
                        if (cell.value == "DATE")
                            for (var ro = i + 1; ro < sheet.rows.length; ro++) {
                                if (j < sheet.rows[ro].cells.length && sheet.rows[ro].cells[j].value)
                                    sheet.rows[ro].cells[j + 2].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j + 2].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
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
        dataSource: {
            data: model.TableData.Data,
            schema: {
                model: {
                    fields: {
                        OpenDate: {
                            type: "date"
                        },
                        CloseDate: {
                            type: "date"
                        },
                    }
                }
            },
            aggregate: [
                { field: "OriginalTotal", aggregate: "sum" },
                { field: "AdjustedTotal", aggregate: "sum" },
                { field: "ProceedsTotal", aggregate: "sum" },
                { field: "ShortTerm", aggregate: "sum" },
                { field: "LongTerm", aggregate: "sum" },
            ],
            sort: {
                field: "CloseDate",
                dir: "desc"
            },
        },
        sortable: {
            mode: "single",
            allowUnsort: false
        },
        scrollable: true,
        dataBound: grid_dataBound,
        columns: [

            {
                field: "Security",
                title: "SECURITY",
                footerTemplate: "TOTAL REALIZED GAIN/LOSS",
                attributes: {
                    "class": "col2 first"
                },
                width: 280,
            },

            {
                field: "Quantity",
                title: "QUANTITY",
                template: "#= (Quantity == null) ? ' ' : kendo.toString(Quantity, 'n0') #",
                attributes: {
                    "class": "col2"
                },
                width: 60
            },


            {
                title: "ORIGINAL COST",
                columns: [
                    {
                        field: "OpenDate", title: "DATE",
                        format: "{0:MM/dd/yyyy}",
                        //                template: "#= kendo.toString(kendo.parseDate(OpenDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                        attributes: {
                            "class": "col2"
                        },
                        headerAttributes: {
                            style: "border-left-width: 2px !important;"
                        },
                        width: 90
                    },
                    {
                        field: "OriginalTotal", title: "TOTAL",
                        template: "#= kendo.toString(OriginalTotal, 'n0') #",
                        footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                        attributes: {
                            "class": "col2"
                        }, width: 90

                    }]
                , headerAttributes: { style: "text-align: center; cursor:default;" }
            },
            {
                title: "ADJUSTED COST",
                columns: [
                    {
                        field: "AdjustedUnit", title: "UNIT",
                        template: "#= (Quantity == null) ? ' ' : kendo.toString(AdjustedUnit, 'n2') #",
                        attributes: {
                            "class": "col2"
                        },
                        width: 60
                    },
                    {
                        field: "AdjustedTotal", title: "TOTAL",
                        template: "#= kendo.toString(AdjustedTotal, 'n0') #",
                        footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                        attributes: {
                            "class": "col2"
                        },
                        width: 90
                    }]
                , headerAttributes: { style: "text-align: center; cursor:default;" }
            },
            {
                title: "PROCEEDS",
                columns: [
                    {
                        field: "CloseDate", title: "DATE",
                        format: "{0:MM/dd/yyyy}",
                        //                template: "#= kendo.toString(kendo.parseDate(CloseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",
                        attributes: {
                            "class": "col2"
                        },
                        width: 90
                    },
                    {
                        field: "ProceedsUnit", title: "UNIT",
                        template: "#= (Quantity == null) ? ' ' : kendo.toString(ProceedsUnit, 'n2') #",
                        attributes: {
                            "class": "col2"
                        },

                        width: 60
                    },
                    {
                        field: "ProceedsTotal", title: "TOTAL",
                        template: "#= kendo.toString(ProceedsTotal, 'n0') #",
                        footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                        attributes: {
                            "class": "col2"
                        },
                        width: 90,
                    }]
                , headerAttributes: { style: "text-align: center; cursor:default;" }
            },
            {
                title: "GAIN/LOSS",
                attributes: {
                    "class": "col3"
                },
                columns: [
                    {
                        field: "ShortTerm", title: "SHORT TERM",
                        footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                        template: "#= kendo.toString(ShortTerm, 'n0') #",
                        attributes: {
                            "class": "col2"
                        },
                        width: 80
                    },
                    {
                        field: "LongTerm", title: "LONG TERM",
                        footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                        template: "#= kendo.toString(LongTerm, 'n0') #",
                        attributes: {
                            "class": "col2"
                        },
                        width: 80
                    }]
                , headerAttributes: { style: "text-align: center; cursor:default;" }
            },
            {
                field: "GroupingOne"
            },
            {
                field: "GroupingSecond"
            },
        ]
    });

}

function getPDF(selector) {
    var grid = $("#salesTable").data("kendoGrid");
    grid.saveAsPDF()
}
function getExcel() {
    $("#salesNoGroupTable").getKendoGrid().saveAsExcel();
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
        var url = '/Client/Transactions/GetSales?searchFrom=' + searchFrom + '&searchTo=' + searchTo + "&isGroupQuery=" + getUrlParam("isGroupQuery") + "&isClientGroupQuery=" + getUrlParam("isClientGroupQuery")
            + "&contactIdQuery=" + getUrlParam("contactIdQuery") + "&entityIdQuery=" + getUrlParam("entityIdQuery");

        location.href = url;
    }
    else {
        showPopup("Invalid date.", true, "Error", true);
    }

    //allCommands(url);
/*
    $.ajax({
        url: '/Client/Transactions/GetSales?searchFrom=' + searchFrom + '&searchTo=' + searchTo + "&isGroup=" + getUrlParam("isGroup") + "&isClientGroup=" + getUrlParam("isClientGroup")
        + "&contactId=" + getUrlParam("contactId") + "&entityId=" + getUrlParam("entityId"),
        type: 'GET',
        dataType: "json",
        success: function (result) {
            var grid = $('#salesTable').getKendoGrid();
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