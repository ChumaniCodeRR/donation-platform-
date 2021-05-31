using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceLayer.Models
{
    public class DonationReport
    {
        public DateTime TransactDate { get;  set; }
        public string TransactionDate {
            get { return TransactDate.ToString("yyyy-MM-dd"); }
        }
        public string DonorType { get; set; }
        public string DonatorType { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }
        public double? Amount { get; set; }
        public string CertificateID { get; set; }
        public string CertificateSendDate { get; set; }
        public string Status { get; set; }
        public double? NetAmount { get; set; }
        public double? Fees { get; set; }
        public string DonationRef { get; set; }
        public string SendStatus { get; set; }
        public string DonationRefOnCertificate { get; set; }

        public string Student { get; set; }
    }
}