var colToHide = [];
var datatable;

$(function () {


});


Array.prototype.contains = function (v) {
    return this.indexOf(v) > -1;
}
var service = "GetDonations";
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
function downloadCertificate(certificateId) {
    window.open("DownloadCertificate?certificateGUID=" + certificateId, '_blank');
}
function resendCertificate(certificateId) {
    $.ajax(
        {
            type: "POST",
            url: "ResendCertification?certificateGUID=" + certificateId,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (data) {
                if (data == true) {
                    alert("Cerficate Send Successfully!");
                }
                else {
                    alert("Failed to Send certificate!");
                }
                $('#loading').hide();
            },
            error: function (data) {
                $('#loading').hide();
                alert("An error occured while re-sending the certificate! Please contact I.T. support");
                location.reload();
            }
        });

}
function exportdonation() {
    var fromDate = moment($('#reservation').data('daterangepicker').startDate).format('YYYY-MM-DD HH:mm');
    var toDate = moment($('#reservation').data('daterangepicker').endDate).format('YYYY-MM-DD HH:mm');
    window.location = "Export?fromDate=" + fromDate + "&toDate=" + toDate;
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
    if (datatable) {
        datatable.destroy();
        datatable = null;
    }
    $('#audits').DataTable().destroy();
    $('#location').html("");

    var type = $('input[name="invoicetype"]:checked').val();
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
                    var downloadbutton = "";
                    var resendbutton = "";
                    if (col.Status == "COMPLETE") {
                        totalAmt = totalAmt + parseFloat(col.NetAmount);
                        downloadbutton = "<button onclick='downloadCertificate(\"" + col.CertificateID + "\");'>Download Certificate</button>";
                        resendbutton = "<button onclick='resendCertificate(\"" + col.CertificateID + "\");'>Resend Certificate</button>";
                    }
                    var column = "";
                    /*<th class="text-left">Donation Date</th>
                            <th class="text-left">Donation Ref</th>
                            <th class="text-left">Donor Type</th>
                            <th class="text-left">Donor Name</th>
                            <th class="text-left">Email</th>
                            <th class="text-left">Amount</th>
                            <th class="text-left">Fee</th>
                            <th class="text-left">Net</th>
                            <th class="text-left">Certificate</th>
                            <th class="text-left">Status</th>
                            <th class="text-left"></th>*/
                    column += '<td class="text-left">' + col.TransactionDate + '</td>';
                    column += '<td class="text-left">' + col.DonationRefOnCertificate + '</td>';
                    column += '<td class="text-left">' + col.DonorType + '</td>';
                    column += '<td class="text-left">' + col.DonorName + '</td>';
                    column += '<td class="text-left">' + col.DonorEmail + '</td>';
                    column += '<td class="text-left">' + toCurrency(parseFloat(col.Amount), 'R', '') + '</td>';
                    column += '<td class="text-left">' + toCurrency(parseFloat(col.Fees), 'R', '') + '</td>';
                    column += '<td class="text-left">' + toCurrency(parseFloat(col.NetAmount), 'R', '') + '</td>';
                    column += '<td class="text-left">' + col.SendStatus + '</td>';
                    column += '<td class="text-left">' + col.Status + '</td>';
                    column += '<td class="text-left">' + col.Student + '</td>';
                    column += '<td class="text-left">' + col.DonatorType + '</td>';
                    column += "<td class='text-left'>" + resendbutton + "</td>";
                    column += "<td class='text-left'>" + downloadbutton + "</td>";
                    var rowclass = "";
                    if (col.Status == "COMPLETE") {
                        rowclass = "danger";
                    }
                    else {
                        rowclass = "info";
                    }
                    trHTML += '<tr class="' + rowclass + '">' + column + '</tr>';
                    $('#location').html(trHTML);

                });

                var empty = "";
                var tdtotal = "";
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + toCurrency(parseFloat(totalAmt), 'R ', '') + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left">' + empty + '</td>';
                tdtotal += '<td class="text-left"><strong>' + empty + ' </strong></td>';
                var totalRow = '<tr class="info">' + tdtotal + '</tr>';
                $('#location').append(totalRow);
                $('#donationrec').text(toCurrency(parseFloat(totalAmt), 'R ', ''));
                $('#loading').hide();


                datatable = $('#audits').DataTable({
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    //"ordering": true,
                    "info": true,
                    "pageLength": 50
                    //"autoWidth": true
                });

            },
            error: function (msg) {
                location.reload();
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