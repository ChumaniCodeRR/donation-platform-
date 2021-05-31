using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Core;
using DataLayer.Model;
using InterfaceLayer.Models;
using ServiceLayer.Models;
using ServiceLayer.Report.DonationCertificate;
using System.IO;
using ClosedXML.Excel;

namespace ServiceLayer.Service
{
    public class StudentAdminService
    {
        public object ViewAllStudents()
        {
            using (var db = new PPSDonationEntities())
            {
                List<StudentsReport> allStudents = (from students in db.AspNetStudents
                                                        //where students.Firstname == "Chumani"
                                                    select new StudentsReport
                                                    {
                                                        Id = students.Id,
                                                        Firstname = students.Firstname,
                                                        Lastname = students.Lastname,
                                                        University = students.University,
                                                        Qualification = students.Qualification,
                                                        YearofStudy = students.YearofStudy,
                                                        YearofRequest = students.YearofRequest,
                                                        AcademicAverage = students.AcademicAverage,
                                                        AmountofFundingNeeded = students.AmountofFundingNeeded,
                                                        WhatTheFundingWillBeUsedFor = students.WhatTheFundingWillBeUsedFor,
                                                        Bio = students.Bio,
                                                        Motivation = students.Motivation,
                                                        CellNumber = students.CellNumber,
                                                        Email = students.Email,

                                                    }).ToList();

                return allStudents;
            }
        }
    }
}

//PPSDonationEntities db = new PPSDonationEntities();


//public List<AspNetStudent> ViewAllStudents()
//{
//    var query = from students in db.AspNetStudents
//                select new
//                {
//                    Firstname = students.Firstname,
//                    Lastname = students.Lastname,
//                    University = students.University,
//                    Qualification = students.Qualification,
//                    YearofStudy = students.YearofStudy,
//                    YearofRequest = students.YearofRequest,
//                    AcademicAverage = students.AcademicAverage,
//                    AmountofFundingNeeded = students.AmountofFundingNeeded,
//                    WhatTheFundingWillBeUsedFor = students.WhatTheFundingWillBeUsedFor,
//                    Bio = students.Bio,
//                    Motivation = students.Motivation,
//                    CellNumber = students.CellNumber,
//                    Email = students.Email,
//                };

//    var products = query.ToList().Select(r => new AspNetStudent
//    {
//        Firstname = r.Firstname,
//        Lastname = r.Lastname,
//        University = r.University,
//        Qualification = r.Qualification,
//        YearofStudy = r.YearofStudy,
//        YearofRequest = r.YearofRequest,
//        AcademicAverage = r.AcademicAverage,
//        AmountofFundingNeeded = r.AmountofFundingNeeded,
//        WhatTheFundingWillBeUsedFor = r.WhatTheFundingWillBeUsedFor,
//        Bio = r.Bio,
//        Motivation = r.Motivation,
//        CellNumber = r.CellNumber,
//        Email = r.Email,
//    }).ToList();

//    return products;
//}
