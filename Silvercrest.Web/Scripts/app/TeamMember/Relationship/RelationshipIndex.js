$(document).ready(function () {
    kendo.pdf.defineFont({
        "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
        "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
        "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
        "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
    });

    $("#UserGrid").kendoGrid({
        dataSource: {
            data: model.TableData.Data,
            sort: {
                field: "MarketValue",
                dir: "desc"
            },
            schema: {
                model: {
                    fields: {
                        MarketValue: {
                            type: "number"
                        },
                        Name: {
                            type: "string"
                        },
                        PercentOfTotal: {
                            type: "number"
                        },
                    }
                }
            },
            groupable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            aggregate: [
            { field: "MarketValue", aggregate: "sum" },
            { field: "PercentOfTotal", aggregate: "sum" }]

        },
        maxheight: 335,
        filterable: true,
        dataBound: function onDataBound(arg) {
            sort();
        },
        sortable: {
            mode: "single",
            allowUnsort: false
        },
        columns: [{
            field: "Name",
            title: "Accounts",
            attributes: {
                "class": "col1"
            },
            footerTemplate: "Total",
            width: 200,
        },
        {
            field: "MarketValue",
            title: "Market Value",
            footerTemplate: "<div class='blue' style='text-align: center'>#= kendo.toString(sum, 'n0') #</div>",
            attributes: {
                "class": "col2"
            },
            format: "{0:n0}",
            width: 70
        },
        {
            field: "PercentOfTotal",
            title: "Percent Of Total",
            footerTemplate: "<div class='blue' style='text-align: center'>#= kendo.toString(sum, 'n1') #</div>",
            attributes: {
                "class": "col3"
            },
            format: "{0:n1}",
            width: 70
        },
        {
            field: "Id",
            hidden: true
        },
        {
            field: "IsGroup",
            hidden: true
        }],

      

    });
function sort() {        
       
    $("th:has(a.k-link)").css("text-align", "center");
    $("th:has(a.k-link)").css("vertical-align", "top");
    $("a.k-link").css("color", "#163574");


    $("th:has(a.k-link)").bind("click", onClickSort);
    $(".k-link").bind("click", function (e) {
        $(this).parent().click(this);
    });

    $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
    $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
    $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");


    $("a.k-link").removeClass("k-link");

}
    function valueFilter(element) {
        element.kendoAutoComplete({
            dataSource: titles
        });
    }

    var grid = $("#UserGrid").data("kendoGrid");
    grid.bind("dataBound", grid_dataBound);
    grid.dataSource.fetch();

    function grid_dataBound(e) {
        var rows = e.sender.tbody.children();
        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            row.addClass("row blue");
        }

        $("th").each(function () {
            if ($(this).attr("data-dir") == "desc" || $(this).attr("data-dir") == "asc") {
               
                onClickSort({ target: this });
            }

        });
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

            if (index > 1) {
                $(this).css("display", "none");
            }
            if (dir == "asc") {
                set.eq(3).css("display", "block");

            }
            if (dir == "desc") {
                set.eq(4).css("display", "block");

            }
            if (dir != "asc" && dir != "desc") {
                set.eq(2).css("display", "block");

            }

        });
    }

    var sortedColumn = $("#UserGrid .k-grid-header tr th[data-field='MarketValue']");
    $('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);
});