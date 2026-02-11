using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application.Ticks;
using Xunit;

namespace BetaBotClimbingAnalytics.Tests.Application.Ticks;

public class GetTicksHandlerTests
{
    [Fact]
    public async Task Handle_Returns_All_Ticks_When_No_Filter()
    {
        var readModel = new InMemoryTicksReadModel();
        var handler = new GetTicksHandler(readModel);

        var result = await handler.Handle(
            new GetTicksQuery(),
            CancellationToken.None);

        Assert.True(result.TotalCount >= 3);
        Assert.Equal(result.Ticks.Count, result.TotalCount);
    }

    [Fact]
    public async Task Handle_Filters_By_Grade()
    {
        var readModel = new InMemoryTicksReadModel();
        var handler = new GetTicksHandler(readModel);

        var result = await handler.Handle(
            new GetTicksQuery(Grade: "5.12"),
            CancellationToken.None);

        Assert.True(result.TotalCount >= 1);
        Assert.All(result.Ticks, t => Assert.Contains("5.12", t.Grade));
    }

    [Fact]
    public async Task Handle_Filters_By_Area()
    {
        var readModel = new InMemoryTicksReadModel();
        var handler = new GetTicksHandler(readModel);

        var result = await handler.Handle(
            new GetTicksQuery(Area: "Ten Sleep"),
            CancellationToken.None);

        Assert.True(result.TotalCount >= 1);
        Assert.All(result.Ticks, t => Assert.Equal("Ten Sleep", t.Area));
    }
}
