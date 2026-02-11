using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using BetaBotClimbingAnalytics.Application;
using Microsoft.Extensions.Configuration;

namespace BetaBotClimbingAnalytics.Application.Uploads;

public sealed record GenerateUploadUrlCommand(string Filename) : ICommand<GenerateUploadUrlResult>;

public sealed record GenerateUploadUrlResult(string Url, string JobId, string Key);

public sealed class GenerateUploadUrlHandler
    : ICommandHandler<GenerateUploadUrlCommand, GenerateUploadUrlResult>
{
    private readonly IAmazonS3 _s3Client;
    private readonly IUploadJobStatusStore _jobStore;
    private readonly string _bucketName;

    public GenerateUploadUrlHandler(
        IAmazonS3 s3Client,
        IUploadJobStatusStore jobStore,
        IConfiguration configuration)
    {
        _s3Client = s3Client;
        _jobStore = jobStore;
        _bucketName = configuration["Upload:BucketName"] ?? "your-bucket-name";
    }

    public Task<GenerateUploadUrlResult> Handle(
        GenerateUploadUrlCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Filename))
        {
            throw new ArgumentException("Filename is required", nameof(command.Filename));
        }

        var jobId = Guid.NewGuid().ToString();
        var key = $"uploads/{jobId}/{command.Filename}";

        _jobStore.SetStatus(jobId, UploadJobState.Pending, "Presigned URL issued; awaiting upload.");

        var presignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(15),
            ContentType = "text/csv"
        };

        var url = _s3Client.GetPreSignedURL(presignedUrlRequest);

        var result = new GenerateUploadUrlResult(url, jobId, key);

        return Task.FromResult(result);
    }
}

