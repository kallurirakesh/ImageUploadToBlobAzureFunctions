using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure_Functions.Utilities;

namespace Azure_Functions
{
    public static class TextExtactFromImage
    {

        [FunctionName("TextExtactFromImage")]
        public static async Task<IActionResult> ExtactImageData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var imageFile = req.Form.Files["image"];
            var imageId = Guid.NewGuid();
            string fileExtension = Path.GetExtension(imageFile.FileName);
            string azFileName = $"{imageId}{fileExtension}";
            string path = $"{Environment.GetEnvironmentVariable("FileStorageUri")}/{Environment.GetEnvironmentVariable("ImageContainer")}/{azFileName}";
            string fileNameWithExt = $"{imageId}{fileExtension}";
            CloudBlockBlob cloudBlockBlob = await GetBlobReference(Environment.GetEnvironmentVariable("ImageContainer"), fileNameWithExt, typeof(CloudBlockBlob));
            await cloudBlockBlob.UploadFromStreamAsync(imageFile.OpenReadStream());

            return new OkObjectResult(true);
        }
        private static async Task<dynamic> GetBlobReference(string containerName, string path, dynamic returnType)
        {
            var blobClient = Singletons.CloudBlobClient;
            var cloudContainer = blobClient.GetContainerReference(containerName);
            await cloudContainer.CreateIfNotExistsAsync();
            return cloudContainer.GetBlockBlobReference(path);   
        }

    }
}
