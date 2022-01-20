$(document).ready(function(){
      google.charts.load('current', {'packages':['corechart']});
      google.charts.setOnLoadCallback(drawChart);
      function drawChart() {


        var data = google.visualization.arrayToDataTable([
          ['Allocation', 'Percent'],
          ['Equitieskkkk', 86.9],
          ['Fixed Income', 10.8],
          ['Cash & Money Funds', 2.3]
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
        };
        
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
});