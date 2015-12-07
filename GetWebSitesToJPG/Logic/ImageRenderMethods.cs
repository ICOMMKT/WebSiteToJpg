using GetWebSitesToJPG.Models;
using iComMkt.Generic.Logic;
using System;
using System.Drawing;

namespace GetWebSitesToJPG.Logic
{
    /// <summary>
    /// Methods to render image cropped
    /// </summary>
    public class ImageRenderMethods
    {
        /// <summary>
        /// Navigate and return image cropped
        /// </summary>
        /// <param name="imgData">image object data</param>
        /// <returns>Web page image cropped.</returns>
        public static Bitmap GetWebsiteImage(ImageCropData imgData, string serverpath)
        {
            var width = imgData.Width;
            var height = imgData.Height;
            var y = imgData.Y;
            var x = imgData.X;

            Bitmap img = ImageUtil.GetWebSiteScreenCapture(imgData.Url, imgData.ContainerWidth, height, (int)y);//, 1024, 768);
            string imgPath = serverpath + "\\img_raw.jpg";

            try
            {
                img.Save(imgPath);
                //b = true;
            }
            catch (Exception ex)
            {
                //lblMsg.Text = "The image can not be loaded, please try again in a few moments.";
                img.Dispose();
            }


            Rectangle rect = new Rectangle
            {
                Width = (int)width,
                Height = (int)height
            };

            img = cropAtRect(img, rect, x, y);


            return img;
        }
        /// <summary>
        /// Crop Image
        /// </summary>
        /// <returns>Image Cropped</returns>
        private static Bitmap cropAtRect(Bitmap b, Rectangle r, float x, float y)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(b, -x, -y);
            return nb;
        }
    }
}