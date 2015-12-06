using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GetWebSitesToJPG
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void SignOut_or_SignIn(object sender, EventArgs e)
        {
            bool val = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if(val)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                authenticationManager.SignOut();
                Response.Redirect("~/Login.aspx");
                var sjdjsd = Context.User.Identity.GetUserName();
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
            
        }
    }
}