document.currentScript = document.currentScript || (function () {
    var scripts = document.getElementsByTagName('script');
    return scripts[scripts.length - 1];
})();

var jsonURL= document.currentScript.getAttribute('json-url');

// Load the Visualization API and the piechart package.
google.charts.load('current', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
google.charts.setOnLoadCallback(drawChart);


function drawChart() {
    var jsonParams = "";
    if (jsonURL.indexOf("Holdings") > 0)
    {
        jsonParams = {
            contactId: $("#contactId").val(),
            isGroup: $("#isGroup").val(),
            isClientGroup: $("#isClientGroup").val(),
            entityId: $("#entityId").val()
        };
    } else
    {
        jsonParams = {
            contactId: $("#contactId").val()
        };
    }

        var jsonData = $.ajax({
        type: 'get',
        dataType: 'json',
        contentType: 'application/json',
        url: jsonURL,//'/client/home/GetCharts',
        data: jsonParams,
        //        url: "getData.php",
        //        dataType: "json",
        async: false,
        error: function () {
            alert("Error Retrieving data! Please try again.");
        }
    }).responseText;

    // Create our data table out of JSON data loaded from server.
    //    var data = new google.visualization.DataTable(jsonData);

    // Instantiate and draw our chart, passing in some options.
    var chartsdata = JSON.parse(jsonData);
    //var chartsdata = model.ChartData.Data;
    var chart = new google.visualization.PieChart(document.getElementById('piechart'));

    if (chartsdata.length != 0 && chartsdata != undefined) {
        data = new google.visualization.DataTable();
        var options = {
            backgroundColor: 'transparent',
            height: '100%',
            width: '100%',
            colors: ['#892134', '#163574', '#7589b4', '#b3b3b3', '#d9d9d9', '#a16985', '#b99c6b', '#d5c4a1', '#7F7F7F', '#afbbd3'],
            pieSliceText: 'none',
            chartArea: { width: '100%', height: '100%', top: '40', bottom: '30'/*, left: '30',right: '40'*/ },//top and bottom only control size of pie
            legend: { 'position': 'right', 'alignment': 'center', textStyle: { 'fontName': 'goudyosSC', 'fontSize': '13' } },
            titlePosition: 'none',
            slices: {},
            is3D: false,
            tooltip: { trigger: 'selection', isHtml: false, ignoreBounds: false, backgroundColor: '#f6eee0', textStyle: { 'fontName': 'goudyosSC', 'fontSize': '12', 'fontStyle': 'normal' } }
        };

        function onSelectAction() {
            var selection = chart.getSelection();

            if (selection.length == 0) {
                return;
            }

            var selectedItem = selection[0];
            var rowIdx = chart.selectedSlice;

            if (selection.length) {
                rowIdx = selectedItem.row;
            }

            //if (rowIdx >= 0) {
            //    if (chart.selectedSlice == rowIdx) {
            //        options.slices[rowIdx] = { offset: '.0' };
            //        chart.selectedSlice = -1;
            //        chart.draw(data, options);
            //    }
            //    else {

            //        if (chart.selectedSlice != -1) {
            //            options.slices[chart.selectedSlice] = { offset: '.0' };
            //        }
            //        options.slices[rowIdx] = { offset: '.1' };
            //        chart.selectedSlice = rowIdx;

            //        chart.draw(data, options);
            //        chart.setSelection(selection);

            //        //                                        chart.setSelection([{ row: rowIdx, column: null }]);
            //    }
            //}
        };

        data.addColumn('string', 'Asset Class');
        //        data.addColumn('number', 'Market Value');
        data.addColumn('number', 'Percent of Total');
        data.addColumn({ type: 'string', role: 'tooltip' });

        for (var i = 0; i < chartsdata.length; i++) {

            var legendName = chartsdata[i].LegendName;
            if (chartsdata[i].LegendName.length >= 20) {
                legendName = chartsdata[i].LegendName.substring(0, 17) + "...";
            }
            var tooltipArguments = chartsdata[i].LegendName + ": " + chartsdata[i].MarketValue.toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " (" + (100 * chartsdata[i].Percent).toFixed(1) + "%)";
            //     legendName = legendName + Array(20 + 1 - legendName.length).join(' ');
            data.addRow([legendName + ": " + (chartsdata[i].Percent * 100).toFixed(1) + "%",/* Math.round(chartsdata[i].MarketValue),*/ chartsdata[i].Percent * 100, tooltipArguments]);


        }


        chart.selectedSlice = -1;
        google.visualization.events.addListener(chart, 'select', onSelectAction);

        chart.draw(data, options);
    }
    else {
        $("#piechart").hide();
        //        $("#legend").text('No data for chart')
    }
}

/****

$(document).ready(function () {
    $(function () {
        $.ajax({
            type: 'get',
            dataType: 'json',
            contentType: 'application/json',
            url: '/client/home/GetCharts',
            data: {
                contactId: $("#contactId").val()
            },
            success: function (chartsdata) {
                google.charts.load('current', { 'packages': ['corechart'] });

                google.charts.setOnLoadCallback(drawChart);

                function drawChart() {
var chart = new google.visualization.PieChart(document.getElementById('piechart'));
if (chartsdata.length != 0 && chartsdata != undefined) {
    data = new google.visualization.DataTable();
    var options = {
        backgroundColor: 'transparent',
        height: '100%',
        width: '100%',
        colors: ['#892134', '#163574', '#7589b4', '#b3b3b3', '#d9d9d9', '#a16985', '#b99c6b', '#d5c4a1', '#7F7F7F', '#afbbd3'],
        pieSliceText: 'none',
        chartArea: { width: '220', height: '100%', top: '20', bottom: '15' },
        legend: 'none',
        titlePosition: 'none',
        slices: {},
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


    data.addColumn('string', 'Employee Name');
    data.addColumn('number', 'Hire Date');
    data.addColumn('number', 'Hire value');
    data.addColumn({ type: 'string', role: 'tooltip' });

    chart.selectedSlice = -1;

    google.visualization.events.addListener(chart, 'select', onSelectAction);

    for (var i = 0; i < chartsdata.length; i++) {
        var legendName = chartsdata[i].LegendName;
        if (chartsdata[i].LegendName.length >= 20) {
            legendName = chartsdata[i].LegendName.substring(0, 17) + "...";
        }
        var tooltipArguments = chartsdata[i].LegendName + ": " + Math.round(chartsdata[i].MarketValue)+ " (" + Number(100 * chartsdata[i].Percent).toFixed(1) + "%)";
        data.addRow([legendName, chartsdata[i].MarketValue, chartsdata[i].Percent, tooltipArguments]);
    }

    chart.draw(data, options);
    var legend = document.getElementById("legend");
    var legItem = [];
    var colors = ['#000000'];
    for (var i = 0; i < data.getNumberOfRows() ; i++) {
        var label = data.getValue(i, 0);
        var value = (data.getValue(i, 2)*100).toFixed(1);
        //var percent = Number(100 * value / total).toFixed(1);

        // This will create legend list for the display
        if (value > 0) {
            legItem[i] = document.createElement('li');
            legItem[i].id = 'legend_' + data.getValue(i, 0);
            legItem[i].innerHTML = '<div class="legendMarker"> <span class ="textValue"  style ="color:' + colors[i] + ';float:left;">' + label + ': ' + '</span>' + '<span class = "percentValue"  style="float:right;">' + value + '%</span></div>';

            legend.appendChild(legItem[i]);
        }
    };
}
else {
    $("#piechart").hide();
    $("#legend").text('No data for chart')
}
}
},
error: function () {
    alert("Error loading data! Please try again.");
}
});
})
});





**/
