using System;
using System.Linq;
using GetWebSitesToJPG.Models;
using ImageService.Models;

namespace GetWebSitesToJPG.Logic
{
    /// <summary>
    /// Actions to Execute into DB
    /// </summary>
    public class ImageDBActions : IDisposable
    {
        private ContextDB _db = new ContextDB();

        /// <summary>
        /// Include an item in ImageCropData table
        /// </summary>
        /// <param name="imgData">image data for crop to save</param>
        public void AddDataToDB(ImageCropData imgData)
        {
            var imgItem = _db.ImageCropData.SingleOrDefault(
                c => c.ImageID == imgData.ImageID);
            if (imgItem == null)
            {
                imgItem = imgData;
                _db.ImageCropData.Add(imgItem);
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }
    }
}