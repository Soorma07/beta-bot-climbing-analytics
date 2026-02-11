namespace BetaBotClimbingAnalytics.Application.Ticks;

public sealed record TickDto(
    string Id,
    string RouteName,
    string Grade,
    string? Area,
    DateTime Date);
