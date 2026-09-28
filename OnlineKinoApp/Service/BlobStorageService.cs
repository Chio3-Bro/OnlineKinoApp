using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace OnlineKinoApp.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration["AzureStorage:ConnectionString"]!;

            string containerName =
                configuration["AzureStorage:ContainerName"]!;

            _containerClient =
                new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            {
                string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                BlobClient blobClient = _containerClient.GetBlobClient(blobName);

                using Stream stream = file.OpenReadStream();

                await blobClient.UploadAsync(stream, overwrite: true);

                return blobName;
            }
        }

        public string GetSasUrl(string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            BlobSasBuilder sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

            return sasUri.ToString();
        }
    }
}