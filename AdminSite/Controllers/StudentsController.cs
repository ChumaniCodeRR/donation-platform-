using InterfaceLayer.Models;
using Microsoft.AspNet.Identity;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using DataLayer.Core;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Schema;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using WebApplication.Models;
using System.Configuration;
//using System.Data.ObjectNotFound;

namespace WebApplication.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class StudentsController : Controller
    {
        PPSDonationEntities dd = new PPSDonationEntities();
        //private List<StudentViewModel> students;

        string filePath = ConfigurationManager.AppSettings["PPSConnect"].ToString();

        public const int ImageMiniBytes = 512;


        public ActionResult Students()
        {
            return View();
            //return View("Students", "Students");
            // return RedirectToAction("ViewAllStudents", "Students");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Students(AspNetStudent st, HttpPostedFileBase file)
        {
            try
            {
                if (string.IsNullOrEmpty(st.Firstname)) { ModelState.AddModelError("Firstname", "*"); }

                if (string.IsNullOrEmpty(st.Lastname)) { ModelState.AddModelError("Lastname", "*"); }

                if (string.IsNullOrEmpty(st.University)) { ModelState.AddModelError("University", "*"); }

                if (string.IsNullOrEmpty(st.Qualification)) { ModelState.AddModelError("Qualification", "*"); }

                //if (ModelState.IsValidField("YearofStudy") && DateTime.Now < st.YearofStudy) { ModelState.AddModelError("YearofStudy", "*"); }

                //if (ModelState.IsValidField("YearofRequest") && DateTime.Now < st.YearofRequest) { ModelState.AddModelError("YearofRequest", "*"); }

                if (ModelState.IsValidField("AcademicAverage") && st.AcademicAverage < -1)
                { ModelState.AddModelError("AcademicAverage", "*"); }

                if (ModelState.IsValidField("AmountofFundingNeeded") && st.AmountofFundingNeeded < -1)
                { ModelState.AddModelError("AmountofFundingNeeded", "*"); }

                if (string.IsNullOrEmpty(st.WhatTheFundingWillBeUsedFor)) { ModelState.AddModelError("WhatTheFundingWillBeUsedFor", "*"); }

                if (string.IsNullOrEmpty(st.Bio)) { ModelState.AddModelError("Bio", "*"); }

                if (string.IsNullOrEmpty(st.Motivation)) { ModelState.AddModelError("Motivation", "*"); }

                if (string.IsNullOrEmpty(st.CellNumber)) { ModelState.AddModelError("CellNumber", "*"); }

                if (string.IsNullOrEmpty(st.Email)) { ModelState.AddModelError("Email", "*"); }
            }
            catch (Exception error)
            {
                return View("Error", new HandleErrorInfo(error, "Students", "Students"));
            }

            try
            {
                // Check if it's not null 

                if (file != null && file.ContentLength > 0)
                {
                    var filename = System.IO.Path.GetFileName(file.FileName);

                    //var path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(filePath + @"/Content/MapImages/"), filename);
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/MapImages/"), filename);
                    file.SaveAs(path);
                    // Upload file and information 

                }

                if (ModelState.IsValid)
                {
                    st.Picture = new byte[file.ContentLength];
                    file.InputStream.Read(st.Picture, 0, file.ContentLength);

                    dd.AddStudent_isp(
                        st.Firstname,
                        st.Lastname,
                        st.University,
                        st.Qualification,
                        st.YearofStudy,
                        st.YearofRequest,
                        st.AcademicAverage,
                        st.AmountofFundingNeeded,
                        st.WhatTheFundingWillBeUsedFor,
                        st.Bio,
                        st.Motivation,
                        st.CellNumber,
                        st.CellNumberConfimed,
                        st.Email
                        , Convert.ToBoolean(st.EmailConfirmed)
                        , st.Picture);

                    TempData["Success"] = "Student has been added successfully !!";
                    return RedirectToAction("Students");
                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View(st);
                }

            }
            catch (Exception err)
            {
                JavaScript("alert('error when uploading !!')" + err.Message);
                return View();
            }
            //return View();
            //return RedirectToAction("ViewAllStudents", "Students");
        }


        public ActionResult ViewAllStudents()
        {
            //students = new List<StudentViewModel>();
            var students = new StudentAdminService().ViewAllStudents();
            return View(students);
        }

        public ActionResult Update(int Id)
        {
            using (var context = new PPSDonationEntities())
            {
                var data = context.AspNetStudents.Where(x => x.Id == Id).SingleOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int Id, AspNetStudent model)
        {
            try
            {
                using (var context = new PPSDonationEntities())
                {
                    // particular record from a database 
                    var data = context.AspNetStudents.FirstOrDefault(x => x.Id == Id);

                    // Checking if any such record exist  
                    if (data != null)
                    {
                        data.Firstname = model.Firstname;
                        data.Lastname = model.Lastname;
                        data.University = model.University;
                        data.Qualification = model.Qualification;
                        data.YearofStudy = model.YearofStudy;
                        data.YearofRequest = model.YearofRequest;
                        data.AcademicAverage = model.AcademicAverage;
                        data.AmountofFundingNeeded = model.AmountofFundingNeeded;
                        data.WhatTheFundingWillBeUsedFor = model.WhatTheFundingWillBeUsedFor;
                        data.Bio = model.Bio;
                        data.Motivation = model.Motivation;
                        data.CellNumber = model.CellNumber;
                        data.Email = model.Email;

                        context.SaveChanges();

                        // It will redirect to the Read method 
                        return RedirectToAction("ViewAllStudents");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception error)
            {
                JavaScript("alert('error when uploading !!')" + error.Message);
                return View();
            }
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            try
            {
                using (var context = new PPSDonationEntities())
                {
                    var data = context.AspNetStudents.FirstOrDefault(x => x.Id == Id);
                    if (data != null)
                    {
                        context.AspNetStudents.Remove(data);
                        context.SaveChanges();
                        return RedirectToAction("ViewAllStudents");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception error)
            {
                JavaScript("alert('error when uploading !!')" + error.Message);
                return View();
            }
        }

        public ActionResult Details(int Id)
        {
            try
            {
                using (var context = new PPSDonationEntities())
                {
                    var data = context.AspNetStudents.Single(x => x.Id == Id);

                    if (data == null)
                        return View("NotFound");
                    else
                        return View(data);
                }
            }
            catch (Exception error)
            {
                JavaScript("alert('error when uploading !!')" + error.Message);
                return View();
            }
        }


        public JsonResult ViewAllStudentsJson()
        {
            JsonResult result = new JsonResult();
            // DataTable dt = new DataTable();
            //  RequestObj reqObj = JsonConvert.DeserializeObject<RequestObj>(jsonInput);
            var data = new StudentAdminService().ViewAllStudents();

            if (data == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
            else
            {
                result = this.Json(JsonConvert.SerializeObject(data), JsonRequestBehavior.AllowGet);
                return result;
            }
        }
    }
}


//if (file.ContentType.ToLower() != "image/jgp" &&
//    file.ContentType.ToLower() != "image/jpeg" &&
//    file.ContentType.ToLower() != "image/pjpeg" &&
//    file.ContentType.ToLower() != "image/gif" &&
//    file.ContentType.ToLower() != "image/x-png" &&
//    file.ContentType.ToLower() != "image/png")
//{
//    JavaScript("alert('Type not found')");
//    return View();
//}

//if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
//    && Path.GetExtension(file.FileName).ToLower() != ".png"
//    && Path.GetExtension(file.FileName).ToLower() != ".gif"
//    && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
//{
//    JavaScript("alert('Extension not found')");
//    return View();
//}

//// Check file size 
//if (file.ContentLength < ImageMiniBytes)
//{
//    JavaScript("alert('Length to big')");
//    return View();
//}

//else
//{
//    JavaScript("alert('No file !!!')");
//    return View();
//}