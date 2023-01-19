using Azure.Storage.Blobs;
using BookStore.Domain.Models;
using System;

namespace BookStore.AzureService;
public class AzureBobImageStorage : IImageStorage
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly DomainOptions _domainOptions;

    public AzureBobImageStorage(
        BlobServiceClient blobServiceClient,
        DomainOptions domainOptions)
    {
        _blobServiceClient = blobServiceClient;
        _domainOptions = domainOptions;
    }

    public async Task SaveBookImage(BookImage bookImage, byte[] fileContent)
    {
        string containerName = _domainOptions.AzureBlob.ImageBaseContainer;

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync();

        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(bookImage.FilePath);

        using (var ms = new MemoryStream())
        {
            await ms.WriteAsync(fileContent);
            await blobClient.UploadAsync(ms);
        }
    }
}

