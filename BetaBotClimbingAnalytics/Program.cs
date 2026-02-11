using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using BetaBotClimbingAnalytics.Application;
using BetaBotClimbingAnalytics.Application.Ticks;
using BetaBotClimbingAnalytics.Application.Uploads;

var builder = WebApplication.CreateBuilder(args);

// Add AWS service configuration
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();

// Add services to the container.
builder.Services.AddControllers();

// Upload job status (in-memory; replace with durable store when needed)
builder.Services.AddSingleton<IUploadJobStatusStore, UploadJobStatusStore>();

// Ticks read model (in-memory stub; replace with DynamoDB/query when pipeline is ready)
builder.Services.AddSingleton<ITicksReadModel, InMemoryTicksReadModel>();

// CQRS / Mediator wiring
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<ICommandHandler<GenerateUploadUrlCommand, GenerateUploadUrlResult>, GenerateUploadUrlHandler>();
builder.Services.AddScoped<IQueryHandler<GetUploadJobStatusQuery, GetUploadJobStatusResult>, GetUploadJobStatusHandler>();
builder.Services.AddScoped<IQueryHandler<GetTicksQuery, GetTicksResult>, GetTicksHandler>();

// OpenAPI
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
