using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace WatchWeb.Common.Helper
{
    public static class ImageHelper
    {
        public static string ConvertImageToBase64(IFormFile image)
        {
            using (var stream = new MemoryStream())
            {
                image.CopyTo(stream);
                var bytes = stream.ToArray();
                var img = BytesToImage(bytes, out IImageFormat format);
                var imgFormat = format.Name.ToLower();

                return $"data:image/{imgFormat};base64," + Convert.ToBase64String(bytes); ;
            }
        }

        private static Image BytesToImage(byte[] imgBytes, out IImageFormat format)
        {
            Stream outputStream = new MemoryStream();

            using (var image = Image.Load(imgBytes, out format))
            {
                image.Mutate(c => c.Grayscale());
                image.Save(outputStream, format);


                return image;
            }
        }
    }
}
