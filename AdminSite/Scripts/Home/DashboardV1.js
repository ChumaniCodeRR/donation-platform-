/*
 * Author: Abdullah A Almsaeed
 * Date: 4 Jan 2014
 * Description:
 *      This is a demo file used only for the main dashboard
 **/

$(document).ready(function () {
    $("#Mytable").jqGrid({
        url: '/Home/GetData/',
        datatype: "json",
        colNames: ['First name', 'Last name', 'Amount Donated'],
        colModel: [
            { name: 'Firstname', index: 'Firstname', width: 300 },
            { name: 'Lastname', index: 'Lastname', width: 300 },
            { name: 'Amount', index: 'Amount', width: 300 },
            //{ name: 'body', index: 'body', width: 350, sortable: false }
        ],
        rowNum: 10,
        loadonce: true,
        rowList: [10, 20, 30],
        pager: '#pager2',
        sortname: 'id',
        viewrecords: true,
        sortorder: "acs",
        caption: ""
    });
    $("#Mytable").jqGrid('navGrid', '#pager2', { edit: true, add: false, del: false });
});
function display() {
    var clientcode = $("#clientlist option:selected").val();
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var endDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');

    $.ajax(
    {
        type: "POST",
        url: "GetAging?clientcode=" + clientcode + "&startdate=" + startDate + "&enddate=" + endDate,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            //-------------
            //- BAR CHART -
            //-------------               
            $("#clientbalance").text("R " + (data.clientbalance).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
            $("#clientlimit").text("R " + (data.clientlimit).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));//.text(data.clientlimit);

            var remaining = (parseFloat(data.openorderbalance) + parseFloat(data.clientbalance) - parseFloat(data.clientlimit));
            var limitinfo = remaining > 0 ? "Over Limit by " + (toCurrency(remaining)) : "Client can only order " + (toCurrency(remaining)) + " before limit is reached!";
            if (remaining > 0) {
                $("#statusbox").removeClass("info-box bg-green");
                $("#statusbox").addClass("info-box bg-red");
            } else {
                $("#statusbox").removeClass("info-box bg-green");
                $("#statusbox").removeClass("info-box bg-red");
                $("#statusbox").addClass("info-box bg-green");
            }

            $("#overlimit").text(limitinfo);//.text(data.clientlimit);
            $("#verified").text(data.verified);
            $("#openordertotal").text(toCurrency(data.openorderbalance));
            $("#finalbalance").text(toCurrency(data.openorderbalance + data.clientbalance));
            var areaChartData = data;
            var barChartData = areaChartData;
            var barChartOptions = {
                //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                scaleBeginAtZero: false,
                //Boolean - Whether grid lines are shown across the chart
                scaleShowGridLines: true,
                //String - Colour of the grid lines
                scaleGridLineColor: "rgba(0,0,0,.05)",
                //Number - Width of the grid lines
                scaleGridLineWidth: 1,
                //Boolean - Whether to show horizontal lines (except X axis)
                scaleShowHorizontalLines: true,
                //Boolean - Whether to show vertical lines (except Y axis)
                scaleShowVerticalLines: true,
                //Boolean - If there is a stroke on each bar
                barShowStroke: true,
                //Number - Pixel width of the bar stroke
                barStrokeWidth: 2,
                //Number - Spacing between each of the X value sets
                barValueSpacing: 5,
                //Number - Spacing between data sets within X values
                barDatasetSpacing: 1,
                //String - A legend template
                //legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>",
                //Boolean - whether to make the chart responsive
                responsive: true,
                maintainAspectRatio: false,
                showTooltips: false,
                scaleFontSize: 16,
                onAnimationComplete: function () {

                    var ctx = this.chart.ctx;
                    ctx.font = "18px \"Helvetica Neue\", Helvetica, Arial, sans-serif";//this.scale.font;
                    ctx.fillStyle = "#000";//this.scale.textColor;
                    ctx.textAlign = "center";
                    ctx.textBaseline = "bottom";
                    ctx.fontStyle = "bold";
                    this.datasets.forEach(function (dataset) {
                        dataset.bars.forEach(function (bar) {
                            ctx.fillText(toCurrency(bar.value), bar.x, bar.y + 1);
                        });
                    })
                }
            };

            barChartOptions.datasetFill = false;

            if (barChart) {
                barChart.clear();
                barChart.destroy();
                barChart = new Chart(barChartCanvas).Bar(barChartData, barChartOptions);
                barChart.update();
            }
            else {
                barChart = new Chart(barChartCanvas).Bar(barChartData, barChartOptions);
            }
            var totalaging = 0;
            for (var i = 0; i < barChart.datasets[0].bars.length; i++) {
                barChart.datasets[0].bars[i].fillColor = "green";
                if (barChart.datasets[0].bars[i].value < 0) {
                    barChart.datasets[0].bars[i].fillColor = "red";
                }
                totalaging = totalaging + barChart.datasets[0].bars[i].value;
            }

            $('#loading').hide();
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });

}
$(function () {

    "use strict";

    //Make the dashboard widgets sortable Using jquery UI
    $(".connectedSortable").sortable({
        placeholder: "sort-highlight",
        connectWith: ".connectedSortable",
        handle: ".box-header, .nav-tabs",
        forcePlaceholderSize: true,
        zIndex: 999999
    });
    $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");

    //jQuery UI sortable for the todo list
    $(".todo-list").sortable({
        placeholder: "sort-highlight",
        handle: ".handle",
        forcePlaceholderSize: true,
        zIndex: 999999
    });

    //bootstrap WYSIHTML5 - text editor
    $(".textarea").wysihtml5();

    $('.daterange').daterangepicker({
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        startDate: moment().subtract(29, 'days'),
        endDate: moment()
    }, function (start, end) {
        window.alert("You chose: " + start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    });

    /* jQueryKnob */
    $(".knob").knob();

    //jvectormap data
    var visitorsData = {
        "US": 398, //USA
        "SA": 400, //Saudi Arabia
        "CA": 1000, //Canada
        "DE": 500, //Germany
        "FR": 760, //France
        "CN": 300, //China
        "AU": 700, //Australia
        "BR": 600, //Brazil
        "IN": 800, //India
        "GB": 320, //Great Britain
        "RU": 3000 //Russia
    };
    //World map by jvectormap
    $('#world-map').vectorMap({
        map: 'world_mill_en',
        backgroundColor: "transparent",
        regionStyle: {
            initial: {
                fill: '#e4e4e4',
                "fill-opacity": 1,
                stroke: 'none',
                "stroke-width": 0,
                "stroke-opacity": 1
            }
        },
        series: {
            regions: [{
                values: visitorsData,
                scale: ["#92c1dc", "#ebf4f9"],
                normalizeFunction: 'polynomial'
            }]
        },
        onRegionLabelShow: function (e, el, code) {
            if (typeof visitorsData[code] != "undefined")
                el.html(el.html() + ': ' + visitorsData[code] + ' new visitors');
        }
    });

    //Sparkline charts
    var myvalues = [1000, 1200, 920, 927, 931, 1027, 819, 930, 1021];
    $('#sparkline-1').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [515, 519, 520, 522, 652, 810, 370, 627, 319, 630, 921];
    $('#sparkline-2').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [15, 19, 20, 22, 33, 27, 31, 27, 19, 30, 21];
    $('#sparkline-3').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });

    //The Calender
    $("#calendar").datepicker();

    //SLIMSCROLL FOR CHAT WIDGET
    $('#chat-box').slimScroll({
        height: '250px'
    });

    /* Morris.js Charts */
    // Sales chart
    var area = new Morris.Area({
        element: 'revenue-chart',
        resize: true,
        data: [
          { y: '2011 Q1', item1: 2666, item2: 2666 },
          { y: '2011 Q2', item1: 2778, item2: 2294 },
          { y: '2011 Q3', item1: 4912, item2: 1969 },
          { y: '2011 Q4', item1: 3767, item2: 3597 },
          { y: '2012 Q1', item1: 6810, item2: 1914 },
          { y: '2012 Q2', item1: 5670, item2: 4293 },
          { y: '2012 Q3', item1: 4820, item2: 3795 },
          { y: '2012 Q4', item1: 15073, item2: 5967 },
          { y: '2013 Q1', item1: 10687, item2: 4460 },
          { y: '2013 Q2', item1: 8432, item2: 5713 }
        ],
        xkey: 'y',
        ykeys: ['item1', 'item2'],
        labels: ['Item 1', 'Item 2'],
        lineColors: ['#a0d0e0', '#3c8dbc'],
        hideHover: 'auto'
    });
    var line = new Morris.Line({
        element: 'line-chart',
        resize: true,
        data: [
          { y: '2011 Q1', item1: 2666 },
          { y: '2011 Q2', item1: 2778 },
          { y: '2011 Q3', item1: 4912 },
          { y: '2011 Q4', item1: 3767 },
          { y: '2012 Q1', item1: 6810 },
          { y: '2012 Q2', item1: 5670 },
          { y: '2012 Q3', item1: 4820 },
          { y: '2012 Q4', item1: 15073 },
          { y: '2013 Q1', item1: 10687 },
          { y: '2013 Q2', item1: 8432 }
        ],
        xkey: 'y',
        ykeys: ['item1'],
        labels: ['Item 1'],
        lineColors: ['#efefef'],
        lineWidth: 2,
        hideHover: 'auto',
        gridTextColor: "#fff",
        gridStrokeWidth: 0.4,
        pointSize: 4,
        pointStrokeColors: ["#efefef"],
        gridLineColor: "#efefef",
        gridTextFamily: "Open Sans",
        gridTextSize: 10
    });

    //Donut Chart
    var donut = new Morris.Donut({
        element: 'sales-chart',
        resize: true,
        colors: ["#3c8dbc", "#f56954", "#00a65a"],
        data: [
          { label: "Download Sales", value: 12 },
          { label: "In-Store Sales", value: 30 },
          { label: "Mail-Order Sales", value: 20 }
        ],
        hideHover: 'auto'
    });

    //Fix for charts under tabs
    $('.box ul.nav a').on('shown.bs.tab', function () {
        area.redraw();
        donut.redraw();
        line.redraw();
    });

    /* The todo list plugin */
    $(".todo-list").todolist({
        onCheck: function (ele) {
            window.console.log("The element has been checked");
            return ele;
        },
        onUncheck: function (ele) {
            window.console.log("The element has been unchecked");
            return ele;
        }
    });

});
