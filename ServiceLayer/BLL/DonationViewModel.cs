using InterfaceLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
namespace ServiceLayer.BLL
{
    [ModelBinder(typeof(DonationModelBinder))]
    public class DonationViewModel : EntityBase, IPayable
    {
        public string Name { get; set; }
        public string GUID { get; set; }
        public DateTime DonationDate { get; set; }
        [Required]
        public double Amount { get; set; }
        public string DonationName { get; set; }
        public string DonationDescription { get; set; }

        public string DonatorType { get; set; }

        public DateTime BillingDate { get; set; }

        public string Frequency { get; set; }

        [JsonIgnore]
        public IDonate DonorDetails { get; set; }
        [JsonIgnore]
        public IStudent StudentDetails { get; set; }
        //public Istudent StudentDetails { get; set; }
        /*
        [Required]
        public double DonationAmount { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {4} characters long.", MinimumLength = 4)]
        public string DonorType { get; set; }
        public string ItemDescription { get; set; }
        public string ItemName { get; set; }*/
    }
}