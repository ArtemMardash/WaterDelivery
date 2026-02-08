using WaterDelivery.Backend.Core.S3;

namespace WaterDelivery.Backend.Core.Interfaces;

public interface IMinioService
{
    public Task<bool> BucketExistsAsync(string bucketName, CancellationToken cancellationToken);

    public Task CreateBucketAsync(string bucketName, CancellationToken cancellationToken);

    public Task<List<BucketInfo>> GetAllBucketsAsync(CancellationToken cancellationToken);

    public Task RemoveBucketAsync(string bucketName, CancellationToken cancellationToken);


    public Task<string> UploadFileAsync(FileUploadModel model, CancellationToken cancellationToken);

    public Task<FileDownloadModel> DownloadFileAsync(string bucketName, string fileName,
        CancellationToken cancellationToken);

    public Task<List<FileDownloadModel>> ListFilesAsync(string bucketName, CancellationToken cancellationToken,
        string prefix = "");

    public Task<bool> FileExistsAsync(string bucketName, string fileName, CancellationToken cancellationToken);

    public Task RemoveFileAsync(string bucketName, string fileName, CancellationToken cancellationToken);

    public Task<Stream> DownloadFileAsStreamAsync(string bucketName, string fileName,
        CancellationToken cancellationToken);
}