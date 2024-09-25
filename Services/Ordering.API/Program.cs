var builder = WebApplication.CreateBuilder(args);

/* Configuration */
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

/* Add services to container */
// Healthchecks service
builder.Services.AddHealthChecks();

var app = builder.Build();

/* Configure the HTTP request pipeline */
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
