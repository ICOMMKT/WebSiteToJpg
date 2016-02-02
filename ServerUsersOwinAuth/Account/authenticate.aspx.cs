using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServerUsersOwinAuth;
using ServerUsersOwinAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PruebaOwinIconmkt.oauth
{
    public partial class authenticate : System.Web.UI.Page
    {
        private AppDBContext db;
         
        protected string AppName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                var clientid = Request.QueryString["client_id"];
                
                if (clientid != null)
                {
                    db = new AppDBContext();
                    var externalAuth = db.ExternalAuthClients.Where(x => x.Id == clientid).SingleOrDefault();
                    if (externalAuth != null)
                    {
                        //Response.StatusCode = 302;
                        AppName = externalAuth.Name;
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        LogForm.Visible = false;
                        AuthPrompt.Visible = true;
                        ErrorMessage.Visible = true;
                    }
                }
                else
                {
                    LogForm.Visible = false;
                    FailureText.Text = "Are you lost?... please go back to the home page";
                    ErrorMessage.Visible = true;
                }
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(txt_Username.Text, txt_Password.Text, false, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        //AuthPrompt.Visible = true;
                        Response.Redirect(Request.RawUrl);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                        Request.QueryString["ReturnUrl"],
                                                        false),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }

        protected void DenyAccess(object sender, EventArgs e)
        {
            string redirectUri = Request.QueryString["redirect_uri"];
            redirectUri = Uri.UnescapeDataString(redirectUri);
            redirectUri = redirectUri.Trim();
            var state = Request.QueryString["state"];
            
            if (!string.IsNullOrEmpty((redirectUri)))
            {
                redirectUri += "?error=access_denied&state="+state;
            }
            Response.Redirect(redirectUri);
        }

        protected void AllowAccess(object sender, EventArgs e)
        {
            var userid = User.Identity.GetUserId();
            

            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}