using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebApplication.Models;
using System.Collections.Generic;
using InterfaceLayer.Models;
using ServiceLayer.Models;
using System.Web;

namespace WebApplication
{
    public class EmailService : IIdentityMessageService
    {
        public static ISetupPayment settings;
        public EmailService()
        {
            settings = new PaymentSettings
            {
                post_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdPaymentURL"],
                return_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdReturnURL"],
                notify_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdNotifyURL"],
                cancel_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdCancelURL"],
                validate_url_prod = System.Configuration.ConfigurationManager.AppSettings["ProdValidateURL"],

                post_url_test = System.Configuration.ConfigurationManager.AppSettings["TestPaymentURL"],
                return_url_test = System.Configuration.ConfigurationManager.AppSettings["TestReturnURL"],
                notify_url_test = System.Configuration.ConfigurationManager.AppSettings["TestNotifyURL"],
                cancel_url_test = System.Configuration.ConfigurationManager.AppSettings["TestCancelURL"],
                validate_url_test = System.Configuration.ConfigurationManager.AppSettings["TestValidateURL"],

                merchant_id = System.Configuration.ConfigurationManager.AppSettings["MerchantID"],
                merchant_key = System.Configuration.ConfigurationManager.AppSettings["MerchantKey"],
                payment_provider = System.Configuration.ConfigurationManager.AppSettings["PaymentProvider"],
                environment_type = System.Configuration.ConfigurationManager.AppSettings["Environment"],

                email_confirmation = System.Configuration.ConfigurationManager.AppSettings["EmailConfirmation"],
                admin_email = System.Configuration.ConfigurationManager.AppSettings["PaymentAdminEmail"],

                smtp_host = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"],
                smtp_port = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"],
                smtp_security = System.Configuration.ConfigurationManager.AppSettings["SMTPSecurity"],
                smtp_username = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"],
                smtp_password = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"],
                report_directory = System.Configuration.ConfigurationManager.AppSettings["ReportDirectory"] ?? HttpContext.Current.Server.MapPath("~/App_Data/")
                
            };
        }

        public Task SendAsync(IdentityMessage message)
        {
            
            List<string> emails = new List<string>();
            emails.Add(message.Destination);
            // Plug in your email service here to send an email.
            new ServiceLayer.Service.EmailService(settings).SendEmail(emails, message.Body, message.Subject);
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            List<string> cellnumbers = new List<string>();
            cellnumbers.Add(message.Destination);
            // Plug in your sms service here to send an sms.
            new ServiceLayer.Service.BulkSmsService().SendSMS(cellnumbers, message.Body);
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
        public override Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };
            this.AuthenticationManager.User.AddIdentity(new ClaimsIdentity(claims));
            return base.SignInAsync(user, isPersistent, rememberBrowser);
        }
    }
   
}
