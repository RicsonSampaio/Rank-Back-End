namespace Rank.Core.DTO.Response;

public record UserResponse
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public int? IdOrganizacao { get; set; }

    public UserResponse(int id, string email, string name, bool isActive, int? idOrganizacao)
    {
        Id = id;
        Email = email;
        Name = name;
        IsActive = isActive;
        IdOrganizacao = idOrganizacao;
    }
}
