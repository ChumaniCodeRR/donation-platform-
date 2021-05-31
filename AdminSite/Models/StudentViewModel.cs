using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class StudentViewModel
    {
        [Required(ErrorMessage = "please enter firstname")]
        // [Display(Name = "FirstName")]
        public string Firstname
        { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Lastname")]
        public string Lastname
        { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "University")]
        public string University
        { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        //  [Display(Name = "Qualification")]
        public string Qualification
        { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        //[Display(Name = "YearofStudy")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime YearofStudy
        { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "YearofRequest")]
        public DateTime YearofRequest
        { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "AcademicAverage")]
        public int AcademicAverage
        { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Range(0.01, 100000, ErrorMessage = "Payment amount is required between .01 and $100,000.")]
        [DataType(DataType.Currency)]
        [Required]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Display(Name = "AmountofFundingNeeded")]
        public double AmountofFundingNeeded
        { get; set; } = 0.00;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "WhatTheFundingWillBeUsedFor")]
        public string WhatTheFundingWillBeUsedFor
        { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Bio")]
        public string Bio
        { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Motivation")]
        public string Motivation
        { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "CellNumber")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string CellNumber
        { get; set; }

        [Display(Name = "Emailaddress")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email
        { get; set; }

        //[Required(ErrorMessage = "")]

        public byte[] Picture { get; set; }

        public List<SelectListItem> allstudents = new List<SelectListItem>();
    }
}