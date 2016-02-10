using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Infrastructure;
using Newtonsoft.Json;
using ServerUsersOwinAuth;
using ServerUsersOwinAuth.Logic;
using ServerUsersOwinAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PruebaOwinIconmkt.oauth
{
    public partial class authenticate : System.Web.UI.Page
    {     
        protected string AppName { get; set; }

        //TODO: Encrypt connection through HTTPS
        //TODO: Encrypt querystring
        protected async void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                var clientid = Request.QueryString["client_id"];
                string redirectUri = Request.QueryString["redirect_uri"];
                var state = Request.QueryString["state"];

                if (clientid != null && redirectUri != null && state != null)
                {
                    var dbAction = new DbActions();
                    var externalAppName = await dbAction.GetExternalAppName(clientid);
                    if (externalAppName != null)
                    {
                        AppName = externalAppName;
                        //TODO: Encrypt hiddens data
                        hdn_IdApp.Value = clientid;
                        redirectUri = Uri.UnescapeDataString(redirectUri).Trim();
                        hdn_redUri.Value = redirectUri;
                        hdn_state.Value = state;                
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        var userid = User.Identity.GetUserId();
                        var tokenGranted = await dbAction.AreGrantedPermissionsAsync(userid);
                        if(tokenGranted != null)
                        { 
                            if (tokenGranted.GrantedAccess)
                            {
                                redirectUri = WebUtilities.AddQueryString(redirectUri, "token", tokenGranted.Token);
                                redirectUri = WebUtilities.AddQueryString(redirectUri, "state", state);
                                Response.Redirect(redirectUri);
                            }
                        }
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
            string redirectUri = hdn_redUri.Value;
            var state = hdn_state.Value;

            if (!string.IsNullOrEmpty((redirectUri)))
            {
                redirectUri += "?error=access_denied&state=" + state;
                Response.Redirect(redirectUri);
            }
        }

        [WebMethod(EnableSession = false)]
        public static string AllowAccess(string idApp, string redirectUri, string state)
        {
            //TODO: UnEncrypt data from parameters
            string json = String.Empty;
            if (!string.IsNullOrEmpty(idApp))
            {
                var page = new authenticate();
                var userid = page.User.Identity.GetUserId();

                var _idApp = idApp;

                var time = DateTime.UtcNow;
                byte[] timeByteArray = BitConverter.GetBytes(time.ToBinary());
                var key = Guid.NewGuid();
                byte[] keyByteArray = key.ToByteArray();
                string token = Convert.ToBase64String(timeByteArray.Concat(keyByteArray).ToArray());
                token = token.Replace("+", "").Replace("/", "");
                var usersAppsAccessGranted = new UsersAppsAccessGranted
                {
                    Userid = userid,
                    ExternalAuthClientsID = _idApp,
                    AccessGranted = true,
                    Key = key.ToString(),
                    Token = token,
                    CreatedOn = time
                };

                var dbAction = new DbActions();
                dbAction.AddUserAppsAccessGranted(usersAppsAccessGranted);

                var returnData = new
                {
                    Token = token,
                    RedirectUri = redirectUri,
                    State = state 
                };

                json  = JsonConvert.SerializeObject(returnData);
            }
            return json;
        }
    }
}