using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace GetWebSitesToJPG
{
    /// <summary>
    /// Summary description for GetImageHandler
    /// </summary>
    public class GetImageHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            var imageId = RequestContext.RouteData.Values["imageID"];
            var hasContent = imageId == null ? false : true;
            if(hasContent)
            {
                string imagePath = "~/Content/Images/Screenshots/" + imageId + ".jpg";
                context.Response.ContentType = "image/jpg";
                context.Response.WriteFile(imagePath);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class RouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new GetImageHandler() { RequestContext = requestContext };
        }
    }

}