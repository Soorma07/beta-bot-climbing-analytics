namespace BetaBotClimbingAnalytics.Application.Uploads;

public enum UploadJobState
{
    Pending,   // URL issued, waiting for client to upload
    Uploaded,  // File landed in S3 (future: S3 event)
    Processing,
    Completed,
    Failed
}

public sealed record UploadJobStatus(
    string JobId,
    UploadJobState Status,
    string? Message,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
