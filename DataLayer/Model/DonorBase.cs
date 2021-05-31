using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DataLayer.Model
{
    public class DonorBase: IDonate
    {
        public bool IsMember { get; set; }
        public String MembershipNumber { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string ContactPersonName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string DonorType { get; set; }

        public string Name { get; set; }
    }
}