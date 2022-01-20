//$(document).ready(function () {
    var url = '/client/holdings/getcashes?isGroup=' + $("#isGroup").val() + '&isClientGroup=' +$("#isClientGroup").val()
                    + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val();
    allCommands(url);

    // To put arrow below text when page Loads
    var sortedColumn = $("#cashTable .k-grid-header tr th[data-field='MarketValueTotal']");
    $('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);

//});

function allCommands(url) {

    kendo.pdf.defineFont({
        "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
        "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
        "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
        "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
    });

    window.calculateWeightedAverage = function () {
        var data = model.CashData.Data;
        var total = 0;
        var compositeSum = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i].MarketValueTotal >= 0) {
                total += data[i].MarketValueTotal;
                compositeSum += data[i].MarketValueTotal * data[i].CurrentYield;
            }
        }
        return compositeSum / total;
    };

    $("#cashTable").kendoGrid({
        dataSource: {
            data: model.CashData.Data,
            //transport: {
            //    read: {
            //        type: "GET",
            //        url: url,
            //        dataType: "json",
            //        complete: function (data, status) {
            //            if (status == "success") {
            //                fix();
            //                //    style();
            //                sort();
            //            }
            //        }
            //    },
            //    serverSorting: false,
            //    pageSize: 20,
            //    serverFiltering: false,
            //    serverPaging: false

            //},
            schema: {
                model: {
                    fields: {
                        Holding: { type: "string" },
                        Quantity: { type: "number" },
                        Total: { type: "number" },
                        MarketValueUnits: { type: "number" },
                        MarketValueTotal: { type: "number" },
                        PercentOfAssets: { type: "number" },
                        AnnualIncome: { type: "number" },
                        CurrentYield: { type: "number" }
                    }
                }
            }, group: {
                field: "Category", aggregates: [
/*                    { field: "Quantity", aggregate: "sum" },
                    { field: "MarketValueUnits", aggregate: "sum" },*/
                    { field: "MarketValueTotal", aggregate: "sum" },
                    { field: "PercentOfAssets", aggregate: "sum" },
                    { field: "AnnualIncome", aggregate: "sum" },
                    { field: "CurrentYield", aggregate: "sum" }
                ]
            },
            sort: {
                field: "MarketValueTotal",
                dir: "desc"
            },

        },

        sortable: true,
        scrollable: true,
        excel: { fileName: "CashHoldings.xlsx" },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];

            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                if (row.type == "header")
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];
                        if (cell.value == "Date")
                            for (var ro = i + 1; ro < sheet.rows.length; ro++) {
                                sheet.rows[ro].cells[j].format = "MM/dd/yyyy";
                            }
                    }
                if (row.type == "group-footer" || row.type == "footer" || row.type == "header" || row.type == "group-header") {
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            if (cell.value.includes("</br>"))
                                cell.value = cell.value.replace("</br>", " ");
                            if (cell.value.includes("<br/>"))
                                cell.value = cell.value.replace("<br/>", " ");
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
        columns: [
            {
                field: "Holding", title: "HOLDINGS", width: 330,
                groupFooterTemplate: "TOTAL CASH & EQUIVALENTS",
                attributes: {/* style: "font-size: 9pt" */ "class": "col1" },
//                , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
//                , footerAttributes: { style: "font-size: 80%" },
                 sortable: {
                    allowUnsort:false,mode:"single"
                }

            },
            {
                field: "Quantity", title: "QUANTITY", width: 100,
                template: "<div align='center'>#=kendo.toString(Quantity,'n0')#</div>",
 //               groupFooterTemplate: "#= kendo.toString( sum,'n1' )#",
                attributes: { /*style: "text-align: center; font-size: 9pt" */ "class": "col2"},
//, headerAttributes: { style: "font-size: 75%; text-align: center" }
//                 footerAttributes: { /*style: "text-align: center; font-size: 9pt",*/ "class": "col2" },
                sortable: {
                    allowUnsort:false,mode:"single"
                }

            },
            {
                title: "MARKET VALUE",
                columns: [
                {
                    field: "MarketValueUnits", title: "UNIT", width: 100,
                    template: "<div align='center'>#=kendo.toString(MarketValueUnits,'n2')#</div>",
                    attributes: { /*style: "text-align: center; font-size: 9pt",*/ "class": "col2" },
 //                   groupFooterTemplate: "#= kendo.toString( sum,'n2' )#",
                    headerAttributes: { style: "text-align: center; border-left-width: 2px !important;" },
//                    footerAttributes: { /*style: "text-align: center; font-size: 9pt", */"class": "col2" },
                    sortable: {
                        allowUnsort: false, mode: "single"
                    }

                },
                {
                    field: "MarketValueTotal", title: "TOTAL", width: 100,
                    template: "<div align='center'>#=kendo.toString(MarketValueTotal,'n0')#</div>",
                    groupFooterTemplate: "#= kendo.toString( sum,'n0' )#",
                    attributes: { /*style: "text-align: center; font-size: 9pt",*/ "class": "col2" },
//                , headerAttributes: { style: "font-size: 75%; text-align: center" }
                    footerAttributes: { style: "text-align: center;" /*font-size: 9pt", "class": "col2 calc"*/ },
                    sortable: {
                    allowUnsort: false, mode: "single"
                }


                }]
                , headerAttributes: { style: "cursor:default; text-align: center" }

            },
            {
                field: "PercentOfAssets", title: "% OF<br/>ASSET<br/>CLASS", width: 70,
                template: "<div align='center'>#=kendo.toString(PercentOfAssets,'n1')#</div>",
                groupFooterTemplate: "#= kendo.toString( sum,'n1' )#",
                attributes: { /*style: "text-align: center; font-size: 9pt",*/ "class": "col2" },
//                , headerAttributes: { style: "font-size: 75%; text-align: center" 
                footerAttributes: { style: "text-align: center;" /*font-size: 9pt", "class": "col2 calc"*/ },
                sortable: {
                    allowUnsort: false, mode: "single"
                }


            },
            {
                field: "AnnualIncome", title: "EST.<br/>ANNUAL</br>INCOME", width: 100,
                template: "<div align='center'>#=kendo.toString(AnnualIncome,'n0')#</div>",
                groupFooterTemplate: "#= kendo.toString( sum,'n0' )#",
                attributes: { /*style: "text-align: center; font-size: 9pt",*/ "class": "col2" },
//                , headerAttributes: { style: "font-size: 75%; text-align: center" }
                footerAttributes: { style: "text-align: center;" /*font-size: 9pt", "class": "col2 calc"*/ },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },
            {
                field: "CurrentYield", title: "CURRENT</br>YIELD", width: 100,
                template: "<div align='center'>#=kendo.toString(CurrentYield,'n1')#</div>",
                groupFooterTemplate: "#= kendo.toString(window.calculateWeightedAverage(),'n1')#",
                attributes: { /*style: "text-align: center; font-size: 9pt",*/ "class": "col2" },
//                , headerAttributes: { style: "font-size: 75%; text-align: center" }
                 footerAttributes: { style: "text-align: center;" /*font-size: 9pt", "class": "col2 calc"*/ },
                sortable: {
                    allowUnsort: false, mode: "single"
                }


                },
            {
                hidden: true, field: "Category", groupHeaderTemplate: "<span class=\"groupOne\">#=value#</span>"
            }
        ],

        dataBound: grid_dataBound
        

    });

    var grid = $("#cashTable").data("kendoGrid");
    var ds = grid.dataSource;
    grid.bind("dataBound", grid_dataBound);

    


    function grid_dataBound(e) {
        fix();
        sort();
        $(".k-group-col,.k-group-cell").remove();
        var spanCells = $(".k-grouping-row").children("td");
        spanCells.attr("colspan", spanCells.attr("colspan") - 1);
        var rows = e.sender.tbody.children();
        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            row.addClass("row black tab");
        }
    }

    function sort() {

        $("th:has(a.k-link)").css("text-align", "center");
        $("a.k-link").css("color", "#163574");

        $("th:has(a.k-link)").bind("click", function (e) {
                 //  style();
                   fix();
                   var target = $(this);

            //remove from another
            $("th.k-header").children().each(function (index, elem) {
                if ($(elem).is("span")) {
                    if (index > 0) {
                        $(elem).css("display", "none");
                    }
                    if (index == 1 && dir != "asc" && dir != "desc") {
                        $(elem).css("display", "block");

                    }
                }
                });
      

            // in case if it's redirect from children
            if (!$(target).is("th")) {
                target = $(target).parent();
            }

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
        });
        $(".k-link").bind("click", function (e) {
            $(this).parent().click(this);
        });

        $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");


        $("a.k-link").removeClass("k-link");
    }
    function fix() {

        var grid = $("#cashTable").data("kendoGrid");
        var ds = grid.dataSource;
        var totalProduct = 0.0;
        var totalValue = 0.0;
        for (var i = 0; i < ds.total() ; i++) {
            totalProduct += ds.at(i).CurrentYield * ds.at(i).MarketValueTotal;
      
            totalValue += ds.at(i).MarketValueTotal;
        }
          
        var value = totalProduct / totalValue;
        $("td:contains('UNDEFINED')").text(Math.round(value * 100) / 100);
    }
/*
    function style() {
        $("td:contains('CASH AND EQUIVALENTS ')").css("background-color", "#e9dfcd");
        $("td:contains('CASH AND EQUIVALENTS ')").css("color", "#163574");
        $("tr.k-group-footer").find('td').each(function () { }).css("background-color", "#f6eee0");
        $("tr.k-group-footer").find('td').each(function () { }).css("color", "#163574");

        $("div.k-grid-content").css("max-height", "400px");
        $('td[role="gridcell"]').css("background-color", "#dfd4bf");

    }
*/
}

function getPDF(selector) {
    var grid = $("#cashTable").data("kendoGrid");
    grid.saveAsPDF();
}
function getExcel() {
    var a = $("#cashTable").getKendoGrid();

    a.saveAsExcel();
}

function change(kendo){
    $(kendo).find("th").each(function () {
        var html = $(this).html();

        if (html.includes("</br>"))
            $(this).html(html.replace("</br>", " "));
        if (html.includes("<br>"))
            $(this).html(html.replace("<br>", " "));


    });
}
function changeBack(kendo) {

}
