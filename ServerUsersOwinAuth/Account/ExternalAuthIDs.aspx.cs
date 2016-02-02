using ServerUsersOwinAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServerUsersOwinAuth.Account
{
    public partial class ExternalAuthIDs : System.Web.UI.Page
    {
        private AppDBContext db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new AppDBContext();

            var externalAuthClients = db.ExternalAuthClients.ToList();
            if(externalAuthClients == null || externalAuthClients.Count == 0)
            {
                StatusText.Text = "Does not exist any record yet";
                StatusPane.Visible = true;
            }
            else
            {
                grdVAuthLogins.DataSource = externalAuthClients;
                grdVAuthLogins.DataBind();
            }

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            db = new AppDBContext();

            var url = txtUrl.Text;

            Guid g = Guid.NewGuid();
            string urlId = g.ToString();
            urlId = urlId.Replace("-", "");
            //urlId = urlId.Replace("+", "");

            var authUrlitem = new ExternalAuthClients
            {
                Id = urlId,
                Url = url,
                Name = txtName.Text
            };
            db.ExternalAuthClients.Add(authUrlitem);
            db.SaveChanges();

            Response.Redirect(Request.RawUrl);
        }
    }
}