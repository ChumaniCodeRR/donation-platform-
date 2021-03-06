using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication.Models;
using ServiceLayer.BLL;
using InterfaceLayer.Models;
using System.Collections.Generic;
using ServiceLayer.Service;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Routing;
using ServiceLayer.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private string _loginEmailBody = "";
        private string _welcome = "Welcome to PPS DonationAdmin System";
        private List<SelectListItem> roles = new List<SelectListItem>();
        private List<SelectListItem> userlist = new List<SelectListItem>();
        
        public AccountController()
        {
            
        
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        void LoadRoles()
        {

            new UserService().GetRoleList(User.IsInRole("Client"), User.Identity.GetUserId()).ForEach(x =>
            {
                roles.Add(new SelectListItem { Text = x.RoleName, Value = x.RoleId });
            });
        }
        void LoadUsers()
        {
            new UserService().GetList(User.IsInRole("Client"), User.Identity.GetUserId(), "All").ForEach(x =>
            {
                userlist.Add(new SelectListItem { Text = x.NameIdentifier + " - " + x.CompanyName, Value = x.Id });
            });

        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //[OutputCache(Duration = 43200, VaryByCustom = "User")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.CompanyName = ConfigurationManager.AppSettings["companyname"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            BusinessMessage ViewData = new BusinessRule().IsAllowedToLogin(model.Email);
            if (ViewData != null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(ViewData.ViewName, ViewData.ViewModel);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            var user = await UserManager.FindByNameAsync(model.Email);
            if(result == SignInStatus.Failure)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            {
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                
                string body = _welcome + "<br/>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                string subject = "Please Confirm Your Email";
                List<string> emaillist = new List<string>();
                emaillist.Add(user.Email);
                try
                {
                    new ServiceLayer.Service.EmailService(EmailService.settings).SendEmail(emaillist, body, subject);
                }catch(Exception ex)
                {
                    //TODO: Log exception to IIS or elma
                }
            }

            
            
            string bodyc = _welcome+"<br/>This is a login notification. You have logged in at "+ DateTime.Now +  " from IP " + Request.UserHostAddress;
            string subjectc = "PPS Online Login";
            List<string> emaillistc = new List<string>();
            emaillistc.Add(user.Email);
            try {
                new ServiceLayer.Service.EmailService(EmailService.settings).SendEmail(emaillistc, bodyc, subjectc);
            }
            catch (Exception ex)
            {
                //TODO: Log exception to IIS or elma
            }
            switch (result)
            {
                case SignInStatus.Success: 
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }        
         }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Settings()
        {
            LoadRoles();
            LoadUsers();
            IdentityRole ident = new IdentityRole();
            List<UserAssignRep> userroles = new List<UserAssignRep>();

            UserClientViewModel Model = new UserClientViewModel();
            Model.Roles = roles;
            Model.Users = userlist;
            Model.UserAssignmnt = new UserService().GetUserRoleAssignmntList(User.IsInRole("Client"), User.Identity.GetUserId());
            return View(Model);

        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, NameIdentifier = model.NameIdentifier, CompanyName = model.CompanyName };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    var currentUser = UserManager.FindByName(user.UserName);
                    //Everyone is a client by default
                    var roleresult = UserManager.AddToRole(currentUser.Id, "Client");
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",   new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    
                    string body = _welcome+"<br/>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    string subject = "Please Confirm Your Email";
                    List<string> emaillist = new List<string>();
                    emaillist.Add(user.Email);
                    emaillist.Add(((PaymentSettings)EmailService.settings).support_admin);
                    new ServiceLayer.Service.EmailService(EmailService.settings).SendEmail(emaillist, body, subject);
                    return RedirectToAction("Login", "Account");
                }
                if (result != null && ((String[])result.Errors)[0].Contains("already taken")){
                    string[] errors = {"Error Occured While Registering. Please contact PPS IT department"};
                    AddErrors(new IdentityResult (errors));
                } else {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                
                string body = _welcome + "<br>Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                string subject = "Reset Password";
                List<string> emaillist = new List<string>();
                emaillist.Add(user.Email);
                new ServiceLayer.Service.EmailService(EmailService.settings).SendEmail(emaillist, body, subject);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string userId)
        {
            var user = UserManager.FindById(userId);
            var viewmodel = user != null ? new ResetPasswordViewModel
            {
                Email = user.Email
            } : null;
            return code == null ? View("Error") : View(viewmodel);
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Dashboard", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        
        /*void LoadRoles()
        {

            new UserService().GetRoleList(User.IsInRole("Client"), User.Identity.GetUserId()).ForEach(x =>
            {
                roles.Add(new SelectListItem { Text = x.RoleName , Value = x.RoleId });
            });
        }
        void LoadUsers()
        {
            new UserService().GetList(User.IsInRole("Client"), User.Identity.GetUserId(), "All").ForEach(x =>
            {
                userlist.Add(new SelectListItem { Text = x.NameIdentifier + " - " + x.CompanyName, Value = x.Id });
            });

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Settings()
        {
            LoadRoles();
            LoadUsers();
            IdentityRole ident = new IdentityRole();
            List<UserAssignRep> userroles = new List<UserAssignRep>();
           
            UserClientViewModel Model = new UserClientViewModel();
            Model.Roles = roles;
            Model.Users = userlist;
            Model.UserAssignmnt = new UserService().GetUserRoleAssignmntList(User.IsInRole("Client"), User.Identity.GetUserId());
            return View(Model);

        }*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveAssignment(UserAssignModel jsondata)
        {

            return Json(new UserService().SaveAssignment(jsondata, User.Identity.Name, "Role", User.Identity.Name));

        }
    }
}
#endregion