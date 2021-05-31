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
        "order": [[ 0, "desc" ]],
        "info": true,
        "pageLength": 50
    });
});

$(document).ready(function () {
    //Date
    $('#reservation').daterangepicker(
        {
            singleDatePicker: true,
            format: 'YYYY/MM/DD'
        }
    );
});

function toCurrency(value, signbef, signafter) {
    return signbef + (value != null ? value.toFixed(2) : 0).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + signafter;
}
function toCurrencyD(value, signbef, signafter, digits) {
    return signbef + (value.toFixed(digits)).toString().replace(/(\d)(?=(\d{4})+(?!\d))/g, "$1,") + signafter;
}
function loadDataTable(data, datatable, clientcode) {
    var table = $("#pricelist");
    table.html("");
    $.each(data, function (index, price) {
        var dtype = price.Type == 'RTL' ? 'info' : '';
        table.append('<tr id="'+price.ID+'" class="' + dtype + '"><td>' +
            (price.ID) + '</td>'
            + '<td>' + price.ProductName + '</td>'
            + '<td>' + price.Type + '</td>'
            + '<td>' + price.Zone + '</td>'
            + '<td>' + price.LoadingPoint + '</td>'
            + '<td>' + price.LoadingDepot + '</td>'
            + '<td>' + price.DeliveryPoint + '</td>'
            + '<td>' + price.Grid + '</td>'
            + '<td>' + toCurrencyD(price.BasePrice, 'R ', '', 4) + '</td>'
            + '<td>' + toCurrencyD(price.Rebate, 'R ', '', 4) + '</td>'
            + '<td>' + toCurrencyD(price.FinalPrice, 'R ', '', 4) + '</td>'
            + '<td>' + price.Comment + '</td>'
            + '<td>' + price.EffDate + '</td>'
            + '<td></td>'
            + "<td><button onclick='copy(\"" + clientcode + "\",\"" + price.ProductName + "\",\"" + price.Zone + "\",\""
            + price.LoadingPoint + "\",\"" + price.LoadingDepot + "\",\"" + price.DeliveryPoint + "\",\"" + price.EffDate + "\",\""
            + price.Type + "\",\""
            + price.Grid + "\",\"" + price.Rebate + "\",\"" + price.ID + "\");'>Copy</button></td>" +            
          
            +'</tr>');
    });
    $('#loading').hide();
    if (datatable) {
        datatable.destroy();
        datatable = null;
    }
    if (data.length != 0) {
        datatable = $('#torders').DataTable({
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "pageLength": 50,
            "order": [[0, "desc"]]
        });
    } else {
        alert("No data for this period!");
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
    
    var clientcode = $("#clientlist option:selected").val();
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    $.ajax(
    {
            type: "POST",
            url: "GetPrice?clientcode=" + clientcode + "&startdate=" + startDate,
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
        fallbackLink: "<p>This browser does not have an online PDF viewer! <a href='DownloadLetter?clientcode=" + clientcode + "&startdate=" + startDate +"'>Please download the Letter here</a></p>",
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

function savePrice() {
    var cocincrease = $("#cocincrease").val();
    var rrtincrease = $("#rrtincrease").val();
    var batchnumber = $("#batchnumber").val();
    var rrtoffset = $("#rrtoffset").val();
    var cocoffset = $("#cocoffset").val();
    var comment = $("#comment").val();
    var productcode = $("#productlist option:selected").val();
    var effectiveDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');

    $.ajax(
    {
        type: "POST",
        url: "SavePriceChange?batchnumber=" + batchnumber
            + "&productcode=" + productcode
            + "&cocincrease=" + cocincrease
            + "&rrtincrease=" + rrtincrease
            + "&cocoffset=" + cocoffset
            + "&rrtoffset=" + rrtoffset
            + "&comment=" + comment         
            + "&effectiveDate=" + effectiveDate,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            if (data && data.Success) {
                $("#batchnumber").val(data.NewBatchNumber);
                alert("Price Updated Successfully!");
            } else if (data.Error) {
                alert("Error: Price Update Failed!: " + data.Error);
            } else {
                alert("Error: Price Update Failed!");
            }
        },
        error: function (msg) {
            $('#loading').hide();
            alert("Update Completed!");
        }
    });
}