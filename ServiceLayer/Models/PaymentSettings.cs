using InterfaceLayer.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer.Service;
namespace ServiceLayer.Models
{
    public class PaymentSettings : ISetupPayment
    {
        public string email_confirmation;
        public string admin_email;
        public string smtp_host;
        public string smtp_port;
        public string smtp_security;
        public string smtp_username;
        public string smtp_password;

        public string validate_url_prod { get; set; }
        public string validate_url_test { get; set; }
        public string post_url_prod { get; set; }
        public string return_url_prod { get; set; }
        public string cancel_url_prod { get; set; }
        public string notify_url_prod { get; set; }
        public string post_url_test { get; set; }
        public string return_url_test { get; set; }
        public string cancel_url_test { get; set; }
        public string notify_url_test { get; set; }
        public string return_url
        {
            get
            {
                return production_mode ? return_url_prod : return_url_test;
            }
        }
        public string notify_url
        {
            get
            {
                return production_mode ? notify_url_prod : notify_url_test;
            }
        }

        public string post_url
        {
            get
            {
                return production_mode ? post_url_prod : post_url_test;
            }
        }
        public string cancel_url
        {
            get
            {
                return production_mode ? cancel_url_prod : cancel_url_test;
            }
        }
        public string validate_url
        {
            get
            {
                return production_mode ? validate_url_prod : validate_url_test;
            }
        }
        public string merchant_id { get; set; }
        public string merchant_key { get; set; }
        public string payment_provider { get; set; }
        public string environment_type { get; set; }
        public string pass_phrase { get; set; }
        
        //public string token { get; set; }
        public Boolean production_mode
        {
            get
            {
                if (environment_type != null && environment_type.Equals("Production")) { return true; }
                else
                {
                    return false;
                };
            }
        }
        public IPay paymentServiceType
        {
            get
            {

                //Gilbert: I should use Ninject here but ... 
                switch (payment_provider)
                {
                    case "Payfast":
                        return new PayfastService(this);
                    default:
                        return null;
                }
            }
        }


        public IEmailSender emailServiceType
        {
            get
            {

                //Gilbert: I should use Ninject here but ... 
                switch (payment_provider)
                {
                    case "Payfast": 
                        return new EmailService(this);
                    default:
                        return null;
                }
            }
        }

        public string report_directory { get; set; }
        public string support_admin { get; set; }
    }
}
