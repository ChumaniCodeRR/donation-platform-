var myorder = null;
var datatable;
$(function () {
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
function toCurrency(value, signbef, signafter) {
    return signbef + (value != null ? value.toFixed(2) : 0).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + signafter;
}
function toCurrencyD(value, signbef, signafter, digits) {
    return signbef + (value.toFixed(digits)).toString().replace(/(\d)(?=(\d{4})+(?!\d))/g, "$1,") + signafter;
}

function display() {

    if (datatable) {
        datatable.destroy();
        datatable = null;
    }
    
    var clientcode = $("#clientlist option:selected").val();
    var startDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var endDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');   
    
    $.ajax(
    {
            type: "POST",
            url: "GetOrders?clientcode=" + clientcode + "&startdate="+startDate+"&enddate="+endDate,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (data) {
                var table = $("#orderlist");
                table.html("");
                $.each(data, function (index, order) {
                   
                    table.append('<tr><td>' +
                        (order.ID) + '</td>' + '<td>' + order.ProductName
                        + '</td>' + '<td>' + toCurrency(order.Quantity, '', ' L') + '<td>' +
                        toCurrencyD(order.UnitPrice, 'R ', '',4) +
                        '<td>' + toCurrency(order.PriceEstimate, 'R ', '') +
                        '</td>' + '<td>' + order.DateCreated +
                        '</td>' + '<td><span class="label ' + order.StatusStyle + '">' +
                        order.OrderStatus + '</span></td>' + '<td>' + order.ClientName + '</td>'
                        + '<td><button  class="btn btn-primary" onclick=\'view(' + JSON.stringify(order) + ');\'>View</button><button  class="btn btn-info" onclick=\'cancelorder(' + JSON.stringify(order) + ');\'>Cancel</button></td></tr>');
                });
                $('#loading').hide();

                datatable = $('#torders').DataTable({
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "pageLength": 50,
                    "order": [[0, "desc"]]
                });
            },
            error: function (msg) {
                $('#loading').hide();
            }
     });
    
    
}

function acceptquote() {
    var orderid = $("#orderid").val();

    var url = "AcceptQuote?orderid=" + orderid;

    $.ajax(
    {
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            $('#loading').hide();
            if (data) {
                alert("You have just confirm the order. Please note that the price might slightly change on your invoice.")
            } else {
                alert("Failed to generate estimate. Please try again later or contact IT at support@buznovation.com");
            }
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });
}
function showOrHideCOC(mstShow) {
    if (mstShow) {

        $("#companyfrmg").show();
        $("#quantityfrmg").show();
        $("#productfrmg").show();
        $("#refineryfrmg").show();
        $("#depotfrmg").show();
        $("#placeorderfrmg").show();
        $("#deliveryaddressfrmg").hide();

    } else {

        $("#companyfrmg").hide();
        $("#quantityfrmg").hide();
        $("#productfrmg").hide();
        $("#refineryfrmg").hide();
        $("#depotfrmg").hide();
        $("#placeorderfrmg").hide();
        $("#deliveryaddressfrmg").hide();
    }
}
function showOrHideRTL(mstShow) {
    if (mstShow) {

        $("#companyfrmg").show();
        $("#quantityfrmg").show();
        $("#productfrmg").show();
        $("#refineryfrmg").hide();
        $("#depotfrmg").hide();
        $("#placeorderfrmg").show();
        $("#deliveryaddressfrmg").show();

    } else {

        $("#companyfrmg").hide();
        $("#quantityfrmg").hide();
        $("#productfrmg").hide();
        $("#refineryfrmg").hide();
        $("#depotfrmg").hide();
        $("#placeorderfrmg").hide();
        $("#deliveryaddressfrmg").hide();
    }
}
function deliverytype(myRadio) {
    if (myRadio.value == "coc") {
        showOrHideCOC(true);
    } else {
        showOrHideRTL(true);
    }
}
function cancelorder(order) {
    myorder = order;
    $('#myModal').modal('show');
    $("#cancelsend").off().on('click', function () { cancelordersend(); });
}
function view(order) {
   
    /*<p id="companyname"></p>
    <p id="productname"></p>
    <p id="quantityv"></p>
    <p id="createdbyv"></p>
    <p id="creationdate"></p>
    <p id="currentstatus"></p>
    <p id="address"></p>
    <p id="depot"></p>
    <p id="ref"></p>*/
    $("#orderid").val(order.ID);    
    $('#orderref').html("<strong>Order Ref:</strong> REF" + order.ID);
    $('#cref').html("<strong>Cst Ref:</strong> " + order.CstRef);
    $('#ordertype').html("<strong>Order Type:</strong> " + order.OrderType);
    $('#companyname').html("<strong>Company Name:</strong> " + order.ClientName);
    $('#productname').html("<strong>Product Name:</strong> " + order.ProductName);
    $('#zonename').html("<strong>Zone Name:</strong> " + order.ZoneName);
    $('#quantityv').html("<strong>Quantity Ordered:</strong> " + toCurrency(order.Quantity, '', ' L'));
    $('#unitprice').html("<strong>Unit Price:</strong> " + toCurrencyD(order.UnitPrice, 'R ', '',4));
    $('#totalprice').html("<strong>Total Price:</strong> " + toCurrency(order.PriceEstimate, 'R ', ''));

    $('#createdbyv').html("<strong>Created By:</strong> " + order.CreatedByName);
    $('#creationdate').html("<strong>Order Date:</strong> " + order.DateCreated);
    $('#address').html("<strong>Delivery Address:</strong> " + order.ClientDeliveryAddress);
    $('#depot').html("<strong>Depot:</strong> " + order.DepotName);
    $('#ref').html("<strong>Refinery:</strong> " + order.RefineryName);
    
    $('#currentstatusv').html("<strong>Current Status:</strong><span class='label " + order.StatusStyle + "'>" + order.OrderStatus + "</span>");
    if (order.OrderStatus == "Not Processed" ||
        order.OrderStatus == "Received" ||
        order.OrderStatus == "Declined" ||
        order.OrderStatus == "Cancelled" ||
        order.OrderStatus == "Order Complete" ||
        order.OrderStatus == "Sales Order Ready") {
        $("#acceptButton").hide();
    } else {
        $("#acceptButton").show();
    }
    $('#myViewModal').modal('show');
    
}
function cancelordersend() {
    //alert(myorder.ID);
    var clientcode = $("#clientlist option:selected").val();
    var orderid = myorder.ID;
    var reason = $("#reason").val();
    $.ajax(
    {
        type: "POST",
        url: "CancelOrder?clientcode=" + clientcode + "&orderid=" + orderid + "&reason=" + escape(reason),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            location.reload();
            $('#loading').hide();
        },
        error: function (msg) {
            $('#loading').hide();
            alert("Error: Cancel request failed! Please contact our orders department at orders@beltonpark.co.za!");
        }
    });
    
}

function getaddresses(clientcode) {   
    //var clientcode = $("#clientlist option:selected").val();   
    $("#addresslist").html("");
    $.ajax(
    {
        type: "POST",
        url: "GetAddresses?clientcode=" + clientcode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            $("#addresslist").html("");
            $.each(data, function (index, addr) {
                $("#addresslist").append('<option value="' + addr.ID + '">' + addr.Address + '</option>');
            });
        },
        error: function (msg) {
            
        }
    });
}
window.onload = function (e) {
    showOrHideCOC(false);
    $('.combobox').combobox();
};
function placeorder() {
    var clientcode = $("#clientlist option:selected").val();
    var quantity = $("#quantity").val();
    var product = $("#productlist option:selected").val();
    var address = $("#addresslist option:selected").val();
    var ordertype = $('input[name="optionsRadios"]:checked').val();
    var refinery = $("#refinerylist option:selected").val();
    var depot = $("#depotlist option:selected").val();
    var url = "PlaceOrder?clientcode=" + clientcode + "&quantity="+ quantity + "&productid=" + product + "&addressid="+ (typeof (address) === "undefined" ? "" : address)
            + "&refid=" + (typeof (refinery) === "undefined" ? "" : refinery)
            + "&depotid=" + (typeof (depot) === "undefined" ? "" : depot);
    $.ajax(
    {
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            if (data) {
                alert("Order Placed Successfully!");
            } else {
                alert("An Error occured while placing the order! Please inform belton park!");
            }
        },
        error: function (msg) {

        }
    });
    
}
