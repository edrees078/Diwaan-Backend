using Diwan.Shared.Primitives;
using Diwan.Shared.Results;

namespace Diwan.Identity.Core.Domain;

public sealed class User : Entity<Guid>, IAggregateRoot
{
    private User() { }

    private User(
        Guid id,
        string username,
        string fullName,
        string passwordHash,
        UserRole role,
        DateTime createdAtUtc) : base(id)
    {
        Username = username;
        FullName = fullName;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
        CreatedAtUtc = createdAtUtc;
    }

    public string Username { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static Result<User> Create(
        string username,
        string fullName,
        string passwordHash,
        UserRole role,
        DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Failure<User>(Error.Validation("User.EmptyUsername", "Username is required."));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            return Result.Failure<User>(Error.Validation("User.EmptyFullName", "Full name is required."));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<User>(Error.Validation("User.EmptyPasswordHash", "Password hash is required."));
        }

        return Result.Success(new User(
            Guid.NewGuid(),
            username.Trim(),
            fullName.Trim(),
            passwordHash,
            role,
            createdAtUtc));
    }

    public Result UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            return Result.Failure(Error.Validation("User.EmptyPasswordHash", "Password hash is required."));
        }

        PasswordHash = newPasswordHash;
        return Result.Success();
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
