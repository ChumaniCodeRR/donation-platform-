using DataLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vetro.Controllers
{
    public class StudentDonationController : Controller
    {
        // GET: StudentDonation
        public ActionResult StudentDonation(string val = null)
        {
            //using (PPSDonationEntities db = new PPSDonationEntities())
            //{
            if (val == null)
            {
                string response = string.Empty;
                switch (val)
                {
                    case "cancelled":
                        response = "Dear Donor please note that you have cancelled your request"; //move to webconfig
                        break;
                }
                return View((object)response);
            }

            AspNetStudent student = TempData["Students"] as AspNetStudent;
            var Idindx = student.Id;

            //Access ID index 
           // StudentID(Idindx);

            return View(student);
            //}
        }

        // Not used yet 
        public ActionResult StudentID(int Id)
        {
            using (PPSDonationEntities db = new PPSDonationEntities())
            {
                //Pass Student index Id
                if (Id > 0)
                {
                    var eachstudent = db.AspNetStudents.First(m => m.Id == Id);
                    TempData["Id"] = eachstudent.Id;
                    return RedirectToAction("Donation", "ReceiveDonation", new { Id = eachstudent.Id });
                }
                return View();
            }
        }
    }
}