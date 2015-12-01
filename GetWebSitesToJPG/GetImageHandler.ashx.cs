using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using GetWebSitesToJPG.Logic;
using GetWebSitesToJPG.Models;
using System.Drawing;

namespace GetWebSitesToJPG
{
    /// <summary>
    /// override ProcessRequest method from IHttpHandler
    /// </summary>
    public class GetImageHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        /// <summary>
        /// Return an image from the imageID param in RouteData value
        /// </summary>
        /// <param name="context">App Context</param>
        public void ProcessRequest(HttpContext context)
        {
            var imageId = RequestContext.RouteData.Values["imageID"];
            var hasContent = imageId == null ? false : true;
            if(hasContent)
            {
                ImageCropData imgData = new ImageCropData();
                using (ImageDBActions readImgFromDB = new ImageDBActions())
                {
                    imgData = readImgFromDB.GetImageData(imageId.ToString());
                }
                //string serverpath = RequestContext.HttpContext.Server.MapPath("Content/Images/Screenshots");
                var image = ImageRenderMethods.GetWebsiteImage(imgData);
                ImageConverter converter = new ImageConverter();
                byte[] buffer = (byte[])converter.ConvertTo(image, typeof(byte[]));

                //string imagePath = "~/Content/Images/Screenshots/" + imageId + ".jpg";
                context.Response.ContentType = "image/jpg";
                context.Response.BinaryWrite(buffer);
                context.Response.Flush();
                image.Dispose();
                //context.Response.WriteFile(imagePath);
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