namespace Rank.Core.DTO.Response;

public record UserResponse(int Id, string Email, string Name, bool IsActive, int? IdOrganizacao);
