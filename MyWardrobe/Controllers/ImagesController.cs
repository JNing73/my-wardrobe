using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MyWardrobe.Controllers
{
    public class ImagesController : Controller
    {
        private readonly string _folderPathBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Images"); // As defined in ClothingItems Controller
        private readonly string? _fileName;

        // Constructor has been setup so that other controllers can correctly call methods from it by passing in a filename
        // Specifically the DeleteImageAsset method
        // Ideally would've had two constructors the default constructor with no parameters for routing and the overloaded
        // constructor for the non-route based methods
        public ImagesController (string? filename = null)
        {
            _fileName = filename;
        }

        public async Task<IActionResult> GetImage(int id, string filename)
        {
            // Build out the filepath
            var filePath = Path.Combine(_folderPathBase, Convert.ToString(id), filename);

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

        public async Task<IActionResult> DeleteImageAsset()
        {

            if (_fileName == null) {
                return BadRequest("This clothing item does not have an imagefile associated with it" +
                    "\nOr you have have tried to call this method through an invalid route");
            }

            var filePath = Path.Combine(_folderPathBase, _fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    // Asynchronous deletion of the stored file
                    await Task.Run(() => System.IO.File.Delete(filePath));
                }
                catch
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
            return Ok();
        }
    }
}

