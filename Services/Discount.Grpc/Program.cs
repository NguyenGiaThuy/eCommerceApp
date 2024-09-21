var builder = WebApplication.CreateBuilder(args);

// Configuration
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
});

builder.Services
    .AddGrpcHealthChecks()
    .AddCheck("discount.grpc.healthchecks", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGrpcService<DiscountService>();
app.UseMigration();
app.MapGrpcHealthChecksService();

app.Run();
