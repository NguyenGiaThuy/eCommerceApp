var builder = WebApplication.CreateBuilder(args);

/* Configuration */
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

/* Add services to container */
var assembly = typeof(Program).Assembly;

// Mediator pipeline for CQRS
builder.Services.AddMediatR(config =>
{
    // configurate to add all associated handlers to container
    config.RegisterServicesFromAssembly(assembly);

    // Configurate to add validation behaviour to the pipeline
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));

    // Configure to add logging behaviour to the pipeline
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

// Oject-document mapper (ODM)
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.Username);
    options.Schema.For<ShoppingCartItem>().Identity(x => x.ProductId);
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<BasketInitialData>();

// Redis cache service
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Cache");
});

// gRPC client service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    if (builder.Environment.IsDevelopment()) handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

// Healthchecks service
builder.Services.AddHealthChecks()
.AddAsyncCheck("grpc", async () =>
{
    var handler = new HttpClientHandler();
    if (builder.Environment.IsDevelopment()) handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var channel = GrpcChannel.ForAddress(builder.Configuration["GrpcSettings:DiscountUrl"]!, new GrpcChannelOptions
    {
        HttpClient = new HttpClient(handler)
    });

    var client = new Health.HealthClient(channel);

    var healthResponse = await client.CheckAsync(new HealthCheckRequest());

    return healthResponse.Status switch
    {
        HealthCheckResponse.Types.ServingStatus.Serving => HealthCheckResult.Healthy(),
        _ => HealthCheckResult.Unhealthy()
    };
})
.AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
.AddRedis(builder.Configuration.GetConnectionString("Cache")!);

// Extension for minimal API
builder.Services.AddCarter();

// Cross-cutting services (ExceptionHandler and Validators)
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddValidatorsFromAssembly(assembly);

// Message broker service
builder.Services.AddMessageBroker(builder.Configuration);

// Repository service
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();

var app = builder.Build();

/* Configure the HTTP request pipeline */
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();