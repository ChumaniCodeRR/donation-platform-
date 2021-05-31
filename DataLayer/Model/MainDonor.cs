using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Model
{
    public class MainDonor : DonorBase
    {
        public string Gender { get; set; }
        public string TaxNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string RegistrationNumber { get; set; }
        public string OrganizationEmail { get; set; }
        public string CertificateEmail { get; set; }
        public string CompanyName { get; set; }
    }
}