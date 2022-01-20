/// <reference path="EquitiesInitGroups.js" />
var url = '/Client/Holdings/GetIncomes?isGroup=' + $("#isGroup").val() + '&isClientGroup=' + $("#isClientGroup").val()
    + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val();

var group1="";
var group2 = "";
var groupID = 1;
var numColumns = -1;

allCommands(url);

// To put arrow below text when page Loads
var groupedSortedColumn = $("#incomeGroupTable .k-grid-header tr th[data-field='MarketValueTotal']");
$('<span class="arrow-down" style="display:block"></span>').appendTo(groupedSortedColumn);


function allCommands(url)
{
    kendo.pdf.defineFont({
        "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
        "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
        "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
        "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
    });

    window.calculateGrpWeightedAverage = function () {

        var data = model.IncomeData.Data;
        var total = 0;
        var compositeSum = 0;
        if (groupID == 1) {
            for (var i = 0; i < data.length; i++) {
                if ((data[i].Category.toUpperCase() == group1.toUpperCase()) && (data[i].MarketValueTotal >= 0)) {
                    total += data[i].MarketValueTotal;
                    compositeSum += data[i].MarketValueTotal * data[i].CurrentYield;
                }
            }
        }
        else {
            for (var i = 0; i < data.length; i++) {
                if ((data[i].Category.toUpperCase() == group1.toUpperCase()) && (data[i].SubCategory.toUpperCase() == group2.toUpperCase()) && (data[i].MarketValueTotal >= 0)) {
                    total += data[i].MarketValueTotal;
                    compositeSum += data[i].MarketValueTotal * data[i].CurrentYield;
                }
            }
            groupID = 1;
        }
        return compositeSum / total;
    };

    window.setGroup1 = function (value) {
        group2 = "";
        groupID = 1;
        group1 = value;

        return group1;
    };

    window.setGroup2 = function (value) {
        group2 = value;
        groupID = 2;
        return group2;
    };

    $("#incomeGroupTable").kendoGrid({
        pdf: {
            allPages: true,
        },
        scrollable: true,
        excel: { fileName: "FixedIncomeHoldings-Grouped.xlsx" },
        excelExport: function (e) {
            var count = 0;
            var firstIndex;
            var secondIndex;
            var sheet = e.workbook.sheets[0];
            var check = 0;
            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                if (row.type == "group-header") {
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];
                        if (cell.value !== undefined && cell.value.indexOf("CategorySort") !== -1) {
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
                            for (var ro = i + 1; ro < sheet.rows.length; ro++) {
                                if (j < sheet.rows[ro].cells.length && sheet.rows[ro].cells[j].value)
                                sheet.rows[ro].cells[j + 2].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j + 2].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
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
        sortable: {
            allowUnsort: false, mode: "single"
        },
        columns: [
            {
                field: "Holdings", title: "HOLDING", width: 250,
                groupFooterTemplate: "TOTAL",
                template: function (data) {
                    var division = data.Holdings.indexOf("\n", 0);
                    var firstStr = data.Holdings.substring(0, division);
                    var secondStr = data.Holdings.substring(division + 1, data.Holdings.length);
                    return firstStr + "</br>" + secondStr;
                }, attributes: { "class": "col2 first" },
                //                 , headerAttributes: { style: "font-size: 75%; text-align: center;  " }
                //                 , footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },
            {
                field: "Quantity", title: "QUANTITY", width: 70,
                template: "<div align='center'>#= kendo.toString(Quantity,'n0' )#</div>"
                 , attributes: { "class": "col2" },
                //                 , headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                sortable: {
                    allowUnsort: false, mode: "single"
                }

            },

            {
                title: "ADJUSTED COST",
                columns: [
                {
                    field: "AdjustedCostDate", title: "DATE", width: 80,
					format: "{0:MM/dd/yyyy}",
                   // template: "<div align='center'>#= kendo.toString(kendo.parseDate(AdjustedCostDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #</div>"
                      attributes: { "class": "col2", "style": "text-align: center" },
                    headerAttributes: { style: "text-align: center;  border-left-width: 2px !important" },
                    sortable: {
                        allowUnsort: false, mode: "single"
                    }

                },
                {
                    field: "AdjustedCostUnit", title: "UNIT", width: 50,
                    template: "<div align='center'>#= kendo.toString( AdjustedCostUnit,'n2' )#</div>"
                    , attributes: { "class": "col2" },
                    //                    , headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                    sortable: {
                        allowUnsort: false, mode: "single"
                    }

                },
                {
                    field: "AdjustedCostTotal", title: "TOTAL", width: 70,
                    template: "<div align='center'>#= kendo.toString( AdjustedCostTotal,'n0' )#</div>",
                    groupFooterTemplate: "<div align='center'>#= kendo.toString( sum ,'n0' )#</div>",
                    attributes: { "class": "col2" },
                    //                    headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                    //                    footerAttributes: { style: "font-size: 80%" },
                    sortable: {
                        allowUnsort: false, mode: "single"
                    }

                }], headerAttributes: { style: "text-align: center; cursor:default;" }
            },
            {
                title: "MARKET VALUE",
                columns: [
                {
                    field: "MarketValueUnits", title: "UNIT", width: 70,
                    template: "<div align='center'>#= kendo.toString( MarketValueUnits,'n2' )#</div>",
                    attributes: { "class": "col2" },
                    //                    headerAttributes: { style: "font-size: 75%; text-align: center;  border-left-width: 2px !important" },
                    sortable: {
                        allowUnsort: false, mode: "single"
                    }

                },
                {
                    field: "MarketValueTotal", title: "TOTAL", width: 70,
                    groupFooterTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                    template: "<div align='center'>#= kendo.toString( MarketValueTotal,'n0' )#</div>",
                    attributes: { "class": "col2" },
                    //                    headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                    //                    footerAttributes: { style: "font-size: 80%; text-align: center; " },
                    sortable: {
                        allowUnsort: false, mode: "single"
                    }
                }
                ], headerAttributes: { style: "text-align: center; cursor:default;" }
            },
            {
                field: "MarketValuePercentOfAssets", title: "% OF<br/>ASSET<br/>CLASS", width: 70,
                groupFooterTemplate: "<div align='center'>#= kendo.toString(sum, 'n1') #</div>",
                template: "<div align='center'>#= kendo.toString( MarketValuePercentOfAssets,'n1' )#</div>",
                attributes: { "class": "col2" },
                //                    headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                //                    footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },

            {
                field: "AccuredInterest", title: "ACCRUED<br/>INTEREST", width: 70,
                groupFooterTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                template: "<div align='center'>#= kendo.toString( AccuredInterest,'n0' )#</div>",
                attributes: { "class": "col2" },
                //                headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                //                footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },
            {
                field: "AnnualIncome", title: "EST.<br/>ANNUAL<br/>INCOME", width: 70,
                template: "<div align='center'>#= kendo.toString( AnnualIncome,'n0' )#</div>",
                groupFooterTemplate: "<div align='center'>#= kendo.toString( sum,'n0' )#</div>",
                attributes: { "class": "col2" },
                //                headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                //                footerAttributes: { style: "font-size: 80%" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },
            {
                field: "CurrentYield", title: "CURRENT<br/>YIELD", width: 70,
                template: "<div align='center'>#= kendo.toString( CurrentYield,'n1' )#</div>",
                groupFooterTemplate: "<div align='center'>#= kendo.toString(window.calculateGrpWeightedAverage(),'n1')#</div>",
                attributes: { "class": "col2" },
                //                headerAttributes: { style: "font-size: 75%; text-align: center;  " },
                //                footerAttributes: { style: "font-size: 80%;text-align: center;" },
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },

            {
                hidden: true, field: "Category", groupHeaderTemplate: "<span class=\"groupOne\">#=setGroup1(value)#</span>"
            },
            {
                hidden: true, field: "SubCategory", groupHeaderTemplate: "<p class=\"groupSecond\">#=setGroup2(value)#</p>"

            }

        ],

        dataSource: {
            data: model.IncomeData.Data,
            //transport: {
            //    read: {
            //        type: "GET",
            //        url: url,
            //        dataType: "json",
            //        success: function (data) {
            //            sortgroups();

            //        }                  
            //    }


            //},
            schema: {
            	model: {
            		fields: {
            			AdjustedCostDate: {
            				type: "date"
            			},
            		}
            	}
            },
            group: [
                {
                    field: "CategorySort",
                    aggregates:
                            [{ field: "AdjustedCostTotal", aggregate: "sum" },
                                { field: "MarketValueTotal", aggregate: "sum" },
                                { field: "MarketValuePercentOfAssets", aggregate: "sum" },
                                { field: "AccuredInterest", aggregate: "sum" },
                                { field: "AnnualIncome", aggregate: "sum" },
                                { field: "CurrentYield", aggregate: "sum" }]
                },
                {
                    field: "Category",
                    aggregates:
                            [{ field: "AdjustedCostTotal", aggregate: "sum" },
                                { field: "MarketValueTotal", aggregate: "sum" },
                                { field: "MarketValuePercentOfAssets", aggregate: "sum" },
                                { field: "AccuredInterest", aggregate: "sum" },
                                { field: "AnnualIncome", aggregate: "sum" },
                                { field: "CurrentYield", aggregate: "sum" }]
                },
                {
                    field: "SubCategorySort",
                    aggregates:
                            [{ field: "AdjustedCostTotal", aggregate: "sum" },
                                { field: "MarketValueTotal", aggregate: "sum" },
                                { field: "MarketValuePercentOfAssets", aggregate: "sum" },
                                { field: "AccuredInterest", aggregate: "sum" },
                                { field: "AnnualIncome", aggregate: "sum" },
                                { field: "CurrentYield", aggregate: "sum" }]
                },
                {
                    field: "SubCategory",
                    aggregates:
                        [{ field: "AdjustedCostTotal", aggregate: "sum" },
                            { field: "MarketValueTotal", aggregate: "sum" },
                            { field: "MarketValuePercentOfAssets", aggregate: "sum" },
                            { field: "AccuredInterest", aggregate: "sum" },
                            { field: "AnnualIncome", aggregate: "sum" },
                            { field: "CurrentYield", aggregate: "sum" }]
                }],
            sort: {
                field: "MarketValueTotal",
                dir: "desc"
            },

        },
        dataBound: grid_dataBound
        //    function onDataBound(arg) {
        //    $("#incomeGroupTable").find(".k-group-col,.k-group-cell").remove();
        //    var spanCells = $("#incomeGroupTable").find(".k-grouping-row").children("td");
        //    spanCells.attr("colspan", spanCells.attr("colspan") - 2);
        //    sortgroups();
        //}

    });

    var grid = $("#incomeGroupTable").data("kendoGrid");
    var ds = grid.dataSource;
    grid.bind("dataBound", grid_dataBound);

    function grid_dataBound(e) {
        $("#incomeGroupTable").find(".k-group-col,.k-group-cell").remove();
        var spanCells = $("#incomeGroupTable").find(".k-grouping-row").children("td");
        if (numColumns <= 0)
        {
            numColumns = spanCells.attr("colspan")-4;
        }
        spanCells.attr("colspan", numColumns);
        sortgroups();
        var rows = e.sender.tbody.children();
        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            row.addClass("row black tab2");
        }
    }

    
    function sortgroups() 
    {
              $("th:has(a.k-link)").css("text-align", "center");
              $("th:has(a.k-link)").css("vertical-align", "top");
              $("a.k-link").css("color", "#163574");
        
              $(".groupOne").closest("tr").addClass(" groupingOne");
              $(".groupSecond").closest("tr").addClass(" groupingSecond");
        
          //      $("#incomeGroupTable th:has(a.k-link)").bind("click", onClickSort2);
          //      $("#incomeGroupTable .k-link").bind("click",onClickSort2);
                 $("th:has(a.k-link)").click(onClickSort);
//   5/3/2017              $(".k-link").click(onClickSort2);
                 var h = $("#incomeGroupTable .k-grouping-row");
                 $.each(h, function (key, elem) {
                     var s = $(elem).attr("class");
                     if (s.indexOf("groupingOne") == -1 && s.indexOf("groupingSecond") == -1) {
                         $(elem).remove();
                     }
                 })

                var t = $("#incomeGroupTable .k-group-footer");
                $.each(t, function (key, elem)
                {
                    if ($(elem).next().hasClass("groupingOne") || $(elem).is($("#incomeGroupTable tr:last"))) {
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
        
                $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
                $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
                $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");
        
                $("#incomeGroupTable a.k-link").removeClass("k-link");
              //        $("a.k-link").removeClass("k-link");
        
          }
    }


function onClickSort(e) {

    var target = $(this);
    //remove from another
    $("#incomeGroupTable th.k-header").children().each(function (index, elem) {
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

// 5/3/2017   sortgroups();
}

function getPDFGroups() {
    var grid = $("#incomeGroupTable").data("kendoGrid");
    grid.saveAsPDF();
}
function getExcelGroups() {
    $("#incomeGroupTable").getKendoGrid().saveAsExcel();
}

