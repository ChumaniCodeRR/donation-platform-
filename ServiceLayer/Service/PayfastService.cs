using InterfaceLayer.Models;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DataLayer.Repository;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceLayer.BLL;
using System.IO;
using ServiceLayer.Report.DonationCertificate;
using TheArtOfDev.HtmlRenderer;
using DataLayer.Model;
using DataLayer.Core;
using ServiceLayer.Service;
using PayFast;
using System.Threading;
using System.Globalization;

namespace ServiceLayer
{
    public class PayfastService : IPay
    {
        ISetupPayment paymentSettings;
        IPayable donationDetails;
        StringBuilder paymnt_url = new StringBuilder();
        private readonly PayFastSettings payFastSettings;
        public IEmailSender emailService;

        public PayfastService(ISetupPayment paymentSettings)
        {
            this.paymentSettings = paymentSettings;

        }

        public RequestReponse SubscriptionPayment(IPayable donationDetails)
        {
            PaymentSettings paymentSettings = (PaymentSettings)this.paymentSettings;
            //var recurringRequest = new PayFastRequest(paymentSettings.pass_phrase);

            var recurringRequest  = new PayFastRequest();

            var donationData = (DonationViewModel)donationDetails;
            using (var con = new PPSDonationEntities())
            {
                try
                {
                    DonationRepositoryExt donation = new DonationRepositoryExt(con);
                    var donorData = (MainDonor)donationData.DonorDetails;
                    var studentData = (StudentBase)donationData.StudentDetails;
                    if (donationData.Amount <= 0) throw new Exception("Invalid Amount");
                    string TransactionRef = donation.CreateDonation(new DonationBase
                    {
                        Amount = donationData.Amount,
                        DonorData = donorData,
                        StudentData = studentData,
                        DonationName = donationData.DonationName,
                        DonationDescription = donationData.DonationDescription,
                        DonatorType = donationData.DonatorType,
                        ///New Values 
                        billingDate = donationData.BillingDate,
                        frequency = donationData.Frequency
                    });
                    
                    recurringRequest.SetPassPhrase(paymentSettings.pass_phrase);
                    recurringRequest.merchant_id = paymentSettings.merchant_id;
                    recurringRequest.merchant_key = paymentSettings.merchant_key;
                    recurringRequest.return_url = paymentSettings.return_url;
                    recurringRequest.cancel_url = paymentSettings.cancel_url;
                    recurringRequest.notify_url = paymentSettings.notify_url;
                    recurringRequest.confirmation_address = donorData.DonorType == "Individual" ? donorData.ContactPersonEmail : donorData.CertificateEmail;

                    // For email confimation true
                    bool emailconfimed = paymentSettings.email_confirmation.Equals("1") ? true : false;
                    recurringRequest.email_confirmation = emailconfimed;

                    // Similar with process payment , also check select billing type 

                    var todaysDate = DateTime.Now;
                    recurringRequest.m_payment_id = TransactionRef;

                    recurringRequest.amount = donationData.Amount;

                    recurringRequest.item_name = donationData.DonationName;
                    recurringRequest.item_description = "Recurring Donation";

                    recurringRequest.subscription_type = SubscriptionType.Subscription;
                    recurringRequest.billing_date = donationData.BillingDate;

                    recurringRequest.recurring_amount = donationData.Amount;
                  

                    if (donationData.Frequency == "3")
                    {
                        recurringRequest.frequency = BillingFrequency.Monthly;
                        recurringRequest.cycles = 12;
                    }

                    if (donationData.Frequency == "4")
                    {
                        recurringRequest.frequency = BillingFrequency.Quarterly;
                        recurringRequest.cycles = 4;
                    }

                    if (donationData.Frequency == "5")
                    {
                        recurringRequest.frequency = BillingFrequency.Biannual;
                        recurringRequest.cycles = 2;
                    }

                    if (donationData.Frequency == "6")
                    {
                        recurringRequest.frequency = BillingFrequency.Annual;
                        recurringRequest.cycles = 1;
                    }

                    HttpClient client = new HttpClient();
                    Task<HttpResponseMessage> postToPayFast = client.GetAsync(paymentSettings.post_url + recurringRequest);
                    postToPayFast.Wait();
                    var response = postToPayFast.Result;
                    return new RequestReponse
                    {
                        URL = response.RequestMessage.RequestUri.OriginalString,
                        Error = false
                    };

                }
                catch (HttpException ex)
                {
                    string exception = ex.InnerException != null ? ex.InnerException.Message : "";
                    return new RequestReponse
                    {
                        URL = "#",
                        FriendlyMessage = "Error Contating Payment Gateway: " + ex.Message,//+" "+exception, //+ " "+ paymentSettings.post_url + paymnt_url,
                        Error = true
                    };
                }
                catch (Exception ex)
                {
                    string exception = ex.InnerException != null ? ex.InnerException.Message : "";
                    return new RequestReponse
                    {
                        URL = "#",
                        FriendlyMessage = "General: Error Contacting Payment Gateway: " + ex.Message,// + " " + exception,// + " " + paymentSettings.post_url + paymnt_url,
                        Error = true
                    };
                }
            }
        }

        public RequestReponse ProcessPayment(IPayable donationDetails)
        {
            PaymentSettings paymentSettings = (PaymentSettings)this.paymentSettings;
            var donationData = (DonationViewModel)donationDetails;
            using (var don = new PPSDonationEntities())
            {
                try
                {

                    DonationRepositoryExt donation = new DonationRepositoryExt(don);
                    var donorData = (MainDonor)donationData.DonorDetails;
                    var studentData = (StudentBase)donationData.StudentDetails;
                    if (donationData.Amount <= 0) throw new Exception("Invalid Amount");
                    string TransactionRef = donation.CreateDonation(new DonationBase
                    {
                        Amount = donationData.Amount,
                        DonorData = donorData,
                        StudentData = studentData,
                        DonationName = donationData.DonationName,
                        DonationDescription = donationData.DonationDescription,
                        DonatorType = donationData.DonatorType,

                    });

                    paymnt_url.Append("merchant_id=" + HttpUtility.UrlEncode(paymentSettings.merchant_id));
                    paymnt_url.Append("&merchant_key=" + HttpUtility.UrlEncode(paymentSettings.merchant_key));
                    paymnt_url.Append("&return_url=" + HttpUtility.UrlEncode(paymentSettings.return_url));
                    paymnt_url.Append("&cancel_url=" + HttpUtility.UrlEncode(paymentSettings.cancel_url));
                    paymnt_url.Append("&notify_url=" + HttpUtility.UrlEncode(paymentSettings.notify_url));
                    paymnt_url.Append("&confirmation_address=" + HttpUtility.UrlEncode(donorData.DonorType == "Individual" ? donorData.ContactPersonEmail : donorData.CertificateEmail));
                    paymnt_url.Append("&email_confirmation=" + HttpUtility.UrlEncode(paymentSettings.email_confirmation));

                    paymnt_url.Append("&m_payment_id=" + HttpUtility.UrlEncode(TransactionRef));
                    paymnt_url.Append("&amount=" + HttpUtility.UrlEncode(donationData.Amount.ToString()));
                    paymnt_url.Append("&item_name=" + HttpUtility.UrlEncode(donationData.DonationName));
                    paymnt_url.Append("&item_description=" + HttpUtility.UrlEncode("Once of Donation"));

                    var values = new List<KeyValuePair<string, string>>();
                    values.Add(new KeyValuePair<string, string>("merchant_id", paymentSettings.merchant_id));
                    values.Add(new KeyValuePair<string, string>("merchant_key", paymentSettings.merchant_key));
                    values.Add(new KeyValuePair<string, string>("return_url", paymentSettings.merchant_key));
                    values.Add(new KeyValuePair<string, string>("cancel_url", paymentSettings.merchant_key));
                    values.Add(new KeyValuePair<string, string>("notify_url", paymentSettings.merchant_key));

                    values.Add(new KeyValuePair<string, string>("m_payment_id", TransactionRef));
                    values.Add(new KeyValuePair<string, string>("amount", donationData.Amount.ToString()));
                    values.Add(new KeyValuePair<string, string>("item_name", donationData.DonationName));
                    values.Add(new KeyValuePair<string, string>("item_description", "Once of Donation"));

                    var content = new FormUrlEncodedContent(values);

                    HttpClient client = new HttpClient();
                    Task<HttpResponseMessage> postToPayfast = client.GetAsync(paymentSettings.post_url + paymnt_url);
                    postToPayfast.Wait();
                    var response = postToPayfast.Result;
                    return new RequestReponse
                    {
                        URL = response.RequestMessage.RequestUri.OriginalString,
                        Error = false
                    };
                }
                catch (HttpException ex)
                {
                    string exception = ex.InnerException != null ? ex.InnerException.Message : "";
                    return new RequestReponse
                    {
                        URL = "#",
                        FriendlyMessage = "Error Contating Payment Gateway: " + ex.Message,//+" "+exception, //+ " "+ paymentSettings.post_url + paymnt_url,
                        Error = true
                    };
                }
                catch (Exception ex)
                {
                    string exception = ex.InnerException != null ? ex.InnerException.Message : "";
                    return new RequestReponse
                    {
                        URL = "#",
                        FriendlyMessage = "General: Error Contacting Payment Gateway: " + ex.Message,// + " " + exception,// + " " + paymentSettings.post_url + paymnt_url,
                        Error = true
                    };
                }

            }
        }
        //TODO: Gilbert: Split validation and emailing to make it testable
        public bool SendCertificate(IPaymentConfirmation PaymentConfirmation, IEmailSender emailServ)
        {
            PayFastNotify payfast = (PayFastNotify)PaymentConfirmation;
            Guid? payfastRef = null;
            try
            {
                payfastRef = Guid.Parse(payfast.m_payment_id);
            }
            catch (Exception e)
            {

            }
            using (var db = new PPSDonationEntities())
            {
                int donationRef;
                var donationCheck = payfastRef != null ? db.Donations.Where(v => v.GUID.Equals(payfastRef)).FirstOrDefault() : null;

                if (donationCheck != null)
                {
                    donationRef = donationCheck.DonationID;
                }
                else
                {
                    donationRef = int.Parse(payfast.m_payment_id);
                }

                var donation = db.Donations.Where(d => d.DonationID.Equals(donationRef)).FirstOrDefault();
                var donor = donation.Donor;
                donation.FeedbackDate = DateTime.Now;
                donation.Fees = double.Parse(payfast.amount_fee, CultureInfo.InvariantCulture);
                donation.NetAmount = donation.Amount + donation.Fees;
                donation.DonationStatu = db.DonationStatus.Where(h => h.StatusName.Equals(payfast.payment_status)).FirstOrDefault();
                string name = donor.DonorType.DonorTypeName.Equals("Individual") ? donor.FirstName + " " + donor.LastName : donor.OrganizationName;
                DonorBase tempDonor = new MainDonor
                {
                    ContactPersonName = donor.ContactName,
                    Name = name,
                    DonorType = donor.DonorType.DonorTypeName,
                    Address1 = donor.Address1,
                    Address2 = donor.Address2,
                    ContactPersonPhoneNumber = donor.ContactPhoneNumber,
                    ContactPersonEmail = donor.ContactEmail,
                    OrganizationEmail = donor.OrganizationEmail,
                    CertificateEmail = donor.OrganizationEmail,

                    City = donor.City,
                    State = donor.State,
                    TaxNumber = donor.TaxNumber,
                    PostCode = donor.PostCode,
                    Country = donor.Country
                };

                DonationBase donationBase = new DonationBase
                {
                    Amount = donation.Amount.Value,
                    GUID = "PPS-" + donation.DonationID.ToString(),
                    DonationName = donation.DonationName,
                    DonationDate = donation.TransactionDate,
                    DonorData = tempDonor,
                    DonationRef = donationRef,
                    DonatorType = donation.DonatorType
                };

                db.SaveChanges();

                if (payfast.payment_status.Equals("COMPLETE"))
                {
                    new DonationAdminService().SendCertificate(donationBase, emailServ, paymentSettings);
                }
            }



            return true;
        }

        public bool ValidatePayment(IPayable donation)
        {
            throw new NotImplementedException();
        }
    }
}