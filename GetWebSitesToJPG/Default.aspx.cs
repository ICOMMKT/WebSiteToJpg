using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using GetWebSitesToJPG.Logic;
using GetWebSitesToJPG.Models;

namespace GetWebSitesToJPG
{
    public partial class _Default : Page
    {
        Uri uri = null;
        
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
            var imageId = Guid.NewGuid().ToString();
            imageId = Regex.Replace(imageId, "-", string.Empty, RegexOptions.IgnoreCase);
            var currentUrl = HttpContext.Current.Request.Url;
            string urlGenerated = currentUrl.Scheme + "://"+ currentUrl.Authority + "/" + imageId;

            using (ImageDBActions newImgToDB = new ImageDBActions())
            {
                var imgData = new ImageCropData {
                    ImageID = imageId,
                    X = x,
                    Y = y,
                    Url = url,
                    Width = (int)width,
                    Height = (int)height,
                    ContainerWidth = (int)containerWidth
                };
                newImgToDB.AddDataToDB(imgData);
            }

            return urlGenerated;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Generates Image from the specified Website
        /// </summary>
        protected void Preview_Gen_Click(object sender, EventArgs e)
        { 
            string url = txtUrl.Text;
            uri = CreateUri(url);
            string domain = uri.Host;
            domain = Regex.Replace(domain, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);

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

            //StatusText.Text = "Wait until this message disappear";
            //ContentLoaded.Visible = true;
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
        /// Navigate and retrive Web Page
        /// </summary>
        /// <param name="url">Webpage URL</param>
        /// <returns>HTML string page</returns>
        public string GetHTML(string url)
        {
            string webPage = String.Empty;
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; AS; rv:11.0) like Gecko");
                var encoding = client.Encoding;
                //client.Encoding = System.Text.Encoding.UTF8;
                webPage = client.DownloadString(url);
                //webPage = client.DownloadString(url);
            }
            return webPage;
        }


        private Uri CreateUri(string pUrl)
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
            return new Uri(url);
        }
        #endregion
    }
}