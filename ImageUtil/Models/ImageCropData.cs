using System.ComponentModel.DataAnnotations;

namespace ImageService.Models
{
    public class ImageCropData
    {
        [Key]
        public string ImageID { get; set; }
        public string UserId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public int ContainerWidth { get; set; }
    }
}