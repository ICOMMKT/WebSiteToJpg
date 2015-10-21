using iComMkt.Generic.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI;

namespace GetWebSitesToJPG
{
    public partial class _Default : Page
    {
        //protected string ImgUrl { get; set; }

        #region Handlers

        //TODO: This method is no returning anything to the Website - BUG
        /// <summary>
        /// Crop The image from determinated point, width and height.
        /// </summary>
        [WebMethod]
        public static string CropImage(float x, float y, float width, float height, float rotate, float scaleX, float scaleY)
        {
            var newImg = x;
            Rectangle rect = new Rectangle
            {
                Width = (int)width,
                Height = (int)height
            };
            var page = new _Default();
            var bitmap = page.CreateBitmap();
            var imgCropped = cropAtRect(bitmap, rect, x, y);
            string path = page.Server.MapPath("Content/Images/Screenshots");
            path = path + "\\file2.jpg";
            imgCropped.Save(path);
            //"Content/Images/Screenshots/file1.jpg"
            string hello = "Hola Mundo!";
            return JsonConvert.SerializeObject(hello);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Generates Image from the specified Website
        /// </summary>
        protected void Preview_Gen_Click(object sender, EventArgs e)
        {
            var url = txtUrl.Text;
            Bitmap img = ImageUtil.GetWebSiteScreenCapture(url, 1024, 768);
            string path = Server.MapPath("Content/Images/Screenshots");
            path = path + "\\file1.jpg";
            img.Save(path);

            imgPreview.Src = "Content/Images/Screenshots/file1.jpg";
            //ImgUrl = "Content/Images/Screenshots/file1.jpg";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recreate a bitmap object from the image Website
        /// </summary>
        /// <returns>Image Website</returns>
        public Bitmap CreateBitmap()
        {
            string serverPath = Server.MapPath("/Content/Images/Screenshots/");
            serverPath = serverPath + "file1.jpg";
            Bitmap image = (Bitmap)Image.FromFile(serverPath, true);
            return image;
        }

        /// <summary>
        /// Crop Image
        /// </summary>
        /// <returns>Image Cropped</returns>
        public static Bitmap cropAtRect(Bitmap b, Rectangle r, float x, float y)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(b, -x, -y);
            return nb;
        }

        [Obsolete]
        private List<string> GetImagesInHTMLString(string htmlString)
        {
            List<string> images = new List<string>();
            string pattern = @"<(img)\b[^>]*>";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            for (int i = 0, l = matches.Count; i < l; i++)
            {
                images.Add(matches[i].Value);
            }

            return images;
        }

        [Obsolete]
        string ReadTextFromUrl(string url)
        {
            // WebClient is still convenient
            // Assume UTF8, but detect BOM - could also honor response charset I suppose
            using (var client = new WebClient())
            using (var stream = client.OpenRead(url))
            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            {
                return textReader.ReadToEnd();
            }
        }

        #endregion

    }
}