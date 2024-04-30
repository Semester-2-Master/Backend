using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
   
    private readonly ILogger<ImageController> _logger;

    public ImageController(ILogger<ImageController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Upload")]
    public async Task<IActionResult> Get(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");
        
        string FilePath = "/Users/botond/Downloads/Pollen";

        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);

        // file.FileName
        
        var filePath = Path.Combine(FilePath, file.FileName);

        await using (FileStream fs = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(fs);
        }
        
        return Ok();
    }
}