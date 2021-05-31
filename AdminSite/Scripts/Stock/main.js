var colToHide = [];

var service = 'Order';
var serviceLogin = 'Login/';
var role = "";
function format(n) {
    var v = parseFloat(n);
    return v.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}
function setCookie(cname, cvalue, exdays) {
    localStorage.setItem(cname, cvalue);
}
function getCookie(cname) {
    return localStorage.getItem(cname);
}

function process(data, msg) {

    if (data.Rows.length > 0) {
        var trHTML = '';
        var totalVolume = 0.0;
        if (data.Filters) {
            $.each(data.Filters, function (index, filter) {
                $('#' + filter.Key).html(filter.Value);
            });
        }
        role = data.UserRole;
        var LoginUserRole = data.LoginUserRole;
        totalVolume = data.TotalVolume;
        if (totalVolume) $('#totalliters').text((totalVolume.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")));
        $.each(data.Rows, function (i, item) {
            var column = '';
            var deliveryID = null;
            var saleorderNbr = null;
            var colors = "";
            var deliveryStatus = "";
            var purchaseOrder = "";
            var comment = "";

            $.each(item.col, function (j, col) {
                col = col.replace("&", " and ");
                if (j == 2) {
                    purchaseOrder = col;
                    column += '<td class="text-left">' + col + '</td>';
                }
                else if (j == 3) {
                    column += '<td class="text-left"><a href="#">' + col + '</a></td>';
                }
                else if (j == 8) {
                    saleorderNbr = col;
                    column += '<td class="text-left"><a href="ClientSalesOrder?salesordernumber=' + saleorderNbr + '" target="_blank">' + col + '</a></td>';
                }
                else if (j == 12) {
                    deliveryID = col;
                    //column += '<td class="text-left">' + col + '</td>';
                } else if (j == 10) {
                    comment = col;
                    if (role != "Admin")
                    { column += '<td class="text-left comment"><textarea>' + (col == null ? "" : col) + '</textarea><button onClick="update(this)">Save</button></td>'; }
                } else if (j == 6) {
                    //Hide ordered volume

                }
                
                else if (j == 16) {
                    if (LoginUserRole != "Client") {
                        column += '<td class="text-left">' + col + '</td>';
                    }
                }
                else if (j == 13) {
                    deliveryStatus = col;
                }
                else if (j == 15) {
                    column += '<td title="' + comment + '" class="text-left">' + col + '</td>';
                }
                else if (j == 9) {
                    //totalVolume = totalVolume + parseFloat(col);
                    column += '<td class="text-left" volume="' + col + '">' + format(col) + '</td>';
                }
                else if (j == 14) {
                    if (col == '01/01/1900' || col == '01-01-1900' || col == '1900-01-01') {
                        column += '<td class="text-left"></td>';
                    } else {
                        column += '<td class="text-left">' + col + '</td>';
                    }
                }
                else {
                    column += '<td class="text-left">' + col + '</td>';
                }

                if (j == 11) {

                    if (col == "COC") {
                        colors = "coc";
                    }
                    else if (col == "Belton Holdings") {
                        colors = "belton";
                    }
                }
            });
            var selection = '<select class="status"><option value="">-------Select Status------- </option><option value="Delivered">Delivered</option><option value="On Route To Customer">On Route To Customer</option><option value="On Route To Load">On Route To Load</option><option value="Loaded">Loaded</option></select>';
            selection = selection.replace('value="' + deliveryStatus + '"', 'value="' + deliveryStatus + '" selected');
            var rowTR = '<tr class="' + colors + '" data-deliveryid="' + deliveryID + '" data-salesordernbr="' + saleorderNbr + '" data-purchaseOrder="' + purchaseOrder + '">' + column + '</tr>';
            var $rowTR = $($.parseHTML(rowTR));
            var finalRow = $($rowTR.find(".comment")[0]).prepend(selection);
            trHTML += '<tr class="' + colors + '" data-deliveryid="' + deliveryID + '" data-salesordernbr="' + saleorderNbr + '" data-purchaseOrder="' + purchaseOrder + '">' + $rowTR.html() + '</tr>';
            $('#location').html(trHTML);

            var date = new Date();
            var n = date.toDateString();
            var time = date.toLocaleTimeString();

            $('#lastupdate').html(msg + "<br>" + date.toLocaleString());
            var records = { 'lastupdate': date.toLocaleString(), 'data': data };

            // Put the object into storage
            localStorage.setItem('recordStored', JSON.stringify(records));


        });
        var currenttable = $('#location').html();
        currenttable += '<tr id="orig" class="total"><td></td><td></td><td></td><td></td><td></td><td></td><td></td> <td><strong>Total Liters</strong></td><td id="totalvolume">' + (totalVolume) + '</td><td></td><td></td><td></td><td></td></tr>';
        $('#location').html(currenttable);

        if (role == "Admin") {
            //$('th:nth-child(' + (10 + 1) + ')').hide();
        }
        $('#loading').hide();
    } else {
        $('#loading').hide();
        alert("No Records!");
    }
}

function refresh(e,type) {
    if (e) {
        var me = $(e);
        e.preventDefault();

        if (me.data == false) {
            return;
        }

        me.data = true;
    }

    $.ajax(
        {
            type: "GET",
            url: service + "?sessionId=" + getCookie("sessionId") + "&type="+type,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (data) {
                if (data != "expired") {

                    process(data, "");
                }

                else {
                    $("#login-modal").modal();
                }
            },

            error: function (msg) {
                //setTimeout(loadInitData("Updated Failed"), 5);
                loadInitData("Updated Failed");
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
function loadInitData(msg) {
    try {
        var records = JSON.parse(localStorage.getItem("recordStored"));
        if (records) {
            var lastupdate = $('#lastupdate');
            lastupdate.html("<strong>" + msg + "</strong><p>Data From the " + records.lastupdate + " are currently displayed</p>");
            $('#loading').hide();
            data = records.data;
            process(data, msg);
        }
    } catch (ex) {

    }
    $('#loading').hide();


}
function update(button){
	
	var row = button.closest('tr');
	var deliveryID = row.getAttribute("data-deliveryid");
	var salesOrderNbr = row.getAttribute("data-salesordernbr");
	var purchaseOrder = row.getAttribute("data-purchaseOrder");
	var comment = null;
	var status = null;
	$(row).find('textarea').each (function(i, textarea) {
	  comment = $(textarea).val();
	});
	$(row).find('select').each(function (i, select) {
	    status = $(select).val();
	});
    //alert("delivery "+ deliveryID + " salesOrderNbr "+ salesOrderNbr + " Comment " + comment );
	if (comment != null && comment.trim() == "") {
	    alert("You can not put an empty comment. Please enter a comment!");
	}
	else if (status != null && status.trim() == "") {
	    alert("Please select a status!");
	}
	else {
	    updateComment(deliveryID, comment, salesOrderNbr, purchaseOrder, status);
	}
}

function updateComment(deliveryId, comment, salesOrderNbr, purchaseOrder, status) {
	var serv = service;
	if((deliveryId == "null")||(deliveryId == null)||(deliveryId == "")){
	    serv = serv + '/UpdateComment?salesOrderNbr=' + encodeURIComponent(salesOrderNbr) + '&status='+encodeURI(status)+ '&Comment=' + encodeURIComponent(comment) + '&purchaseOrder=' + encodeURIComponent(purchaseOrder);
	}else{
	    serv = serv + '/UpdateComment?deliveryId=' + deliveryId + '&status=' + encodeURI(status) + '&Comment=' + encodeURIComponent(comment) + ' &salesOrderNbr=' + encodeURIComponent(salesOrderNbr) + '&purchaseOrder=' + encodeURIComponent(purchaseOrder);
	}
    $.ajax(
    {
        type: "GET",
        url: serv + "&sessionId=" + getCookie("sessionId"),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
		beforeSend : function (){
             $('#loading').show();  
        },
		success: function (data) {
		    if (data != "expired") {
		        try {
		            if (data == true) {
		                alert("Comment saved successfully!");
		            }
		            else if (data.Status == "cancelled") {
		                alert("This sales order has been cancelled by " + data.UserFirstName + " " + data.UserSurname + " on the " + data.UpdateDate + " \nReason:" + data.SalesOrderCancelReason + "\nPlease refresh this page by pressing F5 or clicking the refresh button at the top of the page!");
		            }
		            else {
		                alert("An error occured while saving this comment! Please Refresh and try again.");
		            }
		        } catch (ex) {
		            alert("An error occured while saving this comment! Please Refresh and try again.");
		        }
		    } else {
		        $("#login-modal").modal();
		    }
		  $('#loading').hide(); 
        },

        error: function (msg) {
			alert("An error occured while saving this comment! Please try again.");
			$('#loading').hide(); 
        }
    });
	
}

function searchFunction(col, input) {
    // Declare variables 
    var filter, table, tr, td, i, voltd;
    var totalVolume = 0.0;
    var volcol = 8;
    filter = input.value.toUpperCase().replace("&"," AND ");
    table = document.getElementById("tableData");
    tr = table.getElementsByTagName("tr");
    /*if (role == "Admin") {
        if (col > 10) {
            col = col - 1;
        }
    }*/
    $('table tr.total').remove();
    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[col];
        voltd = tr[i].getElementsByTagName("td")[volcol];

        if (td) {
            if (filter == "NOT COC") {
                if (td.innerHTML.toUpperCase().replace("&"," AND ").indexOf("COC") > -1) {
                    tr[i].style.display = "none";
                   
                } else {
                    tr[i].style.display = "";
                    totalVolume = totalVolume + (isNaN(parseFloat(voltd.getAttribute("volume"))) ? 0 : parseFloat(voltd.getAttribute("volume")));
                }
            }
            else if (filter == "NOT BELTON TRANSPORT") {
                if (td.innerHTML.toUpperCase().replace("&"," AND ").indexOf("BELTON HOLDINGS") > -1) {
                    tr[i].style.display = "none";
                } else {
                    tr[i].style.display = "";
                    totalVolume = totalVolume + (isNaN(parseFloat(voltd.getAttribute("volume"))) ? 0 : parseFloat(voltd.getAttribute("volume")));
                }
            }
            else{
                if (td.innerHTML.toUpperCase().replace("&"," AND ").indexOf(filter) > -1) {
                    tr[i].style.display = "";
                    totalVolume = totalVolume + (isNaN(parseFloat(voltd.getAttribute("volume"))) ? 0 : parseFloat(voltd.getAttribute("volume")));
                } else {
                    tr[i].style.display = "none";
                }
            }
        }
    }
    $('#totalliters').text((totalVolume.toFixed(2)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
    $('table tr.filterv').remove();
    //if (filter.length > 0) {
    $('table tr.total').hide();
    var currenttable = $('#location').html();
    currenttable += '<tr class="filterv"><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td><strong>Total Liters</strong></td><td id="totalvolume">' + format(totalVolume) + '</td><td></td><td></td><td></td><td></td></tr>';
    $('#location').html(currenttable);

}
function exportdata() {
    $.ajax(
        {
            type: "POST",
            url: "Export?sessionId=" + getCookie("sessionId"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            beforeSend: function (xhr) {
                $('#loading').show();
            },
            success: function (data) {
                $('#loading').hide();
                console.log(data.size);
                var link = document.createElement('a');
                link.href = data;
                link.click();
                alert("Export Successful! Please check your email!");
            },

            error: function (msg) {
                $('#loading').hide();
                alert("Export Failed!");
            },
            complete: function () {
                $('#loading').hide();
            }
        });
}
window.onload = function (e) {
    var $formLogin = $('#login-form');
    var $formLost = $('#lost-form');
    var $formRegister = $('#register-form');
    var $divForms = $('#div-forms');
    var $modalAnimateTime = 300;
    var $msgAnimateTime = 150;
    var $msgShowTime = 2000;    
    $("#loadinglogin").hide();
    $("#login-modal").modal();
    $("form").submit(function () {
        switch (this.id) {
            case "login-form":
                var $lg_username = $('#login_username').val();
                var $lg_password = $('#login_password').val();

                //login ajax
                $.ajax(
                {
                    type: "GET",
                    url: serviceLogin + "?username=" + encodeURI($lg_username) + "&password=" + encodeURI($lg_password),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    beforeSend: function (xhr) {
                        $("#loadinglogin").show();
                        $('#loading').show();
                    },
                    success: function (data) {
                        msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "success", "glyphicon-ok", "Login OK");
                        if (data == false) {
                            setCookie("sessionId", null, 1);
                            msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", "Invalid Credentials");
                        } else {
                            setCookie("sessionId", data.sessionId, 1);
                            refresh();
                            $('#login-modal').modal('hide');
                        }
                        $("#loadinglogin").hide();
                    },

                    error: function (xhr, status, error) {
                        msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", xhr.responseText);
                        $("#loadinglogin").hide();

                    },
                    complete: function () {

                    }
                });
                return false;

                break;
            case "lost-form":
                var $ls_email = $('#lost_email').val();
                if ($ls_email == "ERROR") {
                    msgChange($('#div-lost-msg'), $('#icon-lost-msg'), $('#text-lost-msg'), "error", "glyphicon-remove", "Send error");
                } else {
                    msgChange($('#div-lost-msg'), $('#icon-lost-msg'), $('#text-lost-msg'), "success", "glyphicon-ok", "Send OK");
                }
                return false;
                break;
            case "register-form":
                var $rg_username = $('#register_username').val();
                var $rg_email = $('#register_email').val();
                var $rg_password = $('#register_password').val();
                if ($rg_username == "ERROR") {
                    msgChange($('#div-register-msg'), $('#icon-register-msg'), $('#text-register-msg'), "error", "glyphicon-remove", "Register error");
                } else {
                    msgChange($('#div-register-msg'), $('#icon-register-msg'), $('#text-register-msg'), "success", "glyphicon-ok", "Register OK");
                }
                return false;
                break;
            default:
                return false;
        }
        return false;
    });
    $('#login_register_btn').click(function () { modalAnimate($formLogin, $formRegister) });
    $('#register_login_btn').click(function () { modalAnimate($formRegister, $formLogin); });
    $('#login_lost_btn').click(function () { modalAnimate($formLogin, $formLost); });
    $('#lost_login_btn').click(function () { modalAnimate($formLost, $formLogin); });
    $('#lost_register_btn').click(function () { modalAnimate($formLost, $formRegister); });
    $('#register_lost_btn').click(function () { modalAnimate($formRegister, $formLost); });
    function modalAnimate($oldForm, $newForm) {
    var $oldH = $oldForm.height();
    var $newH = $newForm.height();
    $divForms.css("height", $oldH);
    $oldForm.fadeToggle($modalAnimateTime, function () {
        $divForms.animate({ height: $newH }, $modalAnimateTime, function () {
            $newForm.fadeToggle($modalAnimateTime);
        });
    });
    }
    
    function msgFade($msgId, $msgText) {
    $msgId.fadeOut($msgAnimateTime, function () {
        $(this).text($msgText).fadeIn($msgAnimateTime);
    });
    }
    
    function msgChange($divTag, $iconTag, $textTag, $divClass, $iconClass, $msgText) {
    var $msgOld = $divTag.text();
    msgFade($textTag, $msgText);
    $divTag.addClass($divClass);
    $iconTag.removeClass("glyphicon-chevron-right");
    $iconTag.addClass($iconClass + " " + $divClass);
    setTimeout(function () {
        msgFade($textTag, $msgOld);
        $divTag.removeClass($divClass);
        $iconTag.addClass("glyphicon-chevron-right");
        $iconTag.removeClass($iconClass + " " + $divClass);
    }, $msgShowTime);
    }
    
	var $rows = $('#location tr');
	$('#search').keyup(function() {
		var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
		
		$rows.show().filter(function() {
			var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
			return !~text.indexOf(val);
		}).hide();
	});

	$("#refresh").click(function (e) {
	    refresh(e);
	});
    $.support.cors = true;
	//loadInitData("Initial data loaded ... Please wait for new data");
	refresh();
    //setInterval(function(){ refresh(); }, 60000 * 30);
};
