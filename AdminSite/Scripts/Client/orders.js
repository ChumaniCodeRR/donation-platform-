var DeliveredOrNot;
function toCurrency(value, signbef, signafter) {
    return signbef + (value.toFixed(2)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + signafter;
}

function toCurrencyD(value, signbef, signafter, digits) {
    return signbef + (value.toFixed(digits)).toString().replace(/(\d)(?=(\d{4})+(?!\d))/g, "$1,") + signafter;
}


function showOrHideCOC(mstShow) {
    if (mstShow) {

        $("#companyfrmg").show();
        $("#zonefrmg").show();
        $("#quantityfrmg").show();
        $("#productfrmg").show();
        $("#refineryfrmg").show();
        $("#depotfrmg").show();
        $("#placeorderfrmg").hide();
        $("#estimatefrmg").show();
        $("#deliveryaddressfrmg").hide(); 

    } else {

        $("#companyfrmg").hide();
        $("#zonefrmg").hide();
        $("#quantityfrmg").hide();
        $("#productfrmg").hide();
        $("#refineryfrmg").hide();
        $("#depotfrmg").hide();
        $("#placeorderfrmg").hide();
        $("#estimatefrmg").hide();
        $("#deliveryaddressfrmg").hide();
    }
}

function showOrHideRTL(mstShow) {
    if (mstShow) {

        $("#companyfrmg").show();
        $("#zonefrmg").show();
        $("#quantityfrmg").show();
       
        $("#productfrmg").show();
        $("#refineryfrmg").hide();
        $("#depotfrmg").hide();
        $("#placeorderfrmg").hide();
        $("#deliveryaddressfrmg").show();
        $("#estimatefrmg").show();

    } else {

        $("#companyfrmg").hide();
        $("#zonefrmg").hide();
        $("#quantityfrmg").hide();
        
        $("#productfrmg").hide();
        $("#refineryfrmg").hide();
        $("#depotfrmg").hide();
        $("#placeorderfrmg").hide();
        $("#deliveryaddressfrmg").hide();
        $("#estimatefrmg").hide();
    }
}
function estimate() {
    $("#orderid").val("");
    var clientcode = $("#clientlist option:selected").val();
    var quantity = $("#quantity").val();
    var customerref = $("#customer").val();
    var product = $("#productlist option:selected").val();
    var address = $("#addresslist option:selected").val();
    var zone = $("#zonelist option:selected").val();
    var ordertype = $('input[name="optionsRadios"]:checked').val();
    var refinery = $("#refinerylist option:selected").val();
    var depot = $("#depotlist option:selected").val();
    var url = "GenerateEstimate?clientcode=" + clientcode + "&quantity=" + quantity + "&customerref=" + customerref + "&productcode=" + product + "&address=" + (typeof (address) === "undefined" ? "" : address)
            + "&refid=" + (typeof (refinery) === "undefined" ? "" : refinery)
            + "&ordertype=" + (typeof (ordertype) === "undefined" ? "" : ordertype)
            + "&depotid=" + (typeof (depot) === "undefined" ? "" : depot);

    //if ((zone != "")||confirm("You did not select a Zone. The system will not be able to generate an automatic price estimate without it. Do you still want to proceed and wait for a manual estimate?") == true) {

        if (quantity <= 0 || product == "" || ordertype == "" || clientcode == "" ) {
            alert("Please fill in all the required fields [Company, Quantity, Product, Zone, COC or RRT]");
            return;
        }
        $.ajax(
        {
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            beforeSend: function () {
                clearprice();
                $('#loading').show();
            },
            success: function (data) {
                $('#loading').hide();
                if (data) {
                    $('#acceptquote').show();
                    $("#estimateval").text(toCurrency(data.PriceEstimate, "R ", ""));
                    $("#unitPrice").text(toCurrencyD(data.UnitPrice, "R ", "", 4));
                    $("#quanityV").text(toCurrency(data.Quantity, "", " Liters"));
                    $("#orderid").val(data.ID);
                    $("#disclaimer").text("Please note that this is only an estimate.");                    
                } else {
                    alert("Failed to generate estimate. Please try again later or contact IT at support@buznovation.com");
                }
            },
            error: function (msg) {
                $('#loading').hide();
            }
        });
    //} 


      
}
$('#productlist').on('click', function () {
    if ($('#productlist option').size() == 0) {
        $("#modaltext").text("No Pricing Available For " + DeliveredOrNot + ". Please contact us at orders@beltonpark.co.za");
        $('#myViewModal').modal('show');
    }
})
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
                alert("You have just confirm the order. Please note that the price might slightly change on your invoice.");
                location.href = "MyOrders";
            } else {
                alert("Failed to generate estimate. Please try again later or contact IT at support@buznovation.com");
            }
        },
        error: function (msg) {
            $('#loading').hide();
        }
    });
}
function deliverytype(myRadio) {    
    if (myRadio.value == "COC") {
        showOrHideCOC(true);
    } else {
        showOrHideRTL(true);        
    }
    DeliveredOrNot = myRadio.value;

    getProduct($('#clientlist').val(), DeliveredOrNot);
    getaddresses($('#clientlist').val(), DeliveredOrNot);//For RTL    
    getLoadingPoints($('#clientlist').val(), DeliveredOrNot);
    $("#depotlist").html("");
    clearprice();
}
function clearprice() {

    $("#estimateval").text("");
    $("#unitPrice").text("");
    $("#quanityV").text("");
    $("#orderid").val("");
    $("#disclaimer").text("");
}
$('#clientlist').on('change', function () {
    getaddresses(this.value, DeliveredOrNot);//For RTL
    getProduct(this.value, DeliveredOrNot);
    getLoadingPoints(this.value, DeliveredOrNot);
    getDepots(this.value, DeliveredOrNot);
})

$('#productlist').on('change', function () {
    getaddresses($('#clientlist').val(), DeliveredOrNot, this.value);
})

$('#refinerylist').on('change', function () {
    getDepots($('#clientlist').val(), this.value);
})

function getProduct(clientcode, isRTL) {
    //var clientcode = $("#clientlist option:selected").val();   
    $("#addresslist").html("");
    $("#productlist").html("");
    $.ajax(
    {
        type: "POST",
        url: "GetProduct?clientcode=" + clientcode +'&deliveredornot='+isRTL,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            $("#productlist").html("");
            $.each(data, function (index, product) {
                $("#productlist").append('<option value="' + product.ProductCode + '">' + product.ProductCode + '</option>');
            });
        },
        error: function (msg) {

        }
    });
}
function getLoadingPoints(clientcode, isRTL) {
    //var clientcode = $("#clientlist option:selected").val();   
    $("#refinerylist").html("");
    $.ajax(
    {
        type: "POST",
        url: "GetLoadingPoint?clientcode=" + clientcode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            $("#refinerylist").html("");
            $("#refinerylist").append('<option value=>- Select Loading Point -</option>');
            $.each(data, function (index, addr) {
                if (addr.Address != null) {
                    $("#refinerylist").append('<option value="' + addr.Address + '">' + addr.Address + '</option>');
                }
            });
        },
        error: function (msg) {

        }
    });
}


function getDepots(clientcode, refinerycode) {  
    var productcode =  $("#productlist option:selected").val();
    $("#depotlist").html("");
    $.ajax(
    {
        type: "POST",
        url: "GetDepots?clientcode=" + clientcode + "&productcode=" + productcode + "&refinerycode=" + refinerycode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            $("#depotlist").html("");
           
            $.each(data, function (index, addr) {
                $("#depotlist").append('<option value="' + addr.Address + '">' + addr.Address + '</option>');
            });
        },
        error: function (msg) {

        }
    });
}
function getaddresses(clientcode, isRTL, productcode) {
    //var clientcode = $("#clientlist option:selected").val();   
    $("#addresslist").html("");
    $.ajax(
    {
        type: "POST",
        url: "GetAddresses?clientcode=" + clientcode + "&deliveredornot=" + isRTL + "&productcode="+productcode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            $("#addresslist").html("");
            $.each(data, function (index, addr) {
                if (addr != null) {
                    $("#addresslist").append('<option value="' + addr.Address + '">' + addr.Address + '</option>');
                }
            });
        },
        error: function (msg) {
            
        }
    });
}
function getzone(productid, isRTL) {
    $("#zonelist").html("");
    $.ajax(
    {
        type: "POST",
        url: "GetZones?productid=" + productid,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //$('#loading').show();
        },
        success: function (data) {
            $("#zonelist").html("");
            $("#zonelist").append('<option value=""></option>');
            if (data == null || data.length == 0) {
                alert("We currently don't have a price for this item in the system. Please select another product or contact our Sales department at sales@beltonpark.co.za or orders@beltonpark.co.za");
                return;
            }
            $.each(data, function (index, zone) {
                $("#zonelist").append('<option value="' + zone.ID + '">' + zone.ZoneCode + '</option>');
            });

        },
        error: function (msg) {

        }
    });
}
window.onload = function (e) {
    showOrHideCOC(false);
    $('#acceptquote').hide();
    $("#customerref").hide();
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