using iComMkt.Generic.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.Services;
using System.Web.UI;
using System.Windows.Forms;

namespace GetWebSitesToJPG
{
    public partial class _Default : Page
    {
        Uri uri = null;
        string url = "";
        bool iframeVisible = false;

        public bool IframeVisible
        {
            get
            {
                return iframeVisible;
            }
        }

        #region Handlers

        /// <summary>
        /// Crop The image from determinated point, width and height.
        /// </summary>
        [WebMethod]
        public static string CropImage(float x, float y, float width, float height, string url, float containerWidth)// float scaleX, float scaleY, string filename)
        {
            Rectangle rect = new Rectangle
            {
                Width = (int)width,
                Height = (int)height
            };
            var page = new _Default();
            var image = page.GetWebsiteImage(url, (int)containerWidth, (int)height, (int)y);
            if (image.ImageLoaded)
            {
                var imgCropped = cropAtRect(image.Image, rect, x, y);

                string path = page.Server.MapPath("Content/Images/Screenshots");
                var filename = image.Filename;
                string newFilename = filename + ".jpg";
                path = path + "\\" + newFilename;

                imgCropped.Save(path);

                //bitmap.Dispose();
            }
            //var uri = new Uri(url);
            var currentUrl = HttpContext.Current.Request.Url;
            string urlGenerated = currentUrl.Scheme + "://"+ currentUrl.Authority + "/" + image.Filename;

            return urlGenerated;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            /*var id = Guid.NewGuid().GetHashCode();
            RouteValueDictionary parameters = new RouteValueDictionary {
                {"imageID", id.ToString() } };

            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(
              null,
              "ImageMaskedRoute",
              parameters);

            // string url = vpd.VirtualPath;
            redirect.HRef = vpd.VirtualPath;*/
        }

        /// <summary>
        /// Generates Image from the specified Website
        /// </summary>
        protected void Preview_Gen_Click(object sender, EventArgs e)
        {

            url = txtUrl.Text;
            uri = new Uri(url);
            string domain = uri.Host;
            domain = Regex.Replace(domain, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);

            //GetWebsiteImageAsync();

            /**********************************
            * Method WebClient.DownloadString
            *
            ***********************************/
            domain = "http://" + domain;
            string webpage = GetHTML(url);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(webpage);

            var head = doc.DocumentNode.SelectSingleNode("//head");
            var baseTag = HtmlAgilityPack.HtmlNode.CreateNode("<base href=\"" + domain + "\">");
            var fontAwesomeRes = HtmlAgilityPack.HtmlNode.CreateNode("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css\">");
            head.PrependChild(baseTag);
            head.AppendChild(fontAwesomeRes);

            doc = AssignAsbsoluteUri(doc, "//img", "src");
            doc = AssignAsbsoluteUri(doc, "//link", "href");

            string path = Server.MapPath("Content/Images/Screenshots");
            var filepath = path + "\\result.html";
            doc.Save(filepath);//webpageurlNext

            iframeLoader.Visible = true;

            iframeVisible = true;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Assign Absolute Uri by node type and attribute type
        /// </summary>
        /// <param name="doc">HTML Document</param>
        /// <param name="nodeKind">Node type of HTML document</param>
        /// <param name="attribute">Attribute type from a node selected</param>
        /// <returns>HTML Document</returns>
        private HtmlAgilityPack.HtmlDocument AssignAsbsoluteUri(HtmlAgilityPack.HtmlDocument doc, string nodeKind, string attribute)
        {
            var nodes = doc.DocumentNode.SelectNodes(nodeKind);
            if (nodes != null)
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
        [Obsolete]
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
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; AS; rv:11.0) like Gecko");
                webPage = client.DownloadString(url);
            }
            return webPage;
        }

        private ImageReturned GetWebsiteImage(string url, int width, int height, int scrollY)
        {
            bool b = false;
            var image = new ImageReturned();
            //string domain = url;
            uri = new Uri(url);
            string domain = uri.Host;

            domain = Regex.Replace(domain, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);
            //domain = Regex.Replace(domain, @"^(\.)?", string.Empty);
            //var date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            var filename = Guid.NewGuid().ToString();
            filename = Regex.Replace(filename, "-", string.Empty, RegexOptions.IgnoreCase);
            image.Filename = filename;
            filename +=  "_raw.jpg"; ///domain + "_" + date
            

            Bitmap img = ImageUtil.GetWebSiteScreenCapture(url, width, height, scrollY);//, 1024, 768);
            string path = Server.MapPath("Content/Images/Screenshots");
            path = path + "\\" + filename;

            try
            {
                img.Save(path);
                b = true;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "The image can not be loaded, please try again in a few moments.";
                img.Dispose();
            }

            image.Image = (Bitmap)img.Clone();
            image.ImageLoaded = b;
            img.Dispose();

            return image;
        }

        /*private Uri CreateUri(string pUrl)
        {
            string url = null;
            if (pUrl != null)
            {
                if (pUrl.ToString().ToLower().Contains("http://") || pUrl.ToString().ToLower().Contains("https://"))
                {
                    url = pUrl.ToString();
                }
                else
                {
                    url = "http://" + pUrl.ToString();
                }
            }

        }*/
        #endregion
    }
    #region HelperClass
    public class ImageReturned
    {
        bool imageLoaded;
        Bitmap image;
        string filename;

        public bool ImageLoaded
        {
            get
            {
                return imageLoaded;
            }

            set
            {
                imageLoaded = value;
            }
        }

        public Bitmap Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        public string Filename
        {
            get
            {
                return filename;
            }

            set
            {
                filename = value;
            }
        }
    }
    #endregion
}