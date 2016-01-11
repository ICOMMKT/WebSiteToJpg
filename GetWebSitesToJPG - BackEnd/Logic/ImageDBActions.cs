using System;
using System.Linq;
using ImageService.Models;
using GetWebSitesToJPG___BackEnd.Models;

namespace GetWebSitesToJPG___BackEnd.Logic
{
    /// <summary>
    /// Actions to Execute into DB
    /// </summary>
    public class ImageDBActions : IDisposable
    {
        private ContextDB _db = new ContextDB();

        /// <summary>
        /// Get image data
        /// </summary>
        /// <param name="imageId">image Id</param>
        /// <returns>Image data for crop</returns>
        public ImageCropData GetImageData(string imageId)
        {
            IQueryable<ImageCropData> query = _db.ImageCropData;
            query = query.Where(p => p.ImageID == imageId);

            return query.SingleOrDefault();
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