using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application;
using BetaBotClimbingAnalytics.Application.Uploads;
using Microsoft.AspNetCore.Mvc;

namespace BetaBotClimbingAnalytics.Controllers
{
    [ApiController]
    [Route("api/uploads")]
    public class UploadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("presigned-url")]
        public async Task<IActionResult> GetPresignedUrl(
            [FromBody] FileRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Filename))
                return BadRequest("Filename required");

            var result = await _mediator.Send(
                new GenerateUploadUrlCommand(request.Filename),
                cancellationToken);

            return Ok(new { url = result.Url, jobId = result.JobId, key = result.Key });
        }

        [HttpGet("{jobId}/status")]
        public async Task<IActionResult> GetUploadStatus(string jobId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(jobId))
                return BadRequest("JobId required");

            var result = await _mediator.Send(
                new GetUploadJobStatusQuery(jobId),
                cancellationToken);

            return Ok(new
            {
                result.JobId,
                status = result.Status.ToString().ToLowerInvariant(),
                result.Message,
                result.CreatedAt,
                result.UpdatedAt
            });
        }
    }

    public class FileRequest
    {
        public string Filename { get; set; }
    }
}

