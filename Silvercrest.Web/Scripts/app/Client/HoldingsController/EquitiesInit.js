$(document).ready(function () {

kendo.pdf.defineFont({
    "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
    "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
    "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
    "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
});

$("#equitiesTable").kendoGrid({
    scrollable:true,
    sortable: true,
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
                if (row.type == "group-footer" || row.type == "footer" || row.type == "header") {
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
                        }
                    }
                }
            }
        },
    columns: [
        {
            field: "Holdings", title: "HOLDING", width: 300,
            groupFooterTemplate: "TOTAL",
            template: function (data) {
                var division = data.Holdings.indexOf("\n", 0);
                var firstStr = data.Holdings.substring(0, division);
                var secondStr = data.Holdings.substring(division + 1, data.Holdings.length);
                return firstStr + "</br>" + secondStr;
            }, attributes: { style: "font-size: 9pt" }
             , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
             , footerAttributes: { style: "font-size: 80%" },
               sortable: {
                    allowUnsort: false, mode: "single"
                }
        },
        {
            field: "Quantity", title: "QUANTITY", width: 70,
            template: "<div align='center'>#= kendo.toString(Quantity,'n0' )#</div>"
             , attributes: { style: "font-size: 9pt" }
             , headerAttributes: { style: "font-size: 75%; text-align: center;  " },
             sortable: {
                 allowUnsort: false, mode: "single"
             }

        },

        {
            title: "ADJUSTED COST",
            columns: [
            {
                field: "AdjustedCostDate", title: "DATE", width: 70,
                template: "#= kendo.toString(kendo.parseDate(AdjustedCostDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center; border-left-width: 2px !important" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },
            {
                field: "AdjustedCostUnit", title: "UNIT", width: 70,
                template: "<div align='center'>#= kendo.toString( AdjustedCostUnit,'n0' )#</div>"
                , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },
            {
                field: "AdjustedCostTotal", title: "TOTAL", width: 70,
                template: "<div align='center'>$#= kendo.toString( AdjustedCostTotal,'n0' )#</div>",
                groupFooterTemplate: "$#= kendo.toString( sum ,'n0' )#"
                , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
                , footerAttributes: { style: "font-size: 80%; text-align: center;" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            }], headerAttributes: { style: "font-size: 75%; text-align: center" }
        },
        {
            title: "MARKET VALUE",
            columns: [
            {
                field: "MarketValueUnits", title: "UNITS", width: 70,
                template: "<div align='center'>#= kendo.toString( MarketValueUnits,'n0' )#</div>"
                , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center; border-left-width: 2px !important " },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },
            {
                field: "MarketValueTotal", title: "TOTAL", width: 70,
                groupFooterTemplate: "<div align='center'>$#= kendo.toString(sum, 'n0') #</div>",
                template: "<div align='center'>$#= kendo.toString( MarketValueTotal,'n0' )#</div>"
                , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
                , footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },
            {
                field: "MarketValuePercentOfAssets", title: "PERCENT<br/>OF ASSETS", width: 70,
                groupFooterTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #%</div>",
                template: "<div align='center'>#= kendo.toString( MarketValuePercentOfAssets,'n0' )#%</div>"
                                , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
                , footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            }], headerAttributes: { style: "font-size: 75%; text-align: center" }
        },
      
        {
            field: "AnnualIncome", title: "ANNUAL<br/>INCOME", width: 70,
            groupFooterTemplate: "<div align='center'>$#= kendo.toString(sum, 'n0') #</div>",
            template: "<div align='center'>$#= kendo.toString( AnnualIncome,'n0' )#</div>"
                            , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
                , footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
        },
        {
            field: "CurrentYield", title: "CURRENT<br/>YIELD", width: 70,
            groupFooterTemplate: "UNDEFINED",
            template: "<div align='center'>#= kendo.toString( CurrentYield,'n3' )#</div>"
                            , attributes: { style: "font-size: 9pt" }
                , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
                , footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
        },

        {
            hidden: true, field: "SubCategory",
            groupHeaderTemplate: function (data) {

                var ds = $("#equitiesTable").data("kendoGrid").dataSource;

                for (var i = 0; i < ds.total() ; i++) {
                    if (ds.at(i).SubCategory == data.value)
                        return ds.at(i).Category + " " + ds.at(i).SubCategory;
                }

                return "NONE";
            },
        }
    ],
    
    dataSource: {
        transport: {
            read: {
                type: "GET",
                url: '/Client/Holdings/GetEquities?isGroup=' + $("#isGroup").val() + '&isClientGroup=' +$("#isClientGroup").val()
                    + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val(),
                dataType: "json",
                complete(data, status) {
                    if (status == "success") {
                        fix();
                        sort();
                    }
                }
            }

        },
        schema: {
            model: {
                fields: {
                    Holdings: { type: "string" },
                    Quantity: { type: "number" },
                    AdjustedCostDate: { type: "date" },
                    AdjustedCostUnit: { type: "number" },
                    AdjustedCostTotal: { type: "number" },
                    MarketValueUnits: { type: "number" },
                    MarketValueTotal: { type: "number" },
                    MarketValuePercentOfAssets: { type: "number" },   
                    AnnualIncome: { type: "number" },
                    CurrentYield: { type: "number" },

                }
            }
        },
        group: [
               {
                   field: "SubCategory",
                   aggregates: [
                      { field: "AdjustedCostTotal", aggregate: "sum" },
                      { field: "MarketValueTotal", aggregate: "sum" },
                      { field: "MarketValuePercentOfAssets", aggregate: "sum" },         
                      { field: "AnnualIncome", aggregate: "sum" },
                      { field: "CurrentYield", aggregate: "sum" }]
               }]

    },
    dataBound: function onDataBound(arg) {
         $(".k-group-col,.k-group-cell").remove();
        var spanCells = $(".k-grouping-row").children("td");
        spanCells.attr("colspan", spanCells.attr("colspan") - 1);
    }

});
function fix() {

    var grid = $("#equitiesTable").data("kendoGrid");
    var ds = grid.dataSource;
    var totalProduct = 0.0;
    var totalValue = 0.0;
    for (var i = 0; i < ds.total() ; i++) {
        totalProduct += ds.at(i).CurrentYield * ds.at(i).MarketValueTotal;

        totalValue += ds.at(i).MarketValueTotal;
    }

    $('td[role="gridcell"]').css("background-color", "#dfd4bf");

    var value = totalProduct / totalValue;

    $("td:contains('UNDEFINED')").text(Math.round(value * 1000) / 1000);

    $("tr.k-grouping-row").find('td').each(function () {
    }).css("background-color", "#e9dfcd");

    $("tr.k-grouping-row").find('td').each(function () {
    }).css("color", "#163574");

    $("tr.k-group-footer").find('td').each(function () {
    }).css("background-color", "#f6eee0");

    $("tr.k-group-footer").find('td').each(function () {
    }).css("color", "#163574");


}
function sort() {

    $("th:has(a.k-link)").css("text-align", "center");
    $("th:has(a.k-link)").css("vertical-align", "top");
    $("a.k-link").css("color", "#163574");

    $("th:has(a.k-link)").bind("click", function (e) {
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

});

function getPDF(selector) {
   
    var grid = $("#equitiesTable").data("kendoGrid");
    grid.saveAsPDF();
}
function getExcel() {
    $("#incomeTable").getKendoGrid().saveAsExcel();
}
