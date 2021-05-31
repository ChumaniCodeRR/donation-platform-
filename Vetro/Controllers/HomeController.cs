using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vetro.Models;
using DataLayer.Core;
using InterfaceLayer.Models;
using System.Security.Cryptography;
using ServiceLayer.Models;
using PagedList;
using System.Data.Entity.Validation;

//using ServiceLayer.Service;


namespace Vetro.Controllers
{
    public class HomeController : Controller
    {
        //PPSDonationEntities dd = new PPSDonationEntities();
        /// <summary>
        /// This maps to the Main/Index.cshtml file.  This file is the main view for the application.
        /// </summary>
        /// <returns></returns>
        /// 
        //[AntiForgeryToken]
        public ActionResult Index(int? page, int? pageSize)
        {
            //try
            //{
                using (PPSDonationEntities db = new PPSDonationEntities())
                {
                    //Returning All Students 
                    int pageIndex = 1;
                    pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                    int defaSize = (pageSize ?? 10);
                    ViewBag.psize = defaSize;

                    //List<AspNetStudent> Students = new List<AspNetStudent>();

                    //var allstudents = (from Students in db.AspNetStudents
                    //                 select Students).ToList();

                    IPagedList<AspNetStudent> involst = null;

                    involst = db.AspNetStudents.OrderByDescending(x => x.InsertDate).ToPagedList(pageIndex, defaSize);

                    return View(involst);

                }
           // }
           // catch (Exception error)
           // {
             //   return View("Error", new HandleErrorInfo(error, "Home", "Index"));
           // }

        }

        public ActionResult Student(string studentID)
        {
            try
            {
                using (PPSDonationEntities db = new PPSDonationEntities())
                {
                    if (studentID == null)
                    {
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                    }

                    if (studentID != null)
                    {
                        var eachstudent = db.AspNetStudents.First(m => m.StudentID == studentID);
                        TempData["Students"] = eachstudent;
                        return RedirectToAction("StudentDonation", "StudentDonation");
                    }

                    return View();
                }
            }
            catch (Exception error)
            {
                return View("Error", new HandleErrorInfo(error, "Home", "Index"));
            }
        }


        public ActionResult Details(int Id)
        {
            using (PPSDonationEntities dd = new PPSDonationEntities())
            {
                return PartialView("Details", dd.AspNetStudents.Find(Id));
            }
        }
    }
}