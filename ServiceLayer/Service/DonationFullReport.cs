using System;

namespace ServiceLayer.Service
{
    public class DonationFullReport
    {
        public DateTime TransactDate { get; internal set; }
        public DateTime? DonorFeedbackDate { get; internal set; }
        public string DonationRef { get; internal set; }
        public string Status { get; internal set; }
        public string DonorType { get; internal set; }
        public string DonorName { get; internal set; }

        public string DonorSurname { get; internal set; }

        public string OrganizationEmail { get; internal set; }
        //public string DonorSurname { get; internal set; }
        public string OrganizationName { get; internal set; }
        public string RegistrationNumber { get; internal set; }
        
        public string CertificateID { get; internal set; }
        public string DonationContatctPhoneNumber { get; internal set; }
        public string DonorAddress1 { get; internal set; }
        public string DonorAddress2 { get; internal set; }
        public string DonationPostCode { get; internal set; }
        public string DonorCity { get; internal set; }
        public string DonorEmail { get; internal set; }
        
        public string DonorState { get; internal set; }
        public string DonorTax { get; internal set; }
    
        public bool IsPPSMember { get; internal set; }
        public string MembershipNumber { get; internal set; }
        public double? Amount { get; internal set; }
        public double? Fees { get; internal set; }
        public double? NetAmount { get; internal set; }
        public string DonationRefOnCertificate { get; set; }
        public string Gender { get; set; }
        /*
        ws.Cell(1, 18).Value = "State";
        ws.Cell(1, 19).Value = "Tax Number";
        ws.Cell(1, 20).Value = "IsPPSMember";
        ws.Cell(1, 21).Value = "MembershipNumber";

        ws.Cell(1, 22).Value = "Amount";
        ws.Cell(1, 23).Value = "Fees";
        ws.Cell(1, 24).Value = "NetAmount";*/
        
        
    }
}