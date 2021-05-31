using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace DataLayer.Model
{

    public class DonationBase : EntityBase
    {
        public string Name { get; set; }
        public string GUID { get; set; }
        public double Amount { get; set; }
        public string DonationName { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonationDescription { get; set; }

        public DateTime billingDate { get; set; }

        public string frequency { get; set; }
        public IDonate DonorData { get; set; }

        public IStudent StudentData { get; set; }
        public int DonationRef { get; set; }

        public string DonatorType { get; set; }
    }
}