using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application;

namespace BetaBotClimbingAnalytics.Application.Uploads;

public sealed record GetUploadJobStatusQuery(string JobId) : IQuery<GetUploadJobStatusResult>;

public sealed record GetUploadJobStatusResult(
    string JobId,
    UploadJobState Status,
    string? Message,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public sealed class GetUploadJobStatusHandler
    : IQueryHandler<GetUploadJobStatusQuery, GetUploadJobStatusResult>
{
    private readonly IUploadJobStatusStore _store;

    public GetUploadJobStatusHandler(IUploadJobStatusStore store)
    {
        _store = store;
    }

    public Task<GetUploadJobStatusResult> Handle(
        GetUploadJobStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var status = _store.GetStatus(query.JobId);
        if (status is null)
        {
            return Task.FromResult(new GetUploadJobStatusResult(
                query.JobId,
                UploadJobState.Pending,
                Message: null,
                CreatedAt: DateTime.UtcNow,
                UpdatedAt: null));
        }

        var result = new GetUploadJobStatusResult(
            status.JobId,
            status.Status,
            status.Message,
            status.CreatedAt,
            status.UpdatedAt);
        return Task.FromResult(result);
    }
}
