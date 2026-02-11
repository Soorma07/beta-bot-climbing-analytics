using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application;

namespace BetaBotClimbingAnalytics.Application.Ticks;

public sealed record GetTicksQuery(string? Grade = null, string? Area = null)
    : IQuery<GetTicksResult>;

public sealed record GetTicksResult(IReadOnlyList<TickDto> Ticks, int TotalCount);

public sealed class GetTicksHandler : IQueryHandler<GetTicksQuery, GetTicksResult>
{
    private readonly ITicksReadModel _readModel;

    public GetTicksHandler(ITicksReadModel readModel)
    {
        _readModel = readModel;
    }

    public Task<GetTicksResult> Handle(
        GetTicksQuery query,
        CancellationToken cancellationToken = default)
    {
        var ticks = _readModel.GetTicks(query.Grade, query.Area);
        return Task.FromResult(new GetTicksResult(ticks, ticks.Count));
    }
}
