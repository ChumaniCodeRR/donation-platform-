using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using InterfaceLayer.Models;
using ServiceLayer.BLL;
using TheArtOfDev.HtmlRenderer;
using DataLayer.Model;
using System.Reflection;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DataLayer.Model;

namespace ServiceLayer.Report.DonationCertificate
{
    public class DonationCertificateBase : IPdf
    {
        IPayable certificateData;
        DonationBase donationData;
        string reportDirectory;
        public DonationCertificateBase(DonationBase donationData, string reportDirectory)
        {
            this.donationData = donationData;
            this.reportDirectory = reportDirectory;
        }
        public MemoryStream ConvertToPdf()
        {
            var data = (DonorBase)donationData.DonorData;

         
            MemoryStream fs = new MemoryStream();

            try
            {
                //fs.Flush();
                //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Report\DonationCertificate\index.html");

                string appDataFolder = Path.Combine(reportDirectory, @"CertificateTemplate\index.html");

                //string appDataFolder = Path.Combine(reportDirectory, @"CertificateTemplate\text.html");

                string CertificateHTML = File.ReadAllText(appDataFolder);

                CertificateHTML = LoadData(CertificateHTML);

                PdfDocument pdf = PdfGenerator.GeneratePdf(CertificateHTML, PdfSharp.PageSize.A4, 0, null, null, HandleImageLoad);

                //PdfDocument pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(CertificateHTML, PdfSharp.PageSize.A4, 0, null, null, HandleImageLoad);

                pdf.Save(fs, false);

                //byte[] buffer = new byte[fs.Length];

                //buffer.ToArray();

                fs.Seek(0, SeekOrigin.Begin);


                //fs.Close();
                fs.Flush();
                //fs.Read(buffer, 0, (int)fs.Length);
                //fs.Position = 0;

                //fs.Seek(0, SeekOrigin.Begin);
                GC.Collect();
                //fs.CopyTo(pdf);

                return fs;
            }
            catch
            {
                fs.Dispose();
                //fs.Close();
                throw;
            }
           
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public void HandleImageLoad(object sender, HtmlImageLoadEventArgs args)
        {
            //string appDataFolder = Path.Combine(reportDirectory, @"CertificateTemplate\\img\\PPS-Headerbanner-2480px-487px.jpg");
            string appDataFolder = Path.Combine(reportDirectory, @"CertificateTemplate\\img\\PPS-Headerbanner-1920px-377px.jpg");
            FileStream file = new FileStream(appDataFolder, FileMode.Open, FileAccess.Read);
            using (var mem = new MemoryStream())
            {
                //new line 
                //mem.Flush();
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                mem.Write(bytes, 0, (int)file.Length);
                mem.Seek(0, SeekOrigin.Begin);
                System.Drawing.Image sysImg = System.Drawing.Image.FromStream(mem, false, true);
                var ximg = PdfSharp.Drawing.XImage.FromGdiPlusImage(ResizeImage(sysImg, 595, 100));

                args.Callback(ximg);
            }
        }
        private string LoadData(string html)
        {
            DonorBase donorData = (DonorBase)donationData.DonorData;
            string currency = "R";
            string finalCertificate = html;
            /** TODO: [Gilbert]Use get type. This is just in case you have different certificate format
            /*For individual and organization*/


            switch (donorData.DonorType)
            {
                case "Individual":
                    var individualDonor = (MainDonor)donorData;
                    finalCertificate = finalCertificate.Replace("[donor_name]", individualDonor.Name);
                    finalCertificate = finalCertificate.Replace("[donor_address1]", individualDonor.Address1);
                    finalCertificate = finalCertificate.Replace("[donor_address2]", individualDonor.Address2);

                    finalCertificate = finalCertificate.Replace("[taxnumber_label]", String.IsNullOrWhiteSpace(individualDonor.TaxNumber) ? null : "TAX NUMBER:");
                    finalCertificate = finalCertificate.Replace("[donor_taxnumber]", individualDonor.TaxNumber);
                    finalCertificate = finalCertificate.Replace("[donor_province]", individualDonor.State);
                    finalCertificate = finalCertificate.Replace("[donor_city]", individualDonor.City);
                    finalCertificate = finalCertificate.Replace("[donor_country]", individualDonor.Country);
                    finalCertificate = finalCertificate.Replace("[donor_pobox]", individualDonor.PostCode);
                    break;
                case "Organisation":
                    var organizationDonor = (MainDonor)donorData;
                    finalCertificate = finalCertificate.Replace("[donor_name]", organizationDonor.Name);
                    finalCertificate = finalCertificate.Replace("[donor_address1]", organizationDonor.Address1);
                    finalCertificate = finalCertificate.Replace("[donor_address2]", organizationDonor.Address2);

                    finalCertificate = finalCertificate.Replace("[taxnumber_label]", String.IsNullOrWhiteSpace(organizationDonor.TaxNumber) ? null : "TAX NUMBER:");
                    finalCertificate = finalCertificate.Replace("[donor_taxnumber]", organizationDonor.TaxNumber);
                    finalCertificate = finalCertificate.Replace("[donor_province]", organizationDonor.State);
                    finalCertificate = finalCertificate.Replace("[donor_city]", organizationDonor.City);
                    finalCertificate = finalCertificate.Replace("[donor_country]", organizationDonor.Country);
                    finalCertificate = finalCertificate.Replace("[donor_pobox]", organizationDonor.PostCode);
                    break;
            }

            finalCertificate = finalCertificate.Replace("[donation_amount]", currency + " " + donationData.Amount.ToString());
            finalCertificate = finalCertificate.Replace("[donation_receive_date]", donationData.DonationDate.ToString("yyyy/MM/dd"));
            finalCertificate = finalCertificate.Replace("[donation_public_benefit]", "930 - 055 - 026");
            finalCertificate = finalCertificate.Replace("[donation_receiptno]", donationData.GUID);
            finalCertificate = finalCertificate.Replace("[donation_certificate_date]", DateTime.Now.ToString("yyyy/MM/dd"));
            finalCertificate = finalCertificate.Replace("[donator_type]", donationData.DonatorType);

            return finalCertificate;
        }
    }
}