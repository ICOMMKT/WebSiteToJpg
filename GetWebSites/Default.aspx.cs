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

namespace WebPageToJPG
{
    public partial class _Default : Page
    {
        protected string ImgUrl { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //using (var client = new WebClient())
            //{
              //  string webPage = client.DownloadString("http://movistar.com.ve");
                // TODO: do something with the downloaded result from the remote
                // web site
               //var webPage = ReadTextFromUrl("http://www.mercantilbanco.com/mercprod/index.html");
            //var listImages = GetImagesInHTMLString(webPage);
                //iresult.InnerHtml = webPage;
            //}*/
        }

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

        protected void Preview_Gen_Click(object sender, EventArgs e)
        {
            /*var url = txtUrl.Text;
            Bitmap img = ImageUtil.GetWebSiteScreenCapture(url, 1024, 768);
            string path = Server.MapPath("Content/Images/Screenshots");
            path = path + "\\file1.jpg";
            img.Save(path);*/
 
            imgPreview.Src = "Content/Images/Screenshots/file1.jpg";
            //ImgUrl = "Content/Images/Screenshots/file1.jpg";
        }

        [WebMethod]
        public static string CropImage(float x, float y, float width, float height, float rotate, float scaleX, float scaleY)
        {
            var newImg = x;
            Rectangle rect = new Rectangle
            {
                Width =  (int) width,
                Height = (int) height
            };
            var page = new _Default();
            var bitmap = page.CreateBitmap();
            var imgCropped = cropAtRect(bitmap, rect);
            //"Content/Images/Screenshots/file1.jpg"
            string hello = "Hola Mundo!";
            return JsonConvert.SerializeObject(hello);
        }

        public Bitmap CreateBitmap()
        {
            string serverPath = Server.MapPath("/Content/Images/Screenshots/");
            serverPath = serverPath + "file1.jpg";
            Bitmap image = (Bitmap)Image.FromFile(serverPath, true);
            return image;
        }

        public static Bitmap cropAtRect(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(b, -r.X, -r.Y);
            return nb;
        }
    }
}
