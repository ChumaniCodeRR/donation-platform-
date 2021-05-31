$(document).ready(function () {
    $("#vm-id-row-individual input[required]").prop('required', true);
    $("#vm-id-row-organisation input[required]").prop('required', false);
    vm_radio_button_checks();
    vm_name_radio_button_checking();
    vm_existing_individual_member_checks();
    vm_existing_organization_member_checks();
    vm_checkbox_selection();
    vm_datepicker();
    // $("#vm-id-row-organisation input[required]").prop('required',false);



    // $('#vm-submit').on("click",function() {
    //     alert('clicked');
    // });


    //$("#vm-id-row-individual input[required]").css('display', 'none');


    // $("#vm-id-row-organisation input[required]").prop('required',false);


    // Example starter JavaScript for disabling form submissions if there are invalid fields
    (function () {
        'use strict';
        window.addEventListener('load', function () {
            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            var forms = document.getElementsByClassName('needs-validation');
            // Loop over them and prevent submission
            var validation = Array.prototype.filter.call(forms, function (form) {
                form.addEventListener('submit', function (event) {
                    if (form.checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                    form.classList.add('was-validated');
                }, false);
            });
        }, false);
    })();
});

function vm_datepicker() {
    $(function () {

        var dtToday = new Date();

        var month = dtToday.getMonth() + 1;
        var day = dtToday.getDate();
        var year = dtToday.getFullYear();

        if (month < 10)
            month = '0' + month.toString();
        if (day < 10)
            day = '0' + day.toString();

        var maxDate = year + '-' + month + '-' + day;
        $('#vm-donation-billingdate').attr('min', maxDate);

    });
}

function vm_checkbox_selection() {
    //$('input[type="checkbox"]').click(function () {
    //    $('input[type="checkbox"]').not(this).prop("checked", false);
    //});

    $(':checkbox').on('change', function () {
        var th = $(this), name = th.prop('name');
        var value1 = [];
        if (th.is(':checked')) {
            $(':checkbox[name="' + name + '"]').not($(this)).prop('checked', false);
        }
    });



    //$(document).ready(function () {
    //    //$("button").click(function () {
    //    var favorite = [];
    //    $.each($("input[name='ballet']:checked"), function () {
    //        favorite.push($(this).val());
    //    });
    //    //alert("My favourite sports are: " + favorite.join(", "));
    //    //});
    //});


    //$('input[type="checkbox"]').on('change', function () {
    //    $('input[name="' + this.name + '"]').not(this).prop('checked', false);
    //});

    //$(".list-group-item").each(function (i, li) {
    //    var currentli = $(li);
    //    $(currentli).find("#vm-monthly-freq").on('change', function () {
    //        $(currentli).find("#vm-quarterly-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-biannually-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-annual-freq").not(this).prop('checked', false);        
    //    });

    //    $(currentli).find("#vm-quarterly-freq").on('change', function () {
    //        $(currentli).find("#vm-monthly-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-biannually-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-annual-freq").not(this).prop('checked', false);  
    //    });

    //    $(currentli).find("#vm-biannually-freq").on('change', function () {
    //        $(currentli).find("#vm-monthly-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-quarterly-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-annual-freq").not(this).prop('checked', false);
    //    });

    //    $(currentli).find("#vm-annual-freq").on('change', function () {
    //        $(currentli).find("#vm-monthly-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-quarterly-freq").not(this).prop('checked', false);
    //        $(currentli).find("#vm-biannually-freq").not(this).prop('checked', false);
    //    });

    //});
}

function vm_name_radio_button_checking() {

    $('#vm-id-row-reccuting').css('display', 'none');

    $('input[name="vm-name-radio-donatortype"]').click(function () {

        if ($(this).is(':checked')) {

            var vm_name_radio_donatortype = $(this).val();

            //$('#vm-id-row-reccuting').css('display', 'none');

            if ($(this).val() == "Recurring Donation") {

                $('#vm-id-row-reccuting').css('display', 'block');

                $('#vm-donation-billingdate').css('display', 'block');
                $('#vm-monthly-freq').css('display', 'block');
                $('#vm-quarterly-freq').css('display', 'block');
                $('#vm-reccuring-member').css('display', 'block');
                $("#vm-reccuring-member").prop('required', true);
            } else {

                $('#vm-id-row-reccuting').css('display', 'none');

                $('#vm-donation-billingdate').css('display', 'none');
                $('#vm-monthly-freq').css('display', 'none');
                $('#vm-quarterly-freq').css('display', 'none');
                $('#vm-reccuring-member').css('display', 'none');
                $("#vm-reccuring-member").prop('required', false);
                //$('#vm-id-row-reccuting').css('display', 'block');

                ////if ($('input[name="vm-name-radio-donatortype"]').val() == true) {
                //$('#vm-donation-billingdate').css('display', 'block');
                //$('#vm-monthly-freq').css('display', 'block');
                //$('#vm-quarterly-freq').css('display', 'block');
                //$('#vm-reccuring-member').css('display', 'block');
                //}
                //else {
                //    $('#vm-donation-billingdate').css('display', 'none');
                //    $('#vm-monthly-freq').css('display', 'none');
                //    $('#vm-quarterly-freq').css('display', 'none');
                //    $('#vm-reccuring-member').css('display', 'none');
                //}
            }
        }
    });
}



function vm_radio_button_checks() {
    $('input[name="vm-name-radio-capacity"]').click(function () {
        if ($(this).is(':checked')) {

            var vm_radio_capacity = $(this).val();
            var org_inputs = document.querySelectorAll('#vm-id-row-organisation input');
            var individual_inputs = document.querySelectorAll('#vm-id-row-individual input');



            if ($(this).val() == "Individual") {
                // individual checks
                individual_inputs.forEach(function (input) {
                    input.required = true;
                });
                org_inputs.forEach(function (input) {
                    input.required = false;
                });
                //$("#vm-id-row-individual input[required]").prop('required',true);
                //$("#vm-id-row-organisation input[required]").prop('required',false);


                $('#vm-id-row-individual').css('display', 'block');
                $('#vm-id-row-organisation').css('display', 'none');

                if ($('input[name="vm-radio-existing_member"]').val() == "true") {
                    $('#vm-individual-member-number').css('display', 'block');
                    $('#vm-individual-group-member-number').css('display', 'block');
                    $("#vm-individual-member-number").prop('required', false);
                } else {
                    $('#vm-individual-member-number').css('display', 'none');
                    $('#vm-individual-group-member-number').css('display', 'none');
                    $('#vm-individual-member-number').prop('required', false);
                }



            } else if ($(this).val() == "Organisation") {
                //organisation checks

                //$("#vm-id-row-individual input[required]").prop('required',false);
                //$("#vm-id-row-organisation input[required]").prop('required',true);
                individual_inputs.forEach(function (input) {
                    input.required = false;
                });
                org_inputs.forEach(function (input) {
                    input.required = true;
                });

                $('#vm-id-row-individual').css('display', 'none');
                $('#vm-id-row-organisation').css('display', 'block');
                if ($('input[name="vm-pps-member"]').val() == "true") {
                    $('#vm-organisation-pps-member-number').css('display', 'block');
                    $('#vm-organization-group-member-number').css('display', 'block');
                    $("#vm-organisation-pps-member-number").prop('required', false);
                } else {
                    $('#vm-organisation-pps-member-number').css('display', 'none');
                    $('#vm-organization-group-member-number').css('display', 'none');
                    $('#vm-organisation-pps-member-number').prop('required', false);
                }
            }

        }
    });
}


// vm-name-field-state, vm-radio-capacity

function vm_existing_individual_member_checks() {
    $('input[name="vm-radio-existing_member"]').click(function () {

        if ($(this).is(':checked')) {
            if ($(this).val() == "true") {
                console.log($(this).val());
                $('#vm-individual-member-number').css('display', 'block');
                $('#vm-individual-group-member-number').css('display', 'block');
                $("#vm-individual-member-number").prop('required', true);
            } else if ($(this).val() == "false") {
                $('#vm-individual-member-number').css('display', 'none');
                $('#vm-individual-group-member-number').css('display', 'none');
                $('#vm-individual-member-number').prop('required', false);
                console.log($(this).val());
            }
        }

    });
}
function vm_existing_organization_member_checks() {
    $('input[name="vm-pps-member"]').click(function () {

        if ($(this).is(':checked')) {
            if ($(this).val() == "true") {
                $('#vm-organisation-pps-member-number').css('display', 'block');
                $('#vm-organization-group-member-number').css('display', 'block');
                $("#vm-organisation-pps-member-number").prop('required', true);
            } else if ($(this).val() == "false") {
                $('#vm-organisation-pps-member-number').css('display', 'none');
                $('#vm-organization-group-member-number').css('display', 'none');
                $('#vm-organisation-pps-member-number').prop('required', false);
            }
        }

    });
}

/* for menu changing in home page , index.cshtml */

var coll = document.getElementsByClassName("collapsible");
var col2 = document.getElementsByClassName("collapsible-sub")
var i;

for (i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var content = this.nextElementSibling;
        if (content.style.maxHeight) {
            content.style.maxHeight = null;
        } else {
            content.style.maxHeight = content.scrollHeight + "px";
        }
    });
}

for (i = 0; i < col2.length; i++) {
    col2[i].addEventListener("click", function () {
        this.classList.toggle("slide");
        var content = this.nextElementSibling;
        if (content.style.maxHeight) {
            content.style.maxHeight = null;
        } else {
            content.style.maxHeight = content.scrollHeight + "px";
        }
    });
}

//$(document).ready(function () {
//    $("#one11").animate({
//        left: "0"
//    }, {
//        duration: 2000
//    });
//    //$("#two").animate({
//    //    left: "0"
//    //}, {
//    //    duration: 2250
//    //});
//    //$("#three").animate({
//    //    left: "0"
//    //}, {
//    //    duration: 2500
//    //});
//});