var barChartCanvas = $("#barChart").get(0).getContext("2d");
var barChart;
var resetCanvas = function () {
    $('#results-graph').remove(); // this is my <canvas> element
    $('#graph-container').append('<canvas id="results-graph"><canvas>');
    canvas = document.querySelector('#results-graph');
    ctx = canvas.getContext('2d');
    ctx.canvas.width = $('#graph').width(); // resize to parent width
    ctx.canvas.height = $('#graph').height(); // resize to parent height
    var x = canvas.width / 2;
    var y = canvas.height / 2;
    ctx.font = '10pt Verdana';
    ctx.textAlign = 'center';
    ctx.fillText('This text is centered on the canvas', x, y);
};
function getRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function toCurrency(value) {
    return "R " + (value.toFixed(2)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}
function exportaging() {
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var endDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');
    $("#startd").val(startDate);
    $("#endd").val(endDate);
    $("#exportform").submit();
    /*var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var endDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');

    $.ajax(
    {
        type: "POST",
        url: "GetAllAging?startdate=" + startDate + "&enddate=" + endDate,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
       
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });*/

}
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
            $('#clientto').val(data.clientemail);
            $('#subject').val(data.subject);
            
            //$('#compose-textarea').val(data.emailbody);
            /*var editorInstance = $('#compose-textarea').data('wysihtml5').editor;
            editorInstance.setValue(data.emailbody);*/
            //$('#compose-textarea').text(data.emailbody);
            editorElement.setHtml(data.emailbody);
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

function sendmail() {
    var body = editorElement.getHtml();
    var emailto = $('#clientto').val();
    var subject = $('#subject').val();
    $.ajax(
    {
        type: "POST",
        url: "SendMail?emailTo=" + escape(emailto) + "&body=" + escape(body) + "&subject=" + subject,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            if (data == true) {
                emailto.val("");
                body.val("");
                subject.val("");
                alert("Email send successfully!");
            } else {
                alert("An Error Occured! The email was NOT send!. Please try again! or send the email manually from your own outlook!");
            }
        },
        error: function (msg) {
            alert("An Error Occured! The email was NOT send!. Please try again! or send the email manually from your own outlook!");
            $('#loading').hide();
        }
    });
}



var editorElement;
var initCk = (function () {
    return function () {
        editorElement = CKEDITOR.document.getById('editor');
        editorElement.setAttribute('contenteditable', 'true');
        CKEDITOR.inline('editor');
    };
})();
window.onload = function (e) {
    $('.carousel').carousel({
        interval: 1000 * 10
    });

    initCk();
    if (CKEDITOR.env.ie && CKEDITOR.env.version < 9)
        CKEDITOR.tools.enableHtml5Elements(document);

    // The trick to keep the editor in the sample quite small
    // unless user specified own height.
    CKEDITOR.config.height = 150;
    CKEDITOR.config.width = 'auto';
    //$('.textarea').wysihtml5();
    /*$('.textarea').wysihtml5({
        "font-styles": true, //Font styling, e.g. h1, h2, etc. Default true
        "emphasis": true, //Italics, bold, etc. Default true
        "lists": false, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
        "html": false, //Button which allows you to edit the generated HTML. Default false
        "link": false, //Button to insert a link. Default true
        "image": false, //Button to insert an image. Default true,
        "color": false //Button to change color of font
    });*/

    $.ajax(
    {
        type: "POST",
        url: "LimitCompute",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            $.each(data, function (i, d) {
                $('#clientlist').append('<option value="' + d.Value + '">' + d.Text + " - " + d.Value + '</option>');
                
            });
            $('#loading').hide();
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });

    //$('.combobox').combobox();
}
