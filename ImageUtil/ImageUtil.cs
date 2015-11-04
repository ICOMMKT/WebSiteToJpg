using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iComMkt.Generic.Logic
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class ImageUtil
    {
        public static Bitmap GetWebSiteScreenCapture(string pUrl, Nullable<Int32> pSiteWidth, Nullable<Int32> pSiteHeight)
        {
            Bitmap thumb = null;
            string url = null;
            int bw = 800;
            //valor por defeito
            int bh = 600;

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

            if (pSiteWidth >= 0)
            {
                bw = (Int32)pSiteWidth;
            }

            if (pSiteHeight >= 0)
            {
                bh = (Int32)pSiteHeight;
            }

            // return context bitmap
            thumb = GetWebSiteThumbnail(url, bw, bh);

            return thumb;
        }

        public static Bitmap GetWebSiteScreenCapture(string pUrl)
        {
            return GetWebSiteScreenCapture(pUrl, null, null);
        }

        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight)
        {
            WebsiteThumbnailImage thumbnailGenerator = new WebsiteThumbnailImage(Url, BrowserWidth, BrowserHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage();
        }

        private class WebsiteThumbnailImage
        {
            public Boolean bLoaded = false;
            public bool isLazyMan = default(bool);
            private int min_frames = int.MaxValue;
            private int max_height = 0;
            private int iframe_counter = 0;
            /*public DateTime dt = DateTime.Now;*/

            public WebsiteThumbnailImage(string Url, int BrowserWidth, int BrowserHeight)
            {
                this.m_Url = Url;
                this.m_BrowserWidth = BrowserWidth;
                this.m_BrowserHeight = BrowserHeight;
            }

            private string m_Url = null;
            public string Url
            {
                get { return m_Url; }
                set { m_Url = value; }
            }

            private Bitmap m_Bitmap = null;
            public Bitmap ThumbnailImage
            {
                get { return m_Bitmap; }
            }

            private int m_BrowserWidth;
            public int BrowserWidth
            {
                get { return m_BrowserWidth; }
                set { m_BrowserWidth = value; }
            }

            private int m_BrowserHeight;
            public int BrowserHeight
            {
                get { return m_BrowserHeight; }
                set { m_BrowserHeight = value; }
            }

            private int m_ThumbnailWidth;
            public int ThumbnailWidth
            {
                get { return m_ThumbnailWidth; }
                set { m_ThumbnailWidth = value; }
            }

            private int m_ThumbnailHeight;
            public int ThumbnailHeight
            {
                get { return m_ThumbnailHeight; }
                set { m_ThumbnailHeight = value; }
            }
            private bool outOfmemory = false;
            bool firstTime = true;

            public Bitmap GenerateWebSiteThumbnailImage()
            {
                Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
                m_thread.SetApartmentState(ApartmentState.STA);
                m_thread.Start();
                m_thread.Join();
                return m_Bitmap;
            }

            private void _GenerateWebSiteThumbnailImage()
            {
                WebBrowser m_WebBrowser = new WebBrowser();
                m_Bitmap = new Bitmap(100, 100);
                m_WebBrowser.ScrollBarsEnabled = false;
                m_WebBrowser.ScriptErrorsSuppressed = true;
                m_WebBrowser.Navigating += new WebBrowserNavigatingEventHandler(WebBrowser_Navigating);
                //m_WebBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(WebBrowser_ProgressChanged);*/
                m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                //m_WebBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(WebBrowser_ProgressChanged);
                m_WebBrowser.Navigate(m_Url);
                waitPolice();
            }

            public void waitPolice()
            {
                while (isLazyMan) Application.DoEvents();
            }

            private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
            {
                if (!bLoaded)
                {
                    isLazyMan = true;
                }
            }

            void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                WebBrowser m_WebBrowser = (WebBrowser)sender;

                var framesCount = m_WebBrowser.Document.Window.Frames.Count;
                var height = m_WebBrowser.Document.Body.ScrollRectangle.Height;
                if (height > max_height)
                {
                    max_height = height;
                }

                if (framesCount < min_frames)
                {
                    min_frames = framesCount;
                }
                bool inAjax = default(bool);
                string url = e.Url.ToString();

                if (!(url.StartsWith("http://") || url.StartsWith("https://")))
                {
                    inAjax = true;
                }
                if (!inAjax)
                {
                    if (e.Url.AbsolutePath != m_WebBrowser.Url.AbsolutePath)
                    {
                        iframe_counter++;
                    }
                }
                if (min_frames <= iframe_counter && (!inAjax))
                {
                    DocumentCompletedFully(m_WebBrowser, e);
                    isLazyMan = false;
                    bLoaded = true;
                }
            }

            private void DocumentCompletedFully(WebBrowser sender, WebBrowserDocumentCompletedEventArgs e)
            {
                //Thread.Sleep(5000);
                WebBrowser m_WebBrowser = (WebBrowser)sender;

                int scrollWidth = 0;
                int scrollHeight = 0;

                scrollHeight = m_WebBrowser.Document.Body.ScrollRectangle.Height;
                //if (scrollHeight > 10000)
                //{
                //    scrollHeight = 9999;
                //}
                scrollWidth = m_WebBrowser.Document.Body.ScrollRectangle.Width;
                if (scrollWidth < 768)
                {
                    scrollWidth = 1024;
                }
                m_WebBrowser.Size = new Size(scrollWidth, scrollHeight);

                //m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                m_WebBrowser.ScrollBarsEnabled = false;
                m_WebBrowser.ScriptErrorsSuppressed = true;
                
                //firstTime = false;
                //m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                m_WebBrowser.BringToFront();
                try
                {
                    m_Bitmap.Dispose();
                    m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                    m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
                }
                catch (Exception ex)
                {
                    var exType = ex.GetType();
                    outOfmemory = true;
                }
                bLoaded = true;


            }

            [Obsolete]
            private void WebBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
            {
                int max = (int)Math.Max(e.MaximumProgress, e.CurrentProgress);
                int min = (int)Math.Min(e.MaximumProgress, e.CurrentProgress);
                if (min.Equals(max))
                {
                    WebBrowser m_WebBrowser = (WebBrowser)sender;
                    //Run your code here when page is actually 100% complete
                    int scrollWidth = 0;
                    int scrollHeight = 0;

                    scrollHeight = m_WebBrowser.Document.Body.ScrollRectangle.Height;
                    scrollWidth = m_WebBrowser.Document.Body.ScrollRectangle.Width;
                    m_WebBrowser.Size = new Size(scrollWidth, scrollHeight);

                    //m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                    m_WebBrowser.ScrollBarsEnabled = false;
                    m_WebBrowser.ScriptErrorsSuppressed = true;
                    m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                    m_WebBrowser.BringToFront();
                    m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
                    bLoaded = true;
                }
            }

        }

        // Resusable flag
        public bool IsReusable
        {
            get { return false; }
        }

        public static Image GetThumbnail(string pUrl, Nullable<Int32> pSiteWidth, Nullable<Int32> pSiteHeight, Nullable<Int32> pThumbWidth, Nullable<Int32> pThumbHeight)
        {

            //valor por defeito
            int tw = 0;
            // sem thumbnail 
            int th = 0;

            if (pThumbWidth >= 0)
            {
                tw = (Int32)pThumbWidth;
            }

            if (pThumbHeight >= 0)
            {
                th = (Int32)pThumbHeight;
            }

            Bitmap mBitmap = GetWebSiteScreenCapture(pUrl, pSiteWidth, pSiteHeight);

            tw = (tw > mBitmap.Width) ? mBitmap.Width : tw;

            th = tw * mBitmap.Height / mBitmap.Width;

            return mBitmap.GetThumbnailImage(tw, th, null, IntPtr.Zero);
        }
    }
}
