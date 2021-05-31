var myorder = null;
var priceselected = null;
var datatable;
$(function () {
    $.fn.dataTableExt.sErrMode = 'throw';
    if (datatable) {
        datatable.destroy();
        datatable = null;
    }
    datatable = $('#torders').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "order": [[0, "desc"]],
        "info": true,
        "pageLength": 50
    });
});

$(document).ready(function () {
    $('#destinationlist').combobox({ enforceselection: false });
    $('#clientlist').combobox({ enforceselection: true });
    display();
});

function toCurrency(value, signbef, signafter) {
    return signbef + (value != null ? value.toFixed(2) : 0).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + signafter;
}
function toCurrencyD(value, signbef, signafter, digits) {
    return signbef + (value.toFixed(digits)).toString().replace(/(\d)(?=(\d{4})+(?!\d))/g, "$1,") + signafter;
}
function saverebate() {
    var priceid = $("#editval").val();
    var newrebate = $("#newrebate").val();

    $("#editval").val('');
    $("#newrebate").val('');

    
    $.ajax(
    {
        type: "POST",
        url: "SaveTransportRate?priceid=" + priceid + "&rebatev=" + newrebate,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();

        },
        success: function (data) {
            location.reload();
            $('#myModal').modal('hide');
            alert("Rebate saved!");
        },
        error: function (msg) {
            alert("Error: You are not authorized to edit rebates!");
            $('#loading').hide();
            $('#myModal').modal('hide');
        }
    });
    
}
function loadDataTable(data, datatable) {
    var table = $("#pricelist");
    table.html("");
    $.each(data, function (index, price) {

        table.append('<tr id="' + price.AreaID + '">' +
            '<td>' + price.Area + '</td>'
            + '<td>' + price.Destination + '</td>'
            + '<td>' + toCurrencyD(price.Rate, 'R ','',2) + '</td>'
            + '<td>' + price.Ref + '</td>'
            + '<td>' + price.LastUpdate + '</td>'
            + '<td><button onclick="edit(' + price.AreaID + ',' + toCurrencyD(price.Rate, '', '', 2) + ')">Edit</button></td>'
           

           /* + "<td><button onclick='copy(\"" + clientcode + "\",\"" + price.ProductName + "\",\"" + price.Zone + "\",\""
            + price.LoadingPoint + "\",\"" + price.LoadingDepot + "\",\"" + price.DeliveryPoint + "\",\"" + price.EffDate + "\",\""
            + price.Type + "\",\""
            + price.Grid + "\",\"" + price.Rebate + "\",\"" + price.ID + "\");'>Copy</button></td>" +*/

            + '</tr>');
    });
    $('#loading').hide();
    try
    {
        datatable.destroy();
        datatable = null;
    } catch (ex) {

    }
   
    if (data.length != 0) {
        if ($.fn.dataTable.isDataTable('#torders')) {
            datatable = $('#torders').DataTable();
        } else {
            datatable = $('#torders').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "pageLength": 50,
                "order": [[0, "asc"]]
            });
        }
    } else {
        data = null;
        alert("No data for this area!");
    }
}
function copy(clientcode, product, zone, loadingpoint, loadingdepot, deliverypoint, effectiveDate, deliveredornot, grid, rebate, priceid) {
    $("#clientlist option:selected").text(clientcode);
    $("#clientlist option:selected").val(clientcode);

    $("#productlist option:selected").text(product);
    $("#productlist option:selected").val(product);

    $("#zonelist option:selected").text(zone);
    $("#zonelist option:selected").val(zone);

    $("#rebate").val(rebate);
    $("#refineryundefined").val(loadingpoint);
    $("#depotlistundefined").val(loadingdepot);
    $("#addresslistundefined").val(deliverypoint);

    $("#gridlist option:selected").val(grid);
    $("#gridlist option:selected").text(grid);
}

function deleterebate(priceid) {

}
function display() {

    if (datatable) {
        datatable.destroy();
        datatable = null;
    }

    var destination = $("#clientlist option:selected").val();
    var area = $("#destinationlist option:selected").val();
    
    var startDate = null;//moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    $.ajax(
    {
        type: "POST",
        url: "GetTransportRate?area=" + area + "&destination=" + destination,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            loadDataTable(data, datatable);
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });


}

function displayletter() {
    $('#loading').show();
    var clientcode = $("#clientlist option:selected").val();
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');

    if (PDFObject.supportsPDFs) {
        console.log("Yay, this browser supports inline PDFs.");
    } else {
        console.log("Boo, inline PDFs are not supported by this browser");
    }

    var options = {
        height: "900px",
        fallbackLink: "<p>This browser does not have an online PDF viewer! <a href='DownloadLetter?clientcode=" + clientcode + "&startdate=" + startDate + "'>Pleased the Letter here</a></p>",
        pdfOpenParams: {
            navpanes: 1,
            view: "FitV",
            pagemode: "thumbs"
        }
    };
    $('#pdf1').show();
    var pdfem = PDFObject.embed("DownloadLetter?clientcode=" + clientcode + "&startdate=" + startDate, "#pdf1", options);
    if (pdfem) {
        $('#loading').hide();
    }

}
function edit(priceid, newrebate) {
    $("#editval").val(priceid);
    $("#newrebate").val(newrebate);
    
    $('#myModal').modal('show');
}
/*
function saveRebate() {
    var clientcode = $("#clientlist option:selected").val();
    var productcode = $("#productlist option:selected").val();
    var zonecode = $("#zonelist option:selected").text();
    var rebate = $("#rebate").val();
    var loadingpoint = $("#refinerylistundefined").val();
    var loadingdepot = $("#depotlistundefined").val();
    var deliverypoint = $("#addresslistundefined").val();
    var effectiveDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var deliveredornot = $("input[name='optionsRadios']:checked").val() == undefined ? "" : $("input[name='optionsRadios']:checked").val();
    var grid = $("#gridlist option:selected").val();
    var comment = $("#comment").val();
    //Validation 
    if (rebate <= 0 || productcode == "" || deliveredornot == "" || clientcode == ""
        || grid == "" || zonecode == "" || effectiveDate == "" || zonecode == ""
        ) {

        alert("Please fill in all the required fields [Company, Quantity, Product, Zone, COC or RRT]");
        return;
    }

    if (deliveredornot == "COC" && (loadingpoint == "" || loadingdepot == "")) {
        alert("Please fill in a loading point and a depot");
        return;
    } else if (deliveredornot == "RTL" && deliverypoint == "") {
        alert("Please fill in a delivery point");
        return;
    }
    //End Validation

    if (datatable) {
        datatable.destroy();
        datatable = null;
    }

    $.ajax(
    {
        type: "POST",
        url: "SaveRebate?clientcode=" + clientcode
            + "&productcode=" + productcode
            + "&zonecode=" + zonecode
            + "&rebate=" + rebate
            + "&grid=" + grid
            + "&deliveredOrNot=" + deliveredornot
            + "&loadingpoint=" + loadingpoint
            + "&effectiveDate=" + effectiveDate
            + "&deliverypoint=" + deliverypoint
            + "&comment=" + comment
            + "&loadingdepot=" + loadingdepot
        ,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            loadDataTable(data, datatable, clientcode);
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });
}*/