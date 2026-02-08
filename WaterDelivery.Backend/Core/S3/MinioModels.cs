namespace WaterDelivery.Backend.Core.S3;

public class FileUploadModel
{
    public IFormFile File { get; set; }
    
    public string? BucketName { get; set; }
    
    public Dictionary<string, string> Tags { get; set; }
    
    public Dictionary<string, string> MetaData { get; set; }
}

public class FileDownloadModel
{
    public string FileName { get; set; }
    
    public string BucketName { get; set; }
    
    public string ContentType { get; set; }
    
    public Stream Stream { get; set; }
    
    public long Size { get; set; }
    
    public DateTime LastModified { get; set; }
    
    public Dictionary<string, string> Tags { get; set; }
    
    public Dictionary<string, string> MetaData { get; set; }
}

public class BucketInfo
{
    public string Name { get; set; }
    
    public DateTime CreationDate { get; set; }
}