

var colToHide = [];
var datatable;

$(function () {


$.ajax(
    {
        type: 'POST',
        dataType: 'JSON',
        url: '/Students/ViewAllStudentsJson',
        data: { jsonInput: JSON.stringify(jsonInput) },
        success:
            function (response) {
                 //Generate HTML table.  
                convertJsonToHtmlTable(JSON.parse(response), $("#TableId"));
            },
        error:
            function (response) {
                alert("Error: " + response);
            }
    });

});

var access = 'No';
console.log(access);


//$.ajax({
//    url: '@URL.Action("ViewAllStudentsJson","Students")',

//    type: 'GET',
//    dataType: "json",
//    success: function (data, status) {
//        alert(data);
//    },
//    error: function () {
//        alert('error');
//    }

//});

$(function () {


});

Array.prototype.contains = function (v) {
    return this.indexOf(v) > -1;
}
var service = "ViewAllStudentsJson";
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

function Display () {


    
}