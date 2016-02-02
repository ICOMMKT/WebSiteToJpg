using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace GetWebSitesToJPG.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             RegisterHyperLink.NavigateUrl = "/Account/Register.aspx";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            var returnUrl = Request.QueryString["ReturnUrl"];
            //OpenAuthLogin.ReturnUrl = returnUrl;

            var errorExternalAuth = Request.QueryString["error"];
            if (!string.IsNullOrEmpty(errorExternalAuth))
            {
                StatusText.Text = "External authentication failed, try again or use another authentication method.";
                LoginStatus.Visible = true;
            }
              
            bool val = (HttpContext.Current.User != null) && HttpContext.Current.User.Identity.IsAuthenticated;
            if(val)
            {
                logForm.Visible = false;
                RegisterHyperLink.NavigateUrl = "/Default.aspx";
                RegisterHyperLink.Text = "Return to main page";
                StatusText.Text = "Welcome " + User.Identity.GetUserName();
                LoginStatus.Visible = true;
            }
        }

        protected void SignIn(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            // This doen't count login failures towards account lockout
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = signinManager.PasswordSignIn(UserName.Text, Password.Text, false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    FormsAuthentication.RedirectFromLoginPage(UserName.Text, true);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.LockedOut:
                    //Response.Redirect("/Account/Lockout");
                    StatusText.Text = "your account has been locked";
                    LoginStatus.Visible = true;
                    break;
                case SignInStatus.RequiresVerification:
                    //Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                    //                                Request.QueryString["ReturnUrl"],
                    //                                false),
                    //                  true);
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

        protected void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("~/Account/Login.aspx");
        }

    }
}