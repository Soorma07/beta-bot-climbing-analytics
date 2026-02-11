namespace BetaBotClimbingAnalytics.Application.Uploads;

public interface IUploadJobStatusStore
{
    void SetStatus(string jobId, UploadJobState status, string? message = null);
    UploadJobStatus? GetStatus(string jobId);
}
