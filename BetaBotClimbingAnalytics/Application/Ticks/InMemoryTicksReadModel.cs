using System.Collections.Generic;
using System.Linq;

namespace BetaBotClimbingAnalytics.Application.Ticks;

/// <summary>
/// Stub read model for ticks. Replace with DynamoDB/query service when pipeline is ready.
/// </summary>
public sealed class InMemoryTicksReadModel : ITicksReadModel
{
    private readonly List<TickDto> _ticks = new();

    public InMemoryTicksReadModel()
    {
        // Seed a few example ticks for development/explorer UI
        _ticks.AddRange(new[]
        {
            new TickDto("1", "Example Route A", "5.11a", "Ten Sleep", new DateTime(2024, 6, 15)),
            new TickDto("2", "Example Route B", "5.12a", "Ten Sleep", new DateTime(2024, 7, 1)),
            new TickDto("3", "Example Route C", "5.11d", "Wyoming", new DateTime(2024, 8, 10))
        });
    }

    public IReadOnlyList<TickDto> GetTicks(string? gradeFilter = null, string? areaFilter = null)
    {
        var query = _ticks.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(gradeFilter))
            query = query.Where(t => t.Grade.Contains(gradeFilter, System.StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(areaFilter))
            query = query.Where(t => t.Area?.Contains(areaFilter, System.StringComparison.OrdinalIgnoreCase) == true);
        return query.ToList();
    }
}
