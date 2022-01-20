

kendo.pdf.defineFont({
	"DejaVu Sans"             : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
	"DejaVu Sans|Bold"        : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
	"DejaVu Sans|Bold|Italic" : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
	"DejaVu Sans|Italic"      : "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
});

window.calculateWeightedAverage = function () {
	var data = model.TableData.Data;
	var total = 0;
	var compositeSum = 0.0;
	for (var i = 0; i < data.length; i++) {
	    //Only Taking Positive Values in Denominator
	    if (data[i].PercentOfTotal >= 0) {
	        total += data[i].MarketValue;
	    }
	    //Numerator should be all and have a negative value for the Margin value which is why we have the Math.abs since current yield is negative
	    compositeSum += Math.abs(data[i].MarketValue) * data[i].CurrentYield;

	}
	return compositeSum / total;
};

window.calculateWeightedAverageExMargin = function () {
    var data = model.TableData.Data;
    var total = 0;
    var compositeSum = 0.0;
    for (var i = 0; i < data.length; i++) {
        if (data[i].PercentOfTotal >= 0) 
        {
            total += data[i].MarketValue;
            compositeSum += data[i].MarketValue * data[i].CurrentYield;
        }

    }
    return compositeSum / total;
};


$("#accountTable").kendoGrid({
    dataSource: {
        data: model.TableData.Data,
        group: {
            field: "AccountType", aggregates: [
          { field: "MarketValue", aggregate: "sum" },
          { field: "PercentOfTotal", aggregate: "sum" },
          { field: "AnnualIncome", aggregate: "sum" },
          { field: "CurrentYield", aggregate: "sum" }
            ]
        },
    },
    scrollable: true,
    excel: {fileName: "AssetAllocation.xlsx"},
	excelExport: function (e) {
	    var sheet = e.workbook.sheets[0];
	    sheet.rows.splice(2, 1);
		for (var i = 0; i < sheet.rows.length; i++) {
		    var row = sheet.rows[i];
			if (row.type == "header")
				for (var j = 0; j < row.cells.length; j++) {
					var cell = row.cells[j];
					if (cell.value == "Date")
					    for (var ro = i + 1; ro < sheet.rows.length; ro++) {
					        sheet.rows[ro].cells[j].format = "MM/dd/yyyy";
					    }
					if (cell.value == "% of total")
					    for (var ro = i + 1; ro < sheet.rows.length; ro++) {
					        sheet.rows[ro].cells[j+1].format = "#.#%";
					    }
					if (cell.value == "Market Value")
					    cell.hAlign = "center";
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
	sortable: false,
	columns: [
		{
			field: "AssetClass", title: "ASSET CLASS",
			template: function (data) {
				if (data.AssetClass[0] == 'C')
				{
					data.Url = "Cash";
				}
				else if (data.AssetClass[0] == 'E')
				{
					data.Url = "Equities";
				}
				else if (data.AssetClass[0] == 'F')
				{
					data.Url = "Income";
				}
				else if (data.AssetClass[0] == 'O')
				{
					data.Url = "OtherAssets";
				}
                var creds = "isGroupQuery=" + $("#isGroup").val() + "&isClientGroupQuery=" + $("#isClientGroup").val()
                    + "&contactIdQuery=" + $("#contactId").val() + "&entityIdQuery=" + $("#entityId").val();

				return '<a href ="/Client/Holdings/' + data.Url + '?' + creds + ' ">' + data.AssetClass + '</a>';
			},
			groupFooterTemplate: "TOTAL",
			width: 300,
			headerAttributes: { style: "text-align: center; cursor:default;" },
			attributes: { "class": "col1" }
		},
		{
			field: "Market Value", columns: [

			{
				field: "MarketValue", title: "Amount",
				template: "#=kendo.toString(MarketValue, 'n0')#",
				attributes: { style: "text-align: center", "class": "col2" },
				groupFooterTemplate: "#= kendo.toString( sum,'n0' )#",
				footerAttributes: { style: "text-align: center" }, width: 160,
				headerAttributes: { style: "text-align: center; border-left-width: 2px !important; cursor:default;" }, dir: "desc",

			},
		   {
		   	field: "PercentOfTotal", title: "% of total",
		   	template: "#=kendo.toString(PercentOfTotal*100, 'n1')#",
		   	attributes: { style: "text-align: center", "class": "col2" },
		   	groupFooterTemplate: "#= kendo.toString( sum*100 ,'n1' )#",
		   	footerAttributes: { style: "text-align: center" }, width: 160,
		   	headerAttributes: { style: "text-align: center; cursor:default;" },

		   }
			]
			, headerAttributes: { style: "text-align: center; cursor:default;" }
		},

		{
			field: "AnnualIncome", title: "Est. Annual income",
			template: "#=kendo.toString(AnnualIncome, 'n0')#",
			attributes: { style: "text-align: center", "class": "col2" },
			groupFooterTemplate: "#= kendo.toString( sum,'n0' )#",
			footerAttributes: { style: "text-align: center" }, width: 160,
			headerAttributes: { style: "text-align: center; cursor:default;" },

		},
		{
			field: "CurrentYield", title: "Current Yield",
			template: "#=kendo.toString(CurrentYield, 'n1')#",
			attributes: { style: "text-align: center", "class": "col2" },
			groupFooterTemplate: "#= kendo.toString(window.calculateWeightedAverage(),'n1' )#",
			footerAttributes: { style: "text-align: center" }, width: 160,
			headerAttributes: { style: "text-align: center; cursor:default;" },


		}
	],
	dataBound: grid_dataBound

});
var grid = $("#accountTable").data("kendoGrid");
var ds = grid.dataSource;
grid.bind("dataBound", grid_dataBound);


function grid_dataBound(e) {
	sort();
	$(".k-grouping-row").empty();
	if ($("tr:contains('Margin')").length > 0) {
		addrow();
	}
	$(".k-group-col,.k-group-cell").remove();
	var spanCells = $(".k-grouping-row").children("td");
	spanCells.attr("colspan", spanCells.attr("colspan") - 1);

	$(".k-group-footer").find('td').each(function () { })
		  .css("background-color", "#F6EEE0");

	$(".k-group-footer").find('td').each(function () { })
	   .css("color", "#163574");

	//This is to add !important otherwise the !important from css file overrides
	$("tr:contains('Margin')").find("td").each(function () {
		this.style.setProperty('font-family', 'goudyosSC', 'important');
		this.style.setProperty('text-transform', 'none', 'important');
		this.style.setProperty('font-size', '14px', 'important');
		this.style.setProperty('color', '#333', 'important');
		this.style.setProperty('background', '#dfd4bf', 'important');
	});
	//            .css({ "font-family": "goudyosSC !important", "text-transform": "none !important", "font-size": "14px !important", "color": "#333 !important", "background": "#dfd4bf !important" });

	$("td:contains('Margin')").each(function () {
		this.style.setProperty('padding-left', '2%', 'important');
	});

	//        .css({ "padding-left": "2% !important" });

	//        marginCells.attr("font-family", "goudyosSC");
	//        font-family: goudyosSC!important;
	//        text-transform: none!important;
	//        color:#333!important;
	//        font-size:14px;

	var rows = e.sender.tbody.children();
	for (var j = 0; j < rows.length; j++) {
		var row = $(rows[j]);

		row.addClass("row red link tab");
	}

}
function addrow() {
	console.log("addrow");
	//margin   
	$("tr:contains('TOTAL')").clone().insertBefore($("tr:contains('TOTAL')"));

	$("tr:contains('TOTAL')").first().children().each(function (index) {
		var val;
		if( $("tr:contains('Margin')").children().eq(0).text().includes("Margin"))
			val = $("tr:contains('Margin')").children().eq(index + 2).text();
		else
			val = $("tr:contains('Margin')").children().eq(index).text();

		$(this).text(val);
	});

	//subtotal
	$("tr:contains('TOTAL')").clone().insertBefore($("tr:contains('Margin')").last());
	$("tr:contains('TOTAL')").first().children().each(function (index) {
		if (index > 0) {
			var num1 = $("tr:contains('TOTAL')").last().children().eq(index).text().replace("$", "").replace("%", "").replace(",", "").replace(",", "").replace(",", "");
			var num2 = $("tr:contains('Margin')").last().children().eq(index).text().replace("$", "").replace("%", "").replace(",", "").replace(",", "").replace(",", "");


			if (index == 3)
			{
			    $(this).text((parseFloat(num1) - parseFloat(num2)).toFixed(1).replace(/\B(?=(\d{3})+(?!\d))/g, ","));
			}
			else if (index == 5)
			{
                //Current Yield
			    $(this).text(window.calculateWeightedAverageExMargin().toFixed(1).replace(/\B(?=(\d{3})+(?!\d))/g, ","));
			}
			else
			{
				$(this).text((parseFloat(num1) - parseFloat(num2)).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ","));
			}
		}
	});
	$("td:contains('NaN')").text("SUB TOTAL");

	//other
	while ($("td:contains('Margin')").length > 1)
		$("td:contains('Margin')").first().parent().empty();


}
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

function onClickSort (e) {
	$("td:contains('Total')").parent().addClass("k-group-footer");
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
			$("#sorting").val("asc");
		}
		if (dir == "desc") {
			set.eq(3).css("display", "block");
			$("#sorting").val("desc");
		}
		if (dir != "asc" && dir != "desc") {
			set.eq(1).css("display", "block");
			$("#sorting").val("none");
		}

	});
	$("td:contains('Total')").parent().addClass("k-group-footer");

}
function getPDF(selector) {
	$("a").css("text-decoration", "none");

	var grid = $("#accountTable").data("kendoGrid");
	grid.saveAsPDF()
    .done(function () {
    	$("td.col1").children().css("text-decoration", "underline");
    })
}
function getExcel() {
	$("#accountTable").getKendoGrid().saveAsExcel();
}
function Encrypt(value) {
    alert("Encrypting in ViewAccountInitShort.js");
    var s;
    $.ajax({
        url: '/Home/Encrypt?input=' + value,
        async: false,
        type: 'GET',
        dataType: "text",
        success: function (result) {
            s = result;
        },
        error: function (result) {
            return "";
        }
    });
    return s;
}
function Decrypt(value) {
    $.ajax({
        url: '/Home/Decrypt?input=' + value,
        type: 'GET',
        dataType: "text",
        success: function (result) {
            return result;
        }
    });
}