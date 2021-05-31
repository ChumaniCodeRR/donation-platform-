using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using DataLayer.Core;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Vetro.Models;
using Vetro.Providers;
using Vetro.Results;
using Vetro.Models.Utility;
using System.Net;
using System.Text;
using ServiceLayer;
using InterfaceLayer.Models;
using ServiceLayer.Models;
using ServiceLayer.BLL;
using PayFast;
using PayFast.AspNet;
using ServiceLayer.Service;
using DataLayer.Model;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Collections.Specialized;
using System.Collections;

namespace Vetro.Controllers
{
    [Authorize]
    [RoutePrefix("api/Donation")]
    public class DonationController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private static ISetupPayment settings;

        public DonationController()
        {
 
            settings = new PaymentSettings
            {
                post_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdPaymentURL"],
                return_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdReturnURL"],
                notify_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdNotifyURL"],
                cancel_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdCancelURL"],
                validate_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdValidateURL"],

                post_url_test = System.Configuration.ConfigurationManager.AppSettings["TestPaymentURL"],
                return_url_test = System.Configuration.ConfigurationManager.AppSettings["TestReturnURL"],
                notify_url_test = System.Configuration.ConfigurationManager.AppSettings["TestNotifyURL"],
                cancel_url_test = System.Configuration.ConfigurationManager.AppSettings["TestCancelURL"],
                validate_url_test = System.Configuration.ConfigurationManager.AppSettings["TestValidateURL"],

                merchant_id = System.Configuration.ConfigurationManager.AppSettings["MerchantID"],
                merchant_key = System.Configuration.ConfigurationManager.AppSettings["MerchantKey"],
                payment_provider = System.Configuration.ConfigurationManager.AppSettings["PaymentProvider"],
                environment_type = System.Configuration.ConfigurationManager.AppSettings["Environment"],

                email_confirmation = System.Configuration.ConfigurationManager.AppSettings["EmailConfirmation"],
                admin_email = System.Configuration.ConfigurationManager.AppSettings["PaymentAdminEmail"],

                smtp_host = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"],
                smtp_port = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"],
                smtp_security = System.Configuration.ConfigurationManager.AppSettings["SMTPSecurity"],
                smtp_username = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"],
                smtp_password = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"],
                report_directory = System.Configuration.ConfigurationManager.AppSettings["ReportDirectory"] ?? HttpContext.Current.Server.MapPath("~/App_Data/"),
                pass_phrase = System.Configuration.ConfigurationManager.AppSettings["Passphrase"]
            };

        }
        // POST api/Donation/CreateDonationsubscription
        [AllowAnonymous]
        [AntiForgeryToken]
        [Route("CreateDonationsubscription")]
        public RequestReponse CreateDonationsubscription(DonationViewModel donation)
        {
            //Send api to PayFast 
            return settings.paymentServiceType.SubscriptionPayment(donation);
        }

        // POST api/Donation/ReceiveDonation
        [AllowAnonymous]
        [AntiForgeryToken]
        [Route("ReceiveDonation")]
        public RequestReponse ReceiveDonation(DonationViewModel donation)
        {
            // Redirect to PayFast            
            return settings.paymentServiceType.ProcessPayment(donation);   
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpPost]
        [Route("Notify")]
        public async Task<string> Notify(PayFastNotify payfast )
        {
            PaymentSettings setup = (PaymentSettings)settings;
            PayfastService validateService = (PayfastService)setup.paymentServiceType;
            PayFastSettings payfastSettings = new PayFastSettings
            {
                MerchantId = setup.merchant_id ,
                PassPhrase = setup.pass_phrase,
                CancelUrl = setup.cancel_url,
                NotifyUrl = setup.notify_url,
                ValidateUrl = setup.validate_url,
                
            };
            PerformSecurityChecks(payfast.GetNameValueCollection(), setup.merchant_id);
            var payfastValidator = new PayFastValidator(payfastSettings, payfast, IPAddress.Parse(GetIP()));
            var isIPValid = await payfastValidator.ValidateSourceIp();
            var calculatedSignature = payfast.GetCalculatedSignature();
            var isValid = payfast.signature == calculatedSignature;
            try
            {
                //if (isValid && isIPValid)
                //{
                    validateService.SendCertificate(payfast, settings.emailServiceType);
                //}
                return "Working";
            }
            catch (DbEntityValidationException ex)
            {
                string error = "";
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        error = error + ("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                return error;
            }
            catch (Exception exx)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(exx, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                string methodName = frame.GetMethod().Name;
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                return exx.InnerException + " " + exx.Message + " " + exx.ToString()+ " "+line+ " "+ methodName; 
            }
            //throw new Exception("Invalid Request Received "+isValid+ " "+isIPValid);

        }
        private void PerformSecurityChecks(NameValueCollection arrPostedVariables, string merchant_id)
        {
            // Verify that we are the intended merchant
            string receivedMerchant = arrPostedVariables["merchant_id"];

            if (receivedMerchant != merchant_id)
                throw new Exception("Mechant ID mismatch");

            // Verify that the request comes from the payment processor's servers.
            /*
            // Get all valid websites from payment processor
            string[] validSites = new string[] { "www.payfast.co.za", "sandbox.payfast.co.za", "w1w.payfast.co.za", "w2w.payfast.co.za" };

            // Get the requesting ip address
            string requestURL = HttpContext.Current.Request.UrlReferrer.ToString().Trim();
            if (requestURL.Equals("https://www.payfast.co.za") || requestURL.Equals("https://sandbox.payfast.co.za"))
            {
                return;
            }
            else
            {
                throw new Exception("IP address cannot be null");
            }
            
            // Is address in list of websites
            if (!IsIpAddressInList(validSites, requestIp))
                throw new Exception("IP address invalid");*/
        }
        private bool IsIpAddressInList(string[] validSites, string ipAddress)
        {
            // Get the ip addresses of the websites
            ArrayList validIps = new ArrayList();

            for (int i = 0; i < validSites.Length; i++)
            {
                validIps.AddRange(System.Net.Dns.GetHostAddresses(validSites[i]));
            }

            IPAddress ipObject = IPAddress.Parse(ipAddress);

            if (validIps.Contains(ipObject))
                return true;
            else
                return false;
        }
        [AllowAnonymous]
        [AcceptVerbs("POST", "GET")]
        public async Task<string> NotifyT([ModelBinder(typeof(PayFastNotifyModelBinder))]PayFastNotify payFastNotifyViewModel)
        {
            System.IO.StreamWriter file =
            new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/notifypayfast.txt"), true);
            
                payFastNotifyViewModel.SetPassPhrase(((PaymentSettings)settings).pass_phrase);
            var setup = (PaymentSettings)settings;
            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();
            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            file.WriteLine($"Signature Validation Result: {isValid}");
            
            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases

            PayFastSettings payfastSettings = new PayFastSettings
            {
                MerchantId = setup.merchant_id,
                PassPhrase = setup.pass_phrase,
                CancelUrl = setup.cancel_url,
                NotifyUrl = setup.notify_url,
                ValidateUrl = setup.validate_url,
                
            };
            var payfastValidator = new PayFastValidator(payfastSettings, payFastNotifyViewModel, IPAddress.Parse(GetIP()));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            file.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            file.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                file.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                file.WriteLine($"Subscription was cancelled");
            }

            return "nice";
        }

        private string GetIP()
        {
            string ClientIP = string.Empty;
            if (Request.Properties.ContainsKey("MS_HttpContext"))
            {
                HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
                if (context.Request.ServerVariables["HTTP_VIA"] != null)
                {

                    ClientIP = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    ClientIP = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            return ClientIP;
        }

    }
}
