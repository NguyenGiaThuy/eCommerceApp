var builder = WebApplication.CreateBuilder(args);

// Configuration
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to container
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    // configurate to add all associated handlers to container
    config.RegisterServicesFromAssembly(assembly);

    // Configurate to add validation behaviour to the pipeline
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));

    // Configure to add logging behaviour to the pipeline
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.Username);
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<BasketInitialData>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Cache");
});

builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                .AddRedis(builder.Configuration.GetConnectionString("Cache")!);

builder.Services.AddCarter();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();