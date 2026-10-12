using Diwan.Identity.Core.Abstractions;
using Diwan.Identity.Core.UseCases;
using Diwan.Identity.Infrastructure.Persistence;
using Diwan.Identity.Infrastructure.Persistence.Repositories;
using Diwan.Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diwan.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<IdentityDbContext>(options =>
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                options.UseNpgsql(connectionString, npgsql =>
                {
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", IdentityDbContext.Schema);
                });
            }
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<GetUserByIdHandler>();

        return services;
    }
}
