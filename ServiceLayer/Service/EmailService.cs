using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using InterfaceLayer;
using System.IO;
using System.Threading;
using InterfaceLayer.Models;
using ServiceLayer.Models;
using Microsoft.Exchange.WebServices.Data;

namespace ServiceLayer.Service
{
    public class EmailService : IEmailSender 
    {
        PaymentSettings settings;       
        MailMessage mail = new System.Net.Mail.MailMessage();
        SmtpClient client = new SmtpClient();
        EmailMessage message;
        ExchangeService service;
        ITraceListener ITraceListenerInstance;
        public EmailService(ISetupPayment mySettings)
        {
            settings = (PaymentSettings)mySettings;
            /*service = new ExchangeService();
            settings = (PaymentSettings)mySettings;
            ITraceListenerInstance = new TraceListener();
            service.TraceListener = ITraceListenerInstance;
            service.Credentials = new WebCredentials(settings.smtp_username, settings.smtp_password);//put this in the webconfig
            service.UseDefaultCredentials = false;
            service.AutodiscoverUrl(settings.smtp_username, (a) =>
            {
                return true;
            });*/
            //service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

            //Backup Email
            client.Port = int.Parse(settings.smtp_port);//put this in the webconfig
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(settings.smtp_username, settings.smtp_password);//put this in the webconfig
            client.Host = settings.smtp_host;

        }

        public bool SendEmail(List<string> emailTo, string body, string subject, MemoryStream file, string filename)
        {
            //MemoryStream copyStream = new MemoryStream(file.GetBuffer());
            //copyStream.Seek(0, SeekOrigin.Begin);
            /*
            message = new EmailMessage(service);
            message.Subject = subject;
            message.Body = body;
            
            message.Attachments.AddFileAttachment(filename, file);
            */
            /*foreach (string email in emailTo)
            {
                message.ToRecipients.Add(new EmailAddress(email));
            }*/
            
            try
            {
                // message.Save();
                //message.SendAndSaveCopy();
                SendEmailBackup(emailTo, body, subject, file, filename);
                return true;
            }
            catch (Exception ex)
            {
                
               throw new Exception("Failed to send email " + ex.Message );
            }
        }

        public void SendEmail(List<string> emails, string body, string subject)
        {
            /*message = new EmailMessage(service);
            message.Subject = subject;
            message.Body = body;
            foreach (string email in emails)
            {
                message.ToRecipients.Add(new EmailAddress(email));
            }
            */
            try
            {
                //message.Save();
                //message.SendAndSaveCopy();     
                SendEmailBackup(emails, body, subject);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email " + ex.Message);
            }
        }
        public bool SendEmailBackup(List<string> emails, string body, string subject)
        {
            mail.From = new MailAddress(settings.smtp_username);
            foreach (string email in emails)
            {
                mail.To.Add(new MailAddress(email));
            }
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;

            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to send email " + ex.Message + " "+ ex.InnerException);

            }
        }
        public bool SendEmailBackup(List<string> emailTo, string body, string subject, MemoryStream file, string filename)
        {
            mail.From = new MailAddress(settings.smtp_username);            
            mail.Attachments.Add(new System.Net.Mail.Attachment(file, filename));
            foreach (string email in emailTo)
            {
                mail.To.Add(new MailAddress(email));
            }
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;

            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to send email " + ex.Message + " " + ex.InnerException);

            }
        }
    }
}