using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace BetaBotClimbingAnalytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "your-bucket-name";

        public UploadController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpPost("get-presigned-url")]
        public async Task<IActionResult> GetPresignedUrl([FromBody] FileRequest request)
        {
            if (string.IsNullOrEmpty(request.Filename))
                return BadRequest("Filename required");

            var jobId = Guid.NewGuid().ToString();
            var key = $"uploads/{jobId}/{request.Filename}";
            var presignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),
                ContentType = "text/csv"
            };
            var url = _s3Client.GetPreSignedURL(presignedUrlRequest);
            return Ok(new { url, jobId });
        }
    }

    public class FileRequest
    {
        public string Filename { get; set; }
    }
}
