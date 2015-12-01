using System.Data.Entity;

namespace GetWebSitesToJPG.Models
{
    public class ContextDB : DbContext
    {
        public ContextDB() : base("name=ImageGenDB")
        {
            
        }

        public DbSet<ImageCropData> ImageCropData { get; set; }
    }
}