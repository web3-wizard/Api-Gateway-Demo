var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var baseAddress = builder.Configuration.GetSection("BaseAddress").Value;

builder.Services.AddHttpClient("HttpClient", client =>
{
    client.BaseAddress = new Uri(baseAddress!);
    client.Timeout = TimeSpan.FromSeconds(3);

    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

app.MapControllers();

app.Run();