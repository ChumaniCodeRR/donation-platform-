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

namespace WebApplication.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class DonationAdminController : Controller
    {
        private ReportService report;
        
        public ActionResult DonationAdmin()
        {
            return View();
        }
        
        /*public HttpResponseBase Export()
        {          
            
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=\"Stock.xlsx\"");
           
            Response.End();

            return Response;
        }*/
        public ActionResult GetDonations(DateTime? fromDate, DateTime? toDate)
        {
           
            return Json(new DonationAdminService().GetDonations(fromDate, toDate));
        }
        public ActionResult GetDonors(DateTime? fromDate, DateTime? toDate)
        {

            return Json(new DonationAdminService().GetDonations(fromDate, toDate));
        }
        public ActionResult ResendCertification(string certificateGUID)
        {
            //report_directory = System.Configuration.ConfigurationManager.AppSettings["ReportDirectory"]
            return Json(new DonationAdminService().ResendCertification(certificateGUID, EmailService.settings.emailServiceType, EmailService.settings, User.Identity.GetUserId()));
        }
        public FileStreamResult DownloadCertificate(string certificateGUID)
        {            
            MemoryStream stream = new DonationAdminService().DownloadCertificate(certificateGUID, EmailService.settings.emailServiceType, EmailService.settings, User.Identity.GetUserId());
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            Response.AppendHeader("Content-disposition", "inline; filename=PPSDonationCertificate.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }
        public FileStreamResult DownloadReport(string Stockcode,DateTime startdate, DateTime enddate , string reportType)
        {

            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = report.Generate(startdate, enddate, Stockcode);
            Response.AppendHeader("Content-disposition", "inline; filename=InvoiceStatement.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }

        public HttpResponseBase Export(DateTime? fromDate, DateTime? toDate)
        {

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=\"DonationList.xlsx\"");
            MemoryStream file = new DonationAdminService().Export(fromDate, toDate);
            file.Position = 0;
            file.WriteTo(Response.OutputStream);
            file.Close();
            Response.End();

            return Response;
        }
    }
}