using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceLayer.Models
{
    public class StudentsReport
    {
        public int? Id { get; set; }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string University { get; set; }

        public string Qualification { get; set; }

        public DateTime? YearofStudy { get; set; }

        public DateTime? YearofRequest { get; set; }

        public int? AcademicAverage { get; set; }

        public double? AmountofFundingNeeded { get; set; }

        public string WhatTheFundingWillBeUsedFor { get; set; }

        public string Bio { get; set; }

        public string Motivation { get; set; }

        public string CellNumber { get; set; }

        public string Email { get; set; }

        public byte Picture { get; set; }
    }
}