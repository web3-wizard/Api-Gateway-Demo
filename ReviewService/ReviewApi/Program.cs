using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var baseAddress = builder.Configuration.GetSection("BaseAddress").Value;

builder.Services.AddHttpClient("HttpClient", client =>
    {
        client.BaseAddress = new Uri(baseAddress!);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .AddResilienceHandler("Custom", pipelineBuilder =>
        {
            pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(5));

            pipelineBuilder.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromMilliseconds(500)
            });

            pipelineBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.9,
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(10)
            });

            pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(1));
        });

var app = builder.Build();

app.MapControllers();

app.Run();