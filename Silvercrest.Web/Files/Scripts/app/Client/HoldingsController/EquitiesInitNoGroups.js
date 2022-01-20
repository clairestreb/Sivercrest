$(document).ready(function () {

	kendo.pdf.defineFont({
		"DejaVu Sans": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans.ttf",
		"DejaVu Sans|Bold": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Bold.ttf",
		"DejaVu Sans|Bold|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf",
		"DejaVu Sans|Italic": "//kendo.cdn.telerik.com/2016.2.607/styles/fonts/DejaVu/DejaVuSans-Oblique.ttf"
	});

	window.calculateWeightedAverage = function () {
	    var data = model.IncomeData.Data;
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

	$("#equitiesNoGroupTable").kendoGrid({
		pdf: {
			allPages: true
		},
		scrollable: true,
		excel: { fileName: "EquitiesHoldings-Ungrouped.xlsx" },
		excelExport: function (e) {
			var sheet = e.workbook.sheets[0];

			for (var i = 0; i < sheet.rows.length; i++) {
				var row = sheet.rows[i];
				if (row.type == "header")
					for (var j = 0; j < row.cells.length; j++) {
					    var cell = row.cells[j];
					    cell.hAlign = "center";
						if (cell.value == "DATE")
							for (var ro = i + 1; ro < sheet.rows.length; ro++) {
								sheet.rows[ro].cells[j + 5].value = kendo.toString(kendo.parseDate(sheet.rows[ro].cells[j + 5].value, 'yyyy-MM-dd'), 'MM/dd/yyyy');
							}
					}
				if (row.type == "group-footer" || row.type == "footer" || row.type == "header") {
					for (var ci = 0; ci < row.cells.length; ci++) {
						var cell = row.cells[ci];
						if (cell.value) {
						    if (cell.value.indexOf("<br>") !== -1 || cell.value.indexOf("<br/>") !== -1 || cell.value.indexOf("</br>") !== -1) {
                                cell.value = cell.value.replace(/<br>/g, " ");
                                cell.value = cell.value.replace(/<br\/>/g, " ");
                                cell.value = cell.value.replace(/<\/br>/g, " ");
                            }
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
              	field: "Category", title: "ASSET<br/>TYPE", width: 100,
              	attributes: { style: "font-size: 75% !important; text-align: left;", "class": "col2" },
              	headerAttributes: { style: "font-size: 75% !important; text-align: center;" },
              	sortable: {
              	    allowUnsort: false, mode: "single"
              	},
                footerTemplate: "TOTAL"

              },
            {
            	field: "SubCategory", title: "SECTOR", width: 100,
            	attributes: { style: "font-size: 75% !important; text-align: left;", "class": "col2" },
            	headerAttributes: { style: "font-size: 75% !important; text-align: center;" },
            	sortable: {
            	    allowUnsort: false, mode: "single"
            	}
            },
            {
            	field: "Holdings", title: "HOLDING", width: 150,
            	template: function (data) {
            		var division = data.Holdings.indexOf("\n", 0);
            		var firstStr = data.Holdings.substring(0, division);
            		var secondStr = data.Holdings.substring(division + 1, data.Holdings.length);
            		return firstStr + "</br>" + secondStr;
            	}, attributes: { style: "font-size: 75% !important; text-align: left;", "class": "col2" }
                 , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " }
                 , footerAttributes: { style: "font-size: 75% !important" },
            	sortable: {
            	    allowUnsort: false, mode: "single", 
            	}
            },
            {
            	field: "Symbol", title: "SYMBOL", width: 50,
            	attributes: { style: "font-size: 75% !important; text-align: center;", "class": "col2" },
            	headerAttributes: { style: "font-size: 75% !important; text-align: center;" },
            	template: kendo.template(" #if(NumberLots > 1){# <a href='' onclick='tableBound(#= kendo.parseInt(kendo.toString(SecurityId,'n0'))#)' class='red underline' data-toggle='modal' data-target='\\#myModal'>#= kendo.toString(Symbol,'n0' )#</a>#} else{# #= kendo.toString(Symbol,'n0' )# #}#"),
            	sortable: {
            		allowUnsort: false, mode: "single"
            	}
            },
            {
            	field: "Quantity", title: "QUANTITY", width: 70,
            	template: "<div align='center'>#= kendo.toString(Quantity,'n0' )#</div>"
                 , attributes: { style: "font-size: 75% !important", "class": "col2" }
                 , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " },
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
                	//template: "#= kendo.toString(kendo.parseDate(AdjustedCostDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                      attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important; text-align: center;  border-left-width: 2px !important" },
                	sortable: {
                		allowUnsort: false, mode: "single"
                	}

                },
                {
                	field: "AdjustedCostUnit", title: "UNIT", width: 50,
                	template: "<div align='center'>#= kendo.toString( AdjustedCostUnit,'n2' )#</div>"
                    , attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " },
                	sortable: {
                		allowUnsort: false, mode: "single"
                	}

                },
                {
                	field: "AdjustedCostTotal", title: "TOTAL", width: 70,
                	template: "<div align='center'>#= kendo.toString( AdjustedCostTotal,'n0' )#</div>",
                	attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " }
                    , footerAttributes: { style: "font-size: 75% !important" },
                	footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                	sortable: {
                		allowUnsort: false, mode: "single"
                	}

                }], headerAttributes: { style: "font-size: 75% !important; text-align: center; cursor: default;" }
            },
            {
            	title: "MARKET VALUE",
            	columns: [
                {
                	field: "MarketValueUnits", title: "UNIT", width: 70,
                	template: "<div align='center'>#= kendo.toString( MarketValueUnits,'n2' )#</div>"
                    , attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important; text-align: center;  border-left-width: 2px !important" },
                	sortable: {
                		allowUnsort: false, mode: "single"
                	}

                },
                {
                	field: "MarketValueTotal", title: "TOTAL", width: 70,
                	template: "<div align='center'>#= kendo.toString( MarketValueTotal,'n0' )#</div>"
                    , attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " }
                    , footerAttributes: { style: "font-size: 75% !important; text-align: center; " },
                	footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                	sortable: {
                		allowUnsort: false, mode: "single"
                	}
                }], headerAttributes: { style: "font-size: 75% !important; text-align: center; cursor: default;" }
            },
            {
                field: "MarketValuePercentOfAssets", title: "% OF<br/>ASSET<br/>CLASS", width: 70,
            	template: "<div align='center'>#= kendo.toString( MarketValuePercentOfAssets,'n1' )#</div>"
                                , attributes: { style: "font-size: 75% !important", "class": "col2" }
                , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " }
                , footerAttributes: { style: "font-size: 75% !important" },
            	footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n1') #</div>",
            	sortable: {
            		allowUnsort: false, mode: "single"
            	}
            },
/*
            {
                field: "AccuredInterest", title: "ACCRUED<br/>INTEREST", width: 70,
                template: "<div align='center'>#= kendo.toString( AccuredInterest,'n0' )#</div>"
                                , attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " }
                    , footerAttributes: { style: "font-size: 75% !important" },
                footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
                sortable: {
                    allowUnsort: false, mode: "single"
                }
            },
 */
            {
            	field: "AnnualIncome", title: "EST.<br/>ANNUAL<br/>INCOME", width: 70,
            	template: "<div align='center'>#= kendo.toString( AnnualIncome,'n0' )#</div>"
                                , attributes: { style: "font-size: 75% !important", "class": "col2" }
                    , headerAttributes: { style: "font-size: 75% !important ; text-align: center;  " }
                    , footerAttributes: { style: "font-size: 75% !important" },
            	footerTemplate: "<div align='center'>#= kendo.toString(sum, 'n0') #</div>",
            	sortable: {
            		allowUnsort: false, mode: "single"
            	}
            },
            {
            	field: "CurrentYield", title: "CURRENT<br/>YIELD", width: 70,
            	template: "<div align='center'>#= kendo.toString( CurrentYield,'n1' )#</div>"
                , attributes: { style: "font-size: 75% !important", "class": "col2" }
                , headerAttributes: { style: "font-size: 75% !important; text-align: center;  " }
                , footerAttributes: { style: "font-size: 75% !important" },
            	footerTemplate: "<div align='center'>#= kendo.toString(window.calculateWeightedAverage(), 'n1') #</div>",
            	sortable: {
            		allowUnsort: false, mode: "single"
            	}
            }



		],
		dataSource: {
			data: model.IncomeData.Data,
			//transport: {
			//    read: {
			//        type: "GET",
			//        url: '/Client/Holdings/GetEquities?isGroup=' + $("#isGroup").val() + '&isClientGroup=' + $("#isClientGroup").val()
			//        + '&contactId=' + $("#contactId").val() + '&entityId=' + $("#entityId").val(),
			//        dataType: "json",
			//        success: function (data) {
			//            sortnogroups();
			//        }
			//    },
			schema: {
				model: {
					fields: {
						AdjustedCostDate: {
							type: "date"
						},
						AdjustedCostTotal: {
							type: "number"
						},
						MarketValueTotal: {
							type: "number"
						},
						MarketValuePercentOfAssets: {
							type: "number"
						},
						AnnualIncome: {
							type: "number"
						},
						CurrentYield: {
							type: "number"
						},
						Category: {
							type: "string"
						},
						SubCategory: {
							type: "string"
						},
					}
				}
			},

			aggregate: [
               { field: "AdjustedCostTotal", aggregate: "sum" },
               { field: "MarketValueTotal", aggregate: "sum" },
               { field: "MarketValuePercentOfAssets", aggregate: "sum" },
               { field: "AnnualIncome", aggregate: "sum" },
               { field: "CurrentYield", aggregate: "sum" },
			],
			sort: {
			    field: "MarketValueTotal",
			    dir: "desc"
			},

		},



		dataBound: grid_dataBound
    //function onDataBound(arg) {
    //	sortnogroups();
    //}

	});

	var grid = $("#equitiesNoGroupTable").data("kendoGrid");
	var ds = grid.dataSource;
	grid.bind("dataBound", grid_dataBound);

	function grid_dataBound(e) {
	    sortnogroups();
		var rows = e.sender.tbody.children();
		for (var j = 0; j < rows.length; j++) {
			var row = $(rows[j]);
			row.addClass("row black tab");
		}

	}

    function sortnogroups() {
		$("#equitiesNoGroupTable").find("th:has(a.k-link)").css("text-align", "center");
		$("#equitiesNoGroupTable").find("th:has(a.k-link)").css("vertical-align", "top");
		$("#equitiesNoGroupTable").find("a.k-link").css("color", "#163574");
		$("#equitiesNoGroupTable").find("th:has(a.k-link)").bind("click", onClickSort2);

 //		$("#equitiesNoGroupTable").find(".k-link").bind("click", onClickSort2);
//		("th:has(a.k-link)").click(onClickSort2);

		$('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
		$('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
		$('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");

		$("#equitiesNoGroupTable a.k-link").removeClass("k-link");
	}


	function onClickSort2(e) {
		var target = $(this);

		//remove from another
		$("#equitiesNoGroupTable th.k-header").children().each(function (index, elem) {
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
	}
    

    // To put arrow below text when page Loads
	var flatSortedColumn = $("#equitiesNoGroupTable .k-grid-header tr th[data-field='MarketValueTotal']");
	$('<span class="arrow-down" style="display:block"></span>').appendTo(flatSortedColumn);


});

	function getPDF() {
		var grid = $("#equitiesNoGroupTable").data("kendoGrid");
		grid.saveAsPDF();
	}
	function getExcel() {
		var a = $("#equitiesNoGroupTable").getKendoGrid();

		//for (var i = 0; i < a.columns.length; i++) {
		//    if (typeof a.columns[i].columns != "undefined") {
		//        for (var j = 0; j < a.columns[i].columns.length; j++) {
		//            a.columns[i].columns[j].title = a.columns[i].columns[j].title.replace("<br/>", " ");
		//            a.columns[i].columns[j].title = a.columns[i].columns[j].title.replace("<br>", " ");
		//        }
		//    }
		//    a.columns[i].title = a.columns[i].title.replace("<br/>", " ");
		//    a.columns[i].title = a.columns[i].title.replace("<br>", " ");
		//}

		a.saveAsExcel();
	}

	function savePDF() {
	    if ($("#switch").text() === "Ungroup")
	        getPDFGroups();
	    else
	        getPDF();
	}

	function saveExcel() {
	    if ($("#switch").text() === "Ungroup")
	        getExcelGroups();
	    else
	        getExcel();
	}
