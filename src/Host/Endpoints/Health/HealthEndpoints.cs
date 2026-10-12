namespace Diwan.Host.Endpoints.Health;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/health").WithTags("Health");

        group.MapGet("/live", () => Results.Ok(new { status = "live" }))
            .WithName("LivenessProbe")
            .WithSummary("Liveness health probe");

        group.MapGet("/ready", () => Results.Ok(new { status = "ready" }))
            .WithName("ReadinessProbe")
            .WithSummary("Readiness health probe");

        return app;
    }
}
