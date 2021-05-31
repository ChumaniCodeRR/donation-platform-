$(function () {
    
    $('#settings').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true
    });
});

function save() {
    var selecteduser = $("#userlist").val();
    var selected = $("#clientlistm").val();
    var jsondata = { user: selecteduser, list: selected };
    $.ajax(
        {
            type: "POST",
            url: "SaveAssignment",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(jsondata),
            cache: false,
            beforeSend: function () {

            },
            success: function (data) {
                if (data == true) {
                    location.reload();
                    alert("Saved!");
                } else {
                    alert("An error occured while saving! Please try again later or contact Belton park")
                }
            },

            error: function (msg) {
                alert("An error occured while saving! Please try again later or contact Belton park. N156423")
            },
            complete: function () {

            }
        });
}
    function saveroles() {
        var selecteduser = $("#userlist").val();
        var selected = $("#rolelistm").val();
        var jsondata = { user: selecteduser, list: selected };
        $.ajax(
            {
                type: "POST",
                url: "SaveAssignment",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(jsondata),
                cache: false,
                beforeSend: function () {

                },
                success: function (data) {
                    if (data == true) {
                        location.reload();
                        alert("Saved!");
                    } else {
                        alert("An error occured while saving! Please try again later or contact Belton park")
                    }
                },

                error: function (msg) {
                    alert("An error occured while saving! Please try again later or contact Belton park. N156423")
                },
                complete: function () {

                }
            });
  
}