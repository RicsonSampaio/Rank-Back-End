namespace Rank.Core.DTO.Response;

public sealed record UserResponse(long Id, string Email, string Name, bool IsActive);
