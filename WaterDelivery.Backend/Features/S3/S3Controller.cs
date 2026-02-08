using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Core.S3;
using WaterDelivery.Backend.Features.S3.Dtos;

namespace WaterDelivery.Backend.Features.S3;

public static class S3Controller
{
    public static void MapS3Endpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/s3")
            .WithTags("S3");
        
        group.MapPost("/uploadFile",
                async ([FromForm] FileUploadModel model, [FromServices] IMinioService minioService,
                    CancellationToken cancellationToken) =>
                {
                    if (model.File == null || model.File.Length == 0)
                    {
                        throw new Exception();
                    }

                    var fileName = await minioService.UploadFileAsync(model, cancellationToken);
                    return fileName;
                })
            .DisableAntiforgery()
            .WithOpenApi();

        group.MapPost("/createBucket",
                async ([FromBody] BucketDto dto, [FromServices] IMinioService minioService, CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrEmpty(dto.BucketName))
                    {
                        throw new InvalidOperationException("Bucket name cannot be empty");
                    }

                    await minioService.CreateBucketAsync(dto.BucketName, cancellationToken);
                })
            .DisableAntiforgery()
            .WithOpenApi();

        group.MapDelete("/removeBucket",
                async ([FromBody] BucketDto dto, [FromServices] IMinioService minioService,
                    CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrEmpty(dto.BucketName))
                    {
                        throw new InvalidOperationException("Bucket name cannot be empty");
                    }

                    await minioService.RemoveBucketAsync(dto.BucketName, cancellationToken);
                })
            .DisableAntiforgery()
            .WithOpenApi();

        group.MapGet("/getBuckets", async ([FromServices]IMinioService minioService, CancellationToken cancellationToken) =>
            {
                var result = await minioService.GetAllBucketsAsync(cancellationToken);
                return result;
            })
            .DisableAntiforgery()
            .WithOpenApi();

        group.MapGet("/downloadFile/{bucketName}/{fileName}",
                async (string bucketName, string fileName, [FromServices] IMinioService minioService,
                    CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrEmpty(bucketName) || string.IsNullOrEmpty(fileName))
                    {
                        throw new InvalidOperationException("Bucket name or file name cannot be empty");
                    }

                    var result = await minioService.DownloadFileAsync(bucketName, fileName, cancellationToken);

                    return Results.File(result.Stream, result.ContentType, result.FileName, result.LastModified, enableRangeProcessing: true);
                })
            .DisableAntiforgery()
            .WithOpenApi();

        group.MapGet("/listAllFiles/{bucketName}/{prefix}", async (string bucketName, string? prefix, [FromServices] IMinioService minioService,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrEmpty(bucketName))
                {
                    throw new InvalidOperationException("Bucket name cannot be empty");
                }

                if (string.IsNullOrEmpty(prefix))
                {
                    return await minioService.ListFilesAsync(bucketName, cancellationToken);

                }

                return await minioService.ListFilesAsync(bucketName, cancellationToken);
            })
            .DisableAntiforgery()
            .WithOpenApi();

        group.MapDelete("removeFile",
                async ([FromBody] FileDto dto, [FromServices] IMinioService minioService,
                    CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrEmpty(dto.BucketName) || string.IsNullOrEmpty(dto.FileName))
                    {
                        throw new InvalidOperationException("Bucket name or file name cannot be empty");
                    }

                    await minioService.RemoveFileAsync(dto.BucketName, dto.FileName, cancellationToken);
                })
            .DisableAntiforgery()
            .WithOpenApi();
    }
}