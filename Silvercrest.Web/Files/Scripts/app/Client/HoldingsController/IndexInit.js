$(document).ready(function () {
      
    kendo.pdf.defineFont({
        "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
        "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
        "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
        "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
    });
    $("#holdingsAccountTable").kendoGrid({
        pdf: {
            allPages: true
        },
        scrollable: true,
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
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: '/Client/Holdings/GetAccounts',
                    dataType: "json",
                    complete: function (data, status) {

                        if (status === "success") {
                            grid.removeRow($("tr td:contains('TOTAL')").parent().last());
                            sort();
                        }
                    }
                },
                serverSorting: false,
                pageSize: 20,
                serverFiltering: false,
                serverPaging: false,
                height: 335

            },
            sort: {
                field: "MarketValue",
                dir: "desc"
            },
            group: {
                field: "AccountType", aggregates: [
                   { field: "MarketValue", aggregate: "sum" },
                { field: "PercentOfTotal", aggregate: "sum" }],
            }

        },
        sortable: true,
        dataBound: function onDataBound(arg) {
            $(".k-group-col,.k-group-cell").remove();
            var spanCells = $(".k-grouping-row").children("td");
            spanCells.attr("colspan", spanCells.attr("colspan") - 1);
        },
        columns: [
            {
                field: "Account",
                title: "ACCOUNTS",
                template: '<a href="/Client/Holdings/ViewAccount?isGroup=#=IsGroup#&isClientGroup=#=IsClientGroup#&contactId=#=ClientId#&entityId=#=EntityId#">#=Account#</a>',
                groupFooterTemplate: "TOTAL", width: 360,         
                attributes: { "class": "col1" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },
            {
                field: "MarketValue",
                title: "MARKET VALUE",
                groupFooterTemplate: "$#= kendo.toString( sum,'n0' )#",
                attributes: { "class": "col2", style: "text-align: center" },
                footerAttributes: { style: "text-align: center" }, width: 150,
                format: "{0:n}",
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },
            {
                field: "PercentOfTotal",
                title: "PERCENT OF TOTAL",
                template: "#= kendo.toString(PercentOfTotal* 100,'n1' )#",
                groupFooterTemplate: "#= kendo.toString( sum * 100,'n1' )#%",
                attributes: { style: "text-align: center" },
                footerAttributes: { style: "text-align: center;" }, width: 120,
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },
            {
                hidden: true, field: "IsGroup"
            },
            {
                hidden: true, field: "ClientId"
            },
            { hidden: true, field: "IsClientGroup" },
            { hidden: true, field: "EntityId" },
            {
                hidden: true, field: "AccountType", groupHeaderTemplate: "#=value#"
            }
        ],

    });

    var grid = $("#holdingsAccountTable").data("kendoGrid");
       grid.bind("dataBound", grid_dataBound);
       grid.dataSource.fetch();
    
    function grid_dataBound(e) {
            var rows = e.sender.tbody.children();
            for (var j = 0; j < rows.length; j++) {
                    var row = $(rows[j]);

                    row.addClass("row red link tab");
                }


            $("td:contains('Accounts')").css("background-color", "#E9DFCD");
            $("td:contains('Groups')").css("background-color", "#E9DFCD");

            $("tr.k-group-footer").find('td').each(function () {                  
            }).css("background-color", "#F6EEE0");

            $("tr.k-group-footer").find('td').each(function () {
            }).css("color", "#163574");

            $("td:contains('All Accounts')").css("background-color", "#dfd4bf");
            

            $("div.k-grid-content.km-widget.km-scroll-wrapper").css("max-height", "400px");
                

    }
 
    function sort() {
        $(".k-group-footer").children().eq(2).text("100.0%");
        $("th:has(a.k-link)").css("text-align", "center");
        $("th:has(a.k-link)").css("vertical-align", "top");
        $("a.k-link").css("color", "#163574");

        $("th:has(a.k-link)").bind("click", function (e) {
            $(".k-group-footer").children().eq(2).text("100.0%");
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
    $("#holdingsAccountTable").getKendoGrid().saveAsExcel();
}
