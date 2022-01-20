
    $("#navigatino-item-active ul li:nth-child(1)").addClass('active');
    $("#bs-example-navbar-collapse-1 ul li:nth-child(1)").addClass('active');


    kendo.pdf.defineFont({
        "DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
        "DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
        "DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
        "DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
    });

    //$.ajax({
    //    type: "GET",
    //    url: '/client/homeado/getfullinfo',
    //    async: true,
    //    dataType: "json",
    //    data: {
    //        contactId: $("#contactId").val()
    //    },
    //    success: function (data) {
    //        var viewData = data;
    //        $("#dataDate").text(viewData.Date);
    //        $("#custodianAccount").text(viewData.CustodianAccount);
    //        $("#name").text(viewData.Name);
    //    }
    //})

    $("#account_grid").kendoGrid({
        pdf: {
            allPages: true,
        },
        excel: {
            fileName: "Kendo UI Grid Export.xlsx",
            filterable: true
        },
        dataSource: {
            data: model.TableData.Data,
            sort: [{ field: "sortOrder", dir: "asc" }, { field: "AccountName", dir: "asc" }]
        },
        sortable: false ,
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            for (var i = 0; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
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
        dataBound: function onDataBound(arg) {
            $(".k-group-col,.k-group-cell").remove();
            var spanCells = $(".k-grouping-row").children("td");
            spanCells.attr("colspan", spanCells.attr("colspan") - 1);
            $(".k-footer-template").remove();
            $(".k-group-footer").last().remove();
            var rows = arg.sender.tbody.children();
            for (var j = 0; j < rows.length; j++) {
                var row = $(rows[j]);
                row.addClass("row red link tab");
            }
            sort();
/*
            $("th").each(function () {
                if ($(this).attr("data-dir") == "desc" || $(this).attr("data-dir") == "asc") {

                    onClickSort({ target: this });
                }
            });
*/
        },
        columns: [
            {
                field: "AccountName",
                title: "ACCOUNT",
                template: '<a href="\\#" onclick="changeEntity(#=GroupEntityId#,#=GroupIsClientGroup#,#=AccountEntityId#)" >#=AccountName#</a>',
                attributes: {
                    "class": "col1",     
                },
                headerAttributes: { style: "text-align: center; cursor:default;" },
                width: 300
            },
            {
                field: "GroupTotalValue",
                title: "MARKET VALUE",
 //               format: "{0:n0}",
                template: "<div align='center'>#= kendo.toString( AccountEntityId==null ? GroupTotalValue : AccountTotalValue,'n0' )#</div> ",
                attributes: {
                    "class": "col2"
                },
                headerAttributes: { style: "text-align: center; cursor:default;" },
                width: 90
            },
            {
                field: "PercentOfGroup",
                title: "% OF GROUP",
                template: "<div align='center'>#= kendo.toString( PercentOfGroup*100,'n1' )#</div> ",
                attributes: {
                    "class": "col3"
                },
                headerAttributes: { style: "text-align: center; cursor:default;" },
                width: 60
            },
            {
                field: "AccountEntityId",
                title: "DETAILS",
                template: "<div style=\"display:flex; justify-content:center;\"><div style=\"text-align:center;  display:inline-block; width:100%; height:100%; \"><a href=\"\\#\" onclick=\"openHoldings(#=ContactId#,#=GroupEntityId#,#=GroupIsClientGroup#,#=AccountEntityId#)\" class=\"red underline\" style=\"margin-right: 10px\">Holdings</a></div><div style=\"text-align:center; display:inline-block;width:100%; height:100%\"><a href=\"\\#\" onclick=\"openTransactions(#=ContactId#,#=GroupEntityId#,#=GroupIsClientGroup#,#=AccountEntityId#)\" class=\"red underline\" >Transactions</a></div></div>",
//                template: "<div style=\"text-align:center;  display:inline-block;  border-right: thin ridge \\#f6eee0;\"><a href=\"\\#\" onclick=\"openHoldings(#=ContactId#,#=GroupEntityId#,#=GroupIsClientGroup#,#=AccountEntityId#)\" class=\"red underline\" >Holdings</a></div>",
                //                template: "#= kendo.toString( PercentOfGroup*100,'n1' )#",
                attributes: {
                    "class": "col4"
                },
                headerAttributes: { style: "text-align: center; cursor:default;" },
                width: 140,
            },
            {
                hidden: true, field: "sortOrder"
            },
            {
                hidden: true, field: "AccountEntityId",
                template: "<p class=\"identity\">#=AccountEntityId#</p>"
            }
        ]
    });

    var grid = $("#account_grid").data("kendoGrid");
    var ds = grid.dataSource;
    grid.bind("dataBound", grid_dataBound);
    function grid_dataBound(e) {
        var rows = e.sender.tbody.children();
        for (var j = 0; j < rows.length; j++) {
            var row = $(rows[j]);
            row.addClass("row red link tab");
        }
/*
        $("th").each(function () {
            if ($(this).attr("data-dir") == "desc" || $(this).attr("data-dir") == "asc") {

                onClickSort({ target: this });
            }
            
        });
*/
    }

    // To put arrow below text when page Loads
    var sortedColumn = $("#account_grid .k-grid-header tr th[data-field='TotalValue']");
    $('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);



    function sort() {

        $("th:has(a.k-link)").css("text-align", "center");
        $("th:has(a.k-link)").css("vertical-align", "top");
        $("a.k-link").css("color", "#163574");

        $("th:has(a.k-link)").click(onClickSort);
        $(".k-link").click(onClickSort);
        

        $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");
        $("a.k-link").removeClass("k-link");
    }

    function onClickSort(e)
    {

        var target = $(this);
         //in case if it's redirect from children
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


function openHoldings(contactId, groupEntityId, groupIsClientGroup, accountEntityId) {
//    e.preventDefault();
//    var grid = $("#account_grid").data("kendoGrid");
//    var parameters = grid.dataItem($(e.currentTarget).closest("tr"));

    var entityId, isGroup, isClientGroup;
    if (accountEntityId == null || accountEntityId == "null" || accountEntityId == "")
    {
        entityId = groupEntityId;
        isGroup = true;
        isClientGroup = groupIsClientGroup;
    }
    else
    {
        entityId = accountEntityId;
        isGroup = false;
        isClientGroup = false;
    }
    
    var href = "/Client/Holdings/ViewAccount?isGroupQuery=" + Encrypt(isGroup) + "&contactIdQuery=" + Encrypt(contactId) +
                                                            "&entityIdQuery=" + Encrypt(entityId) +
                                                            "&isClientGroupQuery=" + Encrypt(isClientGroup);
    window.location.replace(href);
}


function openTransactions(contactId, groupEntityId, groupIsClientGroup, accountEntityId) {
    //    e.preventDefault();
    //    var grid = $("#account_grid").data("kendoGrid");
    //    var parameters = grid.dataItem($(e.currentTarget).closest("tr"));

    var entityId, isGroup, isClientGroup;
    if (accountEntityId == null || accountEntityId == "null" || accountEntityId == "") {
        entityId = groupEntityId;
        isGroup = true;
        isClientGroup = groupIsClientGroup;
    }
    else {
        entityId = accountEntityId;
        isGroup = false;
        isClientGroup = false;
    }

    window.location.replace("/Client/Transactions?isGroupQuery=" + Encrypt(isGroup) + "&contactIdQuery=" + Encrypt(contactId) +
        "&entityIdQuery=" + Encrypt(entityId) +
        "&isClientGroupQuery=" + Encrypt(isClientGroup));
}


function changeEntity(groupEntityId, groupIsClientGroup, accountEntityId) {
//    e.preventDefault();
//    var grid = $("#account_grid").data("kendoGrid");
    //    var parameters = grid.dataItem($(e.currentTarget).closest("tr"));

    var gEI = "";
    var gICG = "";
    var eI = "";
    var iG = "";
    var iCG = "";

    if (groupEntityId != "null") {
        gEI = groupEntityId;
    }
    if (groupIsClientGroup != "null") {
        gICG = groupIsClientGroup;
    }
    if ((accountEntityId != "null") &&  (accountEntityId != null)) 
    {
        eI = accountEntityId;
        iG = "false";
        iCG = "false";
    }

//    document.cookie = "groupEntityId=" + groupEntityId + "; path=/";
//    document.cookie = "groupIsClientGroup=" + groupIsClientGroup + "; path=/";
//    document.cookie = "entityId=" + accountEntityId + "; path=/";

    window.location.replace("/Client/Home?contactIdQuery=" + getUrlParam("contactIdQuery") + "&groupEntityIdQuery=" + Encrypt(gEI)
        + "&groupIsClientGroupQuery=" + Encrypt(gICG) + "&entityIdQuery=" + Encrypt(eI) + "&isGroupQuery=" + Encrypt(iG) + "&isClientGroupQuery=" + Encrypt(iCG));
        //?isGroup=" + parameters.IsGroup + "&contactId=" + parameters.ContactId +
          //                  "&entityId=" + parameters.EntityId +
            //                "&isClientGroup=" + parameters.IsClientGroup);
}

function getCookieValue(name) {
    cookieList = document.cookie.split('; ');
    cookies = {};
    for (i = cookieList.length - 1; i >= 0; i--) {
        cookie = cookieList[i].split('=');
        cookies[cookie[0]] = cookie[1];
    }
    return cookies[name];
}
/*
function getUrlParam(variable) {
    var query = window.location.search.substring(1);

    alert(query);

    var vars = query.split("&");

    alert(vars.length);


    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
}

*/

function getPDF() {

    $("a").css("text-decoration", "none");

    var grid = $("#account_grid").data("kendoGrid");
    grid.saveAsPDF()
    .done(function () {
        $("td.col1").children().css("text-decoration", "underline");
    })
}
function getExcel() {
    //$("#account_grid").getKendoGrid().saveAsExcel();

    var workbook = new kendo.ooxml.Workbook({
        sheets: [
          {
              columns: [{ autoWidth: true }],
              rows: [
                {
                    cells: [
                      {
                            value: "ACCOUNTS", fontFamily: "Arial" 
                      },
                      {
                          value: "MARKET VALUE", fontFamily: "Arial" 
                      },
                      {
                          value: "% OF GROUP", fontFamily: "Arial" 
                      }
                    ]
                },
              ]
          }
        ]
    });
    model.TableData.Data.sort(compare);
    for (var i = 0; i < model.TableData.Data.length; i++) {
        var data = model.TableData.Data[i];
        workbook._sheets[0].options.rows.push({
            cells: [
                {
                    //                    background: data.AccountEntityId == null ? "#87a188" : "",
                    //                    paddingLeft: data.AccountEntityId != null ? "2em" : "",
                    background: data.AccountEntityId == null ? "#F6EEE0" : "#FFFFFF",
                    value: data.AccountName, fontFamily: "Arial"
                },
                {
                    value: data.AccountEntityId == null ? data.GroupTotalValue : data.AccountTotalValue, fontFamily: "Arial"
                },
                {
                    value: data.PercentOfGroup * 100, fontFamily: "Arial"
                }
            ]
        })
    }
    function compare(a, b) {
        if (a.sortOrder > b.sortOrder)
            return 1;
        if (a.sortOrder < b.sortOrder)
            return -1;
        if (a.sortOrder === b.sortOrder)
        {
            if (a.AccountName > b.AccountName)
                return 1;
            if (a.AccountName < b.AccountName)
                return -1;
            if (a.AccountName === b.AccountName)
                return 0;
        }
    }
    kendo.saveAs({
        dataURI: workbook.toDataURL(),
        fileName: "AccountGroups.xlsx"
    });
}

$('.identity').each(function (key, elem) {
    var link = $(elem).text();
    if (link !== "" && link !== null && link !== "null")
    {
        $(elem).parent().parent().children("td:first").addClass(" entityGroup");
        $(elem).parent().parent().children("td").each(function () {
            $(this).css('background-color', '#f6eee0');
        });

        $(elem).parent().parent().children("td:first").each(function(){
            $(this).css('padding-left', '45px');
            $(this).mouseover(function () {
                $(this).css('background-color', '#841e36');
            });
            $(this).mouseleave(function () {
                $(this).css('background-color', '#f6eee0');
            });
        });
    }
})

//history.pushState("", "", "bar.html");