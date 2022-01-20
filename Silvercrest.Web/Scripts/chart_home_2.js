document.currentScript = document.currentScript || (function () {
    var scripts = document.getElementsByTagName('script');
    return scripts[scripts.length - 1];
})();

var jsonURL1 = document.currentScript.getAttribute('json-url1');
var pieElement1 = document.getElementById(document.currentScript.getAttribute('destination-id1'));
var jsonURL2 = document.currentScript.getAttribute('json-url2');
var pieElement2 = document.getElementById(document.currentScript.getAttribute('destination-id2'));

// Load the Visualization API and the piechart package.
google.charts.load('current', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
google.charts.setOnLoadCallback(drawChart);

var chartsdata1 = model.ChartTypeData.Data;
var chartsdata2 = model.ChartMunicData.Data;

function drawChart() 
{
    var jsonParams = {
        contactId: $("#contactId").val(),
        isGroup: $("#isGroup").val(),
        isClientGroup: $("#isClientGroup").val(),
        entityId: $("#entityId").val()
     };

    //--------Data
    //var jsonData1 = $.ajax({
    //    type: 'get',
    //    dataType: 'json',
    //    contentType: 'application/json',
    //    url: jsonURL1,//'/client/home/GetCharts',
    //    data: jsonParams,
    //    //        url: "getData.php",
    //    //        dataType: "json",
    //    async: false,
    //    error: function () {
    //        alert("Error Retrieving data! Please try again.");
    //    }
    //}).responseText;

    //var jsonData2 = $.ajax({
    //    type: 'get',
    //    dataType: 'json',
    //    contentType: 'application/json',
    //    url: jsonURL2,//'/client/home/GetCharts',
    //    data: jsonParams,
    //    //        url: "getData.php",
    //    //        dataType: "json",
    //    async: false,
    //    error: function () {
    //        alert("Error Retrieving data! Please try again.");
    //    }
    //}).responseText;

    // Instantiate and draw our chart, passing in some options.
    //var chartsdata1 = JSON.parse(jsonData1);
    //var chartsdata2 = JSON.parse(jsonData2);

    //Finding Largest Type
    var max = 0;
    for (var i = 1; i < chartsdata1.length; i++)
    {
        if (chartsdata1[i].Percent > chartsdata1[max].Percent)
            max = i;
    }

    $("#titleToChange").text("ALLOCATION TO " + chartsdata1[max].LegendName.toUpperCase());
  

    var chart1 = new google.visualization.PieChart(pieElement1); 
    var chart2 = new google.visualization.PieChart(pieElement2); 

    data1 = new google.visualization.DataTable();
    data2 = new google.visualization.DataTable();

    var options1 = {
        backgroundColor: 'transparent',
        height: '100%',
        width: '100%',
        colors: ['#892134', '#163574', '#7589b4', '#b3b3b3', '#d9d9d9', '#a16985', '#b99c6b', '#d5c4a1', '#7F7F7F', '#afbbd3'],
        pieSliceText: 'none',
        chartArea: { width: '100%', height: '100%', top: '40', bottom: '30'/*, left: '30',right: '40'*/ },//top and bottom only control size of pie
        legend: { 'position': 'right', 'alignment': 'center', textStyle: { 'fontName': 'goudyosSC', 'fontSize': '12' } },
        titlePosition: 'none',
//            titleTextStyle:{ 'fontName': 'copperplate', 'fontSize': '15', 'color':'#163574' } ,
        slices: {},
        is3D: false,
        tooltip: { trigger: 'selection', isHtml: false, ignoreBounds: false, backgroundColor: '#f6eee0', textStyle: { 'fontName': 'goudyosSC', 'fontSize': '12', 'fontStyle': 'normal' } }
    };

    var options2 = {
        backgroundColor: 'transparent',
        height: '100%',
        width: '100%',
        colors: ['#892134', '#163574', '#7589b4', '#b3b3b3', '#d9d9d9', '#a16985', '#b99c6b', '#d5c4a1', '#7F7F7F', '#afbbd3'],
        pieSliceText: 'none',
        chartArea: { width: '100%', height: '100%', top: '40', bottom: '30'/*, left: '30',right: '40'*/ },//top and bottom only control size of pie
        legend: { 'position': 'right', 'alignment': 'center', textStyle: { 'fontName': 'goudyosSC', 'fontSize': '12' } },
        titlePosition: 'none',
        //            titleTextStyle:{ 'fontName': 'copperplate', 'fontSize': '15', 'color':'#163574' } ,
        slices: {},
        is3D: false,
        tooltip: { trigger: 'selection', isHtml: false, ignoreBounds: false, backgroundColor: '#f6eee0', textStyle: { 'fontName': 'goudyosSC', 'fontSize': '12', 'fontStyle': 'normal' } }
    };


    function onSelectAction1() {
        var selection = chart1.getSelection();

        if (selection.length == 0) {
            return;
        }

        var selectedItem = selection[0];
        var rowIdx = chart1.selectedSlice;

        if (selection.length) {
            rowIdx = selectedItem.row;
        }

        //if (rowIdx >= 0) {
        //    if (chart1.selectedSlice == rowIdx) {
        //        options1.slices[rowIdx] = { offset: '.0' };
        //        chart1.selectedSlice = -1;
        //        chart1.draw(data1, options1);
        //    }
        //    else {
        //        if (chart1.selectedSlice != -1) {
        //            options1.slices[chart1.selectedSlice] = { offset: '.0' };
        //        }
        //        options1.slices[rowIdx] = { offset: '.1' };
        //        chart1.selectedSlice = rowIdx;

        //        chart1.draw(data1, options1);
        //        chart1.setSelection(selection);
        //    }
        //}
    };

    function onSelectAction2() {
        var selection = chart2.getSelection();

        if (selection.length == 0) {
            return;
        }

        var selectedItem = selection[0];
        var rowIdx = chart2.selectedSlice;

        if (selection.length) {
            rowIdx = selectedItem.row;
        }

        //if (rowIdx >= 0) {
        //    if (chart2.selectedSlice == rowIdx) {
        //        options2.slices[rowIdx] = { offset: '.0' };
        //        chart2.selectedSlice = -1;
        //        chart2.draw(data2, options2);
        //    }
        //    else {
        //        if (chart2.selectedSlice != -1) {
        //            options2.slices[chart2.selectedSlice] = { offset: '.0' };
        //        }
        //        options2.slices[rowIdx] = { offset: '.1' };
        //        chart2.selectedSlice = rowIdx;

        //        chart2.draw(data2, options2);
        //        chart2.setSelection(selection);
        //    }
        //}
    };


    data1.addColumn('string', 'Asset Class');
    data1.addColumn('number', 'Percent of Total');
    data1.addColumn({ type: 'string', role: 'tooltip' });

    data2.addColumn('string', 'Asset Class');
    data2.addColumn('number', 'Percent of Total');
    data2.addColumn({ type: 'string', role: 'tooltip' });

    for (var i = 0; i < chartsdata1.length; i++) 
    {
        var legendName = chartsdata1[i].LegendName;
        if (legendName.length >= 20)
        {
            legendName = legendName.substring(0, 17) + "...";
        }
        var tooltipArguments = chartsdata1[i].LegendName + ": " + chartsdata1[i].MarketValue.toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " (" + ( chartsdata1[i].Percent).toFixed(1) + "%)";
        data1.addRow([legendName + ": " + (chartsdata1[i].Percent ).toFixed(1) + "%",/* Math.round(chartsdata[i].MarketValue),*/ chartsdata1[i].Percent, tooltipArguments]);
    }

    for (var i = 0; i < chartsdata2.length; i++) 
    {
        var legendName = chartsdata2[i].LegendName;
        if (legendName.length >= 20)
        {
            legendName = legendName.substring(0, 17) + "...";
        }
        var tooltipArguments = chartsdata2[i].LegendName + ": " + chartsdata2[i].MarketValue.toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " (" + ( chartsdata2[i].Percent).toFixed(1) + "%)";
        data2.addRow([legendName + ": " + (chartsdata2[i].Percent ).toFixed(1) + "%",/* Math.round(chartsdata[i].MarketValue),*/ chartsdata2[i].Percent, tooltipArguments]);
    }

    chart1.selectedSlice = -1;
    chart2.selectedSlice = -1;

    google.visualization.events.addListener(chart1, 'select', onSelectAction1);
    google.visualization.events.addListener(chart2, 'select', onSelectAction2);

    chart1.draw(data1, options1);
    chart2.draw(data2, options2);
}


$(window).resize(function () {
    drawChart();
});

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
