using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeltaForce.OpenAI.TempWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpPost("ImageAnalyser")]
        public async Task<IActionResult> ImageAnalyser([FromForm] IFormFile file)
        {
            string uploads = @"C:\Uploads";

            if(file.Length > 0)
            {
                string filePath = Path.Combine(uploads, file.FileName);
                using Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await file.CopyToAsync(fileStream);
            }

            return Ok();
        }
    }
}
