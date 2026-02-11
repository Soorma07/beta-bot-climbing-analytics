using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application.Uploads;
using Xunit;

namespace BetaBotClimbingAnalytics.Tests.Application.Uploads;

public class GetUploadJobStatusHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Pending_When_Job_Unknown()
    {
        var store = new UploadJobStatusStore();
        var handler = new GetUploadJobStatusHandler(store);

        var result = await handler.Handle(
            new GetUploadJobStatusQuery("unknown-job"),
            CancellationToken.None);

        Assert.Equal("unknown-job", result.JobId);
        Assert.Equal(UploadJobState.Pending, result.Status);
        Assert.Null(result.Message);
    }

    [Fact]
    public async Task Handle_Returns_Stored_Status_When_Job_Exists()
    {
        var store = new UploadJobStatusStore();
        store.SetStatus("job-1", UploadJobState.Processing, "Parsing CSV...");
        var handler = new GetUploadJobStatusHandler(store);

        var result = await handler.Handle(
            new GetUploadJobStatusQuery("job-1"),
            CancellationToken.None);

        Assert.Equal("job-1", result.JobId);
        Assert.Equal(UploadJobState.Processing, result.Status);
        Assert.Equal("Parsing CSV...", result.Message);
        Assert.NotNull(result.UpdatedAt);
    }
}
