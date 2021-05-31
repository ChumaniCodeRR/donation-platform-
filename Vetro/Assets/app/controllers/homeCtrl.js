angular.module('home', [])
    .controller('homeCtrl', ['$scope', '$http', '$location', '$window', '$document', function ($scope, $http, $location, $window, $document) {
        $scope.donatebutton = true;
        $scope.loading = false;
        $scope.countries = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];
        $scope.alert = function () {
            alert("WOW");
        };
        $scope.display = "display:none;";
        $scope.testangular = "Hello test";

        $scope.donation = {
            Amount: "",
            DonatorType: "",
            BillingDate: "",
            Frequency: "",
            DonorType: "Individual",
            DonationName: "PPS Donation",
            DonationDescription: "PPS Donation",

            DonorDetails: {
                DonatorType: null,
                DonorType: null,
                ContactPersonName: null,
                FirstName: null,
                Surname: null,
                ContactPersonPhoneNumber: null,
                ContactPersonEmail: null,
                Gender: null,
                Address1: null,
                Address2: null,
                City: null,
                State: null,
                TaxNumber: " ",
                PostCode: null,
                Country: "South Africa",
                Phone: null,
                IsMember: false,
                MembershipNumber: " ",

                OrganizationName: null,
                OrganizationRegNumber: null,
                OrganizationContactPerson: null,
                OrganizationContactPersonPhone: null,
                OrganizationEmail: null,
                CertificateEmail: null,
                OrganizationCity: null,
            },
            StudentDetails: {
                //Id: StudentDetails.Id,
                Id: 0

            }
        };
        //$scope.selectDonatorType = function (value) {
        //    $scope.donation.DonatorType = value;

        //};

        $scope.dateOptions = {
            minDate: new Date()
        };


        $scope.selectDonation = function (value) {
            $scope.donation.Amount = value;
            //$scope.donation.DonatorType = value;
        };
        $scope.submitDonation = function (form) {
            this.donation.DonorDetails.DonorType = $scope.donation.DonorType;
            this.donation.DonorDetails.DonatorType = $scope.donation.DonatorType;

            //this.donation.StudentDetails.Id = $scope.donation.Id;
            // try this 
            var org_inputs = $document[0].querySelectorAll('#vm-id-row-organisation input');
            var individual_inputs = $document[0].querySelectorAll('#vm-id-row-individual input');
            var postdata = true;
            if (this.donation.DonorDetails.DonorType == "Individual") {
                this.donation.DonorDetails.ContactPersonName = this.donation.DonorDetails.FirstName + " " + this.donation.DonorDetails.Surname;
                individual_inputs.forEach(function (input) {
                    if (input.checkValidity() == false) {
                        $scope.message = "Please review your input before proceeding";
                        postdata = false;
                    }
                });
            }
            else {
                org_inputs.forEach(function (input) {
                    if (input.checkValidity() == false) {
                        $scope.message = "Please review your input before proceeding";
                        postdata = false;
                    }
                });
            }
            if (this.donation.DonatorType == "Once off donation") {
                if (postdata) {
                    $scope.loading = true;
                    $scope.donatebutton = false;
                    $scope.message = "Please wait while we load the payment service...";
                    $http.post('/api/Donation/ReceiveDonation', this.donation)
                        .success(function (data, status, headers, config) {

                            $scope.loading = false;
                            $scope.donatebutton = true;


                            if (!data.Error) {
                                $window.location.href = data.URL;
                            } else {
                                $scope.message = data.FriendlyMessage;
                            }
                        })
                        .error(function (data, status, headers, config) {
                            $scope.loading = false;
                            $scope.donatebutton = true;
                            $scope.message = "An error occured!. Please verify your input and try again.";
                            /*if (!!data && !!data.FriendlyMessage){
                                $scope.message = data.FriendlyMessage;
                            } else {
                                $scope.message = "An error occured!. Please try again.";
                            }*/
                        });
                }
            } else if (this.donation.DonatorType == "Recurring Donation") {
                if (postdata) {
                    $scope.loading = true;
                    $scope.donatebutton = false;
                    $scope.message = "Please wait for payfast subscription services...";
                    $http.post('/api/Donation/CreateDonationsubscription', this.donation)
                        .success(function (data, status, headers, config) {

                            $scope.loading = false;
                            $scope.donatebutton = true;

                            if (!data.Error) {
                                $window.location.href = data.URL;
                            } else {
                                $scope.message = data.FriendlyMessage;
                            }
                        })
                        .error(function (data, status, headers, config) {
                            $scope.loading = false;
                            $scope.donatebutton = true;
                            $scope.mesage = "An error occured!. Please verify your input and try again.";
                        });
                }
            }
        }
    }]);