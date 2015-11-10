using iComMkt.Generic.Logic;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI;
using System.Windows.Forms;

namespace GetWebSitesToJPG
{
    public partial class _Default : Page
    {
        Uri uri = null;
        #region Handlers

        /// <summary>
        /// Crop The image from determinated point, width and height.
        /// </summary>
        [WebMethod]
        public static string CropImage(float x, float y, float width, float height, float scaleX, float scaleY, string filename)
        {
            Rectangle rect = new Rectangle
            {
                Width = (int)width,
                Height = (int)height
            };
            var page = new _Default();
            var bitmap = page.CreateBitmap(filename);
            var imgCropped = cropAtRect(bitmap, rect, x, y);

            string path = page.Server.MapPath("Content/Images/Screenshots");
            filename = Regex.Replace(filename, @"\.(jpg)", string.Empty, RegexOptions.IgnoreCase);
            string newFilename = filename + "_cropped.jpg";
            path = path + "\\" + newFilename;

            imgCropped.Save(path);
            string hello = "Hola Mundo!";

            bitmap.Dispose();

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
            uri = new Uri(url);
            string domain = uri.Host;
            //domain = Regex.Replace(domain, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);
            domain = "http://" + domain;
            string webpage = GetHTML(url);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(webpage);
            var head = doc.DocumentNode.SelectSingleNode("//head");
            var baseTag = HtmlAgilityPack.HtmlNode.CreateNode("<base href=\""+ domain +"\">");
            head.PrependChild(baseTag);

            doc = AssignAsbsoluteUri(doc, "//img", "src");
            doc = AssignAsbsoluteUri(doc, "//link", "href");

            string path = Server.MapPath("Content/Images/Screenshots");
            path = path + "\\result.html";
            doc.Save(path);//webpageurlNext
           iframeLoader.Visible = true;
            //iresult.InnerHtml = doc.DocumentNode.InnerHtml;

            /*foreach (Match m in Regex.Matches(webpage, "<head.*>(.*)</head>"))
            {
                iresult.InnerText = m.Value;
            }*/
            /*
            var date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            var filename = domain + "_" + date + ".jpg";

            Bitmap img = ImageUtil.GetWebSiteScreenCapture(url);//, 1024, 768);
            string path = Server.MapPath("Content/Images/Screenshots");
            path = path + "\\" + filename;

            try
            {
                img.Save(path);
            }
            catch(Exception ex)
            {
                lblMsg.Text = "The image can not be loaded, please try again in a few moments.";
                img.Dispose();
            }
            imgPreview.Src = "Content/Images/Screenshots/" + filename;
            img.Dispose();*/
            }

        #endregion
        #region Methods
        private HtmlAgilityPack.HtmlDocument AssignAsbsoluteUri(HtmlAgilityPack.HtmlDocument doc, string nodeKind, string attribute)
        {
            var nodes = doc.DocumentNode.SelectNodes(nodeKind);
            if(nodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode img in nodes)
                {
                    HtmlAgilityPack.HtmlAttribute att = img.Attributes[attribute];
                    if (att == null) continue;
                    string imgUri = att.Value;

                    Uri urlNext = new Uri(imgUri, UriKind.RelativeOrAbsolute);

                    if (!urlNext.IsAbsoluteUri)
                    {
                        //var domainUri = new Uri(domain);
                        urlNext = new Uri(uri, urlNext);
                        img.Attributes[attribute].Value = urlNext.ToString();
                    }
                }
            }

            return doc;
        }

        /// <summary>
        /// Recreate a bitmap object from the image Website
        /// </summary>
        /// <returns>Image Website</returns>
        public Bitmap CreateBitmap(string filename)
        {
            string serverPath = Server.MapPath("/Content/Images/Screenshots/");
            serverPath = serverPath + filename;
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

        public string GetHTML(string url)
        {
            string webPage = String.Empty;
            using (var client = new WebClient())
            {
                webPage = client.DownloadString(url);
            }
            return webPage;
        }

        #endregion

    }
}