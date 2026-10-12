using Diwan.Identity.Core.Domain;
using Diwan.Identity.Infrastructure.Services;

namespace Diwan.Tests.IdentityTests;

public class UserDomainTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldReturnSuccess()
    {
        var result = User.Create(
            "manager1",
            "Ahmed Al-Manager",
            "hashed_secret",
            UserRole.Manager,
            DateTime.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal("manager1", result.Value.Username);
        Assert.Equal("Ahmed Al-Manager", result.Value.FullName);
        Assert.Equal(UserRole.Manager, result.Value.Role);
        Assert.True(result.Value.IsActive);
    }

    [Theory]
    [InlineData("", "Name", "hash")]
    [InlineData("user", "", "hash")]
    [InlineData("user", "Name", "")]
    public void Create_WithInvalidParameters_ShouldReturnFailure(string username, string fullName, string hash)
    {
        var result = User.Create(username, fullName, hash, UserRole.Employee, DateTime.UtcNow);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void UpdatePassword_WithValidHash_ShouldUpdate()
    {
        var user = User.Create("eng1", "Engineer Ali", "old_hash", UserRole.Engineer, DateTime.UtcNow).Value;
        var updateResult = user.UpdatePassword("new_hash");

        Assert.True(updateResult.IsSuccess);
        Assert.Equal("new_hash", user.PasswordHash);
    }
}

public class PasswordHasherTests
{
    [Fact]
    public void HashAndVerify_ValidPassword_ShouldVerifySuccessfully()
    {
        var hasher = new PasswordHasher();
        var password = "SecurePassword123!";

        var hash = hasher.Hash(password);
        var isValid = hasher.Verify(password, hash);

        Assert.True(isValid);
    }

    [Fact]
    public void Verify_IncorrectPassword_ShouldReturnFalse()
    {
        var hasher = new PasswordHasher();
        var hash = hasher.Hash("RightPassword");

        var isValid = hasher.Verify("WrongPassword", hash);

        Assert.False(isValid);
    }
}
