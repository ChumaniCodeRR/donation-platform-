function displayinvoice() {
    var suppliercode = $("#clientlist option:selected").val();
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var endDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');
    var reporttype = $('input[name=reportypeRadio]:checked').val();

    if (PDFObject.supportsPDFs) {
        console.log("Yay, this browser supports inline PDFs.");
    } else {
        console.log("Boo, inline PDFs are not supported by this browser");
    }

    var options = {
        page: 2,
        height: "900px",
        fallbackLink: "<p>This browser does not have an online PDF viewer! <a href='DownloadReport?suppliercode=" + suppliercode + "&startdate=" + startDate + "&enddate=" + endDate + "&reporttype=" + reporttype+"'>Please download the file here</a></p>",
        pdfOpenParams: {
            navpanes: 1,
            view: "FitV",
            pagemode: "thumbs"
        }
    };
    $('#pdf1').show();
    
    var pdfem = PDFObject.embed("DownloadReport?suppliercode=" + suppliercode + "&startdate=" + startDate + "&enddate=" + endDate + "&reporttype=" + reporttype, "#pdf1", options);
    /*if (reporttype == "invoice") {
        PDFObject.embed("DownloadReport?suppliercode=" + suppliercode + "&startdate=" + startDate + "&enddate=" + endDate + "&reporttype="+ reporttype, "#pdf1", options);
    }
    else if (reporttype == "open") {
        PDFObject.embed("DownloadReport?suppliercode=" + suppliercode + "&startdate=" + startDate + "&enddate=" + endDate + "&reporttype=" + reporttype, "#pdf1", options);
    }*/




    /*pdfem.addEventListener('load', function () {
        // Operate upon the SVG DOM here
        $('#loading').hide();
    });*/
    
    
}