namespace Rank.Core.DTO.Response;

public record UserResponse(long Id, string Email, string Name, bool IsActive, long? IdOrganizacao);
