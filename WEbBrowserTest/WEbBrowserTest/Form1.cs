using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEbBrowserTest
{
    public partial class Form1 : Form
    {
        public Boolean bLoaded = false;

        public Form1()
        {
            InitializeComponent();

        }
        private Bitmap m_Bitmap = null;
        public Bitmap ThumbnailImage
        {
            get { return m_Bitmap; }
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser m_WebBrowser = (WebBrowser)sender;
            int scrollWidth = 0;
            int scrollHeight = 0;

            scrollHeight = m_WebBrowser.Document.Body.ScrollRectangle.Height;
            scrollWidth = m_WebBrowser.Document.Body.ScrollRectangle.Width;
            scrollHeight = 900;
            scrollWidth = 1024;
            m_WebBrowser.Size = new Size(scrollWidth, scrollHeight);

            //m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
            m_WebBrowser.ScrollBarsEnabled = false;
            m_WebBrowser.ScriptErrorsSuppressed = true;
            m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
            m_WebBrowser.BringToFront();
            m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
            bLoaded = true;
            var path = Path.GetFullPath(@"ScreenShots");
            path = path + @"\image1.jpg";
            m_Bitmap.Save(path);
        }
    }
}
