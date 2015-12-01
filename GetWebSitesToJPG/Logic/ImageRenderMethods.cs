using GetWebSitesToJPG.Models;
using iComMkt.Generic.Logic;
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
        /// <returns>Web page image cropped</returns>
        public static Bitmap GetWebsiteImage(ImageCropData imgData)
        {
            var width = imgData.Width;
            var height = imgData.Height;
            var y = imgData.Y;
            var x = imgData.X;

            Bitmap img = ImageUtil.GetWebSiteScreenCapture(imgData.Url, width, height, (int)y);//, 1024, 768);

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