using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Model
{
    public class OrganizationDonor:DonorBase
    {
        public string RegistrationNumber { get; set; }
        public string OrganizationEmail { get; set; }
        public string CertificateEmail { get; set; }
        public string CompanyName { get;  set; }
    }
}