using System.Collections.Concurrent;

namespace BetaBotClimbingAnalytics.Application.Uploads;

public sealed class UploadJobStatusStore : IUploadJobStatusStore
{
    private readonly ConcurrentDictionary<string, UploadJobStatus> _jobs = new();

    public void SetStatus(string jobId, UploadJobState status, string? message = null)
    {
        var now = DateTime.UtcNow;
        _jobs.AddOrUpdate(
            jobId,
            _ => new UploadJobStatus(jobId, status, message, now, now),
            (_, existing) => existing with
            {
                Status = status,
                Message = message ?? existing.Message,
                UpdatedAt = now
            });
    }

    public UploadJobStatus? GetStatus(string jobId) =>
        _jobs.TryGetValue(jobId, out var status) ? status : null;
}
