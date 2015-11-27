using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GetWebSitesToJPG
{
    public partial class GetImagesRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var imageId = Page.RouteData.Values["imageID"];
            lblGetParams.Text = imageId == null ? "Aqui no llego nada" : imageId.ToString(); 
        }
    }
}