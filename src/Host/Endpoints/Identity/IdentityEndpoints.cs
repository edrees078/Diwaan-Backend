using Diwan.Host.Endpoints.Common;
using Diwan.Identity.Core.Domain;
using Diwan.Identity.Core.UseCases;

namespace Diwan.Host.Endpoints.Identity;

public sealed record CreateUserRequest(
    string Username,
    string FullName,
    string Password,
    UserRole Role);

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identity").WithTags("Identity");

        group.MapPost("/users", async (CreateUserRequest request, CreateUserHandler handler, CancellationToken ct) =>
        {
            var command = new CreateUserCommand(request.Username, request.FullName, request.Password, request.Role);
            var result = await handler.HandleAsync(command, ct);
            return result.ToHttpResult();
        })
        .WithName("CreateUser")
        .WithSummary("Create a new user account");

        group.MapGet("/users/{id:guid}", async (Guid id, GetUserByIdHandler handler, CancellationToken ct) =>
        {
            var query = new GetUserByIdQuery(id);
            var result = await handler.HandleAsync(query, ct);
            return result.ToHttpResult();
        })
        .WithName("GetUserById")
        .WithSummary("Retrieve user account by identifier");

        return app;
    }
}
