using Diwan.Identity.Core.Abstractions;
using Diwan.Shared.Results;

namespace Diwan.Identity.Core.UseCases;

public sealed record GetUserByIdQuery(Guid Id);

public sealed class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> HandleAsync(
        GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(query.Id, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(Error.NotFound(
                "User.NotFound",
                $"User with id '{query.Id}' was not found."));
        }

        return Result.Success(new UserResponse(
            user.Id,
            user.Username,
            user.FullName,
            user.Role,
            user.IsActive,
            user.CreatedAtUtc));
    }
}
