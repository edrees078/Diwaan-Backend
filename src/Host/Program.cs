using Diwan.Host.Endpoints.Health;
using Diwan.Host.Endpoints.Identity;
using Diwan.Identity.Infrastructure;
using Diwan.Shared.Common;

var builder = WebApplication.CreateBuilder(args);

// Register Core Kernel Services
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

// Register Modules
builder.Services.AddIdentityModule(builder.Configuration);

// Register OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map Endpoint Modules
app.MapHealthEndpoints();
app.MapIdentityEndpoints();

app.Run();

public partial class Program { }
