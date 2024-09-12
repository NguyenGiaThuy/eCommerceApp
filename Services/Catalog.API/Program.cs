var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(
    typeof(Program).Assembly)); // config to add all associated handlers to container

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.Run();