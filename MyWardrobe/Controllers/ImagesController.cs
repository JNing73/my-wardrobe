using Microsoft.AspNetCore.Mvc;

namespace MyWardrobe.Controllers
{
    public class ImagesController : Controller
    {
        public async Task<IActionResult> GetImage(string filename)
        {
            // Build out the filepath
            var folderPathBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Images"); // As defined in ClothingItems Controller
            var filePath = Path.Combine(folderPathBase, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Determine the content type based on file extension
            var fileExtension = Path.GetExtension(filename).ToLower();
            string contentType;

            switch (fileExtension)
            {
                case ".png":
                    contentType = "image/png";
                    break;
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                default:
                    contentType = "application/octet-stream"; // Default for unknown file types
                    break;
            }

            // Return the image file
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, contentType);
        }
    }
}

