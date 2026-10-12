using Diwan.Identity.Core.Abstractions;
using Diwan.Identity.Core.Domain;
using Diwan.Shared.Common;
using Diwan.Shared.Results;

namespace Diwan.Identity.Core.UseCases;

public sealed record UserResponse(
    Guid Id,
    string Username,
    string FullName,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAtUtc);

public sealed record CreateUserCommand(
    string Username,
    string FullName,
    string Password,
    UserRole Role);

public sealed class CreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<UserResponse>> HandleAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 6)
        {
            return Result.Failure<UserResponse>(Error.Validation(
                "User.InvalidPassword",
                "Password must be at least 6 characters."));
        }

        var exists = await _userRepository.ExistsByUsernameAsync(command.Username, cancellationToken);
        if (exists)
        {
            return Result.Failure<UserResponse>(Error.Conflict(
                "User.UsernameConflict",
                $"Username '{command.Username}' is already taken."));
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var userResult = User.Create(
            command.Username,
            command.FullName,
            passwordHash,
            command.Role,
            _dateTimeProvider.UtcNow);

        if (userResult.IsFailure)
        {
            return Result.Failure<UserResponse>(userResult.Error);
        }

        var user = userResult.Value;
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(new UserResponse(
            user.Id,
            user.Username,
            user.FullName,
            user.Role,
            user.IsActive,
            user.CreatedAtUtc));
    }
}
