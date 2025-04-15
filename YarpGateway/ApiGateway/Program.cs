using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var reverseProxyConfig = builder.Configuration.GetSection("ReverseProxy");

builder.Services.AddReverseProxy().LoadFromConfig(reverseProxyConfig);

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("FixedWindow", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Request.Path,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(10)
            }));
});

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(
        "CustomPolicy",
        builder => builder.Expire(TimeSpan.FromSeconds(10)));
});

var app = builder.Build();

app.MapReverseProxy();

app.UseRateLimiter();

app.UseOutputCache();

await app.RunAsync();