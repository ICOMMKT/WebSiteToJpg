using System.Data.Entity;
using ImageService.Models;

namespace GetWebSitesToJPG___BackEnd.Models
{
    public class ContextDB : DbContext
    {
        public ContextDB() : base("name=ImageGenDB")
        {
            
        }

        public DbSet<ImageCropData> ImageCropData { get; set; }
    }
}