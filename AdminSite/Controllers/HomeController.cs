using InterfaceLayer.Models;
using Microsoft.AspNet.Identity;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLayer.Core;
using DataLayer.Model;
using System.Linq;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public class DashboardMainModel
        {
            public DashboardViewModel Dash { get; set; }
            public List<SelectListItem> Clientlist { get; set; }
            public string TotalDonations { get; set; }

            public string TotalDonationStudent { get; set; }

            public string TotalDonationForTheDay { get; set; }

            public string High { get; set; }

            public string Low { get; set; }

            public string EachDonation { get; set; }

            public string TotalCost { get; set; }

            public List<string> AllStudent { get; set; }
            public string BalanceStyle { get; set; }
            public string Company { get; set; }
        }

        List<SelectListItem> LoadList(Boolean overlimitonly = false)
        {
            List<SelectListItem> clientlist = new List<SelectListItem>();

            return clientlist;
        }
        public decimal GetBalance()
        {
            return new DonationAdminService().GetDonationTotal();
        }
        // Newly implemented functions for student Dashborad  
        public decimal GetStudent()
        {
            return new DonationAdminService().GetStudentDoantion();
        }

        public decimal GetDonationfortheDay()
        {
            return new DonationAdminService().GetTotalDonationfortheDay();
        }

        public decimal GetHighestDonation()
        {
            return new DonationAdminService().HighestDonation();
        }

        public decimal GetLowestDonation()
        {
            return new DonationAdminService().LowestDonation();
        }

        public string GetEachStudentDonations()
        {
            return (new DonationAdminService().GetEachStudentDonations());
        }

        [OutputCache(Duration = 60, VaryByCustom = "User")]
        public ActionResult Dashboard()
        {

            DashboardViewModel Dash = new DashboardViewModel();
            decimal Balance = GetBalance();
            decimal BalanceStudent = GetStudent();
            decimal BalanceForTheDay = GetDonationfortheDay();

            string BalanceStyle = Balance > 0 ? "bg-red" : "bg-green";
            BalanceStyle = BalanceStudent > 0 ? "bg-red" : "bg-green";
            BalanceStyle = BalanceForTheDay > 0 ? "bg-red" : "bg-green";
            string Company = "";//Clientlist.Count == 1 ? "" : Clientlist[0].Text;
            DashboardMainModel ViewData = new DashboardMainModel
            {
                Dash = Dash,
                TotalDonations = Balance.ToString("#,##,##,##0.00"),
                TotalDonationStudent = BalanceStudent.ToString("#,##,##,##0.00"),
                TotalDonationForTheDay = BalanceForTheDay.ToString("#,##,##,##0.00"),
                //EachDonation = EachStudentDonation.ToString(),
                BalanceStyle = BalanceStyle,
                Company = Company
            };

            return View(ViewData);
        }

        public ActionResult EachStudentDonationDashboard()
        {
            decimal balanceHigh = GetHighestDonation();
            decimal balanceLow = GetLowestDonation();

            string BalanceStyle = balanceHigh > 0 ? "bg-red" : "bg-green";
            BalanceStyle = balanceLow > 0 ? "bg-red" : "bg-green";

            DashboardMainModel ViewData = new DashboardMainModel
            {
                High = balanceHigh.ToString("#,##,##,##0.00"),
                Low = balanceLow.ToString("#,##,##,##0.00")
            };
            return View(ViewData);
        }

        public ActionResult GetData()
        {
            JsonResult result = new JsonResult();
            try
            {
                using (PPSDonationEntities db = new PPSDonationEntities())
                {

                    var list = (from a in db.Donations
                                join aa in db.AspNetStudents
                                on a.AspNetStudent.Id equals aa.Id
                                where a.Amount > 0
                                group a by new { a.AspNetStudent.Id, aa.Firstname, aa.Lastname }
                                into g
                                select new
                                {
                                    Amount = g.Sum(a => a.Amount),
                                    Firstname = g.Key.Firstname,
                                    Lastname = g.Key.Lastname
                                }).ToList();


                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Info     
                Console.Write(ex);
            }

            return result;
        }
    }
}


//public string Total()
//{
//    using (var db = new PPSDonationEntities())
//    {
//        var eachDonations = (from a in db.Donations
//                             join AspNetStudent in db.AspNetStudents on a.AspNetStudent.Id equals AspNetStudent.Id
//                             orderby a.AspNetStudent.Firstname
//                             where a.Amount > 0
//                             select a).ToList();

//        return eachDonations.ToString();
//    }
//}