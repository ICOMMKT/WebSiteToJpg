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
        private const string keyEncryptQueryString = "@2^2L9*u";
        //TODO: Encrypt connection through HTTPS
        protected async void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //storage encrypted values
                var id = Request.QueryString["id"];
                string clientid = "", redirectUri = "", state = "";
                //var cipher = new StringCipher();

                //if is not encrypted the value is empty
                if (string.IsNullOrEmpty(id))
                {
                    clientid = Request.QueryString["client_id"];
                    redirectUri = Uri.UnescapeDataString(Request.QueryString["redirect_uri"]).Trim();
                    state = Request.QueryString["state"];
                    if (clientid != null && redirectUri != null && state != null)
                    {
                        //Encrypt URL
                        string[] urlSplit = Request.Url.ToString().Split('?');
                        string encryptedstring = StringCipher.Encrypt(urlSplit[1], keyEncryptQueryString);
                        var encriptedUrlSafe = HttpUtility.UrlEncode(encryptedstring);
                        string urlEncrypted = urlSplit[0] + "?id=" + encriptedUrlSafe;
                        HttpContext.Current.Response.Redirect(urlEncrypted, false);
                    }
                    else
                    {
                        LogForm.Visible = false;
                        FailureText.Text = "Are you lost?... please go back to the home page";
                        ErrorMessage.Visible = true;
                    }                  
                }
                else
                {
                    hdn_Id.Value = id;
                    //Decrypt values
                    var _id = StringCipher.Decrypt(id.ToString(), keyEncryptQueryString);
                    var arrValues = DecryptValues(_id);
                    clientid = arrValues[0];
                    redirectUri = arrValues[1];
                    state = arrValues[2];


                    var dbAction = new DbActions();
                    var externalAppName = await dbAction.GetExternalAppName(clientid);
                    if (externalAppName != null)
                    {
                        AppName = externalAppName;
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        var userid = User.Identity.GetUserId();
                        var tokenGranted = await dbAction.AreGrantedPermissionsAsync(userid);
                        if (tokenGranted != null)
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
            var id = hdn_Id.Value;
            //Decrypt values
            var _id = StringCipher.Decrypt(id.ToString(), keyEncryptQueryString);
            var arrValues = DecryptValues(_id);
            string redirectUri = arrValues[1];
            string state = arrValues[2];

            if (!string.IsNullOrEmpty((redirectUri)))
            {
                redirectUri += "?error=access_denied&state=" + state;
                Response.Redirect(redirectUri);
            }
        }

        [WebMethod(EnableSession = false)]
        public static string AllowAccess(string id)
        {
            string json = String.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                var page = new authenticate();
                var userid = page.User.Identity.GetUserId();

                //Decrypt values
                var _id = StringCipher.Decrypt(id.ToString(), keyEncryptQueryString);
                var arrValues = page.DecryptValues(_id);
                string clientid = arrValues[0];
                string redirectUri = arrValues[1];
                string state = arrValues[2];

                var time = DateTime.UtcNow;
                byte[] timeByteArray = BitConverter.GetBytes(time.ToBinary());
                var key = Guid.NewGuid();
                byte[] keyByteArray = key.ToByteArray();
                string token = Convert.ToBase64String(timeByteArray.Concat(keyByteArray).ToArray());
                token = token.Replace("+", "").Replace("/", "");
                var usersAppsAccessGranted = new UsersAppsAccessGranted
                {
                    Userid = userid,
                    ExternalAuthClientsID = clientid,
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

                json = JsonConvert.SerializeObject(returnData);
            }
            return json;
        }

        public List<string> DecryptValues(string id)
        {
            var valuesReturned = new List<string>();
            string[] arrQuery = id.Split('&');
            string[] arrQueryValues;

            arrQueryValues = arrQuery[0].Split('=');
            valuesReturned.Add(arrQueryValues[1].Trim());

            arrQueryValues = arrQuery[1].Split('=');
            valuesReturned.Add(Uri.UnescapeDataString(arrQueryValues[1]).Trim());

            arrQueryValues = arrQuery[2].Split('=');
            valuesReturned.Add(arrQueryValues[1].Trim());

            return valuesReturned;
        }
    }
}