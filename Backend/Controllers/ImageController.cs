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
                .SetBasePath(Directory.GetCurrentDirectory());

            _configuration = builder.Build();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string connectionString = Environment.GetEnvironmentVariable("AZURESTORAGE_CONNECTION_STRING");
            string containerName = Environment.GetEnvironmentVariable("AZURESTORAGE_CONTAINER_NAME");

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
                return StatusCode(500, $"Failed to upload file: {ex.Message} Connect{connectionString}");
            }
        }

    }

}
