$(function () {
    $('#example').DataTable({
        "ajax": '@Url.Action("EachStudentDonationDashboard", "Home")',
        "columns":
            [
                { "list": "Amount" },
                { "list": "TransactionDate" },
            ]
    });
});