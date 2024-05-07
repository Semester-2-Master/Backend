using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ImageController()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        [HttpGet]
        public IActionResult Image()
        {
            return Ok(new { Endpoint = "Image" });
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string connectionString = _configuration["AzureStorage:ConnectionString"];
            string containerName = _configuration["AzureStorage:ContainerName"];

            try
            {
                // Create BlobServiceClient using the configuration-based connection string
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Get a reference to the Blob Container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique blob name
                string blobName = file.FileName;

                // Get a reference to the Blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload the file to Azure Blob Storage
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Optionally, you can return the URL to the uploaded blob
                string blobUrl = blobClient.Uri.AbsoluteUri;

                return Ok(new { BlobUrl = blobUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to upload file: {ex.Message}");
            }
        }
    }

}
