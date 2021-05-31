function displayinvoice() {
    $('#loading').show();
    var clientcode = $("#clientlist option:selected").val();
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var endDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');
    var reporttype = $('input[name=reportypeRadio]:checked').val();
    var salesorder = $('#sonumber').val();
    if (PDFObject.supportsPDFs) {
        console.log("Yay, this browser supports inline PDFs.");
    } else {
        console.log("Boo, inline PDFs are not supported by this browser");
    }

    var options = {
        height: "900px",
        fallbackLink: "<p>This browser does not have an online PDF viewer! <a href='DownloadReport?clientcode=" + clientcode + "&startdate=" + startDate + "&enddate=" + endDate + "&reporttype=" + reporttype+"'>Please download the file here</a></p>",
        pdfOpenParams: {
            navpanes: 1,
            view: "FitV",
            pagemode: "thumbs"
        }
    };
    $('#pdf1').show();    
    var pdfem = PDFObject.embed("DownloadReport?clientcode=" + clientcode + "&salesorder=" + salesorder + "&startdate=" + startDate + "&enddate=" + endDate + "&reporttype=" + reporttype, "#pdf1", options);
    if (pdfem) {
       $('#loading').hide();
    }
    
    
}

$(document).ready(function () {
    $('.combobox').combobox();
});