using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CorTabernaclChoir.Data;

namespace CorTabernaclChoir.Controllers
{
    [Authorize]
    [Title(nameof(Resources.AdminTitle), nameof(Resources.AdminTitle))]
    public class AccountController : Controller
    {
        private const string ExternalProviderName = "Google";
        private const string InvalidEmailAddressMessage = "You are currently signed in to Google with account {0}, " +
                                                          "which is not a valid account for this site. Please log out of this account " +
                                                          "and log in to the choir Gmail account.";

        private readonly IAppSettingsService _appSettings;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(IAppSettingsService appSettings)
        {
            _appSettings = appSettings;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAppSettingsService appSettings)
            :this(appSettings)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        [AllowAnonymous]
        [Route("~/Account/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("~/Account/ExternalLogin")]
        public ActionResult ExternalLogin()
        {
            return new ChallengeResult(ExternalProviderName, Url.Action("ExternalLoginCallback", "Account"));
        }

        [AllowAnonymous]
        [Route("~/Account/ExternalLoginCallback")]
        public async Task<ActionResult> ExternalLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (loginInfo.Email != _appSettings.ValidGmailLogin)
            {
                ModelState.AddModelError("", string.Format(InvalidEmailAddressMessage, loginInfo.Email));

                return View("Login");
            }

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            if (result == SignInStatus.Success)
                return RedirectToAction("Index", "Admin");

            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View("ExternalLoginFailure");
            }

            var user = new ApplicationUser { UserName = loginInfo.Email, Email = loginInfo.Email };
            var createUser = await UserManager.CreateAsync(user);

            if (createUser.Succeeded)
            {
                var addLogin = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (addLogin.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View("ExternalLoginFailure");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Account/LogOff")]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            // Used for XSRF protection when adding external logins
            private const string XsrfKey = "XsrfId";

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
        #endregion
    }
}