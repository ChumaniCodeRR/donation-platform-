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
    public class DonationAdminService
    {
        /*
        column += '<td class="text-left">' + col.TransactionDate + '</td>';
        column += '<td class="text-left">' + col.DonorType + '</td>';
        column += '<td class="text-left">' + col.DonorName + '</td>';
        column += '<td class="text-left">' + col.DonorEmail + '</td>';
        column += '<td class="text-left">' + toCurrency(parseFloat(col.Amount), '', '') + '</td>';
        column += '<td class="text-left">' + col.CertificateID + '</td>';
        column += '<td class="text-left">' + col.Status + '</td>';*/

        public object GetDonations(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new PPSDonationEntities())
            {
                List<DonationReport> allDonantions = (from donations in db.Donations
                                                          //join donations in db.Donations on certificate.DonationID equals donations.DonationID
                                                      where donations.TransactionDate >= fromDate
                                                      && donations.TransactionDate < toDate


                                                      select new DonationReport
                                                      {
                                                          Amount = donations.Amount,
                                                          NetAmount = donations.NetAmount,
                                                          Fees = donations.Fees,
                                                          DonorEmail = donations.Donor.ContactEmail,
                                                          DonorName = donations.Donor.ContactName,
                                                          DonorType = donations.Donor.DonorType.DonorTypeName,
                                                          TransactDate = donations.TransactionDate,                                                     
                                                          Status = donations.DonationStatu.StatusName,
                                                          DonationRef = donations.GUID.ToString(),
                                                          SendStatus = db.DonationCertificates.Where(v => v.CertificateID == db.DonationCertificates
                                                                       .Where(x => x.DonationID == donations.DonationID)
                                                                       .Max(x => x.CertificateID)).FirstOrDefault().SendStatus,
                                                          CertificateID = db.DonationCertificates.Where(v => v.CertificateID == db.DonationCertificates
                                                                           .Where(x => x.DonationID == donations.DonationID)
                                                                           .Max(x => x.CertificateID)).FirstOrDefault().CertificateReference,
                                                          DonationRefOnCertificate = donations.DonationID.ToString(),
                                                          Student = db.Donations.Where(v => v.AspNetStudent.Id == db.AspNetStudents
                                                                       .Where(x => x.Id == donations.AspNetStudent.Id)
                                                                       .Max(x => x.Id)).FirstOrDefault().AspNetStudent.Firstname,
                                                          DonatorType = donations.DonatorType

                                                      }).ToList();

                return allDonantions;
            }
        }

        public MemoryStream DownloadCertificate(string certificateGUID, IEmailSender emailServ, ISetupPayment paymentSettings, string userId)
        {

            MemoryStream donationCertificate;
            using (var db = new PPSDonationEntities())
            {
                var certificate = db.DonationCertificates.Where(x => x.CertificateReference.Equals(certificateGUID)).FirstOrDefault();
                var donation = certificate != null && certificate.Donation != null ? certificate.Donation : null;
                var donor = donation.Donor;
                string name = donor.DonorType.DonorTypeName.Equals("Individual") ? donor.FirstName + " " + donor.LastName : donor.OrganizationName;

                DonorBase tempDonor = new MainDonor
                {
                    ContactPersonName = donor.ContactName,
                    Name = name,
                    DonorType = donor.DonorType.DonorTypeName,
                    Address1 = donor.Address1,
                    Address2 = donor.Address2,
                    ContactPersonPhoneNumber = donor.ContactPhoneNumber,
                    ContactPersonEmail = donor.ContactEmail,
                    OrganizationEmail = donor.OrganizationEmail,
                    CertificateEmail = donor.OrganizationEmail,

                    City = donor.City,
                    State = donor.State,
                    TaxNumber = donor.TaxNumber,
                    PostCode = donor.PostCode,
                    Country = donor.Country
                };

                DonationBase donationBase = new DonationBase
                {
                    Amount = donation.Amount.Value,
                    GUID = "PPS-" + donation.DonationID.ToString(),
                    DonationName = donation.DonationName,
                    DonationDate = donation.TransactionDate,
                    DonorData = tempDonor,
                    DonatorType = donation.DonatorType
                };


                string reportDirectory = ((PaymentSettings)paymentSettings).report_directory;

                string guid = Guid.NewGuid().ToString();

                DonationCertificateBase certificateFile = new DonationCertificateBase(donationBase, reportDirectory);
                donationCertificate = certificateFile.ConvertToPdf();
                return donationCertificate;
            }
        }

        public MemoryStream Export(DateTime? fromDate, DateTime? toDate)
        {
            MemoryStream fs = new MemoryStream();
            using (var db = new PPSDonationEntities())
            {
                List<DonationFullReport> allDonations = (from donations in db.Donations
                                                             //join donations in db.Donations on certificate.DonationID equals donations.DonationID
                                                         where donations.TransactionDate >= fromDate
                                                         && donations.TransactionDate < toDate

                                                         select new DonationFullReport
                                                         {
                                                             Amount = donations.Amount ?? 0,
                                                             NetAmount = donations.NetAmount ?? 0,
                                                             Fees = donations.Fees ?? 0,
                                                             DonorEmail = donations.Donor.ContactEmail ?? "",
                                                             DonorName = donations.Donor.FirstName ?? "",
                                                             DonorSurname = donations.Donor.LastName ?? "",
                                                             DonorAddress1 = (donations.Donor.Address1 ?? "").Replace(",", " "),
                                                             DonorAddress2 = (donations.Donor.Address2 ?? "").Replace(",", " "),
                                                             IsPPSMember = donations.Donor.IsMember,
                                                             MembershipNumber = donations.Donor.MembershipNumber ?? "",
                                                             DonorTax = donations.Donor.TaxNumber ?? "",
                                                             DonationPostCode = donations.Donor.PostCode ?? "",
                                                             DonationContatctPhoneNumber = donations.Donor.ContactPhoneNumber ?? "",
                                                             RegistrationNumber = donations.Donor.RegistrationNumber ?? "",
                                                             OrganizationName = donations.Donor.OrganizationName ?? "",
                                                             OrganizationEmail = donations.Donor.OrganizationEmail ?? "",
                                                             DonorCity = donations.Donor.City ?? "",
                                                             DonorState = donations.Donor.State ?? "",
                                                             DonorFeedbackDate = donations.FeedbackDate,
                                                             DonorType = donations.Donor.DonorType.DonorTypeName ?? "",
                                                             TransactDate = donations.TransactionDate,
                                                             Status = donations.DonationStatu.StatusName ?? "",
                                                             DonationRef = donations.GUID.ToString() ?? "",
                                                             CertificateID = db.DonationCertificates.Where(v => v.CertificateID == db.DonationCertificates
                                                                              .Where(x => x.DonationID == donations.DonationID)
                                                                              .Max(x => x.CertificateID)).FirstOrDefault().CertificateReference ?? "",
                                                             DonationRefOnCertificate = donations.DonationID.ToString(),
                                                             Gender = donations.Donor.Gender

                                                         }).ToList();

                var wb = new XLWorkbook();

                var ws = wb.Worksheets.Add("Donations");
                ws.Cell(1, 1).Value = "TransactDate";
                ws.Cell(1, 2).Value = "DonorFeedbackDate";
                ws.Cell(1, 3).Value = "DonationRef";
                ws.Cell(1, 4).Value = "Status";
                ws.Cell(1, 5).Value = "DonorType";
                ws.Cell(1, 6).Value = "DonorName";
                ws.Cell(1, 7).Value = "DonorSurname";
                ws.Cell(1, 8).Value = "OrganizationName";
                ws.Cell(1, 9).Value = "OrganizationEmail";
                ws.Cell(1, 10).Value = "RegistrationNumber";

                ws.Cell(1, 11).Value = "CertificateID";
                ws.Cell(1, 12).Value = "DonationContatctPhoneNumber";
                ws.Cell(1, 13).Value = "DonorAddress1";
                ws.Cell(1, 14).Value = "DonorAddress2";
                ws.Cell(1, 15).Value = "DonationPostCode";
                ws.Cell(1, 16).Value = "DonorCity";
                ws.Cell(1, 17).Value = "DonorEmail";

                ws.Cell(1, 18).Value = "Province";
                ws.Cell(1, 19).Value = "Tax Number";
                ws.Cell(1, 20).Value = "IsPPSMember";
                ws.Cell(1, 21).Value = "MembershipNumber";

                ws.Cell(1, 22).Value = "Amount";
                ws.Cell(1, 23).Value = "Fees";
                ws.Cell(1, 24).Value = "NetAmount";
                ws.Cell(1, 25).Value = "Donation Receipt Number";
                ws.Cell(1, 26).Value = "Gender";

                var rangeWithData = ws.Cell(2, 1).InsertData(allDonations.AsEnumerable());

                var range = ws.Range("A1:Z" + (allDonations.Count + 1));
                var excelTable = range.CreateTable();

                // Add the totals row
                excelTable.ShowTotalsRow = true;
                // Notice how we're calling the cell by the column name
                /*excelTable.Field("TotalAmountOnOrder").TotalsRowFunction = XLTotalsRowFunction.Sum;
                excelTable.Field("VolumeOrdered").TotalsRowFunction = XLTotalsRowFunction.Sum;
                excelTable.Field("SOTotal").TotalsRowFunction = XLTotalsRowFunction.Sum;
                excelTable.Field("TransCost").TotalsRowFunction = XLTotalsRowFunction.Sum;
                excelTable.Field("ProfitOrLossAfterTrans").TotalsRowFunction = XLTotalsRowFunction.Sum;
                excelTable.Field("ProfitOrLoss").TotalsRowFunction = XLTotalsRowFunction.Sum;*/
                ws.Columns().AdjustToContents();



                wb.SaveAs(fs);

                return fs;
            }
        }
        public bool ResendCertification(string certificateGUID, IEmailSender emailServ, ISetupPayment paymentSettings, string userId)
        {
            EmailService emailService = (EmailService)emailServ;
            MemoryStream donationCertificate;
            using (var db = new PPSDonationEntities())
            {
                var certificate = db.DonationCertificates.Where(x => x.CertificateReference.Equals(certificateGUID)).FirstOrDefault();
                var donation = certificate != null && certificate.Donation != null ? certificate.Donation : null;
                var donor = donation.Donor;
                string name = donor.DonorType.DonorTypeName.Equals("Individual") ? donor.FirstName + " " + donor.LastName : donor.OrganizationName;

                DonorBase tempDonor = new MainDonor
                {
                    ContactPersonName = donor.ContactName,
                    Name = name,
                    DonorType = donor.DonorType.DonorTypeName,
                    Address1 = donor.Address1,
                    Address2 = donor.Address2,
                    ContactPersonPhoneNumber = donor.ContactPhoneNumber,
                    ContactPersonEmail = donor.ContactEmail,
                    OrganizationEmail = donor.OrganizationEmail,
                    CertificateEmail = donor.OrganizationEmail,

                    City = donor.City,
                    State = donor.State,
                    TaxNumber = donor.TaxNumber,
                    PostCode = donor.PostCode,
                    Country = donor.Country
                };

                DonationBase donationBase = new DonationBase
                {
                    Amount = donation.Amount.Value,
                    GUID = "PPS-" + donation.DonationID.ToString(),
                    DonationName = donation.DonationName,
                    DonationDate = donation.TransactionDate,
                    DonorData = tempDonor,
                    DonatorType = donation.DonatorType
                };


                string reportDirectory = ((PaymentSettings)paymentSettings).report_directory;
                List<string> emailList = new List<string>();
                var loggedinEmail = db.AspNetUsers.Where(x => x.Id.Equals(userId)).FirstOrDefault().Email;

                string emailbody = "Dear " + tempDonor.Name + ",<br><br>Thank you for your donation.<br><br>Please find attached your S18A Certificate that you would be able to utilize for income tax deduction purposes.<br><br>Kind Regards<br><br><strong>PPS Foundation</strong><p><img src='http://ppsfoundation.pps.co.za/Assets/img/PPS_Donation-Page_Logo-Large.jpg' alt='PPS Foundation Logo'></p>";
                emailList.Add(tempDonor.ContactPersonEmail);
                emailList.Add(loggedinEmail);
                string guid = Guid.NewGuid().ToString();

                donation.DonationCertificates.Add(new DonationCertificate
                {
                    Comment = "COMPLETE",
                    CertificateReference = guid,
                    SentEmail = emailbody,
                    SendStatus = "NOT SENT",
                    InsertDate = DateTime.Now,
                    CertificateDate = DateTime.Now
                });
                db.SaveChanges();
                DonationCertificateBase certificateFile = new DonationCertificateBase(donationBase, reportDirectory);
                donationCertificate = certificateFile.ConvertToPdf();
                emailService.SendEmail(emailList, emailbody, "PPS Foundation S18A Tax Certificate", donationCertificate, "PPS Foundation Donation Certificate.pdf");

                var certificateDoc = db.DonationCertificates.Where(x => x.CertificateReference.Equals(guid)).FirstOrDefault();
                if (certificateDoc != null)
                {
                    certificateDoc.ModifiedDate = DateTime.Now;
                    certificateDoc.SendDate = DateTime.Now;
                    certificateDoc.SendStatus = "SENT";
                    db.SaveChanges();
                }

                return true;
            }
        }

        public decimal GetDonationTotal()
        {
            using (var db = new PPSDonationEntities())
            {
                var donationList = db.Donations.Where(x => x.DonationStatu.StatusName.Equals("COMPLETE")).ToList();
                return (decimal)(donationList.Sum(v => v.Amount) ?? 0);
            }
        }
        //Student Donation reports
        public decimal GetStudentDoantion()
        {
            using (var db = new PPSDonationEntities())
            {
                var studentlist = db.AspNetStudents.Where(x => x.AmountofFundingNeeded > 0).ToList();
                return (decimal)(studentlist.Sum(c => c.AmountofFundingNeeded) ?? 0);
            }
        }

        public decimal GetTotalDonationfortheDay()
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1).AddTicks(-1);

            using (var db = new PPSDonationEntities())
            {
                var studentdonation = (from a in db.Donations.OrderByDescending(n => n.DonationID)
                                       where (a.TransactionDate >= start && a.TransactionDate <= end)
                                       select new { a.Amount });
                return (decimal)(studentdonation.Sum(c => c.Amount) ?? 0);
            }
        }

        public string GetEachStudentDonations()
        {
            //var eachDonations = new List<string>();
            using (var db = new PPSDonationEntities())
            {

                //var eachDonations = db.AspNetStudents.Select(x => x.Firstname).FirstOrDefault();
                var eachDonations = (from a in db.Donations
                                     join AspNetStudent in db.AspNetStudents on a.AspNetStudent.Id equals AspNetStudent.Id
                                     where a.Amount > 0
                                     select a).ToList();

                return eachDonations.ToString();
            }
            //return eachDonations.ToString();
        }

        public decimal HighestDonation()
        {
            using (var db = new PPSDonationEntities())
            {
                var heigh = (from a in db.Donations
                             group a by a.DonationID into g
                             select new
                             {
                                 Amount = g.Max(a => a.Amount)
                             });
                //return heigh;
                return (decimal)(heigh.Max(c => c.Amount) ?? 0);
            }
        }

        public decimal LowestDonation()
        {
            using (var db = new PPSDonationEntities())
            {
                var low = (from a in db.Donations
                           group a by a.DonationID into g
                           select new
                           {
                               Amount = g.Min(a => a.Amount)
                           });
                //return heigh;
                return (decimal)(low.Min(c => c.Amount) ?? 0);
            }
        }

        public void SendCertificate(DonationBase donationBase, IEmailSender emailServ, ISetupPayment paymentSettings)
        {
            EmailService emailService = (EmailService)emailServ;
            MemoryStream donationCertificate;
            using (var db = new PPSDonationEntities())
            {
                var donor = (MainDonor)donationBase.DonorData;
                string reportDirectory = ((PaymentSettings)paymentSettings).report_directory;
                List<string> emailList = new List<string>();
                string emailbody = "Dear " + donor.Name + ",<br><br>Thank you for your donation.<br><br>Please find attached your S18A Certificate that you would be able to utilize for income tax deduction purposes.<br><br>Kind Regards<br><br><strong>PPS Foundation</strong><p><img src='http://ppsfoundation.pps.co.za/Assets/img/PPS_Donation-Page_Logo-Large.jpg' alt='PPS Foundation Logo'></p>";
                emailList.Add(donor.DonorType == "Individual" ? donor.ContactPersonEmail : donor.CertificateEmail);
                string guid = Guid.NewGuid().ToString();
                var donation = db.Donations.Where(d => d.DonationID.Equals(donationBase.DonationRef)).FirstOrDefault(); //Refresh
                db.DonationCertificates.Add(new DonationCertificate
                {
                    Comment = "NOT SENT",
                    SendStatus = "NOT SENT",
                    DonationID = donation.DonationID,
                    CertificateReference = guid,
                    SentEmail = emailbody,
                    InsertDate = DateTime.Now,
                    CertificateDate = DateTime.Now
                });
                db.SaveChanges();
                DonationCertificateBase certificate = new DonationCertificateBase(donationBase, reportDirectory);
                donationCertificate = certificate.ConvertToPdf();
                emailService.SendEmail(emailList, emailbody, "PPS Foundation S18A Tax Certificate", donationCertificate, "PPS Foundation Donation Certificate.pdf");

                var certificateDoc = db.DonationCertificates.Where(x => x.CertificateReference.Equals(guid)).FirstOrDefault();
                if (certificateDoc != null)
                {
                    certificateDoc.ModifiedDate = DateTime.Now;
                    certificateDoc.SendDate = DateTime.Now;
                    certificateDoc.SendStatus = "SENT";
                    db.SaveChanges();
                }
            }
        }
    }
}