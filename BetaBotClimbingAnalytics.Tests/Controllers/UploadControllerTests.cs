using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application;
using BetaBotClimbingAnalytics.Application.Uploads;
using BetaBotClimbingAnalytics.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BetaBotClimbingAnalytics.Tests.Controllers;

public class UploadControllerTests
{
    [Fact]
    public async Task GetPresignedUrl_Returns_BadRequest_When_Filename_Missing()
    {
        var mediator = new Mock<IMediator>();
        var controller = new UploadController(mediator.Object);

        var result = await controller.GetPresignedUrl(
            new FileRequest { Filename = string.Empty },
            CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPresignedUrl_Returns_Ok_With_Url_JobId_And_Key()
    {
        var mediator = new Mock<IMediator>();

        mediator
            .Setup(m => m.Send(
                It.IsAny<GenerateUploadUrlCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GenerateUploadUrlResult(
                Url: "https://example.com/upload",
                JobId: "job-123",
                Key: "uploads/job-123/ticks.csv"));

        var controller = new UploadController(mediator.Object);

        var result = await controller.GetPresignedUrl(
            new FileRequest { Filename = "ticks.csv" },
            CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value!;
        var type = value.GetType();

        var url = type.GetProperty("url")!.GetValue(value) as string;
        var jobId = type.GetProperty("jobId")!.GetValue(value) as string;
        var key = type.GetProperty("key")!.GetValue(value) as string;

        Assert.Equal("https://example.com/upload", url);
        Assert.Equal("job-123", jobId);
        Assert.Equal("uploads/job-123/ticks.csv", key);
    }

    [Fact]
    public async Task GetUploadStatus_Returns_BadRequest_When_JobId_Missing()
    {
        var mediator = new Mock<IMediator>();
        var controller = new UploadController(mediator.Object);

        var result = await controller.GetUploadStatus(string.Empty, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetUploadStatus_Returns_Ok_With_Status()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(m => m.Send(
                It.IsAny<GetUploadJobStatusQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetUploadJobStatusResult(
                "job-456",
                UploadJobState.Processing,
                "Parsing CSV...",
                new System.DateTime(2024, 1, 1, 0, 0, 0, System.DateTimeKind.Utc),
                new System.DateTime(2024, 1, 1, 0, 1, 0, System.DateTimeKind.Utc)));

        var controller = new UploadController(mediator.Object);

        var result = await controller.GetUploadStatus("job-456", CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value!;
        var type = value.GetType();
        Assert.Equal("job-456", type.GetProperty("JobId")!.GetValue(value));
        Assert.Equal("processing", type.GetProperty("status")!.GetValue(value));
        Assert.Equal("Parsing CSV...", type.GetProperty("Message")!.GetValue(value));
    }
}

