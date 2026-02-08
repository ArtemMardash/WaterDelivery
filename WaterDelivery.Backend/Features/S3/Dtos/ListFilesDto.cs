namespace WaterDelivery.Backend.Features.S3.Dtos;

public class ListFilesDto
{
    public string BucketName { get; set; }

    public string Prefix { get; set; } = "";
}