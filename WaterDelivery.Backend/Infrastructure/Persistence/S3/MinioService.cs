using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Tags;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Core.S3;

namespace WaterDelivery.Backend.Infrastructure.Persistence.S3;

public class MinioService : IMinioService, IDisposable
{
    private readonly string _defaultBucket;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioService> _logger;

    public MinioService(IConfiguration configuration, ILogger<MinioService> logger)
    {
        var accessKey = configuration["Minio:AccessKey"] ?? "admin";
        var secretKey = configuration["Minio:SecretKey"] ?? "admin123";
        var endpoint = configuration["Minio:Endpoint"] ?? "localhost:9000";
        _defaultBucket = configuration["Minio:DefaultBucket"] ?? "waterbucket";
        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build();
        _logger = logger;
    }

    public Task<bool> BucketExistsAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            var arg = new BucketExistsArgs().WithBucket(bucketName);
            return _minioClient.BucketExistsAsync(arg, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to check if bucket exists with bucketName {bucketName}",
                bucketName);
            throw;
        }
    }

    public async Task CreateBucketAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            if (!await BucketExistsAsync(bucketName, cancellationToken))
            {
                var args = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(args, cancellationToken);
                _logger.LogInformation("Bucket with name {bucketName} was created ", bucketName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bucket creation failed");
            throw;
        }
    }

    public async Task<List<BucketInfo>> GetAllBucketsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var bucketsAsync = await _minioClient.ListBucketsAsync(cancellationToken);
            var result = bucketsAsync.Buckets.Select(b => new BucketInfo
            {
                Name = b.Name,
                CreationDate = DateTime.Parse(b.CreationDate)
            }).ToList();
            _logger.LogInformation("All Buckets successfully received ");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get All Buckets failed");
            throw;
        }
    }

    public async Task RemoveBucketAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            if (!await BucketExistsAsync(bucketName, cancellationToken))
            {
                throw new InvalidOperationException("There is no bucket with such a name");
            }

            var removeBucketArg = new RemoveBucketArgs().WithBucket(bucketName);
            await _minioClient.RemoveBucketAsync(removeBucketArg, cancellationToken);
            _logger.LogInformation("Bucket with a name {bucketName} was successfully removed", bucketName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Bucket remove failed");
            throw;
        }
    }

    public async Task<string> UploadFileAsync(FileUploadModel model, CancellationToken cancellationToken)
    {
        try
        {
            var bucketName = model.BucketName ?? _defaultBucket;
            await CreateBucketAsync(bucketName, cancellationToken);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var contentType = model.File.ContentType;

            using var stream = model.File.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(model.File.Length)
                .WithContentType(contentType);

            if (model.Tags != null && model.Tags.Count > 0)
            {
                var tagging = new Tagging(model.Tags, true);
                putObjectArgs = putObjectArgs.WithTagging(tagging);
            }

            if (model.MetaData != null && model.MetaData.Count > 0)
            {
                putObjectArgs = putObjectArgs.WithHeaders(model.MetaData);
            }

            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            _logger.LogInformation("File with name {fileName} was loaded", fileName);
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File upload failed");
            throw;
        }
    }

    public async Task<FileDownloadModel> DownloadFileAsync(string bucketName, string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!await FileExistsAsync(bucketName, fileName, cancellationToken))
            {
                throw new InvalidOperationException("There is no file with such a name");
            }

            var stream = new MemoryStream();
            var objArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(s=>s.CopyToAsync(stream, cancellationToken));
            var stats = await _minioClient.GetObjectAsync(objArgs, cancellationToken);
            stream.Seek(0, 0);
            var result = new FileDownloadModel
            {
                FileName = fileName,
                BucketName = bucketName,
                ContentType = stats.ContentType,
                Size = stats.Size,
                LastModified = stats.LastModified,
                Tags = await GetObjectTagsAsync(bucketName, fileName, cancellationToken),
                MetaData = stats.MetaData,
                Stream = stream
            };
            
            _logger.LogInformation("File with name {fileName} successfully downloaded", fileName);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Download file failed");
            throw;
        }
    }

    public async Task<List<FileDownloadModel>> ListFilesAsync(string bucketName, CancellationToken cancellationToken,
        string prefix = "")
    {
        try
        {
            if (!await BucketExistsAsync(bucketName, cancellationToken))
            {
                throw new InvalidOperationException("There is no bucket with such bucketName");
            }

            var listObj = new ListObjectsArgs().WithBucket(bucketName).WithPrefix(prefix);
            var stats = _minioClient.ListObjectsEnumAsync(listObj, cancellationToken);

            var result = new List<FileDownloadModel>();
            await foreach (var item in stats)
            {
                var model = new FileDownloadModel
                {
                    FileName = item.Key,
                    BucketName = bucketName,
                    ContentType = item.ContentType,
                    Size = (long)item.Size,
                    LastModified = DateTime.Parse(item.LastModified),
                    Tags = await GetObjectTagsAsync(bucketName, item.Key, cancellationToken),
                    MetaData = item.UserMetadata.Any()? item.UserMetadata.ToDictionary(): new Dictionary<string, string>()
                };

                result.Add(model);
            }

            _logger.LogInformation("Files with prefix {prefix} from bucket {bucketName} are received", prefix,
                bucketName);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "List Filed failed");
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string bucketName, string fileName, CancellationToken cancellationToken)
    {
        try
        {
            if (!await BucketExistsAsync(bucketName, cancellationToken))
            {
                throw new InvalidOperationException("There is no bucket with such a name");
            }

            var objStats = new StatObjectArgs().WithBucket(bucketName).WithObject(fileName);
            var result = await _minioClient.StatObjectAsync(objStats, cancellationToken);
            if (result == null || string.IsNullOrEmpty(result.ETag))
            {
                _logger.LogInformation("Object with name {fileName} doesn't exists", fileName);
                return false;
            }

            _logger.LogInformation("Object with name {fileName} exists", fileName);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "File exists check failed");
            throw;
        }
    }

    public async Task RemoveFileAsync(string bucketName, string fileName, CancellationToken cancellationToken)
    {
        try
        {
            if (!await FileExistsAsync(bucketName, fileName, cancellationToken))
            {
                throw new InvalidOperationException($"There is no file with name {fileName}");
            }

            var remArgs = new RemoveObjectArgs().WithObject(fileName).WithBucket(bucketName);
            await _minioClient.RemoveObjectAsync(remArgs, cancellationToken);
            _logger.LogInformation("Object with a name {fileName} was removedSuccessfully", fileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Remove file failed");
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsStreamAsync(string bucketName, string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!await FileExistsAsync(bucketName, fileName, cancellationToken))
            {
                throw new InvalidOperationException("There is no file with such a name");
            }

            var stream = new MemoryStream();
            var objArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(s => s.CopyToAsync(stream, cancellationToken));

            await _minioClient.GetObjectAsync(objArgs, cancellationToken);

            stream.Position = 0;
            _logger.LogInformation("File with a name {fileName} was downloaded as a stream", fileName);
            return stream;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "File download as a stream failed");
            throw;
        }
    }

    private async Task<Dictionary<string, string>> GetObjectTagsAsync(string bucketName, string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            var args = new GetObjectTagsArgs().WithObject(fileName).WithBucket(bucketName);

            var result = await _minioClient.GetObjectTagsAsync(args, cancellationToken);
            return result.Tags !=null ? result.Tags.ToDictionary() : new Dictionary<string, string>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}