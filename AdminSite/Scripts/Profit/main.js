var colToHide = [];
Array.prototype.contains = function (v) {
    return this.indexOf(v) > -1;
}
var service = "GetProfit";
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
function contains(a, obj) {
    var i = a.length;
    while (i--) {
        if (a[i] === obj) {
            return true;
        }
    }
    return false;
}
function toCurrency(value, signbef, signafter) {
    return signbef + (value.toFixed(2)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + signafter;
}
function refresh(e) {
    if (e) {
        var me = $(e);
        e.preventDefault();

        if (me.data == false) {
            return;
        }

        me.data = true;
    }

    var type = $('input[name="invoicetype"]:checked').val();
    //var fromDate = ($("#fromDate").val());
    //var toDate = ($("#toDate").val());
    var fromDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var toDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');
    var username = "M";

    $.ajax(
    {
        type: "POST",
        url: service + "?&type=" + type + "&fromDate=" + fromDate + "&toDate=" + toDate + "&username=" + username,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (data) {
            $('#location').html("No Records!");           
            var trHTML = '';
            var poArray = Array();
            var creatorArray = Array();
            var totalDiff = 0;
            var totalSold = 0;
            var perloss = 0;
            var totalAmt = 0;
            $.each(data, function (i, col) {
                var diff = 0;
                if (poArray.contains(col.PurchaseOrder)){
                    diff = 0;
                }
                else{
                    poArray.push(col.PurchaseOrder);
                    diff = col.Diff;
                }
                if (!creatorArray.contains(col.Name)) {
                    creatorArray.push(col.Name);
                }
               
                var column = "";
                totalDiff = totalDiff + diff;
                totalSold = totalSold + parseFloat(col.InvoiceVolume);
                totalAmt = totalAmt + parseFloat(col.InvoiceValue);

                column += '<td class="text-left">' + col.PurchaseOrder + '</td>';
                column += '<td class="text-left">' + col.InvoiceNumber + '</td>';
                column += '<td class="text-left">' + col.InvoiceDate + '</td>';
                column += '<td class="text-left">' + col.InsertDate + '</td>';
                column += '<td class="text-left">' + col.Name + '</td>';
                column += '<td class="text-left">' + toCurrency(parseFloat(col.InvoiceValue), '', '') + '</td>';
                column += '<td class="text-left">' + toCurrency(parseFloat(col.InvoiceVolume),'','') + '</td>';
                column += '<td class="text-left">' + toCurrency(parseFloat(col.UpliftedVol),'','') + '</td>';
                column += '<td class="text-left">' + toCurrency(parseFloat(diff),'','') + '</td>';
                var rowclass = "";
                if (parseFloat(col.Diff) < 0) {
                    rowclass = "info";
                }
                else if (parseFloat(col.Diff) > 0) {
                    rowclass = "danger";
                }
                trHTML += '<tr class="'+rowclass+'">' + column + '</tr>';
                $('#location').html(trHTML);

            });
            $('#creatorfilter').append("<option value=''></option>");
            $.each(creatorArray, function (i, col) {
                $('#creatorfilter').append("<option value='"+col+"'>"+col+"</option>");
            });
            perloss = (totalDiff / totalSold) * 100;
            var tdtotal = "";
            var empty = "";
            tdtotal += '<td class="text-left">' + empty + '</td>';
            tdtotal += '<td class="text-left">' + empty + '</td>';
            tdtotal += '<td class="text-left">' + empty + '</td>';
            tdtotal += '<td class="text-left">' + empty + '</td>';
            tdtotal += '<td class="text-left">' + empty + '</td>';
            tdtotal += '<td class="text-left">' + toCurrency(parseFloat(totalAmt), 'R ', '') + '</td>';
            tdtotal += '<td class="text-left">' + toCurrency(parseFloat(totalSold),'',' L') + '</td>';
            tdtotal += '<td class="text-left">' + empty + '</td>';
            tdtotal += '<td class="text-left"><strong>' + toCurrency(parseFloat(totalDiff), '', '') + ' L</strong></td>';
            var totalRow = '<tr class="info">' + tdtotal + '</tr>';
            $('#location').append(totalRow);
            $('#litersold').text(toCurrency(parseFloat(totalSold), '', ''));
            $('#lostliters').text(toCurrency(parseFloat(totalDiff), '', ' L'));
            $('#lostperc').text(toCurrency(parseFloat(perloss), '', ' %'));
            $('#loading').hide();

            if (parseFloat(totalDiff) > 0) {
                $("#statusbox").removeClass("info-box bg-green");
                $("#statusbox").addClass("info-box bg-red");
            } else {
                $("#statusbox").removeClass("info-box bg-green");
                $("#statusbox").removeClass("info-box bg-red");
                $("#statusbox").addClass("info-box bg-green");
            }
        },
        error: function (msg) {
        },
        complete: function () {
            if (me) {
                me.data = false;
            }
        }
    });

}
function inArray(array, value) {

    for (var i = 0; i < array.length; i++) {
        if (array[i] == value) {
            return true;
        }
    }
    return false;
}
window.onload = function (e) {
    var currentDateY = new Date(new Date().getTime() - 24 * 60 * 60 * 1000);
    var dayY = currentDateY.getDate()
    var monthY = currentDateY.getMonth() + 1
    var yearY = currentDateY.getFullYear()

    var currentDate = new Date(new Date().getTime());
    var day = currentDate.getDate()
    var month = currentDate.getMonth() + 1
    var year = currentDate.getFullYear()
    $('#fromDate').datepicker({
        format: 'yyyy-mm-dd'
    });

    $('#toDate').datepicker({
        format: 'yyyy-mm-dd'
    });

    var $rows = $('#location tr');


    $("#refresh").click(function (e) {
        refresh(e);
    });
    $.support.cors = true;
};