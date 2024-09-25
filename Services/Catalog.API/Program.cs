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
    options.Schema.For<Product>().Index(x => x.CategoryIds.Select(categoryId => categoryId));
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

// Extension for minimal API
builder.Services.AddCarter();

// Cross-cutting service
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Healthchecks service
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
builder.Services.AddValidatorsFromAssembly(assembly);

var app = builder.Build();

/* Configure the HTTP request pipeline */
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();