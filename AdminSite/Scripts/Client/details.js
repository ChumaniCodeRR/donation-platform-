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
            
            var totalaging = 0;
            
            var table = $("#addresstable");
            table.html("");
            $.each(data.addresses, function (index, address) {
                
                table.append('<tr><td>' + (index + 1) + '</td><td>' + address.Address + '</td><td><button  class="btn btn-primary" onclick=\'edit(' + address.ID + ', "'+address.Address+'")\'>Edit</button></td></tr>');
            });
            $('#loading').hide();
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });

}
function edit(addressid, address) {
    $("#deliveryAddress").attr("data-addressid", addressid);
    var address = $('#deliveryAddress').val(address);
}
function addaddress() {
    var address = $('#deliveryAddress').val();
    var clientcode = $("#clientlist option:selected").val();
    var addressid = $("#deliveryAddress").data("addressid");

    $.ajax(
    {
        type: "POST",
        url: "AddAddress?address=" + escape(address) + "&clientcode=" + clientcode + "&addressid="+addressid ,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            if (data == true) {
                alert("Address saved successfully!");
            } else {
                alert("An Error Occured while adding the address! Please try again later!");
            }
        },
        error: function (msg) {
            alert("An Error Occured while adding the address! Please try again later!");
            $('#loading').hide();
        }
    });
}

$(document).ready(function () {
    $('.combobox').combobox();
});