namespace BetaBotClimbingAnalytics.Application.Ticks;

public interface ITicksReadModel
{
    IReadOnlyList<TickDto> GetTicks(string? gradeFilter = null, string? areaFilter = null);
}
