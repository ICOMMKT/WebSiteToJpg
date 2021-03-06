﻿using GetWebSitesToJPG___BackEnd.Logic;
using ImageService.Logic;
using ImageService.Models;
using System.Drawing;
using System.Web;
using System.Web.Routing;

namespace GetWebSitesToJPG___BackEnd
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
                string serverpath = RequestContext.HttpContext.Server.MapPath("Content/Images/Screenshots");
                var image = ImageRenderMethods.GetWebsiteImage(imgData, serverpath);
                ImageConverter converter = new ImageConverter();
                byte[] buffer = (byte[])converter.ConvertTo(image, typeof(byte[]));

                context.Response.ContentType = "image/jpg";
                context.Response.BinaryWrite(buffer);
                context.Response.Flush();
                image.Dispose();
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