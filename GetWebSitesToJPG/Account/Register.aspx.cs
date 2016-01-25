using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using GetWebSitesToJPG.Models;

namespace GetWebSitesToJPG.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            string pass = Password.Text;
            string confPass = ConfirmPassword.Text;

            if (String.IsNullOrEmpty(pass))
            {
                StatusMessage.Text = "The Password field can not be empty";
            }
            else
            {
                if (String.IsNullOrEmpty(confPass))
                {
                    StatusMessage.Text = "The Confirm Password field can not be empty";
                }
                else
                {
                    if (!(pass == confPass))
                    {
                        StatusMessage.Text = "The passwords don't match, please try again";
                    }
                    else
                    {
                        // Default UserStore constructor uses the default connection string named: DefaultConnection
                        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                        var user = new ApplicationUser() { UserName = UserName.Text };

                        IdentityResult result = manager.Create(user, Password.Text);

                        if (result.Succeeded)
                        {
                            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                            var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                            authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                            Response.Redirect("~/Default.aspx");
                        }
                        else
                        {
                            StatusMessage.Text = result.Errors.FirstOrDefault();
                        }
                    }
                    
                }
            }
        }
    }
}