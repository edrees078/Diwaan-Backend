using Diwan.Identity.Core.Domain;
using Diwan.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Diwan.Tests.IdentityTests;

public class IdentityDbContextSchemaTests
{
    [Fact]
    public void IdentityDbContext_ShouldMapUserTableToIdentitySchema()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new IdentityDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(User));

        Assert.NotNull(entityType);
        Assert.Equal("users", entityType.GetTableName());
        Assert.Equal(IdentityDbContext.Schema, entityType.GetSchema());
    }
}
