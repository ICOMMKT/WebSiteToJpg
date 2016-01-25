using GetWebSitesToJPG.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GetWebSitesToJPG.Account
{
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderDisplayName
        {
            get { return (string)ViewState["ProviderDisplayName"] ?? String.Empty; }
            private set { ViewState["ProviderDisplayName"] = value; }
        }

        protected string ProviderUserId
        {
            get { return (string)ViewState["ProviderUserId"] ?? String.Empty; }
            private set { ViewState["ProviderUserId"] = value; }
        }

        protected string ProviderUserName
        {
            get { return (string)ViewState["ProviderUserName"] ?? String.Empty; }
            private set { ViewState["ProviderUserName"] = value; }
        }

        protected void LogIn_Click(object sender, EventArgs e)
        {
            CreateAndLoginUser();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Process the result from an auth provider in the request
            ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (String.IsNullOrEmpty(ProviderName))
            {
                Response.Redirect("~/Account/Login");
            }
            if (!IsPostBack)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    Response.Redirect("~/Account/Login");
                }
                var user = manager.Find(loginInfo.Login);
                if (user != null)
                {
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                    var result = signinManager.ExternalSignIn(loginInfo, isPersistent: false);

                    switch (result)
                    {
                        case SignInStatus.Success:
                            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                            break;
                        case SignInStatus.LockedOut:
                            //Response.Redirect("/Account/Lockout");
                            StatusText.Text = "your account has been locked";
                            LoginStatus.Visible = true;
                            break;
                        case SignInStatus.RequiresVerification:
                            StatusText.Text = "your account is enabled to use two factor authentication";
                            LoginStatus.Visible = true;
                            break;
                        case SignInStatus.Failure:
                        default:
                            StatusText.Text = "Invalid login attempt";
                            LoginStatus.Visible = true;
                            break;
                    }
                }
                else if (User.Identity.IsAuthenticated)
                {
                    // Apply Xsrf check when linking
                    var verifiedloginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                    if (verifiedloginInfo == null)
                    {
                        Response.Redirect("~/Account/Login");
                    }

                    var result = manager.AddLogin(User.Identity.GetUserId(), verifiedloginInfo.Login);
                    if (result.Succeeded)
                    {
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    }
                    else
                    {
                        AddErrors(result);
                        return;
                    }
                }
                else
                {
                    Username.Text = loginInfo.DefaultUserName;
                    Application["loginInfo"] = loginInfo;
                }
            }
        }

        private void CreateAndLoginUser()
        {
            if (!IsValid)
            {
                return;
            }
            //var currentApplicationId = new CustomUserDBContext().Applications.SingleOrDefault(x => x.ApplicationName == "/").ApplicationId;

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var user = new User() { UserName = Username.Text, ApplicationId = currentApplicationId };

            //user.CreatePasswordLogin();
            //user.PasswordLogin.IsApproved = false;

            // Copy the PasswordSalt and Password format
            //var passwordHasher = manager.PasswordHasher as UserPasswordHasher;
            //user.PasswordLogin.PasswordFormat = passwordHasher.PasswordFormat;
            // user.PasswordLogin.PasswordSalt = passwordHasher.PasswordSalt;

            // NEed to pass in a random password since the 'Password' column in the DB cannot be null
            var user = new ApplicationUser() { UserName = Username.Text };

            IdentityResult result = manager.Create(user);

            if (result.Succeeded)
            {
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    loginInfo = (ExternalLoginInfo)Application["loginInfo"];
                    if(loginInfo == null)
                    {
                        Response.Redirect("~/Account/Login");
                        return;
                    }
                }
                result = manager.AddLogin(user.Id, loginInfo.Login);
                if (result.Succeeded)
                {
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    return;
                }
            }
            StatusText.Text = result.Errors.FirstOrDefault();
            LoginStatus.Visible = true;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}