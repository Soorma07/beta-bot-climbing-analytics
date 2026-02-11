using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using BetaBotClimbingAnalytics.Application.Uploads;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BetaBotClimbingAnalytics.Tests.Application.Uploads;

public class GenerateUploadUrlHandlerTests
{
    [Fact]
    public async Task Handle_Builds_Key_And_Uses_Configured_Bucket()
    {
        var s3 = new Mock<IAmazonS3>();
        const string expectedUrl = "https://example.com/upload";

        s3.Setup(s => s.GetPreSignedURL(It.Is<GetPreSignedUrlRequest>(r =>
                r.BucketName == "test-bucket" &&
                r.Key.StartsWith("uploads/", StringComparison.Ordinal) &&
                r.Key.EndsWith("/ticks.csv", StringComparison.Ordinal))))
            .Returns(expectedUrl);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Upload:BucketName"] = "test-bucket"
            }!)
            .Build();

        var jobStore = new UploadJobStatusStore();
        var handler = new GenerateUploadUrlHandler(s3.Object, jobStore, configuration);

        var result = await handler.Handle(new GenerateUploadUrlCommand("ticks.csv"), CancellationToken.None);

        Assert.Equal(expectedUrl, result.Url);
        Assert.False(string.IsNullOrWhiteSpace(result.JobId));
        Assert.Equal("ticks.csv", Path.GetFileName(result.Key));

        var status = jobStore.GetStatus(result.JobId);
        Assert.NotNull(status);
        Assert.Equal(UploadJobState.Pending, status.Status);
    }

    [Fact]
    public async Task Handle_Throws_When_Filename_Missing()
    {
        var s3 = new Mock<IAmazonS3>();
        var jobStore = new UploadJobStatusStore();
        var configuration = new ConfigurationBuilder().Build();
        var handler = new GenerateUploadUrlHandler(s3.Object, jobStore, configuration);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new GenerateUploadUrlCommand(string.Empty), CancellationToken.None));
    }
}

