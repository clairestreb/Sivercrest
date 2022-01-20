var width= 0; var height =0;
$(document).ready(function () {

 kendo.pdf.defineFont({
            "DejaVu Sans"             : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
            "DejaVu Sans|Bold"        : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
            "DejaVu Sans|Bold|Italic" : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
            "DejaVu Sans|Italic"      : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
 });
 $("#accountTable").kendoGrid({
   
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
    columns: [
        {
             field: "Operation", title:"Operation",
             template: function (data) {            
                 if (data.Url == "" || data.Operation == "Margin")
                     return data.Operation;
                
                 var src = '/Client/Holdings/'+data.Url+ '?isGroup=' + $("#isGroup").val() + '&isClientGroup=' +$("#isClientGroup").val()
                    + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val();
                 return data.Operation;

             },
             width: 250
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

         },
         {
             field: "CashMoneyFunds", title: "CASH &<br/>MONEY FUNDS",
             template: "#=kendo.toString(CashMoneyFunds, 'n2')#",
             attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80,
             headerAttributes: { style: "font-size: 75%; text-align: center" }

         },
         {
             field: "FixedIncome", title: "FIXED</br>INCOME",
             template: "#=kendo.toString(FixedIncome, 'n2')#"
             , attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

         },  
         {
             field: "Equities", title: "EQUITIES",
             template: "#=kendo.toString(Equities, 'n2')#"
             , attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

           
         },
         {
             field: "OtherAssets", title: "OTHER</br>ASSETS",
             template: "#=kendo.toString(OtherAssets, 'n2')#"
             , attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

         },
         {
             field: "MarketValue", title: "MARKET</br>VALUE",
             template: "#=kendo.toString(MarketValue, 'n2')#",
             attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

             
         },
         {
             field: "PERCENT OF:",
             columns: [
                 {
                     field: "PercentOfAssetClass", title: "ASSET</br>CLASS",
                     template: "#=kendo.toString(PercentOfAssetClass*100, 'n1')#",
                     attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80,
                     headerAttributes: { style: "font-size: 75%; text-align: center; border-left-width: 2px !important" }
                  
                    
                 },
                 {
                     field: "PercentOfTotal", title: "TOTAL",
                     template: "#=kendo.toString(PercentOfTotal*100, 'n1')#",
                     attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80,
                     headerAttributes: { style: "font-size: 75%; text-align: center" }

                     
                 }
             ], headerAttributes: { style: "font-size: 9pt; text-align: center"}
         },
            
        {
            field: "AnnualIncome", title: "ANNUAL</br>INCOME",
            template: "#=kendo.toString(AnnualIncome, 'n2')#",
            attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

          
        },

        {
            field: "CurrentYield", title: "ESTIMATED</br>CURRENT</br>YIELD",
            template: "#=kendo.toString(CurrentYield, 'n2')#",
            attributes: { style: "text-align: center; font-size: 9pt", "class": "col2" }, width: 80
            , headerAttributes: { style: "font-size: 75%; text-align: center" }

        },
        { hidden: true, field: "AssetClass" },
        { hidden: true, field: "Strategy" }
            
        
     ],
    dataSource: {
        data: model.FullBalancesData.Data,
        //transport: {
        //    read: {
        //        type: "GET",
        //        url: "/Client/Holdings/GetBalances?isGroup=" + $("#isGroup").val() + "&isClientGroup=" + $("#isClientGroup").val()
        //            + "&contactId=" + $("#contactId").val() + "&entityId=" + $("#entityId").val(),
        //        dataType: "json",
        //        complete: function (data, status) {
        //            if (status === "success") {
                       
                 
        //                addRow();
        //                styleTotals();
                       
        //            }
                    
        //        }
        //    }
        //},
        schema: {
            model: {
                fields: {
                    Operation: { type: "string" },
                    CashMoneyFunds: { type: "number" },
                    FixedIncome: { type: "number" },
                    Equities: { type: "number" },
                    OtherAssets: { type: "number" },
                    MarketValue: { type: "number" },
                    PercentOfTotal: { type: "number" },
                    AnnualIncome: { type: "number" },
                    CurrentYield: { type: "number" },
                }
            }
        }
    },
    dataBound: grid_dataBound
 });

 function grid_dataBound() {
     addRow();
     styleTotals();
 }
    
 function addRow() {
     var grid = $("#accountTable").data("kendoGrid");
     var ds = grid.dataSource;
        
     for (var i = 0 ; i < ds.total() ; i++) {
         ds.at(i).AssetClass = ds.at(i).AssetClass[0] == 'C' ? "1" + ds.at(i).AssetClass :
         ds.at(i).AssetClass[0] == 'F' ? "2" + ds.at(i).AssetClass :
         ds.at(i).AssetClass[0] == 'E' ? "3" + ds.at(i).AssetClass : "4" + ds.at(i).AssetClass;

         $("td:contains('" + ds.at(i).Operation + "')").css("padding-left", "25pt");
        
     }

     // --------margin 
     var marginIndex;
     var marginRow;

     for (i = ds.total() - 1; i >= 0; i--) {

         if (ds.at(i).AssetClass == "4Margin") {
             marginRow = ds.at(i);
             marginIndex = i;
             ds.remove(ds.data()[i]);
             break;
         }
     }
     // ----------------
     
     var totalValue = generateArray(" TOTAL VALUE", 0, ds.total() - 1, null, null);
     var totalPercent = generateArray(" TOTAL PERCENT", 0, ds.total() - 1, null, null);

     var shift = 0;
     var groupOfSubtotals = [];
     var group = ds.at(0).Strategy;
    
     var startIndex = 0;
     var orderOfStrategy = 0;
          
     for (var i = 1 + shift; i < ds.total() ; i++) {
         
         if (group != ds.at(i).Strategy || (i == ds.total() - 1 && i++)) {

             shift++;
             groupOfSubtotals.push(group);
             
             grid.dataSource.insert(startIndex, generateArray(group, startIndex, i - 1,
                 orderOfStrategy++, ds.at(startIndex).AssetClass));

             if (group == ds.at(ds.total() - 1).Strategy)
                 break;

             startIndex = i + 1;
             group = ds.at(i + 1).Strategy;

         }
     }

     if (ds.total() == 1) {
         groupOfSubtotals.push(group);
         grid.dataSource.insert(startIndex, generateArray(group, startIndex, 0,
                 orderOfStrategy++, ds.at(startIndex).AssetClass));
     }

     var groupOfTotals = [];
     orderOfStrategy = 0;
     group = ds.at(0).AssetClass;
     startIndex = 0;
     shift = 0;
     var subTotals = [];

     for (var i = shift; i < ds.total() ; i++) {
         if (ds.at(i).AssetClass == group && ds.at(i).Strategy == orderOfStrategy) {
             orderOfStrategy++;
             subTotals.push(i);
         }
         if (ds.at(i).AssetClass != group || i == ds.total() - 1) {

             groupOfTotals.push(group.substring(1) + " ");
             grid.dataSource.insert(startIndex, generateArrayFromSet(group, subTotals));
             if (group == ds.at(ds.total() - 1).AssetClass)
                 break;

             shift++;
             subTotals = [];
             startIndex = i + 1;
             group = ds.at(i + 1).AssetClass;
         }
     }

     ds.insert(ds.total(), totalValue);

     var coef = 100 / (totalValue.CashMoneyFunds + totalValue.FixedIncome + totalValue.Equities);
     ds.insert(ds.total(), {
         Operation: " TOTAL PERCENT",
         CashMoneyFunds: totalValue.CashMoneyFunds * coef,
         FixedIncome: totalValue.FixedIncome * coef,
         Equities: totalValue.Equities * coef,
         OtherAssets: 0,
         MarketValue: 0,
         PercentOfTotal: 0,
         PercentOfAssetClass: 0,

         AnnualIncome:0,
         CurrentYield: 0,
         Url: "",
         AssetClass: null,
         Strategy: null
     });

     if (marginIndex >= 0)
         addMargin(marginIndex, marginRow);
 }
 function colorArray(groupList, color) {
   
     for (var i = 0; i < groupList.length; i++) {
         $("td:contains('" + groupList[i] + "')") .find('td').each(function () { })
             .css("background", color);

     }
 }
 function generateArray(group, from, to, orderOfStrategy, assetClass) {

     var grid = $("#accountTable").data("kendoGrid");
     var ds = grid.dataSource;

     var CashMoneyFunds = 0;
     var FixedIncome = 0;
     var Equities = 0;
     var OtherAssets = 0;
     var MarketValue = 0;
     var PercentOfTotal = 0;
     var PercentOfAssetClass = 0;

     var AnnualIncome = 0;
     var CurrentYield = 0;


     for (var i = from; i <= to; i++) {
         CashMoneyFunds += ds.at(i).CashMoneyFunds;
         FixedIncome += ds.at(i).FixedIncome;
         Equities += ds.at(i).Equities;
         OtherAssets += ds.at(i).OtherAssets;
         MarketValue += ds.at(i).MarketValue;
         PercentOfTotal += ds.at(i).PercentOfTotal;
         PercentOfAssetClass += ds.at(i).PercentOfAssetClass;

         AnnualIncome += ds.at(i).AnnualIncome;
         CurrentYield += ds.at(i).CurrentYield;
     }
 
     return {
         Operation: group,
         CashMoneyFunds:  CashMoneyFunds,
         FixedIncome: FixedIncome,
         Equities: Equities,
         OtherAssets:  OtherAssets,
         MarketValue:  MarketValue,
         PercentOfTotal:PercentOfTotal,
         PercentOfAssetClass: PercentOfAssetClass,

         AnnualIncome:  AnnualIncome,
         CurrentYield:  CurrentYield,
         Url: "",
         AssetClass: assetClass,
         Strategy: orderOfStrategy
     };
 }
 function generateArrayFromSet(group, set) {
   
     var grid = $("#accountTable").data("kendoGrid");
     var ds = grid.dataSource;

     var CashMoneyFunds = 0;
     var FixedIncome = 0;
     var Equities = 0;
     var OtherAssets = 0;
     var MarketValue = 0;
     var PercentOfTotal = 0;
     var PercentOfAssetClass = 0;
     var AnnualIncome = 0;
     var CurrentYield = 0;


     for (var i = 0; i < set.length; i++) {

         CashMoneyFunds += ds.at(set[i]).CashMoneyFunds;
         FixedIncome += ds.at(set[i]).FixedIncome;
         Equities += ds.at(set[i]).Equities;
         OtherAssets += ds.at(set[i]).OtherAssets;
         MarketValue += ds.at(set[i]).MarketValue;
         PercentOfTotal += ds.at(set[i]).PercentOfTotal;
         PercentOfAssetClass += ds.at(i).PercentOfAssetClass;

         AnnualIncome += ds.at(set[i]).AnnualIncome;
         CurrentYield += ds.at(set[i]).CurrentYield;
     }
     var url;
     if (group[1] == "C") url = "Cash";
     else if (group[1] == "F") url = "Income";
     else if (group[1] == "E") url = "Equities";
     else url = "ViewAccount";
     return {
         Operation: group.substring(1) + ' ',
         CashMoneyFunds: CashMoneyFunds,
         FixedIncome: FixedIncome,
         Equities: Equities,
         OtherAssets: OtherAssets,
         MarketValue: MarketValue,
         PercentOfTotal: PercentOfTotal,
         PercentOfAssetClass: PercentOfAssetClass,

         AnnualIncome: AnnualIncome,
         CurrentYield: CurrentYield,
         Url: url,
     };
 }
 function addMargin(index, row) {
 

     var grid = $("#accountTable").data("kendoGrid");
     var ds = grid.dataSource;

     var marginRow = row;
     ds.insert(ds.total(), row);

     var totalRow = ds.at(ds.total()-2);
     totalRow.Operation = "SUB TOTAL";

     ds.insert(ds.total(), {
         Operation: "TOTAL VALUE",

         CashMoneyFunds: totalRow.CashMoneyFunds - marginRow.CashMoneyFunds,
         FixedIncome: totalRow.FixedIncome-marginRow.FixedIncome,
         Equities: totalRow.Equities-marginRow.Equities,
         OtherAssets: totalRow.OtherAssets-marginRow.OtherAssets,
         MarketValue: totalRow.MarketValue-marginRow.MarketValue,
         PercentOfTotal: totalRow.PercentOfTotal - marginRow.PercentOfTotal,
         PercentOfAssetClass: totalRow.PercentOfAssetClass - marginRow.PercentOfAssetClass,

         AnnualIncome: totalRow.AnnualIncome-marginRow.AnnualIncome,
         CurrentYield: totalRow.CurrentYield-marginRow.CurrentYield,
         Url: ""
     });


     $("tr:contains('TOTAL VALUE')").find('td').each(function () { })
         .css("background-color", "#F6EEE0");

     $("tr:contains('SUB TOTAL')").find('td').each(function () { })
        .css("background-color", "#F6EEE0");

     $("tr:contains('Margin')").find('td').each(function () { })
        .css("background-color", "#F6EEE0");
     //font
     $("tr:contains('TOTAL VALUE')").find('td').each(function () { })
        .css("color", "#163574");

     $("tr:contains('SUB TOTAL')").find('td').each(function () { })
        .css("color", "#163574");

     $("tr:contains('Margin')").find('td').each(function () { })
        .css("color", "#163574");
   
 }
 function styleTotals()
 {
     var grid = $("#accountTable").data("kendoGrid");
     var ds = grid.dataSource;

     $("td:contains('" + ds.at(ds.total() - 2).Operation.substring(1) + "')").parent().children().each(function (a, b) {        
         if ((a >= 1 && a <= 5) || a == 9 || a == 8)
             $(b).text("$" + $(b).text());
         if (a == 6 || a == 7)
             $(b).text($(b).text() + "%");
     });
     $("td:contains('" + ds.at(ds.total() - 1).Operation.substring(1) + "')").parent().children().each(function (a, b) {       
         if (a >= 1 && a <= 3)
             $(b).text($(b).text() + "%");
        
         if (a == 6 || a == 7)
             $(b).text("-");
     })

     $("tr").css("background-color", "#dfd4bf");
     
     $("td:contains('" + ds.at(ds.total() - 2).Operation.substring(1) + "')").parent().css({ "background": "#f6eee0", "color": "#163574" });
     $("td:contains('" + ds.at(ds.total() - 1).Operation.substring(1) + "')").parent().css({ "background": "#f6eee0", "color": "#163574" });

     var account =  $("div#accountTable td:contains('" + $("#fullName").val() + "')").first().text();

     $("div#accountTable tr").each(function(index, elem){
         var firstCell = $(elem).children().eq(0).text();
         if (index > 1 && !firstCell.includes(account) && !firstCell.includes('TOTAL')) {
             if (index != 2 && $("div#accountTable tr").eq(index + 1).children().first().text().includes(account)) {

                 $(elem).css("color", "#163574");
             }
             if (index == 2 || !$("div#accountTable tr").eq(index + 1).children().first().text().includes(account)) {

                 $(elem).children().first().css("color", "#163574");
                 $(elem).css("background-color", "#e9dfcd");
             }
         }
     });

     $("div#accountTable td:contains('" + $("#fullName").val() + "')").css("padding-left", "25px");
     $("div.k-grid-content.km-widget.km-scroll-wrapper").css("max-height", "400px");

     $("td:contains('0.00')").each(function () {
         $(this).text("-");
     });
     
     if ($("tr:contains('TOTAL PERCENT')").children().eq($(".k-grid-header thead th:contains('CASH')").index()).text() != "-") 
     {
         var cashhref = '/Client/Holdings/Cash?isGroup=' + $("#isGroup").val() + '&isClientGroup=' + $("#isClientGroup").val()
             + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val();
         $(".k-grid-header thead th:contains('CASH')").wrapInner("<a href='" + cashhref + "'><a/>");
     }

     if ($("tr:contains('TOTAL PERCENT')").children().eq($(".k-grid-header thead th:contains('FIXED')").index()).text() != "-")
     {
         var incomehref = '/Client/Holdings/Income?isGroup=' + $("#isGroup").val() + '&isClientGroup=' + $("#isClientGroup").val()
             + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val();
         $(".k-grid-header thead th:contains('FIXED')").wrapInner("<a href='" + incomehref + "'><a/>");
     }

     if ($("tr:contains('TOTAL PERCENT')").children().eq($(".k-grid-header thead th:contains('EQUITIES')").index()).text() != "-") 
     {
         var equitieshref = '/Client/Holdings/Equities?isGroup=' + $("#isGroup").val() + '&isClientGroup=' + $("#isClientGroup").val()
             + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val();
         $(".k-grid-header thead th:contains('EQUITIES')").wrapInner("<a href='" + equitieshref + "'><a/>");
     }

 }
});
function getPDF(selector) {
    kendo.drawing.drawDOM($(selector)).then(function (group) {

        kendo.drawing.pdf.saveAs(group, "Invoice.pdf");
    });
}
function getExcel() {

    var a = $("#accountTable").getKendoGrid();
    
    for (var i = 0; i < a.columns.length; i++) {
        if (typeof a.columns[i].title != "undefined") {
            if (typeof a.columns[i].columns != "undefined") {
                for (var j = 0; j < a.columns[i].columns.length; j++) {
                    a.columns[i].columns[j].title = a.columns[i].columns[j].title
                                                    .replace("<br/>", " ").replace("<br/>", " ")
                                                    .replace("<br>", " ").replace("<br>", " ")
                                                    .replace("</br>", " ").replace("</br>", " ");
                }
            }
            a.columns[i].title = a.columns[i].title
                                           .replace("<br/>", " ").replace("<br/>", " ")
                                           .replace("<br>", " ").replace("<br>", " ")
                                           .replace("</br>", " ").replace("</br>", " ");
        }
    }

    a.saveAsExcel();
}

