using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application;
using BetaBotClimbingAnalytics.Application.Ticks;
using Microsoft.AspNetCore.Mvc;

namespace BetaBotClimbingAnalytics.Controllers;

[ApiController]
[Route("api/ticks")]
public class TicksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTicks(
        [FromQuery] string? grade = null,
        [FromQuery] string? area = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetTicksQuery(grade, area),
            cancellationToken);

        return Ok(new
        {
            result.TotalCount,
            ticks = result.Ticks
        });
    }
}
