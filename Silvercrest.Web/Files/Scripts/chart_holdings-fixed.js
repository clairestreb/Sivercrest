
google.charts.load("current", {packages:["corechart"]});
google.charts.setOnLoadCallback(drawChart);
function drawChart() {
  var data = google.visualization.arrayToDataTable([
    ["ValueName", "ValuePercent", { role: "style" } ],
    ["Under 1 Year", 7.2, "color:#65071d"],
    ["1 - 3 Years", 27.2, "color:#65071d"],
    ["3 - 5 Years", 28.9, "color:#65071d"],
    ["5 - 7 Years", 10.2, "color:#65071d"],
    ["7 - 10 Years", 18.9, "color:#65071d"],
    ["Over 10 Years", 7.7, "color:#65071d"]
  ]);


  var data2 = google.visualization.arrayToDataTable([
    ["ValueName", "ValuePercent", { role: "style" } ],
    ["Under 1 Year", 0.75, "color:#163574"],
    ["1 - 3 Years", 1.49, "color:#163574"],
    ["3 - 5 Years", 1.61, "color:#163574"],
    ["5 - 7 Years", 3.26, "color:#163574"],
    ["7 - 10 Years", 1.96, "color:#163574"],
    ["Over 10 Years", 2.45, "color:#163574"]
  ]);


  var view = new google.visualization.DataView(data);
  view.setColumns([0, 1,
                   { calc: "stringify",
                     sourceColumn: 1,
                     type: "string",
                     role: "annotation" },
                   2]);

  var view2 = new google.visualization.DataView(data2);
  view2.setColumns([0, 1,
                   { calc: "stringify",
                     sourceColumn: 1,
                     type: "string",
                     role: "annotation" },
                   2]);

  var options = {
    title: "VALUE AS A PERCENT OF FIXED INCOME",
    titleTextStyle: {fontSize: 14, auraColor: 'none', color: '#163574', fontName: 'copperplate' },
    width: 518,
    height: 380,
    fontName: 'goudy',
    fontSize: 13,
    backgroundColor: { fill:'transparent' },
    bar: {groupWidth: "60%"},
    legend: { position: "none" },
    lineWidth: 10,
    annotations: { alwaysOutside: true,
      textStyle: {fontSize: 13, auraColor: 'none', color: '#000', fontName: 'goudyosSC'},
      stemColor : 'none'
    },
    hAxis: {
      textPosition: 'none',
      gridlines: {color: 'transparent'}
    },
  }

  var options2 = {
    title: "YEILD TO MATURITY AT MARKET VALUE",
    titleTextStyle: {fontSize: 14, auraColor: 'none', color: '#163574', fontName: 'copperplate' },
    width: 518,
    height: 380,
    fontName: 'goudy',
    fontSize: 13,
    backgroundColor: { fill:'transparent' },
    bar: {groupWidth: "60%"},
    legend: { position: "none" },
    lineWidth: 10,
    annotations: { alwaysOutside: true,
      textStyle: {fontSize: 13, auraColor: 'none', color: '#000', fontName: 'goudyosSC'},
      stemColor : 'none'
    },
    hAxis: {
      textPosition: 'none',
      gridlines: {color: 'transparent'}
    },
  }


  function getValueAt(column, dataTable, row) {
        return dataTable.getFormattedValue(row, column);
      }

var chart = new google.visualization.BarChart(document.getElementById("barchart_values"));
chart.draw(view, options);
var chart2 = new google.visualization.BarChart(document.getElementById("barchart_values2"));
chart2.draw(view2, options2);
};
