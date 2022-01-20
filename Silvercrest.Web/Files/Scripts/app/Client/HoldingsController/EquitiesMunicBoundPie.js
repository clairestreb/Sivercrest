

$(document).ready(function () {

    $(function () {
        $.ajax({
            type: 'get',
            dataType: 'json',
            contentType: 'application/json',
            url: '/Client/Holdings/GetEquitiesChartMunicBound',
            data: {
                contactId: $("#contactId").val(),
                isGroup: $("#isGroup").val(),
                isClientGroup: $("#isClientGroup").val(),
                entityId: $("#entityId").val()
            },
            success: function (chartsdata) {
                google.charts.load('current', { 'packages': ['corechart'] });

                google.charts.setOnLoadCallback(drawChart);

                function drawChart() {
                    var data = new google.visualization.DataTable();

                   

                    data.addColumn('string', 'Type');
                    data.addColumn('number', 'Value');
                    data.addColumn('number', 'Percent');
                    data.addColumn({ type: 'string', role: 'tooltip'});

                    for (var i = 0; i < chartsdata.length; i++) {
                        var legendName = chartsdata[i].LegendName;
                        if (chartsdata[i].LegendName.length >= 20) {
                            legendName = chartsdata[i].LegendName.substring(0, 17) + "...";
                        }
                        var tooltipArguments = chartsdata[i].LegendName + " " + Number(chartsdata[i].MarketValue).toLocaleString() + " (" + Number(100 * chartsdata[i].Percent).toFixed(2) + "%)";
                        data.addRow([legendName, chartsdata[i].MarketValue, chartsdata[i].Percent, tooltipArguments]);
                    }

                    var options = {
                        backgroundColor: 'transparent',
                        height: '100%',
                        width: '100%',
                        colors: ['#892134', '#163574', '#7589b4', '#b3b3b3', '#d9d9d9', '#a16985', '#b99c6b', '#d5c4a1', '#7F7F7F','#afbbd3'],
                        pieSliceText: 'none',
                        chartArea: { width: '100%', height: '100%', top: '20', bottom: '15' },
                        legend: 'none',
                        slices: {},
                        titlePosition: 'none',
                        tooltip: { trigger: 'selection', isHtml: false },
                    };

                    function onSelectAction() {
                        if (chart.getSelection().length) {
                            var row = chart.getSelection()[0].row;
                        }
                        else {
                            row = chart.selectedSlice;
                        }
                        if (row >= 0) {
                            if (chart.selectedSlice == row) {
                                options.slices[row] = { offset: '.0' };
                                chart.selectedSlice = -1;
                                chart.draw(data, options);
                            }
                            else {
                                if (chart.selectedSlice != -1) {
                                    options.slices[chart.selectedSlice] = { offset: '.0' };
                                }
                                options.slices[row] = { offset: '.1' };
                                chart.selectedSlice = row;
                                chart.draw(data, options);
                                chart.setSelection([{ row: row, column: null }]);
                            }
                        }
                    };

                    var chart = new google.visualization.PieChart(document.getElementById('piechart2'));

                    google.visualization.events.addListener(chart, 'select', onSelectAction);

                    chart.draw(data, options);
                    var total = 0;
                    for (var i = 0; i < data.getNumberOfRows() ; i++) {
                        total += data.getValue(i, 1);
                    };
                    var legend = document.getElementById("legend2");
                    var legItem = [];
                    var colors = ['#000000'];
                    for (var i = 0; i < data.getNumberOfRows() ; i++) {
                        var label = data.getValue(i, 0);
                        var value = Math.round(data.getValue(i, 2) * 100) / 100;
                        var percent = Number(100 * value / total).toFixed(1);
                        if (value > 0) {
                        legItem[i] = document.createElement('li');
                        legItem[i].id = 'legend_' + data.getValue(i, 0);
                        legItem[i].innerHTML = '<div class="legendMarker"> <span class ="textValue"  style ="color:' + colors[i] + ';float:left;">' + label + ': ' + '</span>' + '<span class = "percentValue"  style="float:right;">' + value + '%</span></div>';

                        legend.appendChild(legItem[i]);
                        }
                    };
                }
            },
            error: function () {
                alert("Error loading data! Please try again.");
            }
        });
    })
});