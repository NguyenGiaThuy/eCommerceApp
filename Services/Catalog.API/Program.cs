var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    // config to add all associated handlers to container
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.Run();