function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();


    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var time = moment(date).format('HH:mm');
    return [year, month, day].join('-') + " "+time;
}
$(function () {
    $("#loading").hide();
});