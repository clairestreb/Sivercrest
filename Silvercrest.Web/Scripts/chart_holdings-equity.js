$(document).ready(function(){
  google.charts.load('current', {'packages':['corechart']});
  google.charts.setOnLoadCallback(drawPieChart);
  function drawPieChart() {


    var data = google.visualization.arrayToDataTable([
      ['Allocation', 'Percent'],
      ['Common Stock', 49.2],
      ['Capital Market Funds - Equity', 25.4],
      ['Equity-Mutual Funds', 16.6],
      ['Equity-etf, Closed End', 8.7]
    ]);


    var options = {
      backgroundColor: 'transparent',
      height: '100%',
      width: '100%',
      colors:['#892134','#163574','#7589b4','#b3b3b3','#d9d9d9'],
      pieSliceText: 'none',
      chartArea: {width: '100%', height: '100%', top: '20', bottom: '10'},
      legend: 'none',
      titlePosition: 'none',
    };

    var chart = new google.visualization.PieChart(document.getElementById('piechart'));

    chart.draw(data, options);


//  custom legend begin  //

    var total = 0;
    for (var i = 0; i < data.getNumberOfRows(); i++) {
        total += data.getValue(i, 1);
    }
    
    var legend = document.getElementById("legend");
    var legItem = [];
    var colors = ['#000000'];
    for (var i = 0; i < data.getNumberOfRows(); i++) {
        var label = data.getValue(i, 0);
        var value = data.getValue(i, 1);
        var percent = Number(100 * value / total).toFixed(1);

        // This will create legend list for the display
        legItem[i] = document.createElement('li');
        legItem[i].id = 'legend_' + data.getValue(i, 0);
        legItem[i].innerHTML = '<div class="legendMarker"> <span style="color:' + colors[i] + ';">' + label + ': ' + value + '%</span></div>';

        legend.appendChild(legItem[i]);
    };


//  custom legend end  //

  };








  google.charts.setOnLoadCallback(drawBarGraph);
  function drawBarGraph() {

    var data = google.visualization.arrayToDataTable([
      ["ValueName", "ValuePercent", { role: "style" } ],
      ["Financial Services", 9.7, "color:#163574"],
      ["Technology", 9.2, "color:#163574"],
      ["Producer Durables", 7.2, "color:#163574"],
      ["Consumer Disc. & Svcs.", 6.8, "color:#163574"],
      ["Health Care", 4.9, "color:#163574"],
    ]);




    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
                     { calc: "stringify",
                       sourceColumn: 1,
                       type: "string",
                       role: "annotation" },
                     2]);


    var options = {
      title: "TOP 5 COMMON STOCK SECTORS",
      titleTextStyle: {fontSize: 15.5, auraColor: 'none', color: '#163574', fontName: 'copperplate', fontweight: 'normal' },
      width: 518,
      height: 380,
      fontName: 'goudy',
      fontSize: 13,
      backgroundColor: { fill:'transparent' },
      bar: {groupWidth: "60%"},
      legend: { position: "none"},
      lineWidth: 10,
      annotations: { alwaysOutside: true,
        textStyle: {fontSize: 13, auraColor: 'none', color: '#000', fontName: 'goudyosSC'},
        stemColor : 'none'
      },
      hAxis: {
        textPosition: 'none',
        gridlines: {color: 'transparent'}
      },
      chartArea: {left:150}
    }



    function getValueAt(column, dataTable, row) {
          return dataTable.getFormattedValue(row, column);
        }



  var chart = new google.visualization.BarChart(document.getElementById("barchart_values2"));
  chart.draw(view, options);
  };
});